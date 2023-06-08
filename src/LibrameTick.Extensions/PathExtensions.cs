#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions;

/// <summary>
/// <see cref="Path"/> 静态扩展。
/// </summary>
public static class PathExtensions
{
    /// <summary>
    /// 文件缓冲区大小。
    /// </summary>
    public static int FileBufferSize = 1024 * 10;

    /// <summary>
    /// <see cref="Directory.GetCurrentDirectory()"/> 当前目录。
    /// </summary>
    public static readonly string CurrentDirectory
        = Directory.GetCurrentDirectory(); //Environment.ProcessPath;

    /// <summary>
    /// 除去开发相对路径部分的 <see cref="Directory.GetCurrentDirectory()"/> 当前目录。
    /// </summary>
    public static string CurrentDirectoryWithoutDevelopmentRelativeSubpath
        => CurrentDirectory.TrimDevelopmentRelativeSubpath();


    /// <summary>
    /// 创建目录。
    /// </summary>
    /// <param name="path">给定的目录路径。</param>
    /// <returns>返回 <see cref="DirectoryInfo"/>。</returns>
    public static DirectoryInfo CreateDirectory(this string path)
        => Directory.CreateDirectory(path);

    /// <summary>
    /// 确保目录已存在。
    /// </summary>
    /// <param name="directory">给定的目录。</param>
    /// <returns>返回目录字符串。</returns>
    public static string EnsureDirectory(this string directory)
    {
        directory.CreateDirectory();
        return directory;
    }


    /// <summary>
    /// 删除目录。
    /// </summary>
    /// <param name="path">给定的目录路径。</param>
    public static void DirectoryDelete(this string path)
        => Directory.Delete(path);

    /// <summary>
    /// 目录是否存在。
    /// </summary>
    /// <param name="path">给定的目录路径。</param>
    /// <returns>返回布尔值。</returns>
    public static bool DirectoryExists(this string path)
        => Directory.Exists(path);


    /// <summary>
    /// 删除文件。
    /// </summary>
    /// <param name="path">给定的文件路径。</param>
    public static void FileDelete(this string path)
        => File.Delete(path);

    /// <summary>
    /// 文件是否存在。
    /// </summary>
    /// <param name="path">给定的文件路径。</param>
    /// <returns>返回布尔值。</returns>
    public static bool FileExists(this string path)
        => File.Exists(path);


    /// <summary>
    /// 设置基础路径。如果为基础路径为空，则默认使用没有开发相对子路径的当前目录路径。
    /// </summary>
    /// <param name="relativePath">给定的相对路径。</param>
    /// <param name="basePath">给定的基础路径（可选；默认使用没有开发相对子路径的当前目录路径）。</param>
    /// <returns>返回路径字符串。</returns>
    public static string SetBasePath(this string relativePath, string? basePath = null)
    {
        if (string.IsNullOrEmpty(basePath))
            basePath = CurrentDirectoryWithoutDevelopmentRelativeSubpath;

        return relativePath.StartsWith("./")
            || relativePath.StartsWith(".\\")
            || !relativePath.StartsWith(basePath)
            ? Path.Combine(basePath, relativePath)
            : relativePath;
    }


    #region Combine

    /// <summary>
    /// 合并路径。
    /// </summary>
    /// <param name="basePath">给定的基础路径。</param>
    /// <param name="relativePath">给定的相对路径。</param>
    /// <returns>返回路径字符串。</returns>
    public static string CombinePath(this string basePath, string relativePath)
        => Path.Combine(basePath, relativePath);

    /// <summary>
    /// 合并并创建此目录。
    /// </summary>
    /// <param name="baseDir">给定的基础目录。</param>
    /// <param name="relativeDir">给定的相对目录。</param>
    /// <returns>返回目录字符串。</returns>
    public static string CombineDirectory(this string baseDir, string relativeDir)
        => baseDir.CombinePath(relativeDir).CreateDirectory().FullName;


