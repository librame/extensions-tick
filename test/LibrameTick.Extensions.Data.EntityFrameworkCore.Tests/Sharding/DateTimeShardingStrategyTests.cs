using Xunit;

namespace Librame.Extensions.Data.Sharding
{
    public class DateTimeShardingStrategyTests
    {

        [Fact]
        public void AllTest()
        {
            var sharded = new ShardedDescriptor("sharding", "%MM");
            var original = sharded.ToString();

            var strategy = new DateTimeShardingStrategy();
            strategy.FormatSuffix(sharded);

            var format = sharded.ToString();
            Assert.NotEqual(original, format);
        }

    }
}
