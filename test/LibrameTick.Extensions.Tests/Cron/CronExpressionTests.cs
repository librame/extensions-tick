using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Librame.Extensions.Cron
{
    public class CronExpressionTests
    {
        private readonly List<string> _expressions = new()
        {
            // 每天早上六点，忽略周几
            "0 0 6 * * ?",
            // 每隔一分钟执行一次，忽略周几
            "0 */1 * * * ?"
        };


        [Fact]
        public void DateTimeTest()
        {
            var count = 3;
            var baseTime = DateTime.Now;

            foreach (var expression in _expressions)
            {
                var times = expression.GetNextOccurrenceTimes(baseTime, count, out var result);
                Assert.NotEmpty(times); // 明天 +08:00、后天 +08:00、大后天 +08:00
                Assert.NotNull(result);
                Assert.Equal(count, times.Count);
                Assert.Equal(baseTime.Kind, times.First()!.Value.Kind);
            }
        }

        [Fact]
        public void DateTimeOffsetTest()
        {
            var count = 3;

            // +08:00
            var baseTimeOffset = DateTimeOffset.Now;

            foreach (var expression in _expressions)
            {
                var times = expression.GetNextOccurrenceTimes(baseTimeOffset, count, out var result);
                Assert.NotEmpty(times); // 明天 +08:00、后天 +08:00、大后天 +08:00
                Assert.NotNull(result);
                Assert.Equal(count, times.Count);
                Assert.Equal(baseTimeOffset.Offset, times.First()!.Value.Offset);
            }

            // +00:00
            baseTimeOffset = DateTimeOffset.UtcNow;

            foreach (var expression in _expressions)
            {
                var times = expression.GetNextOccurrenceTimes(baseTimeOffset, count);
                Assert.NotEmpty(times); // 今天 +08:00、明天 +08:00、后天 +08:00
                Assert.Equal(count, times.Count);
                Assert.Equal(baseTimeOffset.Offset, times.First()!.Value.Offset);
            }
        }

    }
}
