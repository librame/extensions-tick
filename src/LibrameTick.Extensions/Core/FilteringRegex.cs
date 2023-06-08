#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core;

/// <summary>
/// 定义一个支持筛选模式的正则表达式。
/// </summary>
public class FilteringRegex : Regex
{
    /// <summary>
    /// 构造一个 <see cref="FilteringRegex"/>。
    /// </summary>
    /// <param name="pattern">给定的模式字符串。</param>
    /// <param name="filtering">给定的 <see cref="FilteringMode"/>。</param>
    public FilteringRegex(string pattern, FilteringMode filtering)
        : base(pattern)
    {
        Filtering = filtering;
    }

    /// <summary>
    /// 构造一个 <see cref="FilteringRegex"/>。
    /// </summary>
    /// <param name="pattern">给定的模式字符串。</param>
    /// <param name="filtering">给定的 <see cref="FilteringMode"/>。</param>
    /// <param name="options">给定的 <see cref="RegexOptions"/>。</param>
    public FilteringRegex(string pattern, FilteringMode filtering, RegexOptions options)
        : base(pattern, options)
    {
        Filtering = filtering;
    }

    /// <summary>
    /// 构造一个 <see cref="FilteringRegex"/>。
    /// </summary>
    /// <param name="pattern">给定的模式字符串。</param>
    /// <param name="filtering">给定的 <see cref="FilteringMode"/>。</param>
    /// <param name="options">给定的 <see cref="RegexOptions"/>。</param>
    /// <param name="matchTimeout">给定的 <see cref="TimeSpan"/>。</param>
    public FilteringRegex(string pattern, FilteringMode filtering, RegexOptions options, TimeSpan matchTimeout)
        : base(pattern, options, matchTimeout)
    {
        Filtering = filtering;
    }


    /// <summary>
    /// 筛选模式。
    /// </summary>
    public FilteringMode Filtering { get; init; }


    /// <summary>
    /// 筛选指定的字符串集合。
    /// </summary>
    /// <param name="inputs">给定的输入字符串集合。</param>
    /// <returns>返回筛选后的字符串集合。</returns>
    public IEnumerable<string> FilterBy(IEnumerable<string> inputs)
    {
        return Filtering switch
        {
            FilteringMode.Exclusive => inputs.Where(v => !IsMatch(v)),

            FilteringMode.Inclusive => inputs.Where(v => IsMatch(v)),

            _ => inputs
        };
    }

    /// <summary>
    /// 筛选指定的值集合。
    /// </summary>
    /// <typeparam name="TValue">指定的值类型。</typeparam>
    /// <param name="values">给定的值集合。</param>
    /// <param name="inputSelector">给定用于匹配的输入字符串选择器。</param>
    /// <returns>返回筛选后的值集合。</returns>
    public IEnumerable<TValue> FilterBy<TValue>(IEnumerable<TValue> values, Func<TValue, string> inputSelector)
    {
        return Filtering switch
        {
            FilteringMode.Exclusive => values.Where(v => !IsMatch(inputSelector(v))),

            FilteringMode.Inclusive => values.Where(v => IsMatch(inputSelector(v))),

            _ => values
        };
    }

}
