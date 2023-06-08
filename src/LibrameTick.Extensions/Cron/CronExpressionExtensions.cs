#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Core;
using System;

namespace Librame.Extensions.Cron;

/// <summary>
/// 定义 <see cref="CronExpressionOffset"/> 的静态扩展。
/// </summary>
public static class CronExpressionExtensions
{
    /// <summary>
    /// 句法格式字典集合（默认键为 <see cref="CronSyntax"/>，整数值为0-6）。
    /// </summary>
    public static readonly IReadOnlyDictionary<CronSyntax, int> CronSyntaxs
        = InitialCronSyntaxs();

    /// <summary>
    /// 每周天数字典集合（默认键为枚举项名称英文前3个字符，整数值为1-7）。
    /// </summary>
    public static readonly IReadOnlyDictionary<string, int> DaysOfWeek
        = InitialDaysOfWeek();

    /// <summary>
    /// 日期月份字典集合（默认键为枚举项名称英文前3个字符，整数值为0-11）。
    /// </summary>
    public static readonly IReadOnlyDictionary<string, int> MonthsOfDate
        = InitialMonthsOfDate();


    private static Dictionary<CronSyntax, int> InitialCronSyntaxs()
        => EnumExtensions.GetEnumItems<CronSyntax, int>(static val => val.Value);

    private static Dictionary<string, int> InitialDaysOfWeek()
        => EnumExtensions.GetEnumItems<DayOfWeek, string, int>(static key => key.Name[..3].ToUpperInvariant(),
            static val => val.Value + 1);

    private static Dictionary<string, int> InitialMonthsOfDate()
        => EnumExtensions.GetEnumItems<MonthOfDate, string, int>(static key => key.Name[..3].ToUpperInvariant(),
            static val => val.Value);


    /// <summary>
    /// 获取基础时间之后的下一次触发时间。
    /// </summary>
    /// <param name="cronExpression">给定的 CRON 表达式字符串。</param>
    /// <param name="baseTime">给定的基础时间。</param>
    /// <param name="options">给定的选项（可选）。</param>
    /// <returns>返回 <see cref="Nullable{DateTime}"/>。</returns>
    public static DateTime? GetNextOccurrenceTime(this string cronExpression, DateTime baseTime,
        CronOptions? options = null)
        => cronExpression.GetNextOccurrenceTime(baseTime, out _, options);

    /// <summary>
    /// 获取基础时间之后的下一次触发时间。
    /// </summary>
    /// <param name="cronExpression">给定的 CRON 表达式字符串。</param>
    /// <param name="baseTime">给定的基础时间。</param>
    /// <param name="result">输出 <see cref="CronDescriptor"/>。</param>
    /// <param name="options">给定的选项（可选）。</param>
    /// <returns>返回 <see cref="Nullable{DateTime}"/>。</returns>
    public static DateTime? GetNextOccurrenceTime(this string cronExpression, DateTime baseTime,
        out CronDescriptor result, CronOptions? options = null)
    {
        var cron = new CronExpression(cronExpression, options ?? new());
        result = cron.Descriptor;

        return cron.GetNextOccurrenceTime(baseTime);
    }


    /// <summary>
    /// 获取基础时间之后的下一次触发时间。
    /// </summary>
    /// <param name="cronExpression">给定的 CRON 表达式字符串。</param>
    /// <param name="baseTimeOffset">给定的基础时间。</param>
    /// <param name="options">给定的选项（可选）。</param>
    /// <returns>返回 <see cref="Nullable{DateTimeOffset}"/>。</returns>
    public static DateTimeOffset? GetNextOccurrenceTime(this string cronExpression,
        DateTimeOffset baseTimeOffset, CronOptions? options = null)
        => cronExpression.GetNextOccurrenceTime(baseTimeOffset, out _, options);

