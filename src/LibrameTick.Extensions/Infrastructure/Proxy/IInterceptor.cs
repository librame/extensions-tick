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
/// 定义一个拦截器接口。
/// </summary>
public interface IInterceptor
{
    /// <summary>
    /// 拦截的来源对象。
    /// </summary>
    object? Source { get; set; }

    /// <summary>
    /// 拦截的来源对象类型。
    /// </summary>
    Type? SourceType { get; set; }

    /// <summary>
    /// 当前用于拦截的调用。
    /// </summary>
    IInvocation? Invocation { get; set; }

    /// <summary>
    /// 指定用于拦截特定调用的断定方法。
    /// </summary>
    Predicate<MethodDescriptor>? SpecificInvocation { get; set; }
}
