using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Librame.Extensions
{
    public class NullableExtensionsTests
    {

        [Fact]
        public void UnwrapTest()
        {
            Guid? g = null;
            Assert.Equal(Guid.Empty, g.UnwrapOrDefault());
            Assert.NotEqual(Guid.Empty, g.UnwrapOrDefault(Guid.NewGuid()));
        }

        [Fact]
        public void NotNullAndNotEmptyTest()
        {
            // Guid
            string? g = null;
            Assert.Throws<ArgumentNullException>(() =>
            {
                g.NotNull(nameof(g));
            });

            g = string.Empty;
            Assert.NotNull(g.NotNull(nameof(g)));

            // ICollection
            IEnumerable<int>? enumerable = Enumerable.Range(1, 5).ToArray();
            Assert.NotNull(enumerable.NotEmpty(nameof(enumerable)));
            
            // String
            string str = string.Empty;
            Assert.Throws<ArgumentException>(() =>
            {
                str.NotEmpty(nameof(str));
            });

            str = " ";
            Assert.Throws<ArgumentException>(() =>
            {
                str.NotWhiteSpace(nameof(str));
            });

            str = "123";
            Assert.NotNull(str.NotEmpty(nameof(str)));
        }

    }
}
