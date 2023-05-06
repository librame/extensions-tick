﻿#region License

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
/// <see cref="DateTime"/> 静态扩展。
/// </summary>
public static class DateTimeExtensions
{
    /// <summary>
    /// 系统基础时间（默认为 <see cref="DateTime.UnixEpoch"/>）。
    /// </summary>
    public static readonly DateTime BaseTime
        = DateTime.UnixEpoch;

    /// <summary>
    /// 系统 UTC 基础时间（默认为 <see cref="DateTimeOffset.UnixEpoch"/>）。
    /// </summary>
    public static readonly DateTimeOffset UtcBaseTime
        = DateTimeOffset.UnixEpoch;

    /// <summary>
    /// 系统 UTC 与本地时区偏移量。
    /// </summary>
    public static readonly TimeSpan UtcLocalOffset
        = UtcBaseTime.LocalDateTime - UtcBaseTime.DateTime;


    /// <summary>
    /// 转为 <see cref="DateTimeOffset"/>。
    /// </summary>
    /// <param name="dateTime">给定的 <see cref="DateTime"/>。</param>
    /// <param name="useLocalOffset">使用本地时区偏移量（可选；默认不使用，即与 <see cref="DateTime"/> 相同时钟周期数）。</param>
    /// <returns>返回 <see cref="DateTimeOffset"/>。</returns>
    public static DateTimeOffset ToOffset(this DateTime dateTime, bool useLocalOffset = false)
        => new(dateTime, useLocalOffset ? UtcLocalOffset : UtcBaseTime.Offset);


    /// <summary>
    /// 转为标准字符串形式。
    /// </summary>
    /// <param name="dateTime">给定的 <see cref="DateTime"/>。</param>
    /// <param name="withMilliseconds">是否包含毫秒（可选；默认不包含）。</param>
    /// <returns>返回字符串。</returns>
    public static string AsDateTimeString(this DateTime dateTime, bool withMilliseconds = false)
    {
        var format = "yyyy-MM-dd HH:mm:ss";

        if (withMilliseconds)
            format += ".fff";

        return dateTime.ToString(format);
    }

    /// <summary>
    /// 转为标准字符串形式。
    /// </summary>
    /// <param name="dateTimeOffset">给定的 <see cref="DateTimeOffset"/>。</param>
    /// <param name="withMilliseconds">是否包含毫秒（可选；默认不包含）。</param>
    /// <returns>返回字符串。</returns>
    public static string AsDateTimeString(this DateTimeOffset dateTimeOffset, bool withMilliseconds = false)
    {
        var format = "yyyy-MM-dd HH:mm:ss.fff zzz";

        if (!withMilliseconds)
            format = format.Replace(".fff", string.Empty);
        
        return dateTimeOffset.ToString(format);
    }


    #region DateOfYear

    /// <summary>
    /// 转换为半年数。
    /// </summary>
    /// <param name="dateTime">给定的日期时间。</param>
    /// <returns>返回整数。</returns>
    public static int AsHalfYear(this DateTime dateTime)
        => dateTime.Month / 6 + (dateTime.Month % 6 > 0 ? 1 : 0);

    /// <summary>
    /// 转换为半年数。
    /// </summary>
    /// <param name="dateTime">给定的日期时间。</param>
    /// <returns>返回整数。</returns>
    public static int AsHalfYear(this DateTimeOffset dateTime)
        => dateTime.Month / 6 + (dateTime.Month % 6 > 0 ? 1 : 0);


    /// <summary>
    /// 转换为当年季度数。
    /// </summary>
    /// <param name="dateTime">给定的日期时间。</param>
    /// <returns>返回整数。</returns>
    public static int AsQuarterOfYear(this DateTime dateTime)
        => dateTime.Month / 3 + (dateTime.Month % 3 > 0 ? 1 : 0);

    /// <summary>
    /// 转换为当年季度数。
    /// </summary>
    /// <param name="dateTimeOffset">给定的日期时间。</param>
    /// <returns>返回整数。</returns>
    public static int AsQuarterOfYear(this DateTimeOffset dateTimeOffset)
        => dateTimeOffset.Month / 3 + (dateTimeOffset.Month % 3 > 0 ? 1 : 0);


    /// <summary>
    /// 转换为当年周数。
    /// </summary>
    /// <param name="dateTime">给定的日期时间。</param>
    /// <returns>返回整数。</returns>
    public static int AsWeekOfYear(this DateTime dateTime)
        => CalcWeekOfYear(dateTime.Year, dateTime.DayOfYear);

    /// <summary>
    /// 转换为当年周数。
    /// </summary>
    /// <param name="dateTimeOffset">给定的日期时间。</param>
    /// <returns>返回整数。</returns>
    public static int AsWeekOfYear(this DateTimeOffset dateTimeOffset)
        => CalcWeekOfYear(dateTimeOffset.Year, dateTimeOffset.DayOfYear);

    private static int CalcWeekOfYear(int year, int dayOfYear)
    {
        // 得到今年第一天是周几
        var dayOfWeek = DateTimeOffset.Parse(year + "-1-1", CultureInfo.CurrentCulture).DayOfWeek;
        var firstWeekend = (int)dayOfWeek;

        // 计算第一周的差额（如果是周日，则 firstWeekend 为 0，第一周也就是从周日开始）
        var weekDay = firstWeekend is 0 ? 1 : (7 - firstWeekend + 1);

        //（今天是一年当中的第几天 减去 第一周周末）/7 等于 距第一周有多少周 再加上第一周的1 就是今天是今年的第几周了
        // 刚好考虑了惟一的特殊情况就是，今天刚好在第一周内，那么距第一周就是0 再加上第一周的1 最后还是1
        return Convert.ToInt32(Math.Ceiling((dayOfYear - weekDay) / 7.0)) + 1;
    }

    #endregion

}
