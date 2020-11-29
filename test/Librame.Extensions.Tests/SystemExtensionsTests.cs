using Xunit;

namespace Librame.Extensions
{
    public class SystemExtensionsTests
    {

        #region String

        [Fact]
        public void AppendTest()
        {
            string str = null;
            Assert.Equal(string.Empty, str.Append(null));

            str = nameof(SystemExtensionsTests);
            Assert.Equal(str, str.Append(null));

            var append = "123456";
            Assert.Equal($"{str}{append}", str.Append(append));
        }

        [Fact]
        public void InsertTest()
        {
            string str = null;
            Assert.Equal(string.Empty, str.Insert(null));

            str = nameof(SystemExtensionsTests);
            Assert.Equal(str, str.Insert(null));

            var insert = "123456";
            Assert.Equal($"{insert}{str}", str.Insert(insert));

            Assert.Equal($"{str.Substring(0, 6)}{insert}{str.Substring(6)}",
                str.Insert(insert, startIndex: 6));

            Assert.Equal($"{str}{insert}", str.Insert(insert, startIndex: str.Length));
        }


        [Fact]
        public void LeadingTest()
        {
            var str = nameof(SystemExtensionsTests);

            var testChar = '#';

            var result = str.Leading(testChar);
            Assert.Equal($"{testChar}{str}", result); // inserted

            result = result.Leading(testChar);
            Assert.Equal($"{testChar}{str}", result); // no insert

            var testString = "Insert";

            result = str.Leading(testString);
            Assert.Equal($"{testString}{str}", result); // inserted

            result = result.Leading(testString);
            Assert.Equal($"{testString}{str}", result); // no insert
        }

        [Fact]
        public void TrailingTest()
        {
            var str = nameof(SystemExtensionsTests);

            var testChar = '#';

            var result = str.Trailing(testChar);
            Assert.Equal($"{str}{testChar}", result); // appended

            result = result.Trailing(testChar);
            Assert.Equal($"{str}{testChar}", result); // no append

            var testString = "Append";

            result = str.Leading(testString);
            Assert.Equal($"{str}{testString}", result); // appended

            result = result.Trailing(testString);
            Assert.Equal($"{str}{testString}", result); // no append
        }


        [Fact]
        public void TrimTest()
        {
            var str = "000abcdefg000";
            Assert.Equal("abcdefg", str.Trim("000"));
        }

        [Fact]
        public void TrimStartTest()
        {
            var str = "000abcdefg000";
            Assert.Equal("abcdefg000", str.TrimStart("000"));
        }

        [Fact]
        public void TrimEndTest()
        {
            var str = "000abcdefg000";
            Assert.Equal("000abcdefg", str.TrimEnd("000"));
        }

        #endregion

    }
}
