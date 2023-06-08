#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Proxy;

/// <summary>
/// 定义实现 <see cref="IInvocation"/> 的方法调用。
/// </summary>
public class MethodInvocation : IInvocation
{
    /// <summary>
    /// 构造一个 <see cref="MethodInvocation"/>。
    /// </summary>
    /// <param name="interceptor">给定的 <see cref="IInterceptor"/>。</param>
    /// <param name="method">给定的 <see cref="MethodDescriptor"/>。</param>
    /// <param name="interceptions">给定的 <see cref="ReadOnlyCollection{InterceptionAttribute}"/>。</param>
    public MethodInvocation(IInterceptor interceptor, MethodDescriptor method,
        ReadOnlyCollection<InterceptionAttribute> interceptions)
    {
        Interceptor = interceptor;
        Method = method;
        Interceptions = interceptions;
    }


    /// <summary>
    /// 调用拦截器。
    /// </summary>
    public IInterceptor Interceptor { get; init; }

    /// <summary>
    /// 调用方法描述符。
    /// </summary>
    public MethodDescriptor Method { get; init; }

    /// <summary>
    /// 当前用于拦截的特性集合。
    /// </summary>
    public ReadOnlyCollection<InterceptionAttribute> Interceptions { get; init; }

    /// <summary>
    /// 调用结果。
    /// </summary>
    public object? Result { get; set; }

    /// <summary>
    /// 是否已完成调用。
    /// </summary>
    public bool IsInvoked { get; set; }
}