    /// <summary>
    /// 获取基础时间之后的下一次触发时间。
    /// </summary>
    /// <param name="cronExpression">给定的 CRON 表达式字符串。</param>
    /// <param name="baseTimeOffset">给定的基础时间。</param>
    /// <param name="result">输出 <see cref="CronDescriptor"/>。</param>
    /// <param name="options">给定的选项（可选）。</param>
    /// <returns>返回 <see cref="Nullable{DateTimeOffset}"/>。</returns>
    public static DateTimeOffset? GetNextOccurrenceTime(this string cronExpression,
        DateTimeOffset baseTimeOffset, out CronDescriptor result, CronOptions? options = null)
    {
        var cron = new CronExpressionOffset(cronExpression, options ?? new());
        result = cron.Descriptor;

        return cron.GetNextOccurrenceTime(baseTimeOffset);
    }


    /// <summary>
    /// 获取基础时间之后的多次触发时间。
    /// </summary>
    /// <param name="cronExpression">给定的 CRON 表达式字符串。</param>
    /// <param name="baseTime">给定的基础时间。</param>
    /// <param name="count">给定的次数。</param>
    /// <param name="options">给定的选项（可选）。</param>
    /// <returns>返回 <see cref="List{DateTime}"/>。</returns>
    public static List<DateTime?> GetNextOccurrenceTimes(this string cronExpression,
        DateTime baseTime, int count, CronOptions? options = null)
        => cronExpression.GetNextOccurrenceTimes(baseTime, count, out _, options);

    /// <summary>
    /// 获取基础时间之后的多次触发时间。
    /// </summary>
    /// <param name="cronExpression">给定的 CRON 表达式字符串。</param>
    /// <param name="baseTime">给定的基础时间。</param>
    /// <param name="count">给定的次数。</param>
    /// <param name="result">输出 <see cref="CronDescriptor"/>。</param>
    /// <param name="options">给定的选项（可选）。</param>
    /// <returns>返回 <see cref="List{DateTime}"/>。</returns>
    public static List<DateTime?> GetNextOccurrenceTimes(this string cronExpression,
        DateTime baseTime, int count, out CronDescriptor result, CronOptions? options = null)
    {
        var cron = new CronExpression(cronExpression, options ?? new());
        result = cron.Descriptor;

        return cron.GetNextOccurrenceTimes(baseTime, count).ToList();
    }


    /// <summary>
    /// 获取基础时间之后的多次触发时间。
    /// </summary>
    /// <param name="cronExpression">给定的 CRON 表达式字符串。</param>
    /// <param name="baseTimeOffset">给定的基础时间。</param>
    /// <param name="count">给定的次数。</param>
    /// <param name="options">给定的选项（可选）。</param>
    /// <returns>返回 <see cref="List{DateTimeOffset}"/>。</returns>
    public static List<DateTimeOffset?> GetNextOccurrenceTimes(this string cronExpression,
        DateTimeOffset baseTimeOffset, int count, CronOptions? options = null)
        => cronExpression.GetNextOccurrenceTimes(baseTimeOffset, count, out _, options);

    /// <summary>
    /// 获取基础时间之后的多次触发时间。
    /// </summary>
    /// <param name="cronExpression">给定的 CRON 表达式字符串。</param>
    /// <param name="baseTimeOffset">给定的基础时间。</param>
    /// <param name="count">给定的次数。</param>
    /// <param name="result">输出 <see cref="CronDescriptor"/>。</param>
    /// <param name="options">给定的选项（可选）。</param>
    /// <returns>返回 <see cref="List{DateTimeOffset}"/>。</returns>
    public static List<DateTimeOffset?> GetNextOccurrenceTimes(this string cronExpression,
        DateTimeOffset baseTimeOffset, int count, out CronDescriptor result, CronOptions? options = null)
    {
        var cron = new CronExpressionOffset(cronExpression, options ?? new());
        result = cron.Descriptor;

        return cron.GetNextOccurrenceTimes(baseTimeOffset, count).ToList();
    }

}
