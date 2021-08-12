using Xunit;

namespace Librame.Extensions
{
    public class RandomExtensionsTests
    {

        [Fact]
        public void GenerateByteArrayTest()
        {
            var array = 16.GenerateByteArray();
            Assert.NotEmpty(array);

            var g = new Guid(array);
            Assert.NotEmpty(g.ToString());
        }

        [Fact]
        public void GenerateStringArrayTest()
        {
            var array = 20.GenerateStringArray();
            Assert.NotEmpty(array);

            for (var i = 0; i < array.Length - 1; i = i + 2)
            {
                // ½ûÖ¹³öÏÖÖØ¸´Ëæ»ú×Ö·û´®
                Assert.NotEqual(array[i], array[i + 1]);
            }
        }

    }
}
