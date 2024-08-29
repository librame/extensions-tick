#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Dependency;

namespace Librame.Extensions.Infrastructure;

/// <summary>
/// 定义实现 <see cref="Fluent{TSelf, TChain}"/> 的流畅路径。
/// </summary>
/// <param name="initialPath">给定的初始路径。</param>
/// <param name="dependency">给定的 <see cref="IPathDependency"/>（可选；默认使用 <see cref="DependencyRegistration.CurrentContext"/> 的路径依赖）。</param>
public class FluentPath(string initialPath, IPathDependency? dependency = null)
    : Fluent<FluentPath, string>(initialPath), IEquatable<FluentPath>
{
    /// <summary>
    /// 获取路径依赖。
    /// </summary>
    public IPathDependency Dependency { get; init; }
        = dependency ?? DependencyRegistration.CurrentContext.Paths;


    /// <summary>
    /// 创建当前路径的目录信息。
    /// </summary>
    /// <returns>返回 <see cref="DirectoryInfo"/>。</returns>
    public virtual DirectoryInfo CreateDirectory()
        => Directory.CreateDirectory(CurrentValue);

    /// <summary>
    /// 确保当前路径的目录存在。
    /// </summary>
    /// <returns>返回目录完整路径。</returns>
    public virtual string EnsureDirectory()
        => CreateDirectory().FullName;

    /// <summary>
    /// 获取当前路径的目录名称。如果此目录名称不存在，则返回当前路径。
    /// </summary>
    /// <returns>返回名称字符串。</returns>
    public virtual string GetDirectoryName()
        => Path.GetDirectoryName(CurrentValue) ?? CurrentValue;


    /// <summary>
    /// 删除当前路径。
    /// </summary>
    public virtual void Delete()
        => Directory.Delete(CurrentValue);

    /// <summary>
    /// 检查当前路径是否存在。
    /// </summary>
    /// <returns>返回是否存在的布尔值。</returns>
    public virtual bool Exists()
        => Directory.Exists(CurrentValue);


    /// <summary>
    /// 为当前路径设置依赖路径为基础路径并更新当前路径属性值。
    /// </summary>
    /// <param name="basePathFunc">给定从依赖取得基础路径的方法。</param>
    /// <returns>返回当前 <see cref="FluentPath"/>。</returns>
    public virtual FluentPath SetBasePath(Func<IPathDependency, string> basePathFunc)
        => SetBasePath(basePathFunc(Dependency));

    /// <summary>
    /// 为当前路径设置基础路径。
    /// </summary>
    /// <param name="basePath">给定的基础路径。</param>
    /// <returns>返回当前 <see cref="FluentPath"/>。</returns>
    public virtual FluentPath SetBasePath(string basePath)
        => Switch(fluent => Path.Combine(basePath, fluent.CurrentValue));


    /// <summary>
    /// 切换路径字符串。
    /// </summary>
    /// <param name="newPathfunc">给定新路径字符串的方法。</param>
    /// <returns>返回 <see cref="FluentPath"/>。</returns>
    public override FluentPath Switch(Func<FluentPath, string> newPathfunc)
        => base.Switch(fluent => Path.GetFullPath(newPathfunc(this)));

    /// <summary>
    /// 复制一个当前流畅路径的副本。
    /// </summary>
    /// <returns>返回 <see cref="FluentPath"/>。</returns>
    public override FluentPath Copy()
        => new(CurrentValue, Dependency);


    /// <summary>
    /// 获取哈希码。
    /// </summary>
    /// <returns>返回整数。</returns>
    public override int GetHashCode()
        => CurrentValue.GetHashCode();

    /// <summary>
    /// 转换为字符串。
    /// </summary>
    /// <returns>返回当前路径值字符串。</returns>
    public override string ToString()
        => CurrentValue;


    /// <summary>
    /// 相等的流畅路径。
    /// </summary>
    /// <param name="obj">给定的对象。</param>
    /// <returns>返回是否相等的布尔值。</returns>
    public override bool Equals(object? obj)
        => Equals(obj as FluentPath);

    /// <summary>
    /// 相等的流畅路径。
    /// </summary>
    /// <param name="other">给定要比较的 <see cref="FluentPath"/>。</param>
    /// <returns>返回是否相等的布尔值。</returns>
    public virtual bool Equals(FluentPath? other)
        => PathEqualityComparer.Default.Equals(CurrentValue, other?.CurrentValue);


    /// <summary>
    /// 将当前 <see cref="FluentPath"/> 隐式转换为字符串形式。
    /// </summary>
    /// <param name="path">给定的 <see cref="FluentPath"/>。</param>
    public static implicit operator string(FluentPath path)
        => path.CurrentValue;

}
