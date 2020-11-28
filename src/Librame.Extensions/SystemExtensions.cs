#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions
{
    /// <summary>
    /// <see cref="System"/> 静态扩展。
    /// </summary>
    public static class SystemExtensions
    {

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

    }
}
