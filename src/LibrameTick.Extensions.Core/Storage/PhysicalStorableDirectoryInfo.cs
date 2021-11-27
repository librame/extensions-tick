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
/// 定义实现 <see cref="IStorableFileInfo"/> 的物理可存储目录信息。
/// </summary>
public class PhysicalStorableDirectoryInfo : IStorableFileInfo
{
    private readonly PhysicalDirectoryInfo _info;


    /// <summary>
    /// 构造一个 <see cref="PhysicalStorableDirectoryInfo"/>。
    /// </summary>
    /// <param name="info">给定的 <see cref="DirectoryInfo"/>。</param>
    public PhysicalStorableDirectoryInfo(DirectoryInfo info)
        : this(new PhysicalDirectoryInfo(info))
    {
    }

    /// <summary>
    /// 构造一个 <see cref="PhysicalStorableDirectoryInfo"/>。
    /// </summary>
    /// <param name="info">给定的 <see cref="PhysicalDirectoryInfo"/>。</param>
    public PhysicalStorableDirectoryInfo(PhysicalDirectoryInfo info)
    {
        _info = info;
    }


    /// <summary>
    /// 是否存在。
    /// </summary>
    public bool Exists
        => _info.Exists;

    /// <summary>
    /// 以字节为单位的文件长度，对于目录或不存在的文件，为-1。
    /// </summary>
    public long Length
        => _info.Length;

    /// <summary>
    /// 文件的路径，包括文件名。如果文件不能直接访问，则返回 NULL。
    /// </summary>
    public string PhysicalPath
        => _info.PhysicalPath;

    /// <summary>
    /// 文件或目录的名称，不包括任何路径。
    /// </summary>
    public string Name
        => _info.Name;

    /// <summary>
    /// 文件最后一次修改的时间。
    /// </summary>
    public DateTimeOffset LastModified
        => _info.LastModified;

    /// <summary>
    /// 是否为子目录。
    /// </summary>
    public bool IsDirectory
        => _info.IsDirectory;


    /// <summary>
    /// 创建文件内容为只读流。调用方应该在流完成时处理流。
    /// </summary>
    /// <returns>返回 <see cref="Stream"/>。</returns>
    public virtual Stream CreateReadStream()
        => _info.CreateReadStream();

    /// <summary>
    /// 总是引发异常，因为目录上不支持写入流。
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// 总是抛出异常。
    /// </exception>
    /// <returns>没有返回。</returns>
    public virtual Stream CreateWriteStream()
        => throw new InvalidOperationException("Cannot create a stream for a directory.");

}
