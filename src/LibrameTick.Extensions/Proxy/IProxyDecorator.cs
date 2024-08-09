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
/// 定义一个代理装饰器接口。
/// </summary>
/// <typeparam name="TInterface">指定的接口类型。</typeparam>
public interface IProxyDecorator<TInterface>
{
    /// <summary>
    /// 代理来源复合实例。
    /// </summary>
    TInterface ProxySource { get; }

    /// <summary>
    /// 调用拦截器（同 <see cref="ProxySource"/> 为复合实例）。
    /// </summary>
    IInterceptor Interceptor { get; }

    /// <summary>
    /// 当前用于拦截的调用。
    /// </summary>
    IInvocation? Invocation { get; }

    /// <summary>
    /// 经过调用的当前来源实例。
    /// </summary>
    TInterface CurrentSource { get; }


    /// <summary>
    /// 唯一调用。
    /// </summary>
    /// <typeparam name="TResult">指定的结果类型。</typeparam>
    /// <param name="invokeExpression">给定的调用表达式。</param>
    /// <returns>返回 <typeparamref name="TResult"/>。</returns>
    TResult Only<TResult>(Expression<Func<TInterface, TResult>> invokeExpression);
}
