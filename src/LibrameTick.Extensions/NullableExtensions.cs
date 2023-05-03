#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions;

/// <summary>
/// <see cref="Nullable"/> 静态扩展。
/// </summary>
public static class NullableExtensions
{

    /// <summary>
    /// 解开或默认可空结构体的值。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    /// <param name="value">给定的可空值。</param>
    /// <param name="defaultValue">当可空类型为空时，要返回的默认值（可选）。</param>
    /// <returns>返回值。</returns>
    public static T UnwrapOrDefault<T>([NotNullWhen(true)] this T? value, T defaultValue = default)
        where T : struct
        => value.HasValue ? value.Value : defaultValue;


    #region NotNull

    /// <summary>
    /// 值为非空。
    /// </summary>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value"/> 为空。
    /// </exception>
    /// <typeparam name="T">指定的类型。</typeparam>
    /// <param name="value">给定的值。</param>
    /// <param name="paramName">给定的参数名（可选；默认为 <paramref name="value"/> 调用参数名）。</param>
    /// <returns>返回非空 <typeparamref name="T"/>。</returns>
    public static T NotNull<T>([NotNull] this T? value,
        [CallerArgumentExpression("value")] string? paramName = null)
        => value is null ? throw new ArgumentNullException(paramName) : value;


    /// <summary>
    /// 值为非空可枚举集合。
    /// </summary>
    /// <exception cref="ArgumentException">
    /// <paramref name="value"/> 为空或空可枚举集合。
    /// </exception>
    /// <typeparam name="T">指定的类型。</typeparam>
    /// <param name="value">给定的可枚举集合。</param>
    /// <param name="paramName">给定的参数名（可选；默认为 <paramref name="value"/> 调用参数名）。</param>
    /// <returns>返回非空 <see cref="IEnumerable{T}"/>。</returns>
    public static IEnumerable<T> NotEmpty<T>([NotNull] this IEnumerable<T>? value,
        [CallerArgumentExpression("value")] string? paramName = null)
        => value is null || !value.Any() ? throw new ArgumentException($"'{paramName}' is null or empty.") : value;

    /// <summary>
    /// 值为非空字符串。
    /// </summary>
    /// <exception cref="ArgumentException">
    /// <paramref name="value"/> 为空或空字符串。
    /// </exception>
    /// <param name="value">给定的字符串。</param>
    /// <param name="paramName">给定的参数名（可选；默认为 <paramref name="value"/> 调用参数名）。</param>
    /// <returns>返回字符串。</returns>
    public static string NotEmpty([NotNull] this string? value,
        [CallerArgumentExpression("value")] string? paramName = null)
        => string.IsNullOrEmpty(value) ? throw new ArgumentException($"'{paramName}' is null or empty.") : value;


    /// <summary>
    /// 值为非空格或非空字符串。
    /// </summary>
    /// <exception cref="ArgumentException">
    /// <paramref name="value"/> 为空、空字符串或空格字符。
    /// </exception>
    /// <param name="value">给定的字符串。</param>
    /// <param name="paramName">给定的参数名（可选；默认为 <paramref name="value"/> 调用参数名）。</param>
    /// <returns>返回字符串。</returns>
    public static string NotWhiteSpace([NotNull] this string? value,
        [CallerArgumentExpression("value")] string? paramName = null)
        => string.IsNullOrWhiteSpace(value)
            ? throw new ArgumentException($"'{paramName}' is null or empty or white-space characters.")
            : value;

    #endregion

}
