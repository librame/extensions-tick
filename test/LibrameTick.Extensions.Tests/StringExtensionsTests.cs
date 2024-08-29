using System;
using System.IO;
using Xunit;

namespace Librame.Extensions
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


        #region Append and Insert

        [Fact]
        public void AppendTest()
        {
            var str = string.Empty;
            Assert.Equal(string.Empty, str.Append(string.Empty));

            str = nameof(StringExtensionsTests);
            Assert.Equal(str, str.Append(string.Empty));

            var append = "123456";
            Assert.Equal($"{str}{append}", str.Append(append));
        }

        [Fact]
        public void InsertTest()
        {
            var str = string.Empty;
            Assert.Equal(string.Empty, str.Insert(string.Empty));

            str = nameof(StringExtensionsTests);
            Assert.Equal(str, str.Insert(string.Empty));

            var insert = "123456";
            Assert.Equal($"{insert}{str}", str.Insert(insert));

            Assert.Equal($"{str.Substring(0, 6)}{insert}{str.Substring(6)}",
                str.Insert(insert, startIndex: 6));

            Assert.Equal($"{str}{insert}", str.Insert(insert, startIndex: str.Length));
        }

        #endregion


        #region Format

        [Fact]
        public void FormatStringTest()
        {
            var buffer = Guid.NewGuid().ToByteArray();
            // 8db56dc7163edf5
            var format = buffer.FormatString(DateTime.Now.Ticks);
            Assert.NotEmpty(format);

            var number = 1;
            Assert.Equal("0001", number.FormatString(4));
        }

        #endregion


        #region JoinString

        [Fact]
        public void JoinStringTest()
        {
            var str = "abcdefg";
            Assert.Equal(str, str.ToCharArray().JoinString());
            Assert.Equal("a,b,c,d,e,f,g", str.ToCharArray().JoinString(','));

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
            Assert.Equal($"{testString}{str}", result); // appended

            result = str.Trailing(testString);
            Assert.Equal($"{str}{testString}", result); // no append
        }

        #endregion


        #region Naming Conventions

        [Fact]
        public void PascalCasingAndCamelCasingTest()
        {
            // Word
            var pascalWord = "Naming";
            var camelWord = pascalWord.AsCamelCasing();

            Assert.Equal("naming", camelWord);
            Assert.Equal(pascalWord, camelWord.AsPascalCasing());

            // Words
            var pascalWords = "Naming Conventions";
            var camelWords = pascalWords.AsCamelCasing(' ');

            Assert.Equal("naming conventions", camelWords);
            Assert.Equal(pascalWords, camelWords.AsPascalCasing(' '));
        }

        #endregion


        #region Singular & Plural

        [Fact]
        public void SingularAndPluralTest()
        {
            // Pluralize
            var value = "item";
            Assert.Equal("items", value.AsPluralize());

            value = "child";
            Assert.Equal("children", value.AsPluralize());

            value = "chinese";
            Assert.Equal("chinese", value.AsPluralize());

            value = "foot";
            Assert.Equal("feet", value.AsPluralize());

            value = "half";
            Assert.Equal("halves", value.AsPluralize());

            value = "knife";
            Assert.Equal("knives", value.AsPluralize());

            // Singularize
            value = "items";
            Assert.Equal("item", value.AsSingularize());

            value = "children";
            Assert.Equal("child", value.AsSingularize());

            value = "chinese";
            Assert.Equal("chinese", value.AsSingularize());

            value = "feet";
            Assert.Equal("foot", value.AsSingularize());

            value = "halves";
            Assert.Equal("half", value.AsSingularize());

            value = "knives";
            Assert.Equal("knife", value.AsSingularize());
        }

        #endregion


        #region TrySplitPair

        [Fact]
        public void TrySplitPairTest()
        {
            var value = "123ab==-092cd";

            if (value.TrySplitPair('=', out var pair))
            {
                Assert.Equal("123ab", pair.Key);
                Assert.Equal("=-092cd", pair.Value);
            }

            if (value.TrySplitPairByLastIndexOf('=', out pair))
            {
                Assert.Equal("123ab=", pair.Key);
                Assert.Equal("-092cd", pair.Value);
            }
        }

        #endregion


        #region SystemString

        [Fact]
        public void SystemStringTest()
        {
            var number = DateTime.Now.Ticks;
            // "EVToTnQQBTB"
            Assert.NotEmpty(number.AsSystemString());
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
