using System.Linq;
using Librame.Extensions.Cryptography;
using Xunit;

namespace Librame.Extensions
{
    public class ByteExtensionsTests
    {
        private readonly byte[] _data = RandomExtensions.GenerateByteArray(32);


        [Fact]
        public void Base32StringTest()
        {
            var base32 = _data.AsBase32String();
            Assert.True(_data.SequenceEqual(base32.FromBase32String()));
        }

        [Fact]
        public void HexStringTest()
        {
            var hex = _data.AsHexString();
            Assert.True(_data.SequenceEqual(hex.FromHexString()));
        }

    }
}
