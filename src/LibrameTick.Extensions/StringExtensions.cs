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
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> is null or empty.
        /// </exception>
        /// <param name="value">给定的当前字符串。</param>
        /// <param name="invalidChars">用于检测的无效字符数组。</param>
        /// <returns>返回是否含有的布尔值。</returns>
        public static bool HasInvalidChars(this string value, IEnumerable<char> invalidChars)
            => value.NotEmpty(nameof(value)).ToCharArray().Any(c => invalidChars.Contains(c));


        #region Append and Insert

        /// <summary>
        /// 向当前字符串末尾附加字符串。
        /// </summary>
        /// <param name="current">给定的当前字符串。</param>
        /// <param name="append">给定的附加字符串。</param>
        /// <returns>返回字符串。</returns>
        public static string Append(this string current, string append)
            => $"{current}{append}";

        /// <summary>
        /// 向当前字符串的索引处插入字符串。
        /// </summary>
        /// <exception cref="ArgumentException">
        /// <paramref name="current"/> is null or empty.
        /// </exception>
        /// <param name="current">给定的当前字符串。</param>
        /// <param name="insert">给定的插入字符串。</param>
        /// <param name="startIndex">给定要插入的开始索引（可选；默认为 0 表示在起始处插入）。</param>
        /// <returns>返回字符串。</returns>
        public static string Insert(this string current, string insert, int startIndex = 0)
        {
            current.NotEmpty(nameof(current));

            if (startIndex <= 0)
                return $"{insert}{current}";

            if (startIndex >= current.Length - 1)
                return $"{current}{insert}"; // Append

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
        public static string JoinString(this IEnumerable<string?> values, string? separator = null)
            => string.Join(separator ?? string.Empty, values);

        /// <summary>
        /// 将字符串集合结合为字符串。
        /// </summary>
        /// <param name="values">给定的字符串集合。</param>
        /// <param name="separator">用于连接的分隔符（可选；默认不使用分隔符）。</param>
        /// <returns>返回结合字符串。</returns>
        public static string JoinString(this IEnumerable<string?> values, char separator)
            => string.Join(separator, values);

        /// <summary>
        /// 将字符串集合结合为字符串。
        /// </summary>
        /// <param name="values">给定的字符串集合。</param>
        /// <param name="separator">用于连接的分隔符（可选；默认不使用分隔符）。</param>
        /// <returns>返回结合字符串。</returns>
        public static string JoinString<T>(this IEnumerable<T> values, string? separator = null)
            => string.Join(separator ?? string.Empty, values);

        /// <summary>
        /// 将字符串集合结合为字符串。
        /// </summary>
        /// <param name="values">给定的字符串集合。</param>
        /// <param name="separator">用于连接的分隔符（可选；默认不使用分隔符）。</param>
        /// <returns>返回结合字符串。</returns>
        public static string JoinString<T>(this IEnumerable<T> values, char separator)
            => string.Join(separator, values);

        #endregion


        #region Leading and Trailing

        /// <summary>
        /// 确保当前字符串以指定字符开始。
        /// </summary>
        /// <exception cref="ArgumentException">
        /// <paramref name="current"/> 为空或空字符串。
        /// </exception>
        /// <param name="current">给定的当前字符串。</param>
        /// <param name="leading">给定的开始字符。</param>
        /// <returns>返回字符串。</returns>
        public static string Leading(this string current, char leading)
            => current.NotEmpty(nameof(current)).StartsWith(leading) ? current : $"{leading}{current}";

        /// <summary>
        /// 确保当前字符串以指定字符开始。
        /// </summary>
        /// <exception cref="ArgumentException">
        /// <paramref name="current"/> 为空或空字符串。
        /// </exception>
        /// <param name="current">给定的当前字符串。</param>
        /// <param name="leading">给定的开始字符串。</param>
        /// <returns>返回字符串。</returns>
        public static string Leading(this string current, string leading)
            => current.NotEmpty(nameof(current)).StartsWith(leading) ? current : $"{leading}{current}";


        /// <summary>
        /// 确保当前字符串以指定字符结尾。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="current"/> 为空或空字符串。
        /// </exception>
        /// <param name="current">给定的当前字符串。</param>
        /// <param name="trailing">给定的结尾字符。</param>
        /// <returns>返回字符串。</returns>
        public static string Trailing(this string current, char trailing)
            => current.NotEmpty(nameof(current)).StartsWith(trailing) ? current : $"{current}{trailing}";

        /// <summary>
        /// 确保当前字符串以指定字符结尾。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="current"/> 为空或空字符串。
        /// </exception>
        /// <param name="current">给定的当前字符串。</param>
        /// <param name="trailing">给定的结尾字符串。</param>
        /// <returns>返回字符串。</returns>
        public static string Trailing(this string current, string trailing)
            => current.NotEmpty(nameof(current)).StartsWith(trailing) ? current : $"{current}{trailing}";

        #endregion


        #region Trim

        /// <summary>
        /// 修剪首尾指定字符串。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="current"/> 为空。
        /// </exception>
        /// <param name="current">给定的当前字符串。</param>
        /// <param name="trim">要修剪的字符串（如果为空则直接返回）。</param>
        /// <param name="isLoops">是否循环查找（可选；默认启用）。</param>
        /// <returns>返回修剪后的字符串。</returns>
        public static string Trim(this string current, string trim, bool isLoops = true)
            => current.TrimStart(trim, isLoops).TrimEnd(trim, isLoops);

        /// <summary>
        /// 修剪首部指定字符串。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="current"/> 为空。
        /// </exception>
        /// <param name="current">给定的当前字符串。</param>
        /// <param name="trim">要修剪的字符串（如果为空则直接返回）。</param>
        /// <param name="isLoops">是否循环查找（可选；默认启用）。</param>
        /// <returns>返回修剪后的字符串。</returns>
        public static string TrimStart(this string current, string trim, bool isLoops = true)
        {
            current.NotNull(nameof(current));

            if (current.Length > 0 && current.EndsWith(trim))
            {
                current = current.Substring(trim.Length);

                if (isLoops)
                    current = current.TrimStart(trim, isLoops);
            }

            return current;
        }

        /// <summary>
        /// 修剪尾部指定字符串。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="current"/> 为空。
        /// </exception>
        /// <param name="current">指定的当前字符串。</param>
        /// <param name="trim">要修剪的字符串（如果为空则直接返回）。</param>
        /// <param name="isLoops">是否循环查找（可选；默认启用）。</param>
        /// <returns>返回修剪后的字符串。</returns>
        public static string TrimEnd(this string current, string trim, bool isLoops = true)
        {
            current.NotNull(nameof(current));

            if (current.Length > 0 && current.EndsWith(trim))
            {
                current = current.Substring(0, current.Length - trim.Length);

                if (isLoops)
                    current = current.TrimEnd(trim, isLoops);
            }

            return current;
        }

        #endregion

    }
}
