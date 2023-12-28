using System;
using Xunit;

namespace Librame.Extensions.Data.Sharding
{
    public class DateTimeOffsetShardingStrategyTests
    {

        [Fact]
        public void AllTest()
        {
            var strategry = new DateTimeOffsetShardingStrategy(() => DateTimeOffset.Now);

            foreach (var key in strategry.AllKeys)
            {
                var value = strategry.Format(key, shardingValue: null); // Use Default DateTime
                Assert.True(value.IsDigit());
            }
        }

    }
}
