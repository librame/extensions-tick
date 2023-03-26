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
/// 日期与时间的分片策略支持的参数（区分大小写）包括：%yyyy（4 位年份）、%yy（2 位年份）、%MM（2 位月份）、%dd（2 位天数）、%hh（2 位小时）、%mm（2 位分钟）、%ss（2 位秒数）、%hf（2 位半年）、%qq（2 位季度）、%q（1 位季度）、%ww（2 位周数）。
/// </remarks>
public class DateTimeShardingStrategy : AbstractShardingStrategy<DateTime>
{
    /// <summary>
    /// 构造一个 <see cref="DateTimeShardingStrategy"/>。
    /// </summary>
    public DateTimeShardingStrategy()
        : base()
    {
        AddParameter("yyyy", now => now.ToString("yyyy")); // 4 位年份
        AddParameter("yy", now => now.ToString("yy")); // 2 位年份
        AddParameter("MM", now => now.ToString("MM")); // 2 位月份
        AddParameter("dd", now => now.ToString("dd")); // 2 位天数
        AddParameter("hh", now => now.ToString("hh")); // 2 位小时
        AddParameter("mm", now => now.ToString("mm")); // 2 位分钟
        AddParameter("ss", now => now.ToString("ss")); // 2 位秒数
        AddParameter("hf", now => now.AsHalfYear().FormatString(2)); // 2 位半年
        AddParameter("qq", now => now.AsQuarterOfYear().FormatString(2)); // 2 位季度
        AddParameter("q", now => now.AsQuarterOfYear().ToString()); // 1 位季度
        AddParameter("ww", now => now.AsWeekOfYear().FormatString(2)); // 2 位周数
    }


    /// <summary>
    /// 重写默认值为当前本地日期和时间 <see cref="DateTime.Now"/>。
    /// </summary>
    public override Lazy<DateTime> DefaultValue
        => new Lazy<DateTime>(DateTime.Now);

}
