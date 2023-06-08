using System;
using System.Threading.Tasks;
using Xunit;

namespace Librame.Extensions
{
    public class TaskExtensionsTests
    {

        [Fact]
        public async Task CancelByTimeoutTest()
        {
            var bools = await Task.WhenAll(TestNotTimeoutAsync(), TestTimeoutAsync());
            Assert.True(bools[0]); // 未超时
            Assert.True(bools[1]); // 已超时
        }


        private static async Task<bool> TestTimeoutAsync()
        {
            var timeoutSeconds = 3;
            var curSendCount = await TestSendRequestAsync(5)
                .SkipByTimeout(TimeSpan.FromSeconds(timeoutSeconds));

            // 请求5次，3秒超时，结果超时
            return curSendCount == 0;
        }

        private static async Task<bool> TestNotTimeoutAsync()
        {
            var maxSendCount = 2;
            var curSendCount = await TestSendRequestAsync(maxSendCount)
                .SkipByTimeout(TimeSpan.FromSeconds(5));

            // 请求2次，5秒超时，通常2次均会执行完成，结果不超时
            return curSendCount == maxSendCount;
        }

        private static async Task<int> TestSendRequestAsync(int maxSendCount)
        {
            var curSendCount = 0;

            while (curSendCount < maxSendCount)
            {
                await Task.Delay(1000);

                curSendCount++;
            }

            return curSendCount;
        }

    }
}
