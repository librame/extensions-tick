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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Librame.Extensions
{
    /// <summary>
    /// <see cref="Nullable"/> 静态扩展。
    /// </summary>
    public static class NullableExtensions
    {

        /// <summary>
        /// 解开可空结构体的值。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="value">给定的可空值。</param>
        /// <param name="defaultValue">当可空类型为空时，要返回的默认值（可选）。</param>
        /// <returns>返回值。</returns>
        public static T Unwrap<T>([NotNullWhen(true)] this T? value, T defaultValue = default)
            where T : struct
            => value.HasValue ? value.Value : defaultValue;


        #region IsNull and IsEmpty

        /// <summary>
        /// 值是否为空。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="value">给定的可空值。</param>
        /// <returns>返回是否为空的布尔值。</returns>
        public static bool IsNull<T>([NotNullWhen(false)] this T? value)
            => value is null;

        /// <summary>
        /// 值是否非空。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="value">给定的可空值。</param>
        /// <returns>返回是否非空的布尔值。</returns>
        public static bool IsNotNull<T>([NotNullWhen(true)] this T? value)
            => value is not null;


        /// <summary>
        /// 值是否为空可枚举集合。
        /// </summary>
        /// <param name="value">给定的可枚举集合。</param>
        /// <returns>返回是否为空可枚举集合的布尔值。</returns>
        public static bool IsEmpty([NotNullWhen(false)] this IEnumerable? value)
        {
            if (value is null)
                return true;
            
            var enumerator = value.GetEnumerator();
            if (enumerator is null)
                return true;

            return !enumerator.MoveNext();
        }

        /// <summary>
        /// 值是否为非空可枚举集合。
        /// </summary>
        /// <param name="value">给定的可枚举集合。</param>
        /// <returns>返回是否为非空可枚举集合的布尔值。</returns>
        public static bool IsNotEmpty([NotNullWhen(true)] this IEnumerable? value)
            => !value.IsEmpty(); // NotNullWhen(true) 才能表示没有可能为空的情况


        /// <summary>
        /// 值是否为空集合。
        /// </summary>
        /// <param name="value">给定的可枚举集合。</param>
        /// <returns>返回是否为空集合的布尔值。</returns>
        public static bool IsEmpty<T>([NotNullWhen(false)] this ICollection<T>? value)
            => value is null || value.Count < 1;

        /// <summary>
        /// 值是否为非空集合。
        /// </summary>
        /// <param name="value">给定的可枚举集合。</param>
        /// <returns>返回是否为非空集合的布尔值。</returns>
        public static bool IsNotEmpty<T>([NotNullWhen(true)] this ICollection<T>? value)
            => value is not null && value.Count > 0; // NotNullWhen(true) 才能表示没有可能为空的情况


        /// <summary>
        /// 字符串是否为空字符串。
        /// </summary>
        /// <param name="value">给定的字符串。</param>
        /// <returns>返回是否为空字符串的布尔值。</returns>
        public static bool IsEmpty([NotNullWhen(false)] this string? value)
            => string.IsNullOrEmpty(value);

        /// <summary>
        /// 字符串是否非空字符串。
        /// </summary>
        /// <param name="value">给定的字符串。</param>
        /// <returns>返回是否为非空字符串的布尔值。</returns>
        public static bool IsNotEmpty([NotNullWhen(true)] this string? value)
            => !string.IsNullOrEmpty(value); // NotNullWhen(true) 才能表示没有可能为空的情况

        #endregion


        #region NotNull and NotEmpty

        /// <summary>
        /// 值为非空。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> 为空。
        /// </exception>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="value">给定的值。</param>
        /// <param name="paramName">给定的参数名称。</param>
        /// <returns>返回非空 <typeparamref name="T"/>。</returns>
        public static T NotNull<T>([NotNull] this T? value, string? paramName)
        {
            if (value is null)
                throw new ArgumentNullException(paramName);

            return value;
        }


        /// <summary>
        /// 值为非空可枚举集合。
        /// </summary>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> 为空或空可枚举集合。
        /// </exception>
        /// <typeparam name="TEnumerable">指定的可枚举集合类型。</typeparam>
        /// <param name="value">给定的可枚举集合。</param>
        /// <param name="paramName">给定的参数名称。</param>
        /// <returns>返回非空 <typeparamref name="TEnumerable"/>。</returns>
        public static TEnumerable NotEmpty<TEnumerable>([NotNull] this TEnumerable? value, string? paramName)
            where TEnumerable : IEnumerable
        {
            if (value.IsEmpty())
                throw new ArgumentException($"'{paramName ?? nameof(value)}' is null or empty.");

            return value;
        }

        /// <summary>
        /// 值为非空可枚举集合。
        /// </summary>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> 为空或空可枚举集合。
        /// </exception>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="value">给定的可枚举集合。</param>
        /// <param name="paramName">给定的参数名称。</param>
        /// <returns>返回非空 <see cref="ICollection{T}"/>。</returns>
        public static ICollection<T> NotEmpty<T>([NotNull] this ICollection<T>? value, string? paramName)
        {
            if (value.IsEmpty())
                throw new ArgumentException($"'{paramName ?? nameof(value)}' is null or empty.");

            return value;
        }

        /// <summary>
        /// 值为非空字符串。
        /// </summary>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> 为空或空字符串。
        /// </exception>
        /// <param name="value">给定的字符串。</param>
        /// <param name="paramName">给定的参数名称。</param>
        /// <returns>返回字符串。</returns>
        public static string NotEmpty([NotNull] this string? value, string? paramName)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException($"'{paramName ?? nameof(value)}' is null or empty.");

            return value;
        }


        /// <summary>
        /// 值为非空格或非空字符串。
        /// </summary>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> 为空、空字符串或空格字符。
        /// </exception>
        /// <param name="value">给定的字符串。</param>
        /// <param name="paramName">给定的参数名称。</param>
        /// <returns>返回字符串。</returns>
        public static string NotWhiteSpace([NotNull] this string? value, string? paramName)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException($"'{paramName ?? nameof(value)}' is null or empty or white-space characters.");

            return value;
        }

        #endregion

    }
}
