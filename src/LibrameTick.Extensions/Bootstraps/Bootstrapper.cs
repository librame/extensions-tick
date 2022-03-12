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
    /// 获取自动密钥引导程序（默认使用内置自动密钥）。
    /// </summary>
    /// <returns>返回 <see cref="ILockerBootstrap"/>。</returns>
    public static IAutokeyBootstrap GetAutokey()
        => Container.Resolve(new TypeNamedKey<IAutokeyBootstrap>(), key => new InternalAutokeyBootstrap());

    /// <summary>
    /// 获取时钟引导程序（默认使用内置本地时钟）。
    /// </summary>
    /// <returns>返回 <see cref="IClockBootstrap"/>。</returns>
    public static IClockBootstrap GetClock()
        => Container.Resolve(new TypeNamedKey<IClockBootstrap>(), key => new InternalClockBootstrap());

    /// <summary>
    /// 获取目录集合引导程序（默认使用内置目录集合）。
    /// </summary>
    /// <returns>返回 <see cref="IDirectoryStructureBootstrap"/>。</returns>
    public static IDirectoryStructureBootstrap GetDirectories()
        => Container.Resolve(new TypeNamedKey<IDirectoryStructureBootstrap>(), key => new InternalDirectoyStructureBootstrap());

    /// <summary>
    /// 获取锁定器引导程序（默认使用内置本地锁定器）。
    /// </summary>
    /// <returns>返回 <see cref="ILockerBootstrap"/>。</returns>
    public static ILockerBootstrap GetLocker()
        => Container.Resolve(new TypeNamedKey<ILockerBootstrap>(), key => new InternalLockerBootstrap());

}
