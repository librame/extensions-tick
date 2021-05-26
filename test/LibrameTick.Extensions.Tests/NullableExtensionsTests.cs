using System;
using System.Collections;
using System.Linq;
using Xunit;

namespace Librame.Extensions.Tests
{
    public class NullableExtensionsTests
    {
        [Fact]
        public void UnwrapTest()
        {
            Guid? g = null;
            Assert.Equal(Guid.Empty, g.Unwrap());
            Assert.NotEqual(Guid.Empty, g.Unwrap(Guid.NewGuid()));
        }

        [Fact]
        public void IsEmptyTest()
        {
            IEnumerable? enumerable = Enumerable.Empty<int>();
            Assert.True(enumerable.IsEmpty());

            enumerable = Enumerable.Range(1, 5);
            Assert.True(enumerable.IsNotEmpty());
        }

        [Fact]
        public void NotNullAndNotEmptyTest()
        {
            Guid? g = null;
            Assert.Throws<ArgumentNullException>(() =>
            {
                g.NotNull(nameof(g));
            });

            g = Guid.NewGuid();
            Assert.NotNull(g.NotNull(nameof(g)));


            IEnumerable? enumerable = Enumerable.Empty<int>();
            Assert.Throws<ArgumentException>(() =>
            {
                enumerable.NotEmpty(nameof(enumerable));
            });

            enumerable = Enumerable.Range(1, 5);
            Assert.NotNull(enumerable.NotEmpty(nameof(enumerable)));


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
