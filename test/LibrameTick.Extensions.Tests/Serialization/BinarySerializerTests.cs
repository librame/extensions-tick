using Librame.Extensions.Configuration;
using Librame.Extensions.Device;
using System;
using System.Collections.Generic;
using Xunit;

namespace Librame.Extensions.Serialization
{
    public class BinarySerializerTests
    {

        [Fact]
        public void AllTests()
        {
            var monitor = new LocalDeviceMonitor(new());
            var version1Options = new BinarySerializerOptions() { UseVersion = new(1.0) };
            var version2Options = new BinarySerializerOptions() { UseVersion = new(2.0) };

            var modelType = typeof(TestModel);
            var model = new TestModel()
            {
                Id = Guid.NewGuid(),
                Count = 100,
                Name = "test",
                ByteArray = 32.GenerateByteArray(),
                CreateTime = DateTime.Now,
                CreateTimeOffset = DateTimeOffset.Now,
                Dict = new Dictionary<string, ProcessorDeviceInfo> { { "default", monitor.GetProcessor() } }
            };

            var objectPath = "serialize_binary_object.bin".SetFileBasePath();
            var genericPath = "serialize_binary_generic.bin".SetFileBasePath();

            TestObject();
            TestGeneric();


            void TestObject()
            {
                BinarySerializer.SerializeObject(objectPath.ToString(), modelType, model, version1Options);
                Assert.True(objectPath.Exists());

                var compare = BinarySerializer.DeserializeObject(objectPath.ToString(), modelType, default,
                    version1Options) as TestModel;
                Assert.NotNull(compare);
                Assert.Equal(model.Id, compare.Id);
                Assert.Equal(model.Count, compare.Count);
                Assert.Equal(model.Name, compare.Name);
                Assert.Equal(model.ByteArray, compare.ByteArray);
                Assert.Equal(model.MOD, compare.MOD);
                Assert.NotEqual(model.CreateTime, compare.CreateTime);
                Assert.Equal(model.CreateTimeOffset, compare.CreateTimeOffset);

                objectPath.Delete();
            }

            void TestGeneric()
            {
                BinarySerializer.Serialize(genericPath.ToString(), model, version2Options);
                Assert.True(genericPath.Exists());

                var compare = BinarySerializer.Deserialize<TestModel>(genericPath.ToString(), default, version2Options);
                Assert.NotNull(compare);
                Assert.Equal(model.Id, compare.Id);
                Assert.Equal(model.Count, compare.Count);
                Assert.Equal(model.Name, compare.Name);
                Assert.Equal(model.ByteArray, compare.ByteArray);
                Assert.Equal(model.MOD, compare.MOD);
                Assert.Equal(model.CreateTime, compare.CreateTime);
                Assert.Equal(model.CreateTimeOffset, compare.CreateTimeOffset);

                genericPath.Delete();
            }
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
            [BinaryExpressionMapping(ForValue = true)]
            public Dictionary<string, ProcessorDeviceInfo> Dict { get; set; } = [];
        }

    }
}
