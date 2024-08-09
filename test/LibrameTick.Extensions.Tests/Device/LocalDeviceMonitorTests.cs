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

            var original = monitor.GetAll();

            var buffer = Serialization.BinarySerializer.Serialize(original);
            var compare = Serialization.BinarySerializer.Deserialize<LocalDeviceInfo>(buffer);

            Assert.Equal(original.Processor.UsageRate, compare?.Processor.UsageRate);
            Assert.Equal(original.Memory.UsageRate, compare?.Memory.UsageRate);
            Assert.Equal(original.Disks.UsageRate, compare?.Disks.UsageRate);
            Assert.Equal(original.Networks.UsageRate, compare?.Networks.UsageRate);
        }

    }
}
