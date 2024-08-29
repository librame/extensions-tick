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
/// 定义 <see cref="FluentPath"/> 静态扩展。
/// </summary>
public static class FluentPathExtensions
{

    #region FluentPath

    /// <summary>
    /// 为相对路径设置基础路径并返回流畅路径形式。
    /// </summary>
    /// <param name="relativePath">给定的相对路径。</param>
    /// <returns>返回 <see cref="FluentPath"/>。</returns>
    public static FluentPath SetBasePath(this string relativePath)
        => relativePath.SetBasePath(depend => depend.BasePath);

    /// <summary>
    /// 为相对路径设置基础路径并返回流畅路径形式。
    /// </summary>
    /// <param name="relativePath">给定的相对路径。</param>
    /// <param name="basePathFunc">给定从依赖取得基础路径的方法。</param>
    /// <returns>返回 <see cref="FluentPath"/>。</returns>
    public static FluentPath SetBasePath(this string relativePath, Func<IPathDependency, string> basePathFunc)
        => new FluentPath(relativePath).SetBasePath(basePathFunc);

    /// <summary>
    /// 为相对路径设置基础路径并返回流畅路径形式。
    /// </summary>
    /// <param name="relativePath">给定的相对路径。</param>
    /// <param name="basePath">给定的基础路径。</param>
    /// <returns>返回 <see cref="FluentPath"/>。</returns>
    public static FluentPath SetBasePath(this string relativePath, string basePath)
        => new FluentPath(relativePath).SetBasePath(basePath);

    #endregion


    #region FluentFilePath

    /// <summary>
    /// 将流畅路径转换为流畅文件路径。
    /// </summary>
    /// <param name="path">给定的 <see cref="FluentPath"/>。</param>
    /// <returns>返回 <see cref="FluentFilePath"/>。</returns>
    public static FluentFilePath AsFilePath(this FluentPath path) => new(path);

    /// <summary>
    /// 为相对文件路径设置基础路径并返回流畅文件路径形式。
    /// </summary>
    /// <param name="relativePath">给定的文件相对路径。</param>
    /// <returns>返回 <see cref="FluentFilePath"/>。</returns>
    public static FluentFilePath SetFileBasePath(this string relativePath)
        => relativePath.SetFileBasePath(depend => depend.BasePath);

    /// <summary>
    /// 为相对文件路径设置基础路径并返回流畅文件路径形式。
    /// </summary>
    /// <param name="relativePath">给定的文件相对路径。</param>
    /// <param name="basePathFunc">给定从依赖取得基础路径的方法。</param>
    /// <returns>返回 <see cref="FluentFilePath"/>。</returns>
    public static FluentFilePath SetFileBasePath(this string relativePath, Func<IPathDependency, string> basePathFunc)
    {
        var path = new FluentFilePath(relativePath);
        path.SetBasePath(basePathFunc);

        return path;
    }

    /// <summary>
    /// 为相对文件路径设置基础路径并返回流畅路径形式。
    /// </summary>
    /// <param name="relativePath">给定的文件相对路径。</param>
    /// <param name="basePath">给定的基础路径。</param>
    /// <returns>返回 <see cref="FluentFilePath"/>。</returns>
    public static FluentFilePath SetFileBasePath(this string relativePath, string basePath)
    {
        var path = new FluentFilePath(relativePath);
        path.SetBasePath(basePath);

        return path;
    }

    #endregion

}
