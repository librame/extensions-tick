#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Dependencies.Internal;

namespace Librame.Extensions.Dependencies;

/// <summary>
/// 定义静态扩展依赖的静态实例化器。
/// </summary>
public static class ExtensionsDependencyInstantiator
{

    /// <summary>
    /// 延迟创建（每个线程唯一）实例。
    /// </summary>
    [ThreadStatic]
    private static readonly Lazy<IExtensionsDependency> _lazy;


    static ExtensionsDependencyInstantiator()
    {
        _lazy ??= new(ExtensionsDependency.CreateInstance);
    }


    /// <summary>
    /// 得到单例。
    /// </summary>
    public static IExtensionsDependency Instance
        => _lazy.Value;

}
