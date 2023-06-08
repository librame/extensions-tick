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
/// 定义 CRON 句法格式枚举。
/// </summary>
public enum CronSyntax
{
    /// <summary>
    /// 秒。
    /// </summary>
    Second = 0,

    /// <summary>
    /// 分。
    /// </summary>
    Minute = 1,

    /// <summary>
    /// 小时。
    /// </summary>
    Hour = 2,

    /// <summary>
    /// 每月的天数。
    /// </summary>
    DaysOfMonth = 3,

    /// <summary>
    /// 月份。
    /// </summary>
    Month = 4,

    /// <summary>
    /// 每周的天数。
    /// </summary>
    DaysOfWeek = 5,

    /// <summary>
    /// 年份。
    /// </summary>
    Year = 6
}
