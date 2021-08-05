#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Librame.Extensions
{
    /// <summary>
    /// <see cref="Path"/> 静态扩展。
    /// </summary>
    public static class PathExtensions
    {
        /// <summary>
        /// <see cref="Directory.GetCurrentDirectory()"/> 当前目录。
        /// </summary>
        public static readonly string CurrentDirectory
            = Directory.GetCurrentDirectory();

        /// <summary>
        /// 除去开发相对路径部分的 <see cref="Directory.GetCurrentDirectory()"/> 当前目录。
        /// </summary>
        public static string CurrentDirectoryWithoutDevelopmentRelativePath
            => CurrentDirectory.TrimDevelopmentRelativePath();


        /// <summary>
        /// 创建目录。
        /// </summary>
        /// <param name="path">给定的路径。</param>
        /// <returns>返回 <see cref="DirectoryInfo"/>。</returns>
        public static DirectoryInfo CreateDirectory(this string path)
            => Directory.CreateDirectory(path);


        /// <summary>
        /// 目录是否存在。
        /// </summary>
        /// <param name="path">给定的路径。</param>
        /// <returns>返回布尔值。</returns>
        public static bool DirectoryExists(this string path)
            => Directory.Exists(path);

        /// <summary>
        /// 文件是否存在。
        /// </summary>
        /// <param name="path">给定的路径。</param>
        /// <returns>返回布尔值。</returns>
        public static bool FileExists(this string path)
            => File.Exists(path);


        /// <summary>
        /// 设置基础路径。
        /// </summary>
        /// <exception cref="ArgumentException">
        /// <paramref name="relativePath"/> is null or empty.
        /// </exception>
        /// <param name="relativePath">给定的相对路径。</param>
        /// <param name="basePath">给定的基础路径（可选；默认使用没有开发相对路径的当前目录）。</param>
        /// <returns>返回路径字符串。</returns>
        public static string SetBasePath(this string relativePath, string? basePath = null)
        {
            if (string.IsNullOrEmpty(basePath))
                basePath = CurrentDirectoryWithoutDevelopmentRelativePath;

            if (relativePath.StartsWith("./") || !relativePath.StartsWith(basePath))
                return Path.Combine(basePath, relativePath);

            return relativePath;
        }


        #region CombinePath

        /// <summary>
        /// 合并路径。
        /// </summary>
        /// <param name="basePath">给定的基础路径。</param>
        /// <param name="relativePath">给定的相对路径。</param>
        /// <returns>返回路径字符串。</returns>
        public static string CombinePath(this string basePath, string relativePath)
            => Path.Combine(basePath, relativePath);

        /// <summary>
        /// 将字符串组合为相对路径（如：folder => folder/）。
        /// </summary>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> is null or empty.
        /// </exception>
        /// <param name="value">给定的字符串值。</param>
        /// <param name="pathSeparatorForward">路径分隔符前置（可选；默认后置）。</param>
        /// <returns>返回路径字符串。</returns>
        public static string CombineRelativePath(this string value, bool pathSeparatorForward = false)
        {
            if (value.HasInvalidPathChars())
                throw new InvalidOperationException($"'{value}' has invalid path chars.");

            if (pathSeparatorForward)
                return $"{Path.DirectorySeparatorChar}{value}";

            return $"{value}{Path.DirectorySeparatorChar}";
        }

        #endregion


        #region InvalidPathChars

        /// <summary>
        /// 含有无效的路径字符集合。
        /// </summary>
        /// <exception cref="ArgumentException">
        /// <paramref name="path"/> is null or empty.
        /// </exception>
        /// <param name="path">给定的路径。</param>
        /// <returns>返回布尔值。</returns>
        public static bool HasInvalidPathChars(this string path)
            => path.HasInvalidChars(Path.GetInvalidPathChars());

        /// <summary>
        /// 含有无效的文件名称字符集合。
        /// </summary>
        /// <exception cref="ArgumentException">
        /// <paramref name="fileName"/> is null or empty.
        /// </exception>
        /// <param name="fileName">给定的文件名。</param>
        /// <returns>返回布尔值。</returns>
        public static bool HasInvalidFileNameChars(this string fileName)
            => fileName.HasInvalidChars(Path.GetInvalidFileNameChars());

        #endregion


        #region TrimRelativePath

        private static readonly string _developmentRelativePath = InitDevelopmentRelativePath();

        private static string InitDevelopmentRelativePath()
        {
            var separator = Regex.Escape(Path.DirectorySeparatorChar.ToString());

            var sb = new StringBuilder();
            sb.Append($"({separator}bin|{separator}obj)");
            sb.Append($"({separator}x86|{separator}x64)?");
            sb.Append($"({separator}Debug|{separator}Release)");

            return sb.ToString();
        }

        /// <summary>
        /// 修剪路径中存在的开发相对路径部分（如：prefix/bin/[x64/]Debug => prefix）。
        /// </summary>
        /// <param name="path">给定的路径。</param>
        /// <returns>返回修剪后的路径字符串。</returns>
        public static string TrimDevelopmentRelativePath(this string path)
        {
            var regex = new Regex(_developmentRelativePath);
            if (regex.IsMatch(path))
            {
                var match = regex.Match(path);
                return path.Substring(0, match.Index);
            }

            return path;
        }

        #endregion

    }
}