    /// <summary>
    /// 将文件夹名称集合组合为相对子路径（如：“folder1、folder2 => folder1\folder2\ 或 \folder1\folder2”）。
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// <paramref name="folderNames"/> has invalid path chars.
    /// </exception>
    /// <param name="folderNames">给定的文件夹名称集合。</param>
    /// <param name="pathSeparatorEscaping">将路径分隔符转义（可选；默认不转义）。</param>
    /// <param name="pathSeparatorForward">将路径分隔符前置（可选；默认后置）。</param>
    /// <returns>返回路径字符串。</returns>
    public static string CombineRelativeSubpath(this IEnumerable<string> folderNames,
        bool pathSeparatorEscaping = false, bool pathSeparatorForward = false)
        => folderNames.Select(value =>
        {
            return value.CombineRelativeSubpath(pathSeparatorEscaping, pathSeparatorForward);
        })
        .JoinString();

    /// <summary>
    /// 将文件夹名称组合为相对子路径（如：“folder => folder\ 或 \folder”）。
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// <paramref name="folderName"/> has invalid path chars.
    /// </exception>
    /// <param name="folderName">给定的文件夹名称。</param>
    /// <param name="pathSeparatorEscaping">将路径分隔符转义（可选；默认不转义）。</param>
    /// <param name="pathSeparatorForward">将路径分隔符前置（可选；默认后置）。</param>
    /// <returns>返回路径字符串。</returns>
    public static string CombineRelativeSubpath(this string folderName,
        bool pathSeparatorEscaping = false, bool pathSeparatorForward = false)
    {
        if (folderName.HasInvalidPathChars())
            throw new InvalidOperationException($"'{folderName}' has invalid path chars.");

        var separator = Path.DirectorySeparatorChar.ToString();
        if (pathSeparatorEscaping)
            separator = Regex.Escape(separator);

        if (pathSeparatorForward)
            return $"{separator}{folderName}";

        return $"{folderName}{separator}";
    }

    #endregion


    #region BinaryFile

    /// <summary>
    /// 比较两个文件是否相等（支持比较文件路径、大小、内容等）。
    /// </summary>
    /// <param name="path1">给定的文件路径1。</param>
    /// <param name="path2">给定的文件路径2。</param>
    /// <param name="pathIgnoreCase">比较文件路径时，是否区分路径字符大小写（可选；默认自动根据系统平台判定，如 Linux 平台区分，其他不分区，可手动指定）。</param>
    /// <param name="compareFileSize">是否比较文件大小（可选；默认不启用，也不推荐启用，除非你能确定文件大小相等就表示是同一个文件）。</param>
    /// <returns>返回是否相等的布尔值。</returns>
    public static bool FileEquals(this string path1, string path2, bool? pathIgnoreCase = null, bool compareFileSize = false)
    {
        var pathComparison = pathIgnoreCase is null
            ? RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase
            : !pathIgnoreCase.Value ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;

        // 文件路径相等直接返回相等
        if (path1.Equals(path2, pathComparison))
            return true;

        using (var handle1 = File.OpenHandle(path1))
        using (var handle2 = File.OpenHandle(path2))
        {
            var length1 = RandomAccess.GetLength(handle1);
            var length2 = RandomAccess.GetLength(handle2);

            if (compareFileSize && length1 == length2)
                return true; // 如果启用比较文件大小，则大小相等表示文件相等

            var buffer1 = FileBufferSize.RentByteArray();
            var buffer2 = FileBufferSize.RentByteArray();

            var offset1 = 0L;
            var offset2 = 0L;

            while (true)
            {
                var curLength1 = RandomAccess.Read(handle1, buffer1, offset1);
                var curLength2 = RandomAccess.Read(handle2, buffer2, offset2);

                // 如果同顺序读取指定长度内容不相同，则直接返回不相等
                if (!buffer1.SequenceEqualByReadOnlySpan(buffer2))
                {
                    buffer1.ReturnArray();
                    buffer2.ReturnArray();

                    return false;
                }

                if (curLength1 == 0 || curLength2 == 0)
                    break;

                offset1 += curLength1;
                offset2 += curLength2;
            }

            buffer1.ReturnArray();
            buffer2.ReturnArray();

            return true;
        }
    }


