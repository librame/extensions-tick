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
/// 定义实现 <see cref="IPathConfigurable"/> 的可配置路径。
/// </summary>
/// <param name="initialPath">给定的初始路径。</param>
/// <param name="isDirectory">是目录路径。</param>
/// <param name="dependency">给定的 <see cref="IPathDependency"/>（可选）。</param>
public class PathConfigurable(string initialPath, bool isDirectory, IPathDependency? dependency)
    : Configurable<string>(initialPath), IPathConfigurable
{
    private IFileAccessor? _fileAccessor;


    /// <summary>
    /// 是目录路径。
    /// </summary>
    public bool IsDirectory { get; init; } = isDirectory;

    /// <summary>
    /// 获取或设置路径依赖。
    /// </summary>
    public IPathDependency? Dependency { get; set; } = dependency;


    /// <summary>
    /// 使用文件访问器。
    /// </summary>
    /// <returns>返回 <see cref="IFileAccessor"/>。</returns>
    public IFileAccessor UseFileAccessor()
        => _fileAccessor ??= new FileAccessor(CurrentValue);


    /// <summary>
    /// 获取哈希码。
    /// </summary>
    /// <returns>返回整数。</returns>
    public override int GetHashCode()
        => HashCode.Combine(IsDirectory, CurrentValue);

    /// <summary>
    /// 转换为字符串。
    /// </summary>
    /// <returns>返回当前路径值字符串。</returns>
    public override string ToString()
        => CurrentValue;


    /// <summary>
    /// 相等的 <see cref="IPathConfigurable"/>。
    /// </summary>
    /// <param name="obj">给定的对象。</param>
    /// <returns>返回是否相等的布尔值。</returns>
    public override bool Equals(object? obj)
        => Equals(obj as PathConfigurable);

    /// <summary>
    /// 相等的 <see cref="IPathConfigurable"/>。
    /// </summary>
    /// <param name="other">给定要比较的 <see cref="IPathConfigurable"/>。</param>
    /// <returns>返回是否相等的布尔值。</returns>
    public virtual bool Equals(IPathConfigurable? other)
        => IsDirectory == other?.IsDirectory && PathEqualityComparer.Default.Equals(CurrentValue, other.CurrentValue);


    /// <summary>
    /// 为当前路径配置基础路径并更新当前路径属性值。
    /// </summary>
    /// <param name="customBasePath">给定的自定义基础路径（可选；如果不指定需确保当前 <see cref="Dependency"/> 不为空）。</param>
    /// <returns>返回当前 <see cref="IPathConfigurable"/>。</returns>
    public IPathConfigurable ConfigureBasePath(string? customBasePath = null)
    {
        ConfigureValue(path => CombineBasePath(customBasePath));
        return this;
    }

    /// <summary>
    /// 为当前路径配置基础路径并更新当前路径属性值。
    /// </summary>
    /// <param name="basePathConfigurable">给定的基础路径 <see cref="IPathConfigurable"/>。</param>
    /// <returns>返回当前 <see cref="IPathConfigurable"/>。</returns>
    public IPathConfigurable ConfigureBasePath(IPathConfigurable basePathConfigurable)
    {
        ConfigureValue(path => CombineBasePath(basePathConfigurable));
        return this;
    }

    /// <summary>
    /// 为当前路径设置依赖路径为基础路径并更新当前路径属性值。
    /// </summary>
    /// <param name="basePathFunc">给定从依赖取得基础路径的方法（需确保当前 <see cref="Dependency"/> 不为空）。</param>
    /// <returns>返回当前 <see cref="IPathConfigurable"/>。</returns>
    public IPathConfigurable ConfigureDependencyBasePath(Func<IPathDependency, string> basePathFunc)
    {
        ConfigureValue(path => CombineDependencyBasePath(basePathFunc));
        return this;
    }

    private string CombineBasePath(string? customBasePath = null)
    {
        if (customBasePath is null && Dependency is null)
        {
            throw new ArgumentException($"'{nameof(customBasePath)}' and '{nameof(Dependency)}' are both null.");
        }

        var basePath = customBasePath ?? Dependency!.BasePath;

        return CombineBasePath(basePath, CurrentValue);
    }

    private string CombineBasePath(IPathConfigurable basePathConfigurable)
    {
        var basePath = basePathConfigurable.GetDirectoryName();

        return CombineBasePath(basePath, CurrentValue);
    }

    private string CombineDependencyBasePath(Func<IPathDependency, string> basePathFunc)
    {
        ArgumentNullException.ThrowIfNull(Dependency);

        var basePath = basePathFunc(Dependency);

        return CombineBasePath(basePath, CurrentValue);
    }

    private static string CombineBasePath(string basePath, string relativePath)
    {
        if (relativePath.StartsWith(basePath))
        {
            return relativePath;
        }

        return Path.Combine(basePath, relativePath);
    }


    /// <summary>
    /// 为当前文件路径配置扩展名。如果当前路径是目录将抛出不支持异常。
    /// </summary>
    /// <param name="extension">给定的文件扩展名。</param>
    /// <param name="customPath">给定的自定义路径（可选；默认使用当前路径）。</param>
    /// <returns>返回当前 <see cref="IPathConfigurable"/>。</returns>
    public IPathConfigurable ConfigureFileExtension(string extension, string? customPath = null)
    {
        ConfigureValue(path => ChangeFileExtension(extension, customPath));
        return this;
    }


    /// <summary>
    /// 获取目录名。
    /// </summary>
    /// <param name="customPath">给定的自定义路径（可选；默认使用当前路径）。</param>
    /// <returns>返回目录名。</returns>
    /// <exception cref="FormatException">
    /// Invalid custom path or current path.
    /// </exception>
    public string GetDirectoryName(string? customPath = null)
    {
        if (customPath is not null)
        {
            return Path.GetDirectoryName(customPath) ?? throw new FormatException($"Invalid custom path '{customPath}'.");
        }

        if (IsDirectory)
        {
            return CurrentValue;
        }

        return Path.GetDirectoryName(CurrentValue) ?? throw new FormatException($"Invalid current path '{CurrentValue}'.");
    }

    /// <summary>
    /// 创建目录。如果当前路径非目录，则调用 <see cref="Path.GetDirectoryName(string?)"/> 获取目录后再行创建。
    /// </summary>
    /// <param name="customDirectory">给定的自定义目录（可选；默认使用当前路径创建）。</param>
    /// <returns>返回 <see cref="DirectoryInfo"/>。</returns>
    public DirectoryInfo CreateDirectory(string? customDirectory = null)
    {
        var dir = GetDirectoryName(customDirectory);

        return Directory.CreateDirectory(dir);
    }

    /// <summary>
    /// 确保目录存在。如果不存在则创建此目录。如果当前路径非目录，则调用 <see cref="CreateDirectory(string?)"/> 获取目录后再行创建。
    /// </summary>
    /// <param name="customDirectory">给定的自定义目录（可选；默认确保当前路径）。</param>
    /// <returns>返回目录字符串。</returns>
    public string EnsureDirectory(string? customDirectory = null)
        => CreateDirectory(customDirectory).FullName;

    /// <summary>
    /// 改变文件路径的扩展名。如果当前路径是目录将抛出不支持异常。
    /// </summary>
    /// <param name="extension">给定的文件扩展名。</param>
    /// <param name="customPath">给定的自定义路径（可选；默认使用当前路径）。</param>
    /// <returns>返回是否存在的布尔值。</returns>
    /// <exception cref="NotSupportedException">
    /// Not supported current directory path.
    /// </exception>
    public string ChangeFileExtension(string extension, string? customPath = null)
    {
        if (customPath is not null)
        {
            return Path.ChangeExtension(customPath, extension);
        }

        if (IsDirectory) throw new NotSupportedException($"Not supported current directory path '{CurrentValue}'.");

        return Path.ChangeExtension(CurrentValue, extension);
    }

    /// <summary>
    /// 判断路径是否存在。
    /// </summary>
    /// <param name="customPath">给定的自定义路径（可选；默认确保当前路径）。</param>
    /// <returns>返回是否存在的布尔值。</returns>
    public bool Exists(string? customPath = null)
    {
        if (customPath is not null)
        {
            return Directory.Exists(customPath) || File.Exists(customPath);
        }

        return IsDirectory ? Directory.Exists(CurrentValue) : File.Exists(CurrentValue);
    }

    /// <summary>
    /// 删除路径。
    /// </summary>
    /// <param name="customPath">给定的自定义路径（可选；默认删除当前路径）。</param>
    public void Delete(string? customPath = null)
    {
        if (customPath is not null)
        {
            Directory.Delete(customPath);
            File.Delete(customPath);
        }

        if (IsDirectory)
        {
            Directory.Delete(CurrentValue);
        }
        else
        {
            File.Delete(CurrentValue);
        }
    }

}
