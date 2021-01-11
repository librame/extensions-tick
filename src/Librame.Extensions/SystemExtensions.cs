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
using System.Linq;

namespace Librame.Extensions
{
    /// <summary>
    /// <see cref="System"/> 静态扩展。
    /// </summary>
    public static class SystemExtensions
    {

        /// <summary>
        /// 参数不为空。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="param"/> 为空。
        /// </exception>
        /// <typeparam name="TParam">指定的参数类型。</typeparam>
        /// <param name="param">给定的参数。</param>
        /// <param name="paramName">给定的参数名称。</param>
        /// <returns>返回参数。</returns>
        public static TParam NotNull<TParam>(this TParam? param, string paramName)
        {
            if (param is null)
                throw new ArgumentNullException(paramName);
            
            return param;
        }

        /// <summary>
        /// 参数不为空或空字符串。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="str"/> 为空或空字符串。
        /// </exception>
        /// <param name="str">给定的字符串。</param>
        /// <param name="paramName">给定的参数名称。</param>
        /// <returns>返回字符串。</returns>
        public static string NotEmpty(this string? str, string paramName)
        {
            if (string.IsNullOrEmpty(str))
                throw new ArgumentException($"'{paramName}' is null or empty.");

            return str;
        }

        /// <summary>
        /// 参数不为空、空字符串或空格字符。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="str"/> 为空、空字符串或空格字符。
        /// </exception>
        /// <param name="str">给定的字符串。</param>
        /// <param name="paramName">给定的参数名称。</param>
        /// <returns>返回字符串。</returns>
        public static string NotWhiteSpace(this string? str, string paramName)
        {
            if (string.IsNullOrWhiteSpace(str))
                throw new ArgumentException($"'{paramName}' is null or empty or white-space characters.");

            return str;
        }


        #region Digit & Letter

        /// <summary>
        /// 具有数字。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <returns>返回布尔值。</returns>
        public static bool HasDigit(this string str)
            => str.Any(IsDigit);

        /// <summary>
        /// 是数字。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsDigit(this string str)
            => str.All(IsDigit);

        /// <summary>
        /// 是数字。
        /// </summary>
        /// <param name="c">给定的字符。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsDigit(this char c)
            => c >= '0' && c <= '9';


        /// <summary>
        /// 具有小写字母。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <returns>返回布尔值。</returns>
        public static bool HasLower(this string str)
            => str.Any(IsLower);

        /// <summary>
        /// 是小写字母。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsLower(this string str)
            => str.All(IsLower);

        /// <summary>
        /// 是小写字母。
        /// </summary>
        /// <param name="c">给定的字符。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsLower(this char c)
            => c >= 'a' && c <= 'z';


        /// <summary>
        /// 具有大写字母。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <returns>返回布尔值。</returns>
        public static bool HasUpper(this string str)
            => str.Any(IsUpper);

        /// <summary>
        /// 是大写字母。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsUpper(this string str)
            => str.All(IsUpper);

        /// <summary>
        /// 是大写字母。
        /// </summary>
        /// <param name="c">给定的字符。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsUpper(this char c)
            => c >= 'A' && c <= 'Z';


        /// <summary>
        /// 具有大小写字母。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <param name="both">同时包含大小写字母（可选；默认同时包含）。</param>
        /// <returns>返回布尔值。</returns>
        public static bool HasLetter(this string str, bool both = true)
        {
            if (both)
                return str.HasLower() && str.HasUpper();

            return str.HasLower() || str.HasUpper();
        }

        /// <summary>
        /// 是大小写字母。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsLetter(this string str)
            => str.All(IsLetter);

        /// <summary>
        /// 是大小写字母。
        /// </summary>
        /// <param name="c">给定的字符。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsLetter(this char c)
            => c.IsLower() || c.IsUpper();

        #endregion


        #region String

        /// <summary>
        /// 向当前字符串末尾附加字符串。
        /// </summary>
        /// <param name="str">给定要附加的可空字符串（如果此参数值为空或 <see cref="string.Empty"/>，则直接返回 <paramref name="append"/>；如果 <paramref name="append"/> 也为空，那么将返回 <see cref="string.Empty"/>）。</param>
        /// <param name="append">给定的附加可空字符串（如果此参数值为空或 <see cref="string.Empty"/>，则直接返回 <paramref name="str"/>）。</param>
        /// <returns>返回字符串。</returns>
        public static string Append(this string? str, string? append)
        {
            if (string.IsNullOrEmpty(str))
                return append ?? string.Empty;

            if (string.IsNullOrEmpty(append))
                return str;

            return $"{str}{append}";
        }

