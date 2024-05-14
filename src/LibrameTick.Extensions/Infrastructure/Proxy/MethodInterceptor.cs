#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Infrastructure.Proxy;

/// <summary>
/// 定义实现 <see cref="IInterceptor"/> 与 <see cref="DispatchProxy"/> 的方法拦截器。
/// </summary>
public class MethodInterceptor : DispatchProxy, IInterceptor
{
    /// <summary>
    /// 拦截的来源对象。
    /// </summary>
    public object? Source { get; set; }

    /// <summary>
    /// 拦截的来源对象类型。
    /// </summary>
    public Type? SourceType { get; set; }

    /// <summary>
    /// 当前用于拦截的调用。
    /// </summary>
    public IInvocation? Invocation { get; set; }

    /// <summary>
    /// 指定用于拦截特定调用的断定方法。
    /// </summary>
    public Predicate<MethodDescriptor>? SpecificInvocation { get; set; }


    /// <summary>
    /// 调用方法。
    /// </summary>
    /// <param name="targetMethod">给定的 <see cref="MethodInfo"/>。</param>
    /// <param name="args">给定的参数数组。</param>
    /// <returns>返回调用结果。</returns>
    protected override object? Invoke(MethodInfo? targetMethod, object?[]? args)
    {
        if (targetMethod is null || Source is null)
            return SkipIfParametersNull(targetMethod, args);

        SourceType = Source.GetType();

        var sourceMethod = SourceType.GetMethod(targetMethod.Name)!;
        var method = new MethodDescriptor(targetMethod, sourceMethod, args);

        if (SpecificInvocation is not null && !SpecificInvocation(method))
            return SkipIfNonSpecifiedMethod(method);

        var attribs = GetInterceptionAttributes(method);

        var invocation = new MethodInvocation(this, method, attribs);
        Invocation = invocation;

        BeforeInvoke(invocation);

        try
        {
            invocation.Result = targetMethod.Invoke(Source, args);
            invocation.IsInvoked = true;
        }
        catch (Exception ex)
        {
            ExceptionInvoke(invocation, ex);
        }

        AfterInvoke(invocation);

        return invocation.Result;
    }


    /// <summary>
    /// 获取方法标注的拦截特性集合。
    /// </summary>
    /// <param name="method">给定的 <see cref="MethodDescriptor"/>。</param>
    /// <returns>返回 <see cref="ReadOnlyCollection{InterceptionAttribute}"/> 数组。</returns>
    protected virtual ReadOnlyCollection<InterceptionAttribute> GetInterceptionAttributes(MethodDescriptor method)
        => method.Source.GetCustomAttributes<InterceptionAttribute>(inherit: true).AsReadOnlyCollection();


    /// <summary>
    /// 前置调用。
    /// </summary>
    /// <param name="invocation">给定的 <see cref="IInvocation"/>。</param>
    protected virtual void BeforeInvoke(IInvocation invocation)
    {
        foreach (var interception in invocation.Interceptions)
        {
            interception.PreProcess(invocation);
        }
    }

    /// <summary>
    /// 后置调用。
    /// </summary>
    /// <param name="invocation">给定的 <see cref="IInvocation"/>。</param>
    protected virtual void AfterInvoke(IInvocation invocation)
    {
        foreach (var interception in invocation.Interceptions)
        {
            interception.PostProcess(invocation);
        }
    }

    /// <summary>
    /// 异常调用。
    /// </summary>
    /// <param name="invocation">给定的 <see cref="IInvocation"/>。</param>
    /// <param name="exception">给定的异常。</param>
    protected virtual void ExceptionInvoke(IInvocation invocation, Exception exception)
    {
        foreach (var interception in invocation.Interceptions)
        {
            interception.ExceptionProcess(invocation);
        }
    }


    /// <summary>
    /// 跳过参数为空的调用。
    /// </summary>
    /// <param name="targetMethod">给定的目标方法。</param>
    /// <param name="args">给定的实参数组。</param>
    /// <returns>返回 NULL。</returns>
    protected virtual object? SkipIfParametersNull(MethodInfo? targetMethod, object?[]? args)
        => null; // targetMethod or Instance is null.

    /// <summary>
    /// 跳过非特定的方法调用。
    /// </summary>
    /// <param name="method">给定的 <see cref="MethodDescriptor"/>。</param>
    /// <returns>返回 NULL。</returns>
    protected virtual object? SkipIfNonSpecifiedMethod(MethodDescriptor method)
        => null; // non-specified method.

}
