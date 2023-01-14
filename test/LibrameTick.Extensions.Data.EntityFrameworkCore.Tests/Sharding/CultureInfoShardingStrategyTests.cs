using System;
using System.Globalization;
using Xunit;

namespace Librame.Extensions.Data.Sharding
{
    public class CultureInfoShardingStrategyTests
    {

        [Fact]
        public void AllTest()
        {
            var sharded = new ShardedDescriptor("sharding", "%c")
                .ChangeReference(CultureInfo.CurrentCulture);

            var strategy = new CultureInfoShardingStrategy();
            strategy.FormatSuffix(sharded);

            var parameter = strategy[0];

            //var parameter = "%c";
            //var parameter = strategy[test];
        }

    }
}
