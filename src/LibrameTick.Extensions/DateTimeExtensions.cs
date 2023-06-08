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
/// <see cref="DateTime"/> 静态扩展。
/// </summary>
public static class DateTimeExtensions
{
    /// <summary>
    /// 基础时间（默认为 <see cref="DateTime.UnixEpoch"/>，通常为 1970-01-01 00:00:00）。
    /// </summary>
    public static readonly DateTime BaseTime
        = DateTime.UnixEpoch;

    /// <summary>
    /// 基础时间偏移（默认为 <see cref="DateTimeOffset.UnixEpoch"/>，通常为 1970-01-01 00:00:00 +00:00）。
    /// </summary>
    public static readonly DateTimeOffset BaseTimeOffset
        = DateTimeOffset.UnixEpoch;

    /// <summary>
    /// 本地时区（默认为 <see cref="TimeZoneInfo.Local"/>，通常为 +08:00 北京,上海时区）。
    /// </summary>
    public static readonly TimeZoneInfo LocalTimeZone
        = TimeZoneInfo.Local;

    /// <summary>
    /// 本地时间 UTC 偏移（默认为 <see cref="LocalTimeZone"/> 包含的偏移，通常为 +08:00）。
    /// </summary>
    public static readonly TimeSpan LocalOffset
        = LocalTimeZone.BaseUtcOffset;

    /// <summary>
    /// 基础时间 UTC 偏移（默认为 <see cref="BaseTimeOffset"/> 包含的偏移，通常为 +00:00）。
    /// </summary>
    public static readonly TimeSpan BaseOffset
        = BaseTimeOffset.Offset;


    /// <summary>
    /// 转为 <see cref="DateTimeOffset"/>。
    /// </summary>
    /// <param name="dateTime">给定的 <see cref="DateTime"/>。</param>
    /// <param name="useLocalOffset">强制使用本地时区偏移（可选；默认不使用）。</param>
    /// <returns>返回 <see cref="DateTimeOffset"/>。</returns>
    public static DateTimeOffset AsOffset(this DateTime dateTime, bool useLocalOffset = false)
    {
        return dateTime.Kind switch
        {
            DateTimeKind.Utc => new(dateTime, BaseOffset),

            DateTimeKind.Local => new(dateTime, LocalOffset),

            _ => new(dateTime, useLocalOffset ? LocalOffset : BaseOffset)
        };
    }


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


    #region With

    /// <summary>
    /// 使用指定的日期与时间部分创建一个副本。
    /// </summary>
    /// <param name="baseTime">给定的基础日期与时间。</param>
    /// <param name="newYear">给定的新年份（可选）。</param>
    /// <param name="newMonth">给定的新月份（可选）。</param>
    /// <param name="newDay">给定的新号数（可选）。</param>
    /// <param name="newHour">给定的新小时（可选）。</param>
    /// <param name="newMinute">给定的新分钟（可选）。</param>
    /// <param name="newSecond">给定的新秒数（可选）。</param>
    /// <param name="newMillisecond">给定的新毫秒（可选）。</param>
    /// <returns>返回 <see cref="DateTime"/>。</returns>
    public static DateTime With(this DateTime baseTime, int? newYear = null, int? newMonth = null, int? newDay = null,
        int? newHour = null, int? newMinute = null, int? newSecond = null, int? newMillisecond = null)
    {
        return new
        (
            newYear ?? baseTime.Year,
            newMonth ?? baseTime.Month,
            newDay ?? baseTime.Day,
            newHour ?? baseTime.Hour,
            newMinute ?? baseTime.Minute,
            newSecond ?? baseTime.Second,
            newMillisecond ?? baseTime.Millisecond,
            baseTime.Kind
        );
    }

