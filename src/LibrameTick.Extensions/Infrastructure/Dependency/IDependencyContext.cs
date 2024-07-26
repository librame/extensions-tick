#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Infrastructure.Dependency;

/// <summary>
/// 定义继承 <see cref="IEquatable{IDependencyContext}"/> 的依赖上下文接口。
/// </summary>
public interface IDependencyContext : IEquatable<IDependencyContext>
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
    /// 标识。
    /// </summary>
    string Id { get; }

    /// <summary>
    /// 获取程序集名称字符串。
    /// </summary>
    string AssemblyNameString { get; }

    /// <summary>
    /// 获取或设置字符编码。
    /// </summary>
    Encoding Encoding { get; set; }

    /// <summary>
    /// 获取或设置基础路径。
    /// </summary>
    string BasePath { get; set; }

    /// <summary>
    /// 获取路径依赖。如果 <see cref="BasePath"/> 发生变化，此路径依赖下的相关路径不会发生任何变化。
    /// </summary>
    IPathDependency Paths { get; set; }

    /// <summary>
    /// 获取锁依赖。
    /// </summary>
    ILockDependency Locks { get; set; }

    /// <summary>
    /// 获取时钟依赖。
    /// </summary>
    IClockDependency Clocks { get; set; }
}
