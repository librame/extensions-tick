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
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Librame.Extensions
{
    /// <summary>
    /// <see cref="string"/> 静态扩展。
    /// </summary>
    public static class StringExtensions
    {

        /// <summary>
        /// 含有无效字符集合。
        /// </summary>
        /// <param name="value">给定的当前字符串。</param>
        /// <param name="invalidChars">用于检测的无效字符数组。</param>
        /// <returns>返回是否含有的布尔值。</returns>
        public static bool HasInvalidChars([NotNullWhen(false)] this string? value, IEnumerable<char> invalidChars)
            => value.NotWhiteSpace(nameof(value), str => str.ToCharArray().Any(c => invalidChars.Contains(c)));


        #region Insert and Append

        /// <summary>
        /// 向当前字符串末尾附加字符串。
        /// </summary>
        /// <param name="current">给定的当前字符串。</param>
        /// <param name="append">给定的附加字符串。</param>
        /// <returns>返回字符串。</returns>
        public static string Append([NotNullWhen(false)] this string? current, [NotNullWhen(false)] string? append)
        {
            if (current.IsEmpty()) return append ?? string.Empty;

            if (append.IsEmpty()) return current;

            return $"{current}{append}";
        }

        /// <summary>
        /// 向当前字符串的索引处插入字符串。
        /// </summary>
        /// <param name="current">给定的当前字符串。</param>
        /// <param name="insert">给定的插入字符串。</param>
        /// <param name="startIndex">给定要插入的开始索引（可选；默认为 0 表示在起始处插入）。</param>
        /// <returns>返回字符串。</returns>
        public static string Insert([NotNullWhen(false)] this string? current, [NotNullWhen(false)] string? insert, int startIndex = 0)
        {
            if (current.IsEmpty()) return insert ?? string.Empty;

            if (insert.IsEmpty()) return current;

            if (startIndex <= 0) return $"{insert}{current}";

            if (startIndex >= current.Length - 1) return $"{current}{insert}"; // Append

            return $"{current.Substring(0, startIndex)}{insert}{current.Substring(startIndex)}";
        }

        #endregion


        #region JoinString

        /// <summary>
        /// 将字符串集合结合为字符串。
        /// </summary>
        /// <param name="values">给定的字符串集合。</param>
        /// <param name="separator">用于连接的分隔符（可选；默认不使用分隔符）。</param>
        /// <returns>返回结合字符串。</returns>
        public static string JoinString([NotNullWhen(false)] this IEnumerable<string?> values, string? separator = null)
            => string.Join(separator ?? string.Empty, values);

        /// <summary>
        /// 将字符串集合结合为字符串。
        /// </summary>
        /// <param name="values">给定的字符串集合。</param>
        /// <param name="separator">用于连接的分隔符（可选；默认不使用分隔符）。</param>
        /// <returns>返回结合字符串。</returns>
        public static string JoinString([NotNullWhen(false)] this IEnumerable<string?> values, char separator)
            => string.Join(separator, values);

        /// <summary>
        /// 将字符串集合结合为字符串。
        /// </summary>
        /// <param name="values">给定的字符串集合。</param>
        /// <param name="separator">用于连接的分隔符（可选；默认不使用分隔符）。</param>
        /// <returns>返回结合字符串。</returns>
        public static string JoinString<T>([NotNullWhen(false)] this IEnumerable<T> values, string? separator = null)
            => string.Join(separator ?? string.Empty, values);

        /// <summary>
        /// 将字符串集合结合为字符串。
        /// </summary>
        /// <param name="values">给定的字符串集合。</param>
        /// <param name="separator">用于连接的分隔符（可选；默认不使用分隔符）。</param>
        /// <returns>返回结合字符串。</returns>
        public static string JoinString<T>([NotNullWhen(false)] this IEnumerable<T> values, char separator)
            => string.Join(separator, values);

        #endregion


        #region Leading and Trailing

        /// <summary>
        /// 确保当前字符串以指定字符开始。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="current"/> 为空或空字符串。
        /// </exception>
        /// <param name="current">给定的当前字符串。</param>
        /// <param name="leading">给定的开始字符。</param>
        /// <returns>返回字符串。</returns>
        public static string Leading([NotNullWhen(false)] this string? current, char leading)
            => current.NotEmpty(nameof(current), str => str.StartsWith(leading) ? str : $"{leading}{str}");

        /// <summary>
        /// 确保当前字符串以指定字符开始。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="current"/> 为空或空字符串。
        /// </exception>
        /// <param name="current">给定的当前字符串。</param>
        /// <param name="leading">给定的开始字符串。</param>
        /// <returns>返回字符串。</returns>
        public static string Leading([NotNullWhen(false)] this string? current, string leading)
            => current.NotEmpty(nameof(current), str => str.StartsWith(leading) ? str : $"{leading}{str}");


        /// <summary>
        /// 确保当前字符串以指定字符结尾。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="current"/> 为空或空字符串。
        /// </exception>
        /// <param name="current">给定的当前字符串。</param>
        /// <param name="trailing">给定的结尾字符。</param>
        /// <returns>返回字符串。</returns>
        public static string Trailing([NotNullWhen(false)] this string? current, char trailing)
            => current.NotEmpty(nameof(current), str => str.StartsWith(trailing) ? str : $"{str}{trailing}");

        /// <summary>
        /// 确保当前字符串以指定字符结尾。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="current"/> 为空或空字符串。
        /// </exception>
        /// <param name="current">给定的当前字符串。</param>
        /// <param name="trailing">给定的结尾字符串。</param>
        /// <returns>返回字符串。</returns>
        public static string Trailing([NotNullWhen(false)] this string? current, string trailing)
            => current.NotEmpty(nameof(current), str => str.StartsWith(trailing) ? str : $"{str}{trailing}");

        #endregion


        #region Trim

        /// <summary>
        /// 清除首尾指定字符串。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="current"/> 为空。
        /// </exception>
        /// <param name="current">给定的当前字符串。</param>
        /// <param name="trim">要清除的字符串（如果为空则直接返回）。</param>
        /// <param name="loops">是否循环查找（可选；默认启用）。</param>
        /// <returns>返回清除后的字符串。</returns>
        public static string Trim([NotNullWhen(false)] this string? current, string trim, bool loops = true)
        {
            current = TrimStart(current, trim, loops);
            current = TrimEnd(current, trim, loops);

            return current;
        }

        /// <summary>
        /// 清除首部指定字符串。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="current"/> 为空。
        /// </exception>
        /// <param name="current">给定的当前字符串。</param>
        /// <param name="trim">要清除的字符串（如果为空则直接返回）。</param>
        /// <param name="loops">是否循环查找（可选；默认启用）。</param>
        /// <returns>返回清除后的字符串。</returns>
        public static string TrimStart([NotNullWhen(false)] this string? current, string trim, bool loops = true)
            => current.NotEmpty(nameof(current), str =>
            {
                if (str.Length > 0 && str.EndsWith(trim))
                {
                    str = str.Substring(trim.Length);

                    if (str.Length > 0 && loops)
                        str = str.TrimStart(trim, loops);
                }

                return str;
            });

        /// <summary>
        /// 清除尾部指定字符串。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="current"/> 为空。
        /// </exception>
        /// <param name="current">指定的当前字符串。</param>
        /// <param name="trim">要清除的字符串（如果为空则直接返回）。</param>
        /// <param name="loops">是否循环查找（可选；默认启用）。</param>
        /// <returns>返回清除后的字符串。</returns>
        public static string TrimEnd([NotNullWhen(false)] this string? current, string trim, bool loops = true)
            => current.NotEmpty(nameof(current), str =>
            {
                if (str.Length > 0 && str.EndsWith(trim))
                {
                    str = str.Substring(0, str.Length - trim.Length);

                    if (str.Length > 0 && loops)
                        str = str.TrimEnd(trim, loops);
                }

                return str;
            });

        #endregion

    }
}
