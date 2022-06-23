using System;
using Xunit;

namespace Librame.Extensions.Data.Sharding
{
    public class ShardDescriptorTests
    {

        [Fact]
        public void AllTest()
        {
            var now = DateTime.Now;

            var sharding = new ShardedDescriptor("Shard", now.Year.ToString());

            var expected = $"{sharding.BaseName}{sharding.SuffixConnector}{now.Year}";
            Assert.Equal(expected, sharding);

            var copyShard = ShardedDescriptor.Parse(expected);
            Assert.Equal(expected, copyShard);

            var newSharding = sharding.WithSuffix(now.Month.ToString());
            expected = $"{sharding.BaseName}{sharding.SuffixConnector}{now.Month}";
            Assert.Equal(expected, newSharding);
        }

    }
}
