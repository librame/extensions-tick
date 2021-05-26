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
using System.Diagnostics.CodeAnalysis;
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
        /// 创建目录。
        /// </summary>
        /// <param name="path">给定的路径。</param>
        /// <returns>返回 <see cref="DirectoryInfo"/>。</returns>
        public static DirectoryInfo CreateDirectory([NotNullWhen(false)] this string? path)
        {
            return path.NotEmpty(nameof(path), p =>
            {
                if (p.EndsWith(ExtensionDefaults.CurrentPathSeparator))
                    p = p.TrimEnd(ExtensionDefaults.CurrentPathSeparator);

                return Directory.CreateDirectory(p);
            });
        }

        /// <summary>
        /// 设置基础路径。
        /// </summary>
        /// <param name="relativePath">给定的相对路径。</param>
        /// <param name="basePath">给定的基础路径（可选；默认使用没有开发相对路径的当前目录）。</param>
        /// <returns>返回路径字符串。</returns>
        public static string SetBasePath([NotNullWhen(false)] this string? relativePath,
            [NotNullWhen(false)] string? basePath = null)
        {
            if (basePath.IsEmpty())
                basePath = ExtensionDefaults.CurrentDirectory.TrimDevelopmentRelativePath();

            return relativePath.NotEmpty(nameof(relativePath), p =>
            {
                if (p.StartsWith("./") || !p.StartsWith(basePath))
                    return Path.Combine(basePath, p);

                return p;
            });
        }


        #region CombinePath

        /// <summary>
        /// 合并路径。
        /// </summary>
        /// <param name="basePath">给定的基础路径。</param>
        /// <param name="relativePath">给定的相对路径。</param>
        /// <returns>返回路径字符串。</returns>
        public static string CombinePath([NotNullWhen(false)] this string? basePath, string relativePath)
            => basePath.NotEmpty(nameof(basePath), p => Path.Combine(p, relativePath));

        /// <summary>
        /// 将字符串组合为相对路径（如：folder => folder/）。
        /// </summary>
        /// <param name="value">给定的字符串值。</param>
        /// <param name="pathSeparatorForward">路径分隔符前置（可选；默认后置）。</param>
        /// <returns>返回路径字符串。</returns>
        public static string CombineRelativePath([NotNullWhen(false)] this string? value, bool pathSeparatorForward = false)
        {
            if (value.HasInvalidPathChars())
                throw new InvalidOperationException($"'{value}' has invalid path chars.");

            if (pathSeparatorForward)
                return $"{ExtensionDefaults.CurrentPathSeparator}{value}";

            return $"{value}{ExtensionDefaults.CurrentPathSeparator}";
        }

        #endregion


        #region InvalidPathChars

        /// <summary>
        /// 含有无效的路径字符集合。
        /// </summary>
        /// <param name="path">给定的路径。</param>
        /// <returns>返回布尔值。</returns>
        public static bool HasInvalidPathChars([NotNullWhen(false)] this string? path)
            => path.HasInvalidChars(Path.GetInvalidPathChars());

        /// <summary>
        /// 含有无效的文件名称字符集合。
        /// </summary>
        /// <param name="fileName">给定的文件名。</param>
        /// <returns>返回布尔值。</returns>
        public static bool HasInvalidFileNameChars([NotNullWhen(false)] this string? fileName)
            => fileName.HasInvalidChars(Path.GetInvalidFileNameChars());

        #endregion


        #region TrimRelativePath

        private static readonly string _developmentRelativePath = InitDevelopmentRelativePath();

        private static string InitDevelopmentRelativePath()
        {
            var separator = Regex.Escape(ExtensionDefaults.CurrentPathSeparatorString);

            var sb = new StringBuilder();
            sb.Append($"({separator}bin|{separator}obj)");
            sb.Append($"({separator}x86|{separator}x64)?");
            sb.Append($"({separator}Debug|{separator}Release)");

            return sb.ToString();
        }

        /// <summary>
        /// 修剪路径中存在的开发相对路径（如：prefix/bin/[x64/]Debug => prefix）。
        /// </summary>
        /// <param name="path">给定的路径。</param>
        /// <returns>返回修剪后的路径字符串。</returns>
        public static string TrimDevelopmentRelativePath([NotNullWhen(true)] this string? path)
        {
            return path.NotEmpty(nameof(path), p =>
            {
                var regex = new Regex(_developmentRelativePath);
                if (regex.IsMatch(p))
                {
                    var match = regex.Match(p);
                    return p.Substring(0, match.Index);
                }

                return p;
            });
        }

        #endregion

    }
}
