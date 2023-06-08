using System.Drawing;
using Xunit;

namespace Librame.Extensions
{
    public class ColorExtensionsTests
    {
        [Fact]
        public void AllTest()
        {
            var left = Color.FromArgb(255, 255, 255);
            var right = Color.FromArgb(250, 250, 250);

            Assert.True(left.IsSimilar(right));
        }

    }
}
