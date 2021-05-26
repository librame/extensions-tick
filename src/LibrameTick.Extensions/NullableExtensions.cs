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


        #region IsEmpty

        /// <summary>
        /// 值是否为空可枚举集合。
        /// </summary>
        /// <param name="value">给定的可枚举集合。</param>
        /// <returns>返回是否为空可枚举集合的布尔值。</returns>
        public static bool IsEmpty([NotNullWhen(false)] this IEnumerable? value)
        {
            if (value is null) return true;

            var enumerator = value.GetEnumerator();
            if (enumerator is null) return true;

            return !enumerator.MoveNext();
        }

        /// <summary>
        /// 值是否为非空可枚举集合。
        /// </summary>
        /// <param name="value">给定的可枚举集合。</param>
        /// <returns>返回是否为非空可枚举集合的布尔值。</returns>
        public static bool IsNotEmpty([NotNullWhen(false)] this IEnumerable? value)
            => !value.IsEmpty();

        #endregion


        #region IfNull

        /// <summary>
        /// 值如果为空。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="value">给定的值。</param>
        /// <param name="nullAction">值为空要执行的动作。</param>
        public static void IfNull<T>([NotNullWhen(false)] this T? value, Action nullAction)
        {
            if (value is null)
                nullAction.Invoke();
        }

        /// <summary>
        /// 值如果非空。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="value">给定的值。</param>
        /// <param name="notNullAction">值非空要执行的动作。</param>
        public static void IfNotNull<T>([NotNullWhen(false)] this T? value, Action<T> notNullAction)
        {
            if (value is not null)
                notNullAction.Invoke(value);
        }

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
        /// <param name="notNullAction">给定值为非空时的动作。</param>
        /// <returns>返回非空值。</returns>
        public static T NotNull<T>([NotNullWhen(true)] this T? value, string? paramName,
            Action<T>? notNullAction = null)
        {
            if (value is null)
                throw new ArgumentNullException(paramName);

            notNullAction?.Invoke(value);

            return value;
        }

        /// <summary>
        /// 值为非空。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> or <paramref name="notNullFunc"/> 为空。
        /// </exception>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="value">给定的值。</param>
        /// <param name="paramName">给定的参数名称。</param>
        /// <param name="notNullFunc">给定值为非空时的函数。</param>
        /// <returns>返回非空值。</returns>
        public static T NotNull<T>([NotNullWhen(true)] this T? value, string? paramName,
            Func<T, T> notNullFunc)
        {
            if (value is null)
                throw new ArgumentNullException(paramName);

            if (notNullFunc is null)
                throw new ArgumentNullException(nameof(notNullFunc));

            return notNullFunc.Invoke(value);
        }

        /// <summary>
        /// 值为非空。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> or <paramref name="notNullFunc"/> 为空。
        /// </exception>
        /// <typeparam name="TInput">指定的输入类型。</typeparam>
        /// <typeparam name="TOutput">指定的输出类型。</typeparam>
        /// <param name="value">给定的输入值。</param>
        /// <param name="paramName">给定的参数名称。</param>
        /// <param name="notNullFunc">给定值为非空时的函数。</param>
        /// <returns>返回输出。</returns>
        public static TOutput NotNull<TInput, TOutput>([NotNullWhen(true)] this TInput? value, string? paramName,
            Func<TInput, TOutput> notNullFunc)
        {
            if (value is null)
                throw new ArgumentNullException(paramName);

            if (notNullFunc is null)
                throw new ArgumentNullException(nameof(notNullFunc));

            return notNullFunc.Invoke(value);
        }


        /// <summary>
        /// 值为非空可枚举集合。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> 为空或空可枚举集合。
        /// </exception>
        /// <param name="value">给定的可枚举集合。</param>
        /// <param name="paramName">给定的参数名称。</param>
        /// <param name="notNullAction">给定值为非空时的动作。</param>
        /// <returns>返回非空可枚举集合。</returns>
        public static IEnumerable NotEmpty([NotNullWhen(true)] this IEnumerable? value, string? paramName,
            Action<IEnumerable>? notNullAction = null)
        {
            if (value.IsEmpty())
                throw new ArgumentException($"'{paramName ?? nameof(value)}' is null or empty.");

            notNullAction?.Invoke(value);

            return value;
        }

        /// <summary>
        /// 值为非空可枚举集合。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> 为空或空可枚举集合。
        /// </exception>
        /// <param name="value">给定的可枚举集合。</param>
        /// <param name="paramName">给定的参数名称。</param>
        /// <param name="notNullFunc">给定值为非空时的函数。</param>
        /// <returns>返回非空可枚举集合。</returns>
        public static IEnumerable NotEmpty([NotNullWhen(true)] this IEnumerable? value, string? paramName,
            Func<IEnumerable, IEnumerable>? notNullFunc)
        {
            if (value.IsEmpty())
                throw new ArgumentException($"'{paramName ?? nameof(value)}' is null or empty.");

            if (notNullFunc is null)
                throw new ArgumentNullException(nameof(notNullFunc));

            return notNullFunc.Invoke(value);
        }

        /// <summary>
        /// 值为非空可枚举集合。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> 为空或空可枚举集合。
        /// </exception>
        /// <typeparam name="TResult">指定的结果类型。</typeparam>
        /// <param name="value">给定的可枚举集合。</param>
        /// <param name="paramName">给定的参数名称。</param>
        /// <param name="notNullFunc">给定值为非空时的函数。</param>
        /// <returns>返回结果。</returns>
        public static TResult NotEmpty<TResult>([NotNullWhen(true)] this IEnumerable? value, string? paramName,
            Func<IEnumerable, TResult>? notNullFunc)
        {
            if (value.IsEmpty())
                throw new ArgumentException($"'{paramName ?? nameof(value)}' is null or empty.");

            if (notNullFunc is null)
                throw new ArgumentNullException(nameof(notNullFunc));

            return notNullFunc.Invoke(value);
        }


        /// <summary>
        /// 值为非空可枚举集合。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> 为空或空可枚举集合。
        /// </exception>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="value">给定的可枚举集合。</param>
        /// <param name="paramName">给定的参数名称。</param>
        /// <param name="notNullAction">给定值为非空时的动作。</param>
        /// <returns>返回非空可枚举集合。</returns>
        public static IEnumerable<T> NotEmpty<T>([NotNullWhen(true)] this IEnumerable<T>? value, string? paramName,
            Action<IEnumerable>? notNullAction = null)
        {
            if (value.IsEmpty())
                throw new ArgumentException($"'{paramName ?? nameof(value)}' is null or empty.");

            notNullAction?.Invoke(value);

            return value;
        }

        /// <summary>
        /// 值为非空可枚举集合。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> 为空或空可枚举集合。
        /// </exception>
        /// <param name="value">给定的可枚举集合。</param>
        /// <param name="paramName">给定的参数名称。</param>
        /// <param name="notNullFunc">给定值为非空时的函数。</param>
        /// <returns>返回非空可枚举集合。</returns>
        public static IEnumerable<T> NotEmpty<T>([NotNullWhen(true)] this IEnumerable<T>? value, string? paramName,
            Func<IEnumerable<T>, IEnumerable<T>>? notNullFunc)
        {
            if (value.IsEmpty())
                throw new ArgumentException($"'{paramName ?? nameof(value)}' is null or empty.");

            if (notNullFunc is null)
                throw new ArgumentNullException(nameof(notNullFunc));

            return notNullFunc.Invoke(value);
        }

        /// <summary>
        /// 值为非空可枚举集合。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> 为空或空可枚举集合。
        /// </exception>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <typeparam name="TResult">指定的结果类型。</typeparam>
        /// <param name="value">给定的可枚举集合。</param>
        /// <param name="paramName">给定的参数名称。</param>
        /// <param name="notNullFunc">给定值为非空时的函数。</param>
        /// <returns>返回结果。</returns>
        public static TResult NotEmpty<T, TResult>([NotNullWhen(true)] this IEnumerable<T>? value, string? paramName,
            Func<IEnumerable<T>, TResult>? notNullFunc)
        {
            if (value.IsEmpty())
                throw new ArgumentException($"'{paramName ?? nameof(value)}' is null or empty.");

            if (notNullFunc is null)
                throw new ArgumentNullException(nameof(notNullFunc));

            return notNullFunc.Invoke(value);
        }


        /// <summary>
        /// 值为非空字符串。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> 为空或空字符串。
        /// </exception>
        /// <param name="value">给定的字符串。</param>
        /// <param name="paramName">给定的参数名称。</param>
        /// <param name="notNullAction">给定值为非空时的动作。</param>
        /// <returns>返回字符串。</returns>
        public static string NotEmpty([NotNullWhen(true)] this string? value, string? paramName,
            Action<string>? notNullAction = null)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException($"'{paramName ?? nameof(value)}' is null or empty.");

            notNullAction?.Invoke(value);

            return value;
        }

        /// <summary>
        /// 值为非空字符串。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> 为空或空字符串。
        /// </exception>
        /// <param name="value">给定的字符串。</param>
        /// <param name="paramName">给定的参数名称。</param>
        /// <param name="notNullFunc">给定值为非空时的函数。</param>
        /// <returns>返回字符串。</returns>
        public static string NotEmpty([NotNullWhen(true)] this string? value, string? paramName,
            Func<string, string>? notNullFunc)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException($"'{paramName ?? nameof(value)}' is null or empty.");

            if (notNullFunc is null)
                throw new ArgumentNullException(nameof(notNullFunc));

            return notNullFunc.Invoke(value);
        }

        /// <summary>
        /// 值为非空字符串。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> 为空或空字符串。
        /// </exception>
        /// <typeparam name="TResult">指定的结果类型。</typeparam>
        /// <param name="value">给定的字符串。</param>
        /// <param name="paramName">给定的参数名称。</param>
        /// <param name="notNullFunc">给定值为非空时的函数。</param>
        /// <returns>返回结果。</returns>
        public static TResult NotEmpty<TResult>([NotNullWhen(true)] this string? value, string? paramName,
            Func<string, TResult>? notNullFunc)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException($"'{paramName ?? nameof(value)}' is null or empty.");

            if (notNullFunc is null)
                throw new ArgumentNullException(nameof(notNullFunc));

            return notNullFunc.Invoke(value);
        }


        /// <summary>
        /// 值为非空格或非空字符串。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> 为空、空字符串或空格字符。
        /// </exception>
        /// <param name="value">给定的字符串。</param>
        /// <param name="paramName">给定的参数名称。</param>
        /// <param name="notNullAction">给定值为非空时的动作。</param>
        /// <returns>返回字符串。</returns>
        public static string NotWhiteSpace([NotNullWhen(true)] this string? value, string? paramName,
            Action<string>? notNullAction = null)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException($"'{paramName ?? nameof(value)}' is null or empty or white-space characters.");

            notNullAction?.Invoke(value);

            return value;
        }

        /// <summary>
        /// 值为非空格或非空字符串。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> 为空、空字符串或空格字符。
        /// </exception>
        /// <param name="value">给定的字符串。</param>
        /// <param name="paramName">给定的参数名称。</param>
        /// <param name="notNullFunc">给定值为非空时的函数。</param>
        /// <returns>返回字符串。</returns>
        public static string NotWhiteSpace([NotNullWhen(true)] this string? value, string? paramName,
            Func<string, string>? notNullFunc)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException($"'{paramName ?? nameof(value)}' is null or empty or white-space characters.");

            if (notNullFunc is null)
                throw new ArgumentNullException(nameof(notNullFunc));

            return notNullFunc.Invoke(value);
        }

        /// <summary>
        /// 值为非空格或非空字符串。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> 为空、空字符串或空格字符。
        /// </exception>
        /// <typeparam name="TResult">指定的结果类型。</typeparam>
        /// <param name="value">给定的字符串。</param>
        /// <param name="paramName">给定的参数名称。</param>
        /// <param name="notNullFunc">给定值为非空时的函数。</param>
        /// <returns>返回结果。</returns>
        public static TResult NotWhiteSpace<TResult>([NotNullWhen(true)] this string? value, string? paramName,
            Func<string, TResult>? notNullFunc)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException($"'{paramName ?? nameof(value)}' is null or empty or white-space characters.");

            if (notNullFunc is null)
                throw new ArgumentNullException(nameof(notNullFunc));

            return notNullFunc.Invoke(value);
        }

        #endregion

    }
}
