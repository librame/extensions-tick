using Librame.Extensions.Device;
using Librame.Extensions.Infrastructure;
using Librame.Extensions.Resources;
using System;
using System.Collections.Generic;
using Xunit;

namespace Librame.Extensions.Serialization
{
    public class BinarySerializerTests
    {

        [Fact]
        public void ByteArrayTest()
        {
            var options = new BinarySerializerOptions(enableAlgorithms: true, enableCompressions: true)
            {
                UseVersion = new(3.0)
            };

            var model = TestModel.Create();

            var bytes = BinarySerializer.Serialize(model, options);
            var compare = BinarySerializer.Deserialize<TestModel>(bytes, options);

            Verify(model, compare, options.UseVersion);

            var modelType = typeof(TestModel);
            bytes = BinarySerializer.SerializeObject(model, modelType, options);
            compare = (TestModel?)BinarySerializer.DeserializeObject(bytes, modelType, options);

            Verify(model, compare, options.UseVersion);
        }

        [Fact]
        public void AllStreamTest()
        {
            var monitor = new LocalDeviceMonitor(new());
            var v1_Options = new BinarySerializerOptions(enableAlgorithms: true, enableCompressions: true)
            {
                UseVersion = new(1.0)
            };
            var v2_Options = new BinarySerializerOptions(enableAlgorithms: true, enableCompressions: true)
            {
                UseVersion = new(2.0)
            };

            var processors = new Dictionary<string, ProcessorDeviceInfo> { { "default", monitor.GetProcessor() } };

            var model = TestModel.Create(processors);
            var modelType = typeof(TestModel);

            var objectPath = "serialize_binary_object.bin".SetFileBasePath();
            var genericPath = "serialize_binary_generic.bin".SetFileBasePath();

            TestObject();
            TestGeneric();

            objectPath.Delete();
            genericPath.Delete();


            void TestObject()
            {
                BinarySerializer.SerializeObject(objectPath, model, modelType, v1_Options);
                Assert.True(objectPath.Exists());

                var compare = (TestModel?)BinarySerializer.DeserializeObject(objectPath, modelType, v1_Options);
                Verify(model, compare, v1_Options.UseVersion);
            }

            void TestGeneric()
            {
                BinarySerializer.Serialize(genericPath, model, v2_Options);
                Assert.True(genericPath.Exists());

                var compare = BinarySerializer.Deserialize<TestModel>(genericPath, v2_Options);
                Verify(model, compare, v2_Options.UseVersion);
            }
        }

        void Verify(TestModel model, TestModel? compare, BinarySerializerVersion version)
        {
            Assert.NotNull(compare);
            Assert.Equal(model.Id, compare.Id);
            Assert.Equal(model.Count, compare.Count);
            Assert.Equal(model.Name, compare.Name);
            Assert.Equal(model.ByteArray, compare.ByteArray);
            Assert.Equal(model.MOD, compare.MOD);

            if (version.IsSupported(2.0))
            {
                Assert.Equal(model.CreateTime, compare.CreateTime);
            }

            Assert.Equal(model.CreateTimeOffset, compare.CreateTimeOffset);
        }


        public class TestModel
        {
            [BinaryVersion(1.0)]
            [BinaryOrder(2)]
            public string? Name { get; set; }

            [BinaryVersion(1.0)]
            [BinaryOrder(1)]
            public Guid? Id { get; set; }

            [BinaryVersion(1.0)]
            public int? Count { get; set; }

            [BinaryVersion(1.0)]
            [BinaryArray(32)]
            public byte[]? ByteArray { get; set; }

            [BinaryVersion(1.0)]
            public MonthOfDate MOD { get; set; } = MonthOfDate.March;

            [BinaryVersion(2.0)]
            public DateTime? CreateTime { get; set; }

            [BinaryVersion(1.0)]
            public DateTimeOffset CreateTimeOffset { get; set; }

            [BinaryVersion(1.0)]
            [BinaryMapping(ForValue = true)]
            public Dictionary<string, ProcessorDeviceInfo> Processors { get; set; } = [];

            [BinaryVersion(3.0)]
            [BinaryMapping]
            public DataResource? Resource { get; set; }


            public static TestModel Create(Dictionary<string, ProcessorDeviceInfo>? processors = null)
            {
                var model = new TestModel()
                {
                    Id = Guid.NewGuid(),
                    Count = 100,
                    Name = "test",
                    ByteArray = 32.GenerateByteArray(),
                    CreateTime = DateTime.Now,
                    CreateTimeOffset = DateTimeOffset.Now
                };

                if (processors is not null)
                {
                    model.Processors = processors;
                }

                return model;
            }
        }

    }
}
