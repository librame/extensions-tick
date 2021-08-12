using Xunit;

namespace Librame.Extensions
{
    public class ValidationExtensionsTests
    {

        #region Compare

        [Fact]
        public void IsMultiplesTest()
        {
            Assert.True(4.IsMultiples(2));
            Assert.True(9.IsMultiples(3));
            Assert.False(5.IsMultiples(9));
        }

        [Fact]
        public void IsGreaterTest()
        {
            var num = 3;
            Assert.True(num.IsGreater(2));
            Assert.True(num.IsGreater(3, true));
            Assert.False(num.IsGreater(4));
        }

        [Fact]
        public void IsLesserTest()
        {
            var num = 3;
            Assert.True(num.IsLesser(4));
            Assert.True(num.IsLesser(3, true));
            Assert.False(num.IsLesser(2));
        }

        [Fact]
        public void IsOutOfRangeTest()
        {
            var num = 3;
            Assert.False(num.IsOutOfRange(1, 9));
            Assert.True(num.IsOutOfRange(3, 9, true));
            Assert.False(num.IsOutOfRange(1, 4, false, true));
            Assert.True(num.IsOutOfRange(10, 30));
        }

        #endregion


        #region Digit and Letter

        [Fact]
        public void DigitLetterTest()
        {
            Assert.True("012x".HasDigit());
            Assert.False("012x".IsDigit());
            Assert.True("012".IsDigit());

            Assert.True("xX".HasLower());
            Assert.False("xX".IsLower());
            Assert.True("x".IsLower());

            Assert.True("xX".HasUpper());
            Assert.False("xX".IsUpper());
            Assert.True("X".IsUpper());

            Assert.True("012xX".HasLetter());
            Assert.False("012xX".IsLetter());
            Assert.True("xX".IsLetter());
        }

        #endregion

    }
}
