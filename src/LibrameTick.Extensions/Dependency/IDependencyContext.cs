#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Dependency;

/// <summary>
/// 定义继承 <see cref="IEquatable{IDependencyContext}"/> 的依赖上下文接口。
/// </summary>
public interface IDependencyContext : IEquatable<IDependencyContext>
{
    /// <summary>
    /// 获取开始时间。
    /// </summary>
    /// <value>
    /// 返回 <see cref="DateTimeOffset"/>。
    /// </value>
    DateTimeOffset StartTime { get; }

    /// <summary>
    /// 获取 UTC 开始时间。
    /// </summary>
    /// <value>
    /// 返回 <see cref="DateTimeOffset"/>。
    /// </value>
    DateTimeOffset StartTimeUtc { get; }

    /// <summary>
    /// 标识。
    /// </summary>
    /// <value>
    /// 返回字符串。
    /// </value>
    string Id { get; }

    /// <summary>
    /// 获取程序集名称字符串。
    /// </summary>
    /// <value>
    /// 返回字符串。
    /// </value>
    string AssemblyNameString { get; }

    /// <summary>
    /// 获取或设置字符编码。
    /// </summary>
    /// <value>
    /// 返回 <see cref="System.Text.Encoding"/>。
    /// </value>
    Encoding Encoding { get; set; }

    /// <summary>
    /// 获取或设置基础路径。
    /// </summary>
    /// <value>
    /// 返回字符串。
    /// </value>
    string BasePath { get; set; }

    /// <summary>
    /// 获取或设置路径依赖。如果 <see cref="BasePath"/> 发生变化，此路径依赖下的相关路径不会发生任何变化。
    /// </summary>
    /// <value>
    /// 返回 <see cref="IPathDependency"/>。
    /// </value>
    IPathDependency Paths { get; set; }

    /// <summary>
    /// 获取或设置锁依赖。
    /// </summary>
    /// <value>
    /// 返回 <see cref="ILockDependency"/>。
    /// </value>
    ILockDependency Locks { get; set; }

    /// <summary>
    /// 获取或设置时钟依赖。
    /// </summary>
    /// <value>
    /// 返回 <see cref="IClockDependency"/>。
    /// </value>
    IClockDependency Clocks { get; set; }

    /// <summary>
    /// 获取或设置内存流依赖。
    /// </summary>
    /// <value>
    /// 返回 <see cref="IMemoryStreamDependency"/>。
    /// </value>
    IMemoryStreamDependency MemoryStreams { get; set; }
}