    /// <summary>
    /// 读取二进制文件。
    /// </summary>
    /// <param name="path">给定的文件路径。</param>
    /// <returns>返回字节数组。</returns>
    public static byte[] ReadBinaryFile(this string path)
        => path.ReadBinaryFile(0L);

    /// <summary>
    /// 读取二进制文件。
    /// </summary>
    /// <param name="path">给定的文件路径。</param>
    /// <param name="offset">给定的读取偏移量。</param>
    /// <returns>返回字节数组。</returns>
    public static byte[] ReadBinaryFile(this string path, long offset)
    {
        using (var handle = File.OpenHandle(path))
        {
            var length = RandomAccess.GetLength(handle);
            var buffer = new byte[length - offset];

            var readLength = RandomAccess.Read(handle, buffer, offset);
            return buffer;
        }
    }


    /// <summary>
    /// 写入二进制文件。
    /// </summary>
    /// <param name="path">给定的文件路径。</param>
    /// <param name="buffer">给定的字节数组。</param>
    public static void WriteBinaryFile(this string path, byte[] buffer)
        => path.WriteBinaryFile(buffer, 0L);

    /// <summary>
    /// 写入二进制文件。
    /// </summary>
    /// <param name="path">给定的文件路径。</param>
    /// <param name="buffer">给定的字节数组。</param>
    /// <param name="offset">给定的写入偏移量。</param>
    public static void WriteBinaryFile(this string path, byte[] buffer, long offset)
    {
        using (var handle = File.OpenHandle(path, FileMode.Create, FileAccess.Write, FileShare.Read))
        {
            RandomAccess.Write(handle, buffer, offset);
        }
    }

    #endregion


    #region InvalidPathChars

    /// <summary>
    /// 含无效的路径字符集合。
    /// </summary>
    /// <param name="path">给定的路径。</param>
    /// <returns>返回布尔值。</returns>
    public static bool HasInvalidPathChars(this string path)
        => path.HasInvalidChars(Path.GetInvalidPathChars());

    /// <summary>
    /// 含无效的文件名称字符集合。
    /// </summary>
    /// <param name="fileName">给定的文件名。</param>
    /// <returns>返回布尔值。</returns>
    public static bool HasInvalidFileNameChars(this string fileName)
        => fileName.HasInvalidChars(Path.GetInvalidFileNameChars());

    #endregion


    #region TrimRelativeSubpath

    private static readonly string _developmentRelativeSubpath
        = InitDevelopmentRelativeSubpath();

    private static string InitDevelopmentRelativeSubpath()
    {
        var separator = Regex.Escape(Path.DirectorySeparatorChar.ToString());

        var sb = new StringBuilder();

        // 采用路径分隔符前置方案
        sb.Append($"({separator}bin|{separator}obj)");
        sb.Append($"({separator}x86|{separator}x64)?");
        sb.Append($"({separator}Debug|{separator}Release)");

        return sb.ToString();
    }

    /// <summary>
    /// 修剪路径中存在的开发相对路径部分（如：prefix\bin\[x64\]Debug => prefix）。
    /// </summary>
    /// <param name="path">给定的路径。</param>
    /// <returns>返回修剪后的路径字符串。</returns>
    public static string TrimDevelopmentRelativeSubpath(this string path)
    {
        var regex = new Regex(_developmentRelativeSubpath);
        if (regex.IsMatch(path))
        {
            var match = regex.Match(path);
            return path.Substring(0, match.Index);
        }

        return path;
    }

    #endregion

}
