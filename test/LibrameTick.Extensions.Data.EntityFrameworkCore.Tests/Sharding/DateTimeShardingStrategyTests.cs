using System;
using Xunit;

namespace Librame.Extensions.Data.Sharding
{
    public class DateTimeShardingStrategyTests
    {

        [Fact]
        public void AllTest()
        {
            var strategry = new DateTimeShardingStrategy(() => DateTime.Now);

            foreach (var key in strategry.AllKeys)
            {
                var value = strategry.Format(key, shardingValue: null); // Use Default DateTime
                Assert.True(value.IsDigit());
            }
        }

    }
}
