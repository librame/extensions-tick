﻿using Xunit;

namespace Librame.Extensions.Data.Sharding
{
    public class CultureInfoShardingStrategyTests
    {

        [Fact]
        public void AllTest()
        {
            var sharded = new ShardedDescriptor("sharding", "%c");
            var original = sharded.ToString();

            var strategy = new CultureInfoShardingStrategy();
            strategy.FormatSuffix(sharded);

            var format = sharded.ToString();
            Assert.NotEqual(original, format);
        }

    }
}
