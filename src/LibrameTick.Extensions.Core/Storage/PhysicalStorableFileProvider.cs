#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core.Storage;

/// <summary>
/// 定义实现 <see cref="IStorableFileProvider"/> 的物理可存储文件提供程序。
/// </summary>
public class PhysicalStorableFileProvider : IStorableFileProvider
{
    private readonly PhysicalFileProvider _provider;


    /// <summary>
    /// 构造一个 <see cref="PhysicalStorableFileProvider"/>。
    /// </summary>
    /// <param name="root">给定的根路径。</param>
    public PhysicalStorableFileProvider(string root)
        : this(root, ExclusionFilters.Sensitive)
    {
    }

    /// <summary>
    /// 构造一个 <see cref="PhysicalStorableFileProvider"/>。
    /// </summary>
    /// <param name="root">给定的根路径。</param>
    /// <param name="filters">指定排除哪些文件或目录。</param>
    public PhysicalStorableFileProvider(string root, ExclusionFilters filters)
        : this(new PhysicalFileProvider(root, filters))
    {
    }

    /// <summary>
    /// 构造一个 <see cref="PhysicalStorableFileProvider"/>。
    /// </summary>
    /// <param name="provider">给定的 <see cref="PhysicalFileProvider"/>。</param>
    public PhysicalStorableFileProvider(PhysicalFileProvider provider)
    {
        _provider = provider;
    }


    IFileInfo IFileProvider.GetFileInfo(string subpath)
        => GetFileInfo(subpath);

    /// <summary>
    /// 获取文件信息。
    /// </summary>
    /// <param name="subpath">给定的子路径。</param>
    /// <returns>返回 <see cref="IStorableFileInfo"/>。</returns>
    public virtual IStorableFileInfo GetFileInfo(string subpath)
        => new PhysicalStorableFileInfo(_provider.GetFileInfo(subpath));


    IDirectoryContents IFileProvider.GetDirectoryContents(string subpath)
        => GetDirectoryContents(subpath);

    /// <summary>
    /// 获取目录内容集合。
    /// </summary>
    /// <param name="subpath">给定的子路径。</param>
    /// <returns>返回 <see cref="IStorableDirectoryContents"/>。</returns>
    public virtual IStorableDirectoryContents GetDirectoryContents(string subpath)
        => new PhysicalStorableDirectoryContents(_provider.GetDirectoryContents(subpath));


    /// <summary>
    /// 为指定的过滤器创建修改令牌。
    /// </summary>
    /// <param name="filter">给定的过滤器（如：**/*.cs, *.*, subFolder/**/*.cshtml.）。</param>
    /// <returns>返回 <see cref="IChangeToken"/>。</returns>
    public virtual IChangeToken Watch(string filter)
        => _provider.Watch(filter);

}
