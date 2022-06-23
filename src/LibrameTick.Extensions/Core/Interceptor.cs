#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core;

/// <summary>
/// 定义封装 <see cref="Interceptable"/> 的实例拦截器。
/// </summary>
public static class Interceptor
{

    /// <summary>
    /// 创建接口拦截代理实例。
    /// </summary>
    /// <typeparam name="TInterface">指定的接口类型。</typeparam>
    /// <typeparam name="TContext">指定的引用上下文类型。</typeparam>
    /// <param name="source">给定的 <typeparamref name="TInterface"/> 源实例。</param>
    /// <param name="context">给定的 <typeparamref name="TContext"/> 引用上下文实例。</param>
    /// <param name="interceptAction">给定的拦截动作。</param>
    /// <returns>返回 <typeparamref name="TInterface"/>。</returns>
    public static TInterface CreateInterface<TInterface, TContext>(TInterface source, TContext context,
        Action<IInterceptable, TContext>? interceptAction)
        where TInterface : class
    {
        var proxy = DispatchProxy.Create<TInterface, Interceptable>();

        var interceptable = proxy.As<Interceptable>();
        interceptable.Source = source;

        interceptAction?.Invoke(interceptable, context);

        return proxy;
    }

    /// <summary>
    /// 创建接口拦截代理实例。
    /// </summary>
    /// <typeparam name="TInterface">指定的接口类型。</typeparam>
    /// <param name="source">给定的接口源实例。</param>
    /// <param name="interceptAction">给定的拦截动作。</param>
    /// <returns>返回 <typeparamref name="TInterface"/>。</returns>
    public static TInterface CreateInterface<TInterface>(TInterface source,
        Action<IInterceptable>? interceptAction)
        where TInterface : class
    {
        var proxy = DispatchProxy.Create<TInterface, Interceptable>();

        var interceptable = proxy.As<Interceptable>();
        interceptable.Source = source;

        interceptAction?.Invoke(interceptable);

        return proxy;
    }

}
