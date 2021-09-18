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
    /// <param name="path">给定的路径。</param>
    /// <returns>返回 <see cref="DirectoryInfo"/>。</returns>
    public static DirectoryInfo CreateDirectory(this string path)
        => Directory.CreateDirectory(path);


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
    /// 文件读取。
    /// </summary>
    /// <param name="path">给定的文件路径。</param>
    /// <returns>返回字节数组。</returns>
    public static byte[] FileRead(this string path)
        => path.FileRead(0L);

    /// <summary>
    /// 文件读取。
    /// </summary>
    /// <param name="path">给定的文件路径。</param>
    /// <param name="fileOffset">给定的读取偏移量。</param>
    /// <returns>返回字节数组。</returns>
    public static byte[] FileRead(this string path, long fileOffset)
    {
        using (var handle = File.OpenHandle(path))
        {
            var length = RandomAccess.GetLength(handle);
            var buffer = new byte[length - fileOffset];

            var readLength = RandomAccess.Read(handle, buffer, fileOffset);
            return buffer;
        }
    }

    /// <summary>
    /// 文件写入。
    /// </summary>
    /// <param name="path">给定的文件路径。</param>
    /// <param name="buffer">给定的字节数组。</param>
    public static void FileWrite(this string path, byte[] buffer)
        => path.FileWrite(buffer, 0L);

    /// <summary>
    /// 文件写入。
    /// </summary>
    /// <param name="path">给定的文件路径。</param>
    /// <param name="buffer">给定的字节数组。</param>
    /// <param name="fileOffset">给定的读取偏移量。</param>
    public static void FileWrite(this string path, byte[] buffer, long fileOffset)
    {
        using (var handle = File.OpenHandle(path, FileMode.Create, FileAccess.Write, FileShare.Read))
        {
            RandomAccess.Write(handle, buffer, fileOffset);
        }
    }


    /// <summary>
    /// 设置基础路径。
    /// </summary>
    /// <param name="relativePath">给定的相对路径。</param>
    /// <param name="basePath">给定的基础路径（可选；默认使用没有开发相对子路径的当前目录）。</param>
    /// <returns>返回路径字符串。</returns>
    public static string SetBasePath(this string relativePath, string? basePath = null)
    {
        if (string.IsNullOrEmpty(basePath))
            basePath = CurrentDirectoryWithoutDevelopmentRelativeSubpath;

        if (relativePath.StartsWith("./")
            || relativePath.StartsWith(".\\")
            || !relativePath.StartsWith(basePath))
        {
            return Path.Combine(basePath, relativePath);
        }

        return relativePath;
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
