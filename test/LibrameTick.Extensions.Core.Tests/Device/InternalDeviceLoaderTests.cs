using Librame.Extensions.Core;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using Xunit;

namespace Librame.Extensions.Device
{
    public class InternalDeviceLoaderTests
    {
        private IDeviceLoader _loader;


        public InternalDeviceLoaderTests()
        {
            _loader = CoreExtensionBuilderHelper.CurrentServices.GetRequiredService<IDeviceLoader>();
        }


        [Fact]
        public void AllTest()
        {
            for (var i = 0; i < 2; i++)
            {
                foreach (var usage in _loader.GetUsages(realtimeForEverytime: false))
                {
                    Assert.True(usage.Processor > 0);
                    Assert.True(usage.Memory > 0);
                    Assert.True(usage.Network > 0);
                    Assert.True(usage.Disk > 0);
                }

                Thread.Sleep(1000);
            }

            _loader.Dispose();
        }

    }
}
