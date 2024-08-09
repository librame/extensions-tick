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
using Librame.Extensions.Storage;

namespace Librame.Extensions.Configuration;

/// <summary>
/// 定义实现 <see cref="IConfigurable{String}"/> 的可配置路径接口。
/// </summary>
public interface IPathConfigurable : IConfigurable<string>, IEquatable<IPathConfigurable>
{
    /// <summary>
    /// 是目录路径。
    /// </summary>
    bool IsDirectory { get; }

    /// <summary>
    /// 获取或设置路径依赖。
    /// </summary>
    IPathDependency? Dependency { get; set; }


    /// <summary>
    /// 使用文件存取器。
    /// </summary>
    /// <returns>返回 <see cref="IFileAccessor"/>。</returns>
    IFileAccessor UseFileAccessor();


    /// <summary>
    /// 转换为字符串。
    /// </summary>
    /// <remarks>
    /// 如果不定义此方法会默认调用 <see cref="object.ToString()"/> 返回的可空字符串导致不能通过分析验证。
    /// </remarks>
    /// <returns>返回当前路径值字符串。</returns>
    string ToString();


    /// <summary>
    /// 为当前路径配置基础路径并更新当前路径属性值。
    /// </summary>
    /// <param name="customBasePath">给定的自定义基础路径（可选；如果不指定需确保当前 <see cref="Dependency"/> 不为空）。</param>
    /// <returns>返回当前 <see cref="IPathConfigurable"/>。</returns>
    IPathConfigurable ConfigureBasePath(string? customBasePath = null);

    /// <summary>
    /// 为当前路径配置基础路径并更新当前路径属性值。
    /// </summary>
    /// <param name="basePathConfigurable">给定的基础路径 <see cref="IPathConfigurable"/>。</param>
    /// <returns>返回当前 <see cref="IPathConfigurable"/>。</returns>
    IPathConfigurable ConfigureBasePath(IPathConfigurable basePathConfigurable);

    /// <summary>
    /// 为当前路径设置依赖路径为基础路径并更新当前路径属性值。
    /// </summary>
    /// <param name="basePathFunc">给定从依赖取得基础路径的方法（需确保当前 <see cref="Dependency"/> 不为空）。</param>
    /// <returns>返回当前 <see cref="IPathConfigurable"/>。</returns>
    IPathConfigurable ConfigureDependencyBasePath(Func<IPathDependency, string> basePathFunc);


    /// <summary>
    /// 为当前文件路径配置扩展名。如果当前路径是目录将抛出不支持异常。
    /// </summary>
    /// <param name="extension">给定的文件扩展名。</param>
    /// <param name="customPath">给定的自定义路径（可选；默认使用当前路径）。</param>
    /// <returns>返回当前 <see cref="IPathConfigurable"/>。</returns>
    IPathConfigurable ConfigureFileExtension(string extension, string? customPath = null);


    /// <summary>
    /// 获取目录名。
    /// </summary>
    /// <param name="customPath">给定的自定义路径（可选；默认使用当前路径）。</param>
    /// <returns>返回目录名。</returns>
    /// <exception cref="FormatException">
    /// Invalid custom path or current path.
    /// </exception>
    string GetDirectoryName(string? customPath = null);

    /// <summary>
    /// 创建指定目录。如果当前路径非目录，则调用 <see cref="Path.GetDirectoryName(string?)"/> 获取目录后再行创建。
    /// </summary>
    /// <param name="customDirectory">给定的自定义目录（可选；默认使用当前路径创建）。</param>
    /// <returns>返回 <see cref="DirectoryInfo"/>。</returns>
    DirectoryInfo CreateDirectory(string? customDirectory = null);

    /// <summary>
    /// 确保目录存在。如果不存在则创建此目录。如果当前路径非目录，则调用 <see cref="CreateDirectory(string?)"/> 获取目录后再行创建。
    /// </summary>
    /// <param name="customDirectory">给定的自定义目录（可选；默认确保当前路径）。</param>
    /// <returns>返回目录字符串。</returns>
    string EnsureDirectory(string? customDirectory = null);

    /// <summary>
    /// 改变文件路径的扩展名。
    /// </summary>
    /// <param name="extension">给定的文件扩展名。</param>
    /// <param name="customPath">给定的自定义路径（可选；默认使用当前路径）。</param>
    /// <returns>返回是否存在的布尔值。</returns>
    /// <exception cref="NotSupportedException">
    /// Not supported current directory path.
    /// </exception>
    string ChangeFileExtension(string extension, string? customPath = null);

    /// <summary>
    /// 判断路径是否存在。
    /// </summary>
    /// <param name="customPath">给定的自定义路径（可选；默认确保当前路径）。</param>
    /// <returns>返回是否存在的布尔值。</returns>
    bool Exists(string? customPath = null);

    /// <summary>
    /// 删除指定路径。
    /// </summary>
    /// <param name="customPath">给定的自定义路径（可选；默认删除当前路径）。</param>
    void Delete(string? customPath = null);
}
