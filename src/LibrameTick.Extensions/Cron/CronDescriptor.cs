#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Cron;

/// <summary>
/// 定义 CRON 描述符。
/// </summary>
/// <param name="expression">给定的 CRON 表达式。</param>
public class CronDescriptor(string expression)
{
    /// <summary>
    /// 当前表达式。
    /// </summary>
    public string Expression { get; init; } = expression;

    /// <summary>
    /// 经过全大写处理的表达式。
    /// </summary>
    public string UpperExpression => Expression.ToUpperInvariant();

    /// <summary>
    /// 当前表达式描述。
    /// </summary>
    public string? Description { get; set; }


    /// <summary>
    /// 最后一天偏移。
    /// </summary>
    public int LastdayOffset { get; set; }

    /// <summary>
    /// 一月的最后一天。
    /// </summary>
    public bool LastdayOfMonth { get; set; }

    /// <summary>
    /// 一周的最后一天。
    /// </summary>
    public bool LastdayOfWeek { get; set; }

    /// <summary>
    /// 每第 N 周。
    /// </summary>
    public int EveryNthWeek { get; set; }

    /// <summary>
    /// 一周的第 N 天。
    /// </summary>
    public int NthdayOfWeek { get; set; }

    /// <summary>
    /// 最近的工作日。
    /// </summary>
    public bool NearestWeekday { get; set; }


    /// <summary>
    /// 秒集合。
    /// </summary>
    public SortedSet<int> Seconds { get; init; } = new();

    /// <summary>
    /// 分集合。
    /// </summary>
    public SortedSet<int> Minutes { get; init; } = new();

    /// <summary>
    /// 时集合。
    /// </summary>
    public SortedSet<int> Hours { get; init; } = new();

    /// <summary>
    /// 每月天数集合。
    /// </summary>
    public SortedSet<int> DaysOfMonth { get; init; } = new();

    /// <summary>
    /// 月集合。
    /// </summary>
    public SortedSet<int> Months { get; init; } = new();

    /// <summary>
    /// 每周星期集合。
    /// </summary>
    public SortedSet<int> DaysOfWeek { get; init; } = new();

    /// <summary>
    /// 年集合。
    /// </summary>
    public SortedSet<int> Years { get; init; } = new();
}
