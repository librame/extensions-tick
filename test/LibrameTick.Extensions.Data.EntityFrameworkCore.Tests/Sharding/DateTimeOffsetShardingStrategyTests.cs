using Xunit;

namespace Librame.Extensions.Data.Sharding
{
    public class DateTimeOffsetShardingStrategyTests
    {

        [Fact]
        public void AllTest()
        {
            var sharded = new ShardedDescriptor("sharding", "%qq");
            var original = sharded.ToString();

            var strategy = new DateTimeOffsetShardingStrategy();
            strategy.FormatSuffix(sharded);

            var format = sharded.ToString();
            Assert.NotEqual(original, format);
        }

    }
}
