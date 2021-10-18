#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions;

namespace Microsoft.Extensions.Localization;

/// <summary>
/// <see cref="IStringLocalizer"/> 静态扩展。
/// </summary>
public static class StringLocalizerExtensions
{

    /// <summary>
    /// 获取具有给定名称的字符串资源。
    /// </summary>
    /// <param name="localizer">给定的 <see cref="IStringLocalizer{T}"/>。</param>
    /// <param name="nameExpression">给定的资源名称表达式。</param>
    /// <returns>返回 <see cref="LocalizedString"/>。</returns>
    public static LocalizedString GetString<T>(this IStringLocalizer<T> localizer,
        Expression<Func<T, string>> nameExpression)
        => localizer[nameExpression.AsPropertyName()];

    /// <summary>
    /// 获取具有给定名称的字符串资源。
    /// </summary>
    /// <param name="localizer">给定的 <see cref="IStringLocalizer{T}"/>。</param>
    /// <param name="nameExpression">给定的资源名称表达式。</param>
    /// <param name="arguments">给定的格式化参数列表。</param>
    /// <returns>返回 <see cref="LocalizedString"/>。</returns>
    public static LocalizedString GetString<T>(this IStringLocalizer<T> localizer,
        Expression<Func<T, string>> nameExpression, params object[] arguments)
        => localizer[nameExpression.AsPropertyName(), arguments];

}
