using System.Threading;
using Xunit;

namespace Librame.Extensions
{
    public class StopwatchExtensionsTests
    {

        [Fact]
        public void RunTest()
        {
            var ts = StopwatchExtensions.Run(s =>
            {
                Thread.Sleep(100);
                return s.ElapsedMilliseconds;
            });
            Assert.True(ts >= 100);
        }

    }
}
