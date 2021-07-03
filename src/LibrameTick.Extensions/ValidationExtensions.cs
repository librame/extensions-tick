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
using System.Linq;

namespace Librame.Extensions
{
    /// <summary>
    /// 验证静态扩展。
    /// </summary>
    public static class ValidationExtensions
    {

        /// <summary>
        /// 是否为倍数。
        /// </summary>
        /// <param name="value">给定的数字。</param>
        /// <param name="multiples">给定的倍数。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsMultiples(this int value, int multiples)
            => 0 == value % multiples;


        #region Compare

        /// <summary>
        /// 是否大于或大于等于对比值。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="value">给定的值。</param>
        /// <param name="compare">给定的比较值。</param>
        /// <param name="equals">是否比较等于（可选；默认不比较）。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsGreater<T>([NotNullWhen(false)] this T? value, T compare, bool equals = false)
            where T : IComparable<T>
        {
            value.NotNull(nameof(value));

            return equals ? value.CompareTo(compare) >= 0 : value.CompareTo(compare) > 0;
        }

        /// <summary>
        /// 是否小于或小于等于对比值。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="value">给定的值。</param>
        /// <param name="compare">给定的比较值。</param>
        /// <param name="equals">是否比较等于（可选；默认不比较）。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsLesser<T>([NotNullWhen(false)] this T? value, T compare, bool equals = false)
            where T : IComparable<T>
        {
            value.NotNull(nameof(value));

            return equals ? value.CompareTo(compare) <= 0 : value.CompareTo(compare) < 0;
        }


        /// <summary>
        /// 是否不超出范围对比值。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="value">给定的值。</param>
        /// <param name="compareMinimum">给定的最小比较值。</param>
        /// <param name="compareMaximum">给定的最大比较值。</param>
        /// <param name="equalMinimum">是否比较等于最小值（可选；默认不比较）。</param>
        /// <param name="equalMaximum">是否比较等于最大值（可选；默认不比较）。</param>
        public static bool IsNotOutOfRange<T>([NotNullWhen(false)] this T? value, T compareMinimum, T compareMaximum,
            bool equalMinimum = false, bool equalMaximum = false)
            where T : IComparable<T>
            => !value.IsOutOfRange(compareMinimum, compareMaximum, equalMinimum, equalMaximum);

        /// <summary>
        /// 是否超出范围对比值。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="value">给定的值。</param>
        /// <param name="compareMinimum">给定的最小比较值。</param>
        /// <param name="compareMaximum">给定的最大比较值。</param>
        /// <param name="equalMinimum">是否比较等于最小值（可选；默认不比较）。</param>
        /// <param name="equalMaximum">是否比较等于最大值（可选；默认不比较）。</param>
        public static bool IsOutOfRange<T>([NotNullWhen(false)] this T? value, T compareMinimum, T compareMaximum,
            bool equalMinimum = false, bool equalMaximum = false)
            where T : IComparable<T>
        {
            if (value.IsLesser(compareMinimum, equalMinimum))
                return true;

            return value.IsGreater(compareMaximum, equalMaximum);
        }

        #endregion


        #region Digit and Letter

        /// <summary>
        /// 具有数字。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> is null.
        /// </exception>
        /// <param name="value">给定的字符串。</param>
        /// <returns>返回布尔值。</returns>
        public static bool HasDigit(this string value)
            => value.Any(IsDigit);

        /// <summary>
        /// 是数字。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> is null.
        /// </exception>
        /// <param name="value">给定的字符串。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsDigit(this string value)
            => value.All(IsDigit);

        /// <summary>
        /// 是数字。
        /// </summary>
        /// <param name="value">给定的字符。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsDigit(this char value)
            => value >= '0' && value <= '9';


        /// <summary>
        /// 具有小写字母。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> is null.
        /// </exception>
        /// <param name="value">给定的字符串。</param>
        /// <returns>返回布尔值。</returns>
        public static bool HasLower(this string value)
            => value.Any(IsLower);

        /// <summary>
        /// 是小写字母。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> is null.
        /// </exception>
        /// <param name="value">给定的字符串。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsLower(this string value)
            => value.All(IsLower);

        /// <summary>
        /// 是小写字母。
        /// </summary>
        /// <param name="value">给定的字符。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsLower(this char value)
            => value >= 'a' && value <= 'z';


        /// <summary>
        /// 具有大写字母。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> is null.
        /// </exception>
        /// <param name="value">给定的字符串。</param>
        /// <returns>返回布尔值。</returns>
        public static bool HasUpper(this string value)
            => value.Any(IsUpper);

        /// <summary>
        /// 是大写字母。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> is null.
        /// </exception>
        /// <param name="value">给定的字符串。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsUpper(this string value)
            => value.All(IsUpper);

        /// <summary>
        /// 是大写字母。
        /// </summary>
        /// <param name="value">给定的字符。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsUpper(this char value)
            => value >= 'A' && value <= 'Z';


        /// <summary>
        /// 具有大小写字母。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> is null.
        /// </exception>
        /// <param name="value">给定的字符串。</param>
        /// <param name="both">同时包含大小写字母（可选；默认同时包含）。</param>
        /// <returns>返回布尔值。</returns>
        public static bool HasLetter(this string value, bool both = true)
        {
            if (both)
                return value.HasLower() && value.HasUpper();

            return value.HasLower() || value.HasUpper();
        }

        /// <summary>
        /// 是大小写字母。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> is null.
        /// </exception>
        /// <param name="value">给定的字符串。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsLetter(this string value)
            => value.All(IsLetter);

        /// <summary>
        /// 是大小写字母。
        /// </summary>
        /// <param name="value">给定的字符。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsLetter(this char value)
            => value.IsLower() || value.IsUpper();

        #endregion

    }
}
