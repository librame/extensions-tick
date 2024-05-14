#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Infrastructure.Filtration;

/// <summary>
/// 定义支持筛选模式的正则表达式。
/// </summary>
public class FiltrationRegex : Regex
{
    /// <summary>
    /// 构造一个 <see cref="FiltrationRegex"/>。
    /// </summary>
    /// <param name="pattern">给定的模式字符串。</param>
    /// <param name="filtering">给定的 <see cref="FiltrationMode"/>。</param>
    public FiltrationRegex(string pattern, FiltrationMode filtering)
        : base(pattern)
    {
        Filtering = filtering;
    }

    /// <summary>
    /// 构造一个 <see cref="FiltrationRegex"/>。
    /// </summary>
    /// <param name="pattern">给定的模式字符串。</param>
    /// <param name="filtering">给定的 <see cref="FiltrationMode"/>。</param>
    /// <param name="options">给定的 <see cref="RegexOptions"/>。</param>
    public FiltrationRegex(string pattern, FiltrationMode filtering, RegexOptions options)
        : base(pattern, options)
    {
        Filtering = filtering;
    }

    /// <summary>
    /// 构造一个 <see cref="FiltrationRegex"/>。
    /// </summary>
    /// <param name="pattern">给定的模式字符串。</param>
    /// <param name="filtering">给定的 <see cref="FiltrationMode"/>。</param>
    /// <param name="options">给定的 <see cref="RegexOptions"/>。</param>
    /// <param name="matchTimeout">给定的 <see cref="TimeSpan"/>。</param>
    public FiltrationRegex(string pattern, FiltrationMode filtering, RegexOptions options, TimeSpan matchTimeout)
        : base(pattern, options, matchTimeout)
    {
        Filtering = filtering;
    }


    /// <summary>
    /// 筛选模式。
    /// </summary>
    public FiltrationMode Filtering { get; init; }


    /// <summary>
    /// 筛选指定的字符串集合。
    /// </summary>
    /// <param name="inputs">给定的输入字符串集合。</param>
    /// <returns>返回筛选后的字符串集合。</returns>
    public IEnumerable<string> FilterBy(IEnumerable<string> inputs)
    {
        return Filtering switch
        {
            FiltrationMode.Exclusive => inputs.Where(v => !IsMatch(v)),

            FiltrationMode.Inclusive => inputs.Where(v => IsMatch(v)),

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
            FiltrationMode.Exclusive => values.Where(v => !IsMatch(inputSelector(v))),

            FiltrationMode.Inclusive => values.Where(v => IsMatch(inputSelector(v))),

            _ => values
        };
    }

}
