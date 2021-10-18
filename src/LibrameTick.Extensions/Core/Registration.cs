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
/// 定义实现 <see cref="IRegisterableContainer"/> 的静态注册实例。
/// </summary>
public static class Registration
{
    private static readonly IRegisterableContainer _container;


    static Registration()
    {
        if (_container is null)
            _container = new InternalRegisterableContainer();
    }


    /// <summary>
    /// 实例容器。
    /// </summary>
    public static IRegisterableContainer Container
        => _container;


    /// <summary>
    /// 获取可注册时钟（默认使用内置本地时钟）。
    /// </summary>
    /// <returns>返回 <see cref="IRegisterableClock"/>。</returns>
    public static IRegisterableClock GetRegisterableClock()
        => Container.Resolve(new TypeNamedKey<IRegisterableClock>(), key => new InternalRegisterableClock());

    /// <summary>
    /// 获取可注册目录集合（默认使用内置应用目录集合）。
    /// </summary>
    /// <returns>返回 <see cref="IRegisterableDirectories"/>。</returns>
    public static IRegisterableDirectories GetRegisterableDirectories()
        => Container.Resolve(new TypeNamedKey<IRegisterableDirectories>(), key => new InternalRegisterableDirectories());

    /// <summary>
    /// 获取可注册锁定器（默认使用内置本地锁定器）。
    /// </summary>
    /// <returns>返回 <see cref="IRegisterableLocker"/>。</returns>
    public static IRegisterableLocker GetRegisterableLocker()
        => Container.Resolve(new TypeNamedKey<IRegisterableLocker>(), key => new InternalRegisterableLocker());

}
