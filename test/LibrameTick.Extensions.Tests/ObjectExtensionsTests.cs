using System;
using Xunit;

namespace Librame.Extensions
{
    public class ObjectExtensionsTests
    {

        [Fact]
        public void AsNotNullTest()
        {
            object? obj = null;
            Assert.Throws<ArgumentNullException>(() =>
            {
                obj!.AsNotNull<int>(nameof(obj));
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
