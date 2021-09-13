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

namespace Librame.Extensions.Data.Sharding
{
    /// <summary>
    /// 定义基于日期和时间的分片策略。
    /// </summary>
    /// <remarks>
    /// 日期与时间的分片策略支持的参数（区分大小写）包括：%y4（4 位数年份）、%y（2 位年份）、%M（2 位月份）、%d（2 位天数）、%h（2 位小时数）、%m（2 位分钟数）、%s（2 位秒数）、%q2（2 位本年季度）、%q（1 位本年季度）、%w（2 位本年周数）。
    /// </remarks>
    public class DateTimeShardingStrategy : AbstractShardingStrategy
    {
        private readonly Dictionary<string, Func<DateTime, string>> _parameters = new()
        {
            { BuildParameterKey("y4"), now => now.ToString("yyyy") }, // 4 位数年份
            { BuildParameterKey("y"), now => now.ToString("yy") }, // 2 位年份
            { BuildParameterKey("M"), now => now.ToString("MM") }, // 2 位月份
            { BuildParameterKey("d"), now => now.ToString("dd") }, // 2 位天数
            { BuildParameterKey("h"), now => now.ToString("hh") }, // 2 位小时数
            { BuildParameterKey("m"), now => now.ToString("mm") }, // 2 位分钟数
            { BuildParameterKey("s"), now => now.ToString("ss") }, // 2 位秒数
            { BuildParameterKey("q2"), now => now.AsQuarterOfYear().FormatString(2) }, // 2 位本年季度
            { BuildParameterKey("q"), now => now.AsQuarterOfYear().ToString() }, // 1 位本年季度
            { BuildParameterKey("w"), now => now.AsWeekOfYear().FormatString(2) } // 2 位本年周数
        };


        /// <summary>
        /// 构造一个 <see cref="DateTimeShardingStrategy"/>。
        /// </summary>
        /// <param name="clock">给定的 <see cref="IClock"/>。</param>
        public DateTimeShardingStrategy(IClock clock)
        {
            Clock = clock;
        }


        /// <summary>
        /// 时钟。
        /// </summary>
        public IClock Clock { get; init; }


        /// <summary>
        /// 格式化后缀核心。
        /// </summary>
        /// <param name="suffix">给定的后缀。</param>
        /// <param name="basis">给定的分片依据。</param>
        /// <returns>返回字符串。</returns>
        protected override string FormatSuffixCore(string suffix, object? basis)
        {
            var now = Clock.GetNow();

            foreach (var p in _parameters)
            {
                // 区分大小写
                suffix = suffix.Replace(p.Key, p.Value.Invoke(now), StringComparison.Ordinal);
            }

            return suffix;
        }

    }
}
