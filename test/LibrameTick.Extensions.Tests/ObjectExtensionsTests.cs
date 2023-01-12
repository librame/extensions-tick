using System;
using Xunit;

namespace Librame.Extensions
{
    public class ConversionExtensionsTests
    {

        [Fact]
        public void AsTest()
        {
            object? obj = null;
            Assert.Throws<ArgumentException>(() =>
            {
                obj!.As<int>(nameof(obj));
            });
        }

        [Fact]
        public void AsOrDefaultTest()
        {
            object? obj = null;
            Assert.Equal(1, obj.AsOrDefault(1));
            Assert.Equal(3, obj.AsOrDefault(() => 3));
        }

    }
}
