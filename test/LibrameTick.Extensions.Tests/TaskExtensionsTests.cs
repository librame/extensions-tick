using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Librame.Extensions
{
    public class TaskExtensionsTests
    {

        [Fact]
        public async Task InvokeAsyncTest()
        {
            var actionId = 0;
            var action = () =>
            {
                Thread.Sleep(3000);
                actionId = Environment.CurrentManagedThreadId;
            };

            var funcId = 0;
            var func = () =>
            {
                Thread.Sleep(3000);
                funcId = Environment.CurrentManagedThreadId;
                return true;
            };

            await Task.WhenAll(action.InvokeAsync(), func.InvokeAsync());

            Assert.NotEqual(actionId, funcId);
        }

        [Fact]
        public async Task CancelFromTimeoutAsyncTest()
        {
            var action = () =>
            {
                Thread.Sleep(3000);
            };

            var status = await action.CancelByTimeoutAsync(TimeSpan.FromSeconds(5));
            Assert.Equal(TaskStatus.RanToCompletion, status);

            status = await action.CancelByTimeoutAsync(TimeSpan.FromSeconds(1));
            Assert.Equal(TaskStatus.Canceled, status);
        }

        [Fact]
        public async Task CancelFromTimeoutResultAsyncTest()
        {
            var func = () =>
            {
                Thread.Sleep(3000);
                return true;
            };

            var result = await func.CancelByTimeoutAsync(TimeSpan.FromSeconds(5),
                canceledResult: false);
            Assert.True(result);

            result = await func.CancelByTimeoutAsync(TimeSpan.FromSeconds(1),
                canceledResult: false);
            Assert.False(result);
        }

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
                .SkipByTimeoutAsync(TimeSpan.FromSeconds(timeoutSeconds));

            // 请求5次，3秒超时，结果超时
            return curSendCount == 0;
        }

        private static async Task<bool> TestNotTimeoutAsync()
        {
            var maxSendCount = 2;
            var curSendCount = await TestSendRequestAsync(maxSendCount)
                .SkipByTimeoutAsync(TimeSpan.FromSeconds(5));

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
