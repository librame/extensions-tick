using Xunit;

namespace Librame.Extensions
{
    public class NumberExtensionsTests
    {
        [Fact]
        public void TruncateTest()
        {
            // 8 位精度正常（最大 9 位）
            var f = 1234.56789F;

            var f1 = f.Truncate();
            Assert.Equal(1234.56F, f1);
            Assert.Equal(1234F, f.Truncate(0));
            // 如果保留 4 位将出现精度丢失导致不相等
            Assert.Equal(1234.567F, f.Truncate(3));

            // 16 位精度正常（最大 17 位）
            var d = 123456789.98765432;

            var d1 = d.Truncate();
            Assert.Equal(123456789.98, d1);
            Assert.Equal(123456789D, d.Truncate(0));
            Assert.Equal(123456789.9876, d.Truncate(4));
            Assert.Equal(123456789.987654, d.Truncate(6));
        }

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
