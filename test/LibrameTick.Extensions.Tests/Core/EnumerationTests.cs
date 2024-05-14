using System;
using Librame.Extensions.Infrastructure.Mapping;
using Xunit;

namespace Librame.Extensions.Core
{
    public class EnumerationTests
    {
        [Fact]
        public void AllTest()
        {
            var pairs = HexColor.GetNameValuePairs();
            Assert.NotEmpty(pairs);

            var names = HexColor.GetNames();
            Assert.NotEmpty(names);

            var values = HexColor.GetValues();
            Assert.NotEmpty(values);

            Assert.True(HexColor.ContainsName(nameof(HexColor.White)));
            Assert.False(HexColor.ContainsName("Red"));

            var white = HexColor.FromValue(1);
            Assert.True(HexColor.White.Equals(white));

            var black = HexColor.FromName(nameof(HexColor.Black));
            Assert.True(HexColor.Black.Equals(black));

            Assert.Throws<ArgumentException>(() => HexColor.FromValue(0));
        }

    }


    public abstract class HexColor : Enumeration<HexColor>
    {
        public static readonly HexColor White = new WhiteColorEnumeration();

        public static readonly HexColor Black = new BlackColorEnumeration();


        private HexColor(string name, int value)
            : base(name, value)
        {
        }


        public abstract string Code { get; }


        private sealed class WhiteColorEnumeration : HexColor
        {
            public WhiteColorEnumeration() : base(nameof(White), 1)
            {
            }

            public override string Code => "#FFFFFFF";
        }

        private sealed class BlackColorEnumeration : HexColor
        {
            public BlackColorEnumeration() : base(nameof(Black), 2)
            {
            }

            public override string Code => "#000000";
        }

    }

}
