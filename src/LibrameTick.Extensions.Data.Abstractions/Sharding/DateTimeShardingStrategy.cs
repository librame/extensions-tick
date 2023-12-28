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
/// 定义基于日期和时间的分片策略（策略前缀为 dt）。
/// </summary>
/// <remarks>
/// 日期与时间的分片策略支持的参数（区分大小写）包括：
///     <para>%dt:y4（4 位年份）、</para>
///     <para>%dt:y2（2 位年份）、</para>
///     <para>%dt:mo（2 位月份）、</para>
///     <para>%dt:dd（2 位天数）、</para>
///     <para>%dt:hh（2 位小时）、</para>
///     <para>%dt:mi（2 位分钟）、</para>
///     <para>%dt:ss（2 位秒数）、</para>
///     <para>%dt:wk（2 位周数）、</para>
///     <para>%dt:hy（1 位半年）、</para>
///     <para>%dt:qy（1 位季度）、</para>
///     <para>%dt:hq（1 位半年 + 1 位季度）、</para>
///     <para>%dt:d8（4 位年份 + 2 位月份 + 2 位天数）、</para>
///     <para>%dt:d6（2 位年份 + 2 位月份 + 2 位天数）、</para>
///     <para>%dt:t9（2 位小时 + 2 位分钟 + 2 位秒数 + 3 位毫秒数）、</para>
///     <para>%dt:t6（2 位小时 + 2 位分钟 + 2 位秒数）、</para>
///     <para>%dt:ts（计时周期数）。</para>
/// </remarks>
public class DateTimeShardingStrategy : AbstractShardingStrategy<DateTime>
{
    /// <summary>
    /// 构造一个 <see cref="DateTimeShardingStrategy"/>。
    /// </summary>
    /// <param name="defaultValueFactory">给定的默认值工厂方法。</param>
    public DateTimeShardingStrategy(Func<DateTime> defaultValueFactory)
        : base("dt", defaultValueFactory)
    {
        AddParameter("y4", static now => now.ToString("yyyy")); // 4 位年份
        AddParameter("y2", static now => now.ToString("yy")); // 2 位年份
        AddParameter("mo", static now => now.ToString("MM")); // 2 位月份
        AddParameter("dd", static now => now.ToString("dd")); // 2 位天数
        AddParameter("hh", static now => now.ToString("HH")); // 2 位小时
        AddParameter("mi", static now => now.ToString("mm")); // 2 位分钟
        AddParameter("ss", static now => now.ToString("ss")); // 2 位秒数
        AddParameter("wk", static now => now.AsWeekOfYear().FormatString(2)); // 2 位周数
        AddParameter("hy", static now => now.AsHalfYear().ToString()); // 1 位半年
        AddParameter("qy", static now => now.AsQuarterOfYear().ToString()); // 1 位季度
        AddParameter("hq", static now => now.AsHalfYear().ToString() + now.AsQuarterOfYear().ToString()); // 1 位半年 + 1 位季度
        AddParameter("d8", static now => now.ToString("yyyyMMdd")); // 4 位年份 + 2 位月份 + 2 位天数
        AddParameter("d6", static now => now.ToString("yyMMdd")); // 2 位年份 + 2 位月份 + 2 位天数
        AddParameter("t9", static now => now.ToString("HHmmssfff")); // 2 位小时 + 2 位分钟 + 2 位秒数 + 3 位毫秒数
        AddParameter("t6", static now => now.ToString("HHmmss")); // 2 位小时 + 2 位分钟 + 2 位秒数
        AddParameter("ts", static now => now.Ticks.ToString()); // 计时周期数
    }

}
