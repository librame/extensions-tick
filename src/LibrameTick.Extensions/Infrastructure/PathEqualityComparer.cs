#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Infrastructure.Dependency;

namespace Librame.Extensions.Infrastructure;

/// <summary>
/// 定义路径相等比较器。主要用于区别在不同的操作系统下路径的大小写比较。
/// </summary>
/// <seealso cref="IEqualityComparer{String}" />
public sealed class PathEqualityComparer : IEqualityComparer<string>
{
    /// <summary>
    /// 获取默认比较器实例。
    /// </summary>
    /// <value>
    /// 返回 <see cref="PathEqualityComparer"/> 的默认实例。
    /// </value>
    public static PathEqualityComparer Default { get; } = new PathEqualityComparer();


    /// <summary>
    /// 确定两个路径是否相等。如果是 <see cref="OSPlatform.Linux"/> 或 <see cref="OSPlatform.FreeBSD"/>，则区分大小写，反之则忽略大小写。
    /// </summary>
    /// <param name="x">给定第一个要比较的路径。</param>
    /// <param name="y">给定另一个要比较的路径。</param>
    /// <returns>
    /// 返回是否相等的布尔值。
    /// </returns>
    public bool Equals(string? x, string? y)
        => string.Equals(x, y, DependencyRegistration.CurrentContext.Paths.OSComparison);

    /// <summary>
    /// 获取路径哈希码。
    /// </summary>
    /// <param name="obj">给定的路径对象。</param>
    /// <returns>
    /// 返回整数。
    /// </returns>
    public int GetHashCode([DisallowNull] string obj)
        => obj.GetHashCode();

}