    /// <summary>
    /// 使用指定的日期与时间部分创建一个副本。
    /// </summary>
    /// <param name="baseTimeOffset">给定的基础日期与时间。</param>
    /// <param name="newYear">给定的新年份（可选）。</param>
    /// <param name="newMonth">给定的新月份（可选）。</param>
    /// <param name="newDay">给定的新号数（可选）。</param>
    /// <param name="newHour">给定的新小时（可选）。</param>
    /// <param name="newMinute">给定的新分钟（可选）。</param>
    /// <param name="newSecond">给定的新秒数（可选）。</param>
    /// <param name="newMillisecond">给定的新毫秒（可选）。</param>
    /// <param name="newOffset">给定的新 UTC 偏移（可选）。</param>
    /// <returns>返回 <see cref="DateTimeOffset"/>。</returns>
    public static DateTimeOffset With(this DateTimeOffset baseTimeOffset, int? newYear = null, int? newMonth = null, int? newDay = null,
        int? newHour = null, int? newMinute = null, int? newSecond = null, int? newMillisecond = null, TimeSpan? newOffset = null)
    {
        return new
        (
            newYear ?? baseTimeOffset.Year,
            newMonth ?? baseTimeOffset.Month,
            newDay ?? baseTimeOffset.Day,
            newHour ?? baseTimeOffset.Hour,
            newMinute ?? baseTimeOffset.Minute,
            newSecond ?? baseTimeOffset.Second,
            newMillisecond ?? baseTimeOffset.Millisecond,
            newOffset ?? baseTimeOffset.Offset
        );
    }


    /// <summary>
    /// 使用指定的日期与时间部分（除毫秒外，毫秒将被重置为 0）创建一个副本。
    /// </summary>
    /// <param name="baseTime">给定的基础日期与时间。</param>
    /// <param name="newYear">给定的新年份（可选）。</param>
    /// <param name="newMonth">给定的新月份（可选）。</param>
    /// <param name="newDay">给定的新号数（可选）。</param>
    /// <param name="newHour">给定的新小时（可选）。</param>
    /// <param name="newMinute">给定的新分钟（可选）。</param>
    /// <param name="newSecond">给定的新秒数（可选）。</param>
    /// <returns>返回 <see cref="DateTime"/>。</returns>
    public static DateTime WithoutMillisecond(this DateTime baseTime, int? newYear = null, int? newMonth = null, int? newDay = null,
        int? newHour = null, int? newMinute = null, int? newSecond = null)
    {
        return new
        (
            newYear ?? baseTime.Year,
            newMonth ?? baseTime.Month,
            newDay ?? baseTime.Day,
            newHour ?? baseTime.Hour,
            newMinute ?? baseTime.Minute,
            newSecond ?? baseTime.Second,
            baseTime.Kind
        );
    }

    /// <summary>
    /// 使用指定的日期与时间部分（除毫秒外，毫秒将被重置为 0）创建一个副本。
    /// </summary>
    /// <param name="baseTimeOffset">给定的基础日期与时间。</param>
    /// <param name="newYear">给定的新年份（可选）。</param>
    /// <param name="newMonth">给定的新月份（可选）。</param>
    /// <param name="newDay">给定的新号数（可选）。</param>
    /// <param name="newHour">给定的新小时（可选）。</param>
    /// <param name="newMinute">给定的新分钟（可选）。</param>
    /// <param name="newSecond">给定的新秒数（可选）。</param>
    /// <param name="newOffset">给定的新 UTC 偏移（可选）。</param>
    /// <returns>返回 <see cref="DateTimeOffset"/>。</returns>
    public static DateTimeOffset WithoutMillisecond(this DateTimeOffset baseTimeOffset, int? newYear = null, int? newMonth = null,
        int? newDay = null, int? newHour = null, int? newMinute = null, int? newSecond = null, TimeSpan? newOffset = null)
    {
        return new
        (
            newYear ?? baseTimeOffset.Year,
            newMonth ?? baseTimeOffset.Month,
            newDay ?? baseTimeOffset.Day,
            newHour ?? baseTimeOffset.Hour,
            newMinute ?? baseTimeOffset.Minute,
            newSecond ?? baseTimeOffset.Second,
            newOffset ?? baseTimeOffset.Offset
        );
    }


    /// <summary>
    /// 使用指定的年份创建该年第一天零时分秒的日期与时间。
    /// </summary>
    /// <param name="newYear">给定的新年份。</param>
    /// <returns>返回 <see cref="DateTime"/>。</returns>
    public static DateTime WithYearFirstDay(this int newYear)
        => DateTime.UnixEpoch.With(newYear: newYear);

    /// <summary>
    /// 使用指定的年份创建该年第一天零时分秒的日期与时间偏移。
    /// </summary>
    /// <param name="newYear">给定的新年份。</param>
    /// <returns>返回 <see cref="DateTimeOffset"/>。</returns>
    public static DateTimeOffset WithYearFirstDayOffset(this int newYear)
        => DateTimeOffset.UnixEpoch.With(newYear: newYear);

    #endregion


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
