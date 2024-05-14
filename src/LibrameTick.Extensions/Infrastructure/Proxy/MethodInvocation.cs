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
/// 定义实现 <see cref="IInvocation"/> 的方法调用。
/// </summary>
/// <remarks>
/// 构造一个 <see cref="MethodInvocation"/>。
/// </remarks>
/// <param name="interceptor">给定的 <see cref="IInterceptor"/>。</param>
/// <param name="method">给定的 <see cref="MethodDescriptor"/>。</param>
/// <param name="interceptions">给定的 <see cref="ReadOnlyCollection{InterceptionAttribute}"/>。</param>
public class MethodInvocation(IInterceptor interceptor, MethodDescriptor method,
    ReadOnlyCollection<InterceptionAttribute> interceptions) : IInvocation
{
    /// <summary>
    /// 调用拦截器。
    /// </summary>
    public IInterceptor Interceptor { get; init; } = interceptor;

    /// <summary>
    /// 调用方法描述符。
    /// </summary>
    public MethodDescriptor Method { get; init; } = method;

    /// <summary>
    /// 当前用于拦截的特性集合。
    /// </summary>
    public ReadOnlyCollection<InterceptionAttribute> Interceptions { get; init; } = interceptions;

    /// <summary>
    /// 调用结果。
    /// </summary>
    public object? Result { get; set; }

    /// <summary>
    /// 是否已完成调用。
    /// </summary>
    public bool IsInvoked { get; set; }
}
