using System;
using Xunit;

namespace Librame.Extensions
{
    public class ObjectExtensionsTests
    {

        [Fact]
        public void AsTest()
        {
            object? obj = 1;
            Assert.Equal(1, obj.AsOrDefaultIfNull<int>());

            obj = null;
            Assert.Throws<ArgumentNullException>(() =>
            {
                obj!.AsNotNull<int>(nameof(obj));
            });
        }

    }
}