        /// <summary>
        /// 向当前字符串的索引处插入字符串。
        /// </summary>
        /// <param name="str">给定要插入的可空字符串（如果此参数值为空或 <see cref="string.Empty"/>，则直接返回 <paramref name="insert"/>；如果 <paramref name="insert"/> 也为空，那么将返回 <see cref="string.Empty"/>）。</param>
        /// <param name="insert">给定的插入可空字符串。</param>
        /// <param name="startIndex">给定要插入的开始索引（可选；默认为 0 表示在起始处插入）。</param>
        /// <returns>返回字符串。</returns>
        public static string Insert(this string? str, string? insert, int startIndex = 0)
        {
            if (string.IsNullOrEmpty(str))
                return insert ?? string.Empty;

            if (string.IsNullOrEmpty(insert))
                return str;

            if (startIndex <= 0)
                return $"{insert}{str}";

            if (startIndex >= str.Length - 1)
                return $"{str}{insert}"; // Append

            return $"{str.Substring(0, startIndex)}{insert}{str.Substring(startIndex)}";
        }


        /// <summary>
        /// 确保当前字符串以指定字符开始。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="str"/> 为空或空字符串。
        /// </exception>
        /// <param name="str">给定的字符串。</param>
        /// <param name="leading">给定的开始字符。</param>
        /// <returns>返回字符串。</returns>
        public static string Leading(this string str, char leading)
        {
            str.NotEmpty(nameof(str));

            return str.StartsWith(leading) ? str : $"{leading}{str}";
        }

        /// <summary>
        /// 确保当前字符串以指定字符开始。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="str"/> 为空或空字符串。
        /// </exception>
        /// <param name="str">给定的字符串。</param>
        /// <param name="leading">给定的开始字符串。</param>
        /// <returns>返回字符串。</returns>
        public static string Leading(this string str, string leading)
        {
            str.NotEmpty(nameof(str));

            return str.StartsWith(leading) ? str : $"{leading}{str}";
        }

        /// <summary>
        /// 确保当前字符串以指定字符结尾。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="str"/> 为空或空字符串。
        /// </exception>
        /// <param name="str">给定的字符串。</param>
        /// <param name="trailing">给定的结尾字符。</param>
        /// <returns>返回字符串。</returns>
        public static string Trailing(this string str, char trailing)
        {
            str.NotEmpty(nameof(str));

            return str.StartsWith(trailing) ? str : $"{str}{trailing}";
        }

        /// <summary>
        /// 确保当前字符串以指定字符结尾。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="str"/> 为空或空字符串。
        /// </exception>
        /// <param name="str">给定的字符串。</param>
        /// <param name="trailing">给定的结尾字符串。</param>
        /// <returns>返回字符串。</returns>
        public static string Trailing(this string str, string trailing)
        {
            str.NotEmpty(nameof(str));

            return str.StartsWith(trailing) ? str : $"{str}{trailing}";
        }


        /// <summary>
        /// 清除首尾指定字符串。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="str"/> 为空。
        /// </exception>
        /// <param name="str">给定的字符串。</param>
        /// <param name="trim">要清除的字符串（如果为空则直接返回）。</param>
        /// <param name="loops">是否循环查找（可选；默认启用）。</param>
        /// <returns>返回清除后的字符串。</returns>
        public static string Trim(this string str, string trim, bool loops = true)
        {
            str = TrimStart(str, trim, loops);
            str = TrimEnd(str, trim, loops);

            return str;
        }

        /// <summary>
        /// 清除首部指定字符串。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="str"/> 为空。
        /// </exception>
        /// <param name="str">给定的字符串。</param>
        /// <param name="trim">要清除的字符串（如果为空则直接返回）。</param>
        /// <param name="loops">是否循环查找（可选；默认启用）。</param>
        /// <returns>返回清除后的字符串。</returns>
        public static string TrimStart(this string str, string trim, bool loops = true)
        {
            str.NotNull(nameof(str));

            if (str.Length > 0 && str.StartsWith(trim))
            {
                str = str.Substring(trim.Length);

                if (str.Length > 0 && loops)
                    str = str.TrimStart(trim, loops);
            }

            return str;
        }

        /// <summary>
        /// 清除尾部指定字符串。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="str"/> 为空。
        /// </exception>
        /// <param name="str">指定的字符串。</param>
        /// <param name="trim">要清除的字符串（如果为空则直接返回）。</param>
        /// <param name="loops">是否循环查找（可选；默认启用）。</param>
        /// <returns>返回清除后的字符串。</returns>
        public static string TrimEnd(this string str, string trim, bool loops = true)
        {
            str.NotNull(nameof(str));

            if (str.Length > 0 && str.EndsWith(trim))
            {
                str = str.Substring(0, str.Length - trim.Length);

                if (str.Length > 0 && loops)
                    str = str.TrimEnd(trim, loops);
            }

            return str;
        }

        #endregion

    }
}
