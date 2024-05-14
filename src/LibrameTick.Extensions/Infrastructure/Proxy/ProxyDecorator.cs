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
/// 定义实现 <see cref="IProxyDecorator{TInterface}"/> 的代理装饰器。
/// </summary>
/// <typeparam name="TInterface">指定的接口类型。</typeparam>
/// <remarks>
/// 构造一个 <see cref="ProxyDecorator{TInterface}"/>。
/// </remarks>
/// <param name="proxySource">给定的代理来源复合实例。</param>
/// <param name="interceptor">给定的调用拦截器。</param>
public class ProxyDecorator<TInterface>(TInterface proxySource, IInterceptor interceptor) : IProxyDecorator<TInterface>
{
    /// <summary>
    /// 代理来源复合实例。
    /// </summary>
    public TInterface ProxySource { get; init; } = proxySource;

    /// <summary>
    /// 调用拦截器（同 <see cref="ProxySource"/> 为复合实例）。
    /// </summary>
    public IInterceptor Interceptor { get; init; } = interceptor;

    /// <summary>
    /// 当前用于拦截的调用。
    /// </summary>
    public IInvocation? Invocation => Interceptor.Invocation;

    /// <summary>
    /// 经过调用的当前来源实例。
    /// </summary>
    public TInterface CurrentSource
    {
        get
        {
            if (Invocation?.IsInvoked == true
                && Interceptor.Source is TInterface invokedSource)
            {
                return invokedSource;
            }

            return ProxySource;
        }
    }


    /// <summary>
    /// 唯一调用。
    /// </summary>
    /// <typeparam name="TResult">指定的结果类型。</typeparam>
    /// <param name="invokeExpression">给定的调用表达式。</param>
    /// <returns>返回 <typeparamref name="TResult"/>。</returns>
    /// <exception cref="NotSupportedException">
    /// Invalid method call expression.
    /// </exception>
    public virtual TResult Only<TResult>(Expression<Func<TInterface, TResult>> invokeExpression)
    {
        if (invokeExpression.Body is not MethodCallExpression methodCall)
            throw new NotSupportedException($"Invalid method call expression '{invokeExpression.Body.Type}'.");

        var sourceMethod = Interceptor.SourceType?.GetMethod(methodCall.Method.Name)!;
        var onlyMethod = new MethodDescriptor(methodCall.Method, sourceMethod, methodCall.Arguments.ToArray());

        // 限定拦截指定方法
        Interceptor.SpecificInvocation = onlyMethod.Equals;

        // 每次限定拦截单个方法
        var result = invokeExpression.Compile().Invoke(ProxySource);

        return result;
    }

}
