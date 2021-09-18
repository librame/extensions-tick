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
/// 文件系统信息静态扩展。
/// </summary>
public static class FileSystemInfoExtensions
{

    /// <summary>
    /// 枚举存储文件信息集合。
    /// </summary>
    /// <param name="directory">给定的本地目录字符串。</param>
    /// <param name="filters">给定的 <see cref="ExclusionFilters"/>。</param>
    /// <returns>返回 <see cref="IEnumerable{IStorageFileInfo}"/>。</returns>
    public static IEnumerable<IStorageFileInfo> EnumerateStorageFileInfos(this string directory, ExclusionFilters filters)
        => new DirectoryInfo(directory).EnumerateStorageFileInfos(filters);

    /// <summary>
    /// 枚举存储文件信息集合。
    /// </summary>
    /// <param name="directory">给定的本地 <see cref="DirectoryInfo"/>。</param>
    /// <param name="filters">给定的 <see cref="ExclusionFilters"/>。</param>
    /// <returns>返回 <see cref="IEnumerable{IStorageFileInfo}"/>。</returns>
    public static IEnumerable<IStorageFileInfo> EnumerateStorageFileInfos(this DirectoryInfo directory, ExclusionFilters filters)
    {
        try
        {
            return directory.EnumerateFileSystemInfos()
                .Where(info => !info.IsExcluded(filters)) // 不被排除的文件系统信息
                .Select<FileSystemInfo, IStorageFileInfo>(info =>
                {
                    if (info is FileInfo file)
                    {
                        return new PhysicalStorageFileInfo(file);
                    }
                    else if (info is DirectoryInfo dir)
                    {
                        return new PhysicalStorageDirectoryInfo(dir);
                    }
                    // shouldn't happen unless BCL introduces new implementation of base type
                    throw new InvalidOperationException("Unexpected type of FileSystemInfo");
                });
        }
        catch (Exception ex) when (ex is DirectoryNotFoundException || ex is IOException)
        {
            return Enumerable.Empty<IStorageFileInfo>();
        }
    }

    private static bool IsExcluded(this FileSystemInfo fileSystemInfo, ExclusionFilters filters)
    {
        if (filters is ExclusionFilters.None)
        {
            return false;
        }
        else if (fileSystemInfo.Name.StartsWith(".", StringComparison.Ordinal) && (filters & ExclusionFilters.DotPrefixed) != 0)
        {
            return true;
        }
        else if (fileSystemInfo.Exists &&
            (((fileSystemInfo.Attributes & FileAttributes.Hidden) != 0 && (filters & ExclusionFilters.Hidden) != 0) ||
                ((fileSystemInfo.Attributes & FileAttributes.System) != 0 && (filters & ExclusionFilters.System) != 0)))
        {
            return true;
        }

        return false;
    }

}
