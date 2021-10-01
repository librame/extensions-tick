#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Drawing.Drawers;

/// <summary>
/// <see cref="IServiceProvider"/>、<see cref="IDrawer"/> 静态扩展。
/// </summary>
public static class ServiceProviderDrawerExtensions
{

    /// <summary>
    /// 获取已注册的绘制器。
    /// </summary>
    /// <typeparam name="TDrawer">指定的绘制器类型。</typeparam>
    /// <param name="services">给定的 <see cref="IServiceProvider"/>。</param>
    /// <param name="serviceType">给定已注册的绘制器类型（可选；默认使用指定的绘制器类型）。</param>
    /// <returns>返回 <typeparamref name="TDrawer"/>。</returns>
    public static TDrawer GetDrawer<TDrawer>(this IServiceProvider services, Type? serviceType = null)
        where TDrawer : IDrawer
        => (TDrawer)services.GetRequiredService(serviceType ?? typeof(TDrawer));

    /// <summary>
    /// 获取存储绘制器。
    /// </summary>
    /// <param name="services">给定的 <see cref="IServiceProvider"/>。</param>
    /// <returns>返回 <see cref="ISavingDrawer"/>。</returns>
    public static ISavingDrawer GetSavingDrawer(this IServiceProvider services)
        => services.GetDrawer<ISavingDrawer>();

    /// <summary>
    /// 获取缩放绘制器。
    /// </summary>
    /// <param name="services">给定的 <see cref="IServiceProvider"/>。</param>
    /// <returns>返回 <see cref="IScalingDrawer"/>。</returns>
    public static IScalingDrawer GetScalingDrawer(this IServiceProvider services)
        => services.GetDrawer<IScalingDrawer>();

    /// <summary>
    /// 获取水印绘制器。
    /// </summary>
    /// <param name="services">给定的 <see cref="IServiceProvider"/>。</param>
    /// <returns>返回 <see cref="IWatermarkDrawer"/>。</returns>
    public static IWatermarkDrawer GetWatermarkDrawer(this IServiceProvider services)
        => services.GetDrawer<IWatermarkDrawer>();

}
