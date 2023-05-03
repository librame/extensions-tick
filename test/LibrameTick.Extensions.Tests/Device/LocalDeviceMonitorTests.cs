using System.Net.NetworkInformation;
using Xunit;

namespace Librame.Extensions.Device
{
    public class LocalDeviceMonitorTests
    {
        [Fact]
        public void AllTest()
        {
            var monitor = new LocalDeviceMonitor(new());

            var ping = monitor.SendPing("www.baidu.com");
            Assert.Equal(IPStatus.Success, ping.Status);
            Assert.NotEmpty(ping.Address.ToString());

            var processor = monitor.GetProcessor();
            Assert.True(processor.UsageRate > 0);

            var memory = monitor.GetMemory();
            Assert.True(memory.UsageRate > 0);

            var disk = monitor.GetDisk();
            Assert.True(disk.UsageRate > 0);

            var network = monitor.GetNetwork();
            Assert.True(network.UsageRate >= 0);
        }

    }
}
