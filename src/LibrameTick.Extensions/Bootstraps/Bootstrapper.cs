#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Core;

namespace Librame.Extensions.Bootstraps;

/// <summary>
/// 定义静态引导程序。
/// </summary>
public static class Bootstrapper
{
    private static readonly IBootstrapContainer _container;


    static Bootstrapper()
    {
        if (_container is null)
            _container = new BootstrapContainer();
    }


    /// <summary>
    /// 实例容器。
    /// </summary>
    public static IBootstrapContainer Container
        => _container;


    /// <summary>
    /// 获取指定类型的引导程序。
    /// </summary>
    /// <typeparam name="TInterface">指定实现 <see cref="IBootstrap"/> 的接口。</typeparam>
    /// <typeparam name="TImplementation">指定实现 <typeparamref name="TInterface"/> 的引导程序。</typeparam>
    /// <param name="named">给定的命名（可选）。</param>
    /// <returns>返回 <typeparamref name="TInterface"/>。</returns>
    public static TInterface GetBootstrap<TInterface, TImplementation>(string? named = null)
        where TInterface : IBootstrap
        where TImplementation : TInterface, new()
        => Container.Resolve(new TypeNamedKey<TInterface>(named), static key => new TImplementation());

    /// <summary>
    /// 获取指定类型的引导程序。
    /// </summary>
    /// <typeparam name="TInterface">指定实现 <see cref="IBootstrap"/> 的接口。</typeparam>
    /// <typeparam name="TImplementation">指定实现 <typeparamref name="TInterface"/> 的引导程序。</typeparam>
    /// <param name="key">给定的 <see cref="TypeNamedKey"/>。</param>
    /// <returns>返回 <typeparamref name="TInterface"/>。</returns>
    public static TInterface GetBootstrap<TInterface, TImplementation>(TypeNamedKey key)
        where TInterface : IBootstrap
        where TImplementation : TInterface, new()
        => Container.Resolve(key, static key => new TImplementation());


    /// <summary>
    /// 获取自动密钥引导程序（默认使用内置自动密钥）。
    /// </summary>
    /// <returns>返回 <see cref="ILockerBootstrap"/>。</returns>
    public static IAutokeyBootstrap GetAutokey()
        => GetBootstrap<IAutokeyBootstrap, InternalAutokeyBootstrap>();

    /// <summary>
    /// 获取时钟引导程序（默认使用内置本地时钟）。
    /// </summary>
    /// <returns>返回 <see cref="IClockBootstrap"/>。</returns>
    public static IClockBootstrap GetClock()
        => GetBootstrap<IClockBootstrap, InternalClockBootstrap>();

    /// <summary>
    /// 获取目录集合引导程序（默认使用内置目录集合）。
    /// </summary>
    /// <returns>返回 <see cref="IDirectoryStructureBootstrap"/>。</returns>
    public static IDirectoryStructureBootstrap GetDirectories()
        => GetBootstrap<IDirectoryStructureBootstrap, InternalDirectoyStructureBootstrap>();

    /// <summary>
    /// 获取锁定器引导程序（默认使用内置本地锁定器）。
    /// </summary>
    /// <returns>返回 <see cref="ILockerBootstrap"/>。</returns>
    public static ILockerBootstrap GetLocker()
        => GetBootstrap<ILockerBootstrap, InternalLockerBootstrap>();

}
