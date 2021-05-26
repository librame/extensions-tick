using Xunit;

namespace Librame.Extensions.Tests
{
    public class StringExtensionsTests
    {

        [Fact]
        public void HasInvalidCharsTest()
        {
            var invalidChars = "#%&".ToCharArray();
            var str = "abcdefghijklmn#%&opqrstuvwxyz";
            Assert.True(str.HasInvalidChars(invalidChars));
        }


        #region Insert and Append

        [Fact]
        public void AppendTest()
        {
            var str = string.Empty;
            Assert.Equal(string.Empty, str.Append(null));

            str = nameof(StringExtensionsTests);
            Assert.Equal(str, str.Append(null));

            var append = "123456";
            Assert.Equal($"{str}{append}", str.Append(append));
        }

        [Fact]
        public void InsertTest()
        {
            var str = string.Empty;
            Assert.Equal(string.Empty, str.Insert(null));

            str = nameof(StringExtensionsTests);
            Assert.Equal(str, str.Insert(null));

            var insert = "123456";
            Assert.Equal($"{insert}{str}", str.Insert(insert));

            Assert.Equal($"{str.Substring(0, 6)}{insert}{str.Substring(6)}",
                str.Insert(insert, startIndex: 6));

            Assert.Equal($"{str}{insert}", str.Insert(insert, startIndex: str.Length));
        }

        #endregion


        #region JoinString

        [Fact]
        public void JoinStringTest()
        {
            var str = "abcdefg";
            Assert.Equal(str, str.ToCharArray().JoinString());
            Assert.Equal(str, str.ToCharArray().JoinString(','));

            var array = new string[] { "123", "456" };
            Assert.Equal("123456", array.JoinString());
            Assert.Equal("123,456", array.JoinString(','));
        }

        #endregion


        #region Leading and Trailing

        [Fact]
        public void LeadingTest()
        {
            var str = nameof(StringExtensionsTests);

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
            var str = nameof(StringExtensionsTests);

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

        #endregion


        #region Trim

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
