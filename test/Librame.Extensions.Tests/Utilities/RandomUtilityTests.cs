using System;
using Xunit;

namespace Librame.Extensions.Tests
{
    public class RandomUtilityTests
    {

        [Fact]
        public void GenerateByteArrayTest()
        {
            var buffer = RandomUtility.GenerateByteArray(16);
            Assert.NotEmpty(buffer);

            var g = new Guid(buffer);
            Assert.NotEmpty(g.ToString());
        }

        [Fact]
        public void RandomStringsTest()
        {
            var strs = RandomUtility.GenerateStrings(20);
            Assert.NotEmpty(strs);
        }

    }
}
