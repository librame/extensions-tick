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

namespace Librame.Extensions.Configuration;

/// <summary>
/// 定义 <see cref="IConfigurable{T}"/> 的可配置静态扩展。
/// </summary>
public static class ConfigurableExtensions
{

    #region PathConfigurable

    /// <summary>
    /// 将路径转为可配置接口。
    /// </summary>
    /// <param name="initialPath">给定的初始路径。</param>
    /// <param name="isDirectory">是目录路径。</param>
    /// <param name="dependency">给定的 <see cref="IPathDependency"/>（可选）。</param>
    /// <returns>返回 <see cref="IPathConfigurable"/>。</returns>
    public static IPathConfigurable AsPathConfigurable(this string initialPath, bool isDirectory, IPathDependency? dependency = null)
        => new PathConfigurable(initialPath, isDirectory, dependency);

    /// <summary>
    /// 将路径转为可配置接口。默认使用 <see cref="DependencyRegistration.CurrentContext"/> 路径依赖。
    /// </summary>
    /// <param name="initialPath">给定的初始路径。</param>
    /// <param name="isDirectory">是目录路径。</param>
    /// <returns>返回 <see cref="IPathConfigurable"/>。</returns>
    public static IPathConfigurable AsPathConfigurableWithDependency(this string initialPath, bool isDirectory)
        => initialPath.AsPathConfigurable(isDirectory, DependencyRegistration.CurrentContext.Paths);


    /// <summary>
    /// 为当前目录相对路径配置基础路径。
    /// </summary>
    /// <param name="relativePath">给定的文件相对路径。</param>
    /// <param name="customBasePath">给定的自定义基础路径（可选；如果不指定需确保当前 <see cref="Dependency"/> 不为空）。</param>
    /// <returns>返回当前 <see cref="IPathConfigurable"/>。</returns>
    public static IPathConfigurable SetDirectoryBasePath(this string relativePath, string? customBasePath = null)
    {
        var path = relativePath.AsPathConfigurableWithDependency(isDirectory: true);
        return path.ConfigureBasePath(customBasePath);
    }

    /// <summary>
    /// 为当前目录相对路径配置基础路径。
    /// </summary>
    /// <param name="relativePath">给定的文件相对路径。</param>
    /// <param name="basePathConfigurable">给定的基础路径 <see cref="IPathConfigurable"/>。</param>
    /// <returns>返回当前 <see cref="IPathConfigurable"/>。</returns>
    public static IPathConfigurable SetDirectoryBasePath(this string relativePath, IPathConfigurable basePathConfigurable)
    {
        var path = relativePath.AsPathConfigurableWithDependency(isDirectory: true);
        return path.ConfigureBasePath(basePathConfigurable);
    }

    /// <summary>
    /// 为当前目录相对路径配置基础路径。
    /// </summary>
    /// <param name="relativePath">给定的文件相对路径。</param>
    /// <param name="basePathFunc">给定从依赖取得基础路径的方法（需确保当前 <see cref="Dependency"/> 不为空）。</param>
    /// <returns>返回当前 <see cref="IPathConfigurable"/>。</returns>
    public static IPathConfigurable SetDirectoryBasePath(this string relativePath, Func<IPathDependency, string> basePathFunc)
    {
        var path = relativePath.AsPathConfigurableWithDependency(isDirectory: true);
        return path.ConfigureDependencyBasePath(basePathFunc);
    }


    /// <summary>
    /// 为当前文件相对路径配置基础路径。
    /// </summary>
    /// <param name="relativePath">给定的文件相对路径。</param>
    /// <param name="customBasePath">给定的自定义基础路径（可选；如果不指定需确保当前 <see cref="Dependency"/> 不为空）。</param>
    /// <returns>返回当前 <see cref="IPathConfigurable"/>。</returns>
    public static IPathConfigurable SetFileBasePath(this string relativePath, string? customBasePath = null)
    {
        var path = relativePath.AsPathConfigurableWithDependency(isDirectory: false);
        return path.ConfigureBasePath(customBasePath);
    }

    /// <summary>
    /// 为当前文件相对路径配置基础路径。
    /// </summary>
    /// <param name="relativePath">给定的文件相对路径。</param>
    /// <param name="basePathConfigurable">给定的基础路径 <see cref="IPathConfigurable"/>。</param>
    /// <returns>返回当前 <see cref="IPathConfigurable"/>。</returns>
    public static IPathConfigurable SetFileBasePath(this string relativePath, IPathConfigurable basePathConfigurable)
    {
        var path = relativePath.AsPathConfigurableWithDependency(isDirectory: false);
        return path.ConfigureBasePath(basePathConfigurable);
    }

    /// <summary>
    /// 为当前文件相对路径配置基础路径。
    /// </summary>
    /// <param name="relativePath">给定的文件相对路径。</param>
    /// <param name="basePathFunc">给定从依赖取得基础路径的方法（需确保当前 <see cref="Dependency"/> 不为空）。</param>
    /// <returns>返回当前 <see cref="IPathConfigurable"/>。</returns>
    public static IPathConfigurable SetFileBasePath(this string relativePath, Func<IPathDependency, string> basePathFunc)
    {
        var path = relativePath.AsPathConfigurableWithDependency(isDirectory: false);
        return path.ConfigureDependencyBasePath(basePathFunc);
    }


    public static IPathConfigurable WriteAllText(this string relativePath)
    {

    }

    #endregion

}
