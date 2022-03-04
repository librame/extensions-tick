using Xunit;

namespace Librame.Extensions
{
    public class IntegerExtensionsTests
    {
        [Fact]
        public void IPv4LongTest()
        {
            var ip = "192.168.0.1";

            var num = ip.AsIPv4Long();
            var str = num.FromIPv4LongAsString();

            Assert.Equal(str, ip);
        }

        [Fact]
        public void IPv6BigIntegerTest()
        {
            var ip = "2400:A480:aaaa:400:a1:b2:c3:d4";

            var num = ip.AsIPv6BigInteger();
            var str = num.FromIPv6BigIntegerAsString();

            // IPv6 不区分大小写
            Assert.Equal(str, ip, ignoreCase: true);
        }

    }
}
