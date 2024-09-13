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
/// 定义实现 <see cref="IProxyGenerator"/> 的代理生成器。
/// </summary>
public sealed class ProxyGenerator : IProxyGenerator
{

    /// <summary>
    /// 创建接口代理。
    /// </summary>
    /// <typeparam name="TInterface">指定的接口类型。</typeparam>
    /// <typeparam name="TInterceptor">指定的拦截器类型。</typeparam>
    /// <param name="source">给定的来源实例。</param>
    /// <returns>返回 <typeparamref name="TInterface"/>。</returns>
    public TInterface CreateInterfaceProxy<TInterface, TInterceptor>(TInterface source)
        where TInterceptor : DispatchProxy, IInterceptor
    {
        try
        {
            var proxy = DispatchProxy.Create<TInterface, TInterceptor>();

            if (proxy is IInterceptor interceptor)
            {
                interceptor.Source = source;

                return proxy;
            }

            throw new InvalidOperationException($"Failed to create an '{nameof(TInterceptor)}' interceptor for the '{nameof(TInterface)}', for details refer to {nameof(DispatchProxy)}.{nameof(DispatchProxy.Create)}.");
        }
        catch (Exception)
        {
            throw;
        }
    }

    /// <summary>
    /// 创建接口代理装饰器。
    /// </summary>
    /// <typeparam name="TInterface">指定的接口类型。</typeparam>
    /// <typeparam name="TInterceptor">指定的拦截器类型。</typeparam>
    /// <param name="source">给定的来源实例。</param>
    /// <returns>返回 <see cref="IProxyDecorator{TInterface}"/>。</returns>
    public IProxyDecorator<TInterface> CreateInterfaceProxyDecorator<TInterface, TInterceptor>(TInterface source)
        where TInterceptor : DispatchProxy, IInterceptor
    {
        try
        {
            var proxy = DispatchProxy.Create<TInterface, TInterceptor>();

            if (proxy is IInterceptor interceptor)
            {
                interceptor.Source = source;

                return new ProxyDecorator<TInterface>(proxy, interceptor);
            }

            throw new InvalidOperationException($"Failed to create an '{nameof(TInterceptor)}' interceptor for the '{nameof(TInterface)}', for details refer to {nameof(DispatchProxy)}.{nameof(DispatchProxy.Create)}.");
        }
        catch (Exception)
        {
            throw;
        }
    }

}
