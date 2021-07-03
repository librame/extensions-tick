using System.Linq;
using Xunit;

namespace Librame.Extensions
{
    public class EnumerableExtensionsTests
    {

        #region Trim

        [Fact]
        public void TrimTest()
        {
            var items = new int[] { 1, 1, 2, 3, 4, 5, 1, 1, 1 };
            items = items.Trim(1).ToArray();

            Assert.Equal(4, items.Length);
        }

        [Fact]
        public void TrimFirstTest()
        {
            var items = Enumerable.Range(1, 10);
            items = items.TrimFirst(1);

            Assert.Equal(9, items.Count());
        }

        [Fact]
        public void TrimLastTest()
        {
            var items = Enumerable.Range(1, 10);
            items = items.TrimLast(10);

            Assert.Equal(9, items.Count());
        }

        #endregion

    }
}
