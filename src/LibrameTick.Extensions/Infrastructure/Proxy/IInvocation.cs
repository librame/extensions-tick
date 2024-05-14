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
/// 定义一个用于拦截的调用接口。
/// </summary>
public interface IInvocation
{
    /// <summary>
    /// 调用拦截器。
    /// </summary>
    IInterceptor Interceptor { get; }

    /// <summary>
    /// 调用方法描述符。
    /// </summary>
    MethodDescriptor Method { get; }

    /// <summary>
    /// 当前用于拦截的特性集合。
    /// </summary>
    ReadOnlyCollection<InterceptionAttribute> Interceptions { get; }

    /// <summary>
    /// 调用结果。
    /// </summary>
    object? Result { get; set; }

    /// <summary>
    /// 是否已完成调用。
    /// </summary>
    bool IsInvoked { get; }
}
