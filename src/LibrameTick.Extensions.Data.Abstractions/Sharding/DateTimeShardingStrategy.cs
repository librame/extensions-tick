#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data.Sharding;

/// <summary>
/// 定义基于日期和时间的分片策略。
/// </summary>
/// <remarks>
/// 日期与时间的分片策略支持的参数（区分大小写）包括：%y4（4 位数年份）、%y（2 位年份）、%M（2 位月份）、%d（2 位天数）、%h（2 位小时数）、%m（2 位分钟数）、%s（2 位秒数）、%q2（2 位本年季度）、%q（1 位本年季度）、%w（2 位本年周数）。
/// </remarks>
public class DateTimeShardingStrategy : AbstractShardingStrategy<DateTime>
{
    /// <summary>
    /// 构造一个 <see cref="DateTimeShardingStrategy"/>。
    /// </summary>
    public DateTimeShardingStrategy()
        : base()
    {
        AddParameter("y4", now => now.ToString("yyyy")); // 4 位数年份
        AddParameter("y", now => now.ToString("yy")); // 2 位年份
        AddParameter("M", now => now.ToString("MM")); // 2 位月份
        AddParameter("d", now => now.ToString("dd")); // 2 位天数
        AddParameter("h", now => now.ToString("hh")); // 2 位小时数
        AddParameter("m", now => now.ToString("mm")); // 2 位分钟数
        AddParameter("s", now => now.ToString("ss")); // 2 位秒数
        AddParameter("q2", now => now.AsQuarterOfYear().FormatString(2)); // 2 位本年季度
        AddParameter("q", now => now.AsQuarterOfYear().ToString()); // 1 位本年季度
        AddParameter("w", now => now.AsWeekOfYear().FormatString(2)); // 2 位本年周数
    }

}
