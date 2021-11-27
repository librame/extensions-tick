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
/// 定义实现 <see cref="IStorableDirectoryContents"/> 的物理可存储目录内容集合。
/// </summary>
public class PhysicalStorableDirectoryContents : IStorableDirectoryContents
{
    private readonly IDirectoryContents _contents;
    private IEnumerable<IStorableFileInfo>? _infos;


    /// <summary>
    /// 构造一个 <see cref="PhysicalStorableDirectoryContents"/>。
    /// </summary>
    /// <param name="directory">给定的目录。</param>
    public PhysicalStorableDirectoryContents(string directory)
        : this(directory, ExclusionFilters.Sensitive)
    {
    }

    /// <summary>
    /// 构造一个 <see cref="PhysicalStorableDirectoryContents"/>。
    /// </summary>
    /// <param name="directory">给定的目录。</param>
    /// <param name="filters">指定排除哪些文件或目录。</param>
    public PhysicalStorableDirectoryContents(string directory, ExclusionFilters filters)
        : this(new PhysicalDirectoryContents(directory, filters))
    {
    }

    /// <summary>
    /// 构造一个 <see cref="PhysicalStorableDirectoryContents"/>。
    /// </summary>
    /// <param name="contents">给定的 <see cref="IDirectoryContents"/>。</param>
    public PhysicalStorableDirectoryContents(IDirectoryContents contents)
    {
        _contents = contents;
    }


    /// <summary>
    /// 目录存在。
    /// </summary>
    public bool Exists
        => _contents.Exists;


    /// <summary>
    /// 获取 <see cref="IStorableFileInfo"/> 枚举器。
    /// </summary>
    /// <returns>返回 <see cref="IEnumerator{IStorableFileInfo}"/>。</returns>
    public virtual IEnumerator<IStorableFileInfo> GetEnumerator()
    {
        if (_infos is null)
        {
            _infos = _contents.Select<IFileInfo, IStorableFileInfo>(info =>
            {
                if (info is PhysicalFileInfo file)
                {
                    return new PhysicalStorableFileInfo(file);
                }
                else if (info is PhysicalDirectoryInfo dir)
                {
                    return new PhysicalStorableDirectoryInfo(dir);
                }

                // shouldn't happen unless BCL introduces new implementation of base type
                throw new InvalidOperationException($"Unexpected type of {nameof(FileSystemInfo)}.");
            });
        }
        
        return _infos.GetEnumerator();
    }

    IEnumerator<IFileInfo> IEnumerable<IFileInfo>.GetEnumerator()
        => GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();

}
