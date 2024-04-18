#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Dependencies.Cryptography;

namespace Librame.Extensions.Dependencies;

/// <summary>
/// 定义静态扩展依赖接口。
/// </summary>
public interface IExtensionsDependency
{
    /// <summary>
    /// 获取开始时间。
    /// </summary>
    DateTimeOffset StartTime { get; }

    /// <summary>
    /// 获取 UTC 开始时间。
    /// </summary>
    DateTimeOffset StartTimeUtc { get; }

    /// <summary>
    /// 获取或设置字符编码。
    /// </summary>
    Encoding Encoding { get; set; }

    /// <summary>
    /// 获取程序集名称字符串。
    /// </summary>
    string AssemblyNameString { get; }

    /// <summary>
    /// 获取或设置基础路径。
    /// </summary>
    string BasePath { get; set; }

    /// <summary>
    /// 获取锁管理器。
    /// </summary>
    ILockManager LockManager { get; }

    /// <summary>
    /// 获取路径管理器。如果 <see cref="BasePath"/> 发生变化，此路径管理器下的相关路径将不会发生任何变化。
    /// </summary>
    IPathManager PathManager { get; }

    /// <summary>
    /// 延迟获取算法管理器。
    /// </summary>
    Lazy<IAlgorithmManager> LazyAlgorithmManager { get; }
}
