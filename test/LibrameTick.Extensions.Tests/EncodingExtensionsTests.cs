using Xunit;

namespace Librame.Extensions
{
    public class EncodingExtensionsTests
    {

        [Fact]
        public void EncodingNameTest()
        {
            var encodingName = EncodingExtensions.UTF8Encoding.AsEncodingName();
            Assert.Equal(EncodingExtensions.UTF8Encoding, encodingName.FromEncodingName());
        }

        [Fact]
        public void EncodingStringTest()
        {
            var str = nameof(EncodingExtensionsTests);
            var buffer = str.FromEncodingString();
            Assert.Equal(str, buffer.AsEncodingString());
        }

    }
}
