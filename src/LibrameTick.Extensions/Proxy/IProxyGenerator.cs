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
/// 定义一个代理生成器接口。
/// </summary>
public interface IProxyGenerator
{
    /// <summary>
    /// 创建接口代理。
    /// </summary>
    /// <typeparam name="TInterface">指定的接口类型。</typeparam>
    /// <typeparam name="TInterceptor">指定的拦截器类型。</typeparam>
    /// <param name="source">给定的来源实例。</param>
    /// <returns>返回 <typeparamref name="TInterface"/>。</returns>
    TInterface CreateInterfaceProxy<TInterface, TInterceptor>(TInterface source)
        where TInterceptor : DispatchProxy, IInterceptor;

    /// <summary>
    /// 创建接口代理装饰器。
    /// </summary>
    /// <typeparam name="TInterface">指定的接口类型。</typeparam>
    /// <typeparam name="TInterceptor">指定的拦截器类型。</typeparam>
    /// <param name="source">给定的来源实例。</param>
    /// <returns>返回 <see cref="IProxyDecorator{TInterface}"/>。</returns>
    IProxyDecorator<TInterface> CreateInterfaceProxyDecorator<TInterface, TInterceptor>(TInterface source)
        where TInterceptor : DispatchProxy, IInterceptor;
}
