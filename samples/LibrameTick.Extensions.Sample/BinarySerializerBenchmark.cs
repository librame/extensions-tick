using BenchmarkDotNet.Attributes;
using Librame.Extensions.Dependency;
using Librame.Extensions.Device;
using Librame.Extensions.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace LibrameTick.Extensions.Sample
{
    [MemoryDiagnoser]
    public class BinarySerializerBenchmark
    {
        private readonly ProcessorDeviceInfo _processorInfo;
        private readonly int _count = 1000;


        public BinarySerializerBenchmark()
        {
            _processorInfo = new LocalDeviceMonitor(new()).GetProcessor();
        }


        [Benchmark]
        public void FormatterTest()
        {
#pragma warning disable SYSLIB0011 // 类型或成员已过时

            var formatter = new BinaryFormatter();

            byte[] buffer;

            using (var ms = DependencyRegistration.CurrentContext.MemoryStreams.GetStream())
            {
                formatter.Serialize(ms, _processorInfo);
                buffer = ms.ToArray();
            }

            for (int i = 0; i < _count; i++)
            {
                using (var ms = DependencyRegistration.CurrentContext.MemoryStreams.GetStream(buffer))
                {
                    var _ = (ProcessorDeviceInfo)formatter.Deserialize(ms);
                }
            }

#pragma warning restore SYSLIB0011 // 类型或成员已过时
        }


        [Benchmark]
        public void XfrogcnTest()
        {
            var buffer = Xfrogcn.BinaryFormatter.BinarySerializer.Serialize(_processorInfo);

            for (int i = 0; i < _count; i++)
            {
                var _ = Xfrogcn.BinaryFormatter.BinarySerializer.Deserialize<ProcessorDeviceInfo>(buffer);
            }
        }

        [Benchmark]
        public void MyObjectTest()
        {
            var processorType = typeof(ProcessorDeviceInfo);

            var buffer = BinarySerializer.SerializeObject(processorType, _processorInfo);

            for (int i = 0; i < _count; i++)
            {
                var _ = BinarySerializer.DeserializeObject(buffer, processorType);
            }
        }

        [Benchmark]
        public void MyGenericTest()
        {
            var buffer = BinarySerializer.Serialize(_processorInfo);

            for (int i = 0; i < _count; i++)
            {
                var _ = BinarySerializer.Deserialize<ProcessorDeviceInfo>(buffer);
            }
        }

    }
}
