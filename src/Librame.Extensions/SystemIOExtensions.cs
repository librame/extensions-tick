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
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Librame.Extensions
{
    /// <summary>
    /// <see cref="System.IO"/> 静态扩展。
    /// </summary>
    public static class SystemIOExtensions
    {

        #region Path.InvalidChars

        /// <summary>
        /// 含有无效的路径字符集合。
        /// </summary>
        /// <param name="path">给定的可空路径字符串。</param>
        /// <returns>返回布尔值。</returns>
        public static bool HasInvalidPathChars(this string? path)
            => path.HasInvalidChars(Path.GetInvalidPathChars());

        /// <summary>
        /// 含有无效的文件名称字符集合。
        /// </summary>
        /// <param name="fileName">给定的可空文件名称字符串。</param>
        /// <returns>返回布尔值。</returns>
        public static bool HasInvalidFileNameChars(this string? fileName)
            => fileName.HasInvalidChars(Path.GetInvalidFileNameChars());

        private static bool HasInvalidChars(this string? str, char[] invalidChars)
        {
            if (string.IsNullOrWhiteSpace(str))
                throw new ArgumentNullException(nameof(str));

            return str.ToCharArray().Any(p => invalidChars.Contains(p));
        }

        #endregion


        /// <summary>
        /// 将字符串组合为相对路径（如：folder => folder/）。
        /// </summary>
        /// <param name="str">给定的可空字符串。</param>
        /// <param name="separatorForward">分隔符前置（可选；默认后置）。</param>
        /// <returns>返回字符串。</returns>
        public static string CombineRelativePath(this string? str, bool separatorForward = false)
        {
            if (str.HasInvalidPathChars())
                throw new InvalidOperationException($"'{str}' has invalid path chars.");

            if (separatorForward)
                return $"{Path.DirectorySeparatorChar}{str}";
            
            return $"{str}{Path.DirectorySeparatorChar}";
        }

        /// <summary>
        /// 修剪基础路径中存在的开发相对路径（如：basePath/bin/[x64/]Debug => basePath）。
        /// </summary>
        /// <param name="basePath">给定的可空基础路径。</param>
        /// <returns>返回目录字符串。</returns>
        public static string TrimDevelopmentRelativePath(this string? basePath)
        {
            if (string.IsNullOrWhiteSpace(basePath))
                throw new ArgumentNullException(nameof(basePath));

            var regex = new Regex(CombineDevelopmentRelativePath());
            if (regex.IsMatch(basePath))
            {
                var match = regex.Match(basePath);
                return basePath.Substring(0, match.Index);
            }

            return basePath;

            string CombineDevelopmentRelativePath()
            {
                var separator = Regex.Escape(Path.DirectorySeparatorChar.ToString());
                
                var sb = new StringBuilder();
                sb.Append($"({separator}bin|{separator}obj)");
                sb.Append($"({separator}x86|{separator}x64)?");
                sb.Append($"({separator}Debug|{separator}Release)");

                return sb.ToString();
            }
        }

    }
}
