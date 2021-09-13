using Xunit;

namespace Librame.Extensions.Data
{
    public class ShardingDescriptorTests
    {

        [Fact]
        public void AllTest()
        {
            var now = DateTime.Now;

            var sharding = new ShardingDescriptor(nameof(ShardingDescriptor))
                .AppendNamedSuffix(now.Year.ToString());

            var expected = $"{sharding.BaseName}{sharding.NamedConnector}{now.Year}";
            Assert.Equal(expected, sharding);

            var copyShard = ShardingDescriptor.Parse(expected);
            Assert.Equal(expected, copyShard);

            var newSharding = sharding.WithNamedSuffix(s => now.Month.ToString());
            expected = $"{sharding}{sharding.NamedConnector}{now.Month}";
            Assert.Equal(expected, newSharding);
        }

    }
}
