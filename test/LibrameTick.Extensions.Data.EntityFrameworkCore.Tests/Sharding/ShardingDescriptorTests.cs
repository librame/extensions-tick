using Xunit;

namespace Librame.Extensions.Data.Sharding
{
    public class ShardingDescriptorTests
    {

        [Fact]
        public void AllTest()
        {
            var now = DateTime.Now;

            var sharding = new ShardingNamingDescriptor("Sharding", now.Year.ToString());

            var expected = $"{sharding.BaseName}{sharding.SuffixConnector}{now.Year}";
            Assert.Equal(expected, sharding);

            var copyShard = ShardingNamingDescriptor.Parse(expected);
            Assert.Equal(expected, copyShard);

            var newSharding = sharding.WithSuffix(now.Month.ToString());
            expected = $"{sharding.BaseName}{sharding.SuffixConnector}{now.Month}";
            Assert.Equal(expected, newSharding);
        }

    }
}
