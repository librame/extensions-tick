using Librame.Extensions.Infrastructure.Configuration;
using System;
using Xunit;

namespace Librame.Extensions.Serialization
{
    public class BinarySerializerTests
    {

        [Fact]
        public void AllTests()
        {
            var modelType = typeof(TestModel);
            var model = new TestModel();
            //{
            //    Id = Guid.NewGuid(),
            //    Count = 100,
            //    Name = "test",
            //    //ByteArray = 32.GenerateByteArray(),
            //    CreateTime = DateTime.Now,
            //    CreateTimeOffset = DateTimeOffset.Now
            //};

            var objectPath = "serialize_binary_object.bin".SetFileBasePath();
            var genericPath = "serialize_binary_generic.bin".SetFileBasePath();

            //TestObject();
            TestGeneric();


            void TestObject()
            {
                BinarySerializer.Serialize(objectPath.ToString(), modelType, model);
                Assert.True(objectPath.Exists());

                var compare = BinarySerializer.Deserialize<TestModel>(objectPath.ToString());
                Assert.NotNull(compare);
                Assert.Equal(model.Id, compare.Id);
                Assert.Equal(model.Count, compare.Count);
                Assert.Equal(model.Name, compare.Name);
                //Assert.Equal(model.ByteArray, compare.ByteArray);
                Assert.Equal(model.CreateTime, compare.CreateTime);
                Assert.Equal(model.CreateTimeOffset, compare.CreateTimeOffset);

                //objectPath.Delete();
            }

            void TestGeneric()
            {
                BinarySerializer.Serialize(genericPath.ToString(), model);
                Assert.True(genericPath.Exists());

                var compare = BinarySerializer.Deserialize<TestModel>(genericPath.ToString());
                Assert.NotNull(compare);
                Assert.Equal(model.Id, compare.Id);
                Assert.Equal(model.Count, compare.Count);
                Assert.Equal(model.Name, compare.Name);
                //Assert.Equal(model.ByteArray, compare.ByteArray);
                Assert.Equal(model.CreateTime, compare.CreateTime);
                Assert.Equal(model.CreateTimeOffset, compare.CreateTimeOffset);

                //genericPath.Delete();
            }
        }


        public class TestModel
        {
            [BinaryOrder(3)]
            public Guid? Id { get; set; }

            [BinaryOrder(2)]
            public int? Count { get; set; }

            [BinaryOrder(1)]
            public string? Name { get; set; }

            //[BinaryOrder(4)]
            [BinaryIgnore]
            [BinaryArray(32)]
            public byte[]? ByteArray { get; set; }

            //[BinaryOrder(5)]
            public DateTime? CreateTime { get; set; }

            //[BinaryOrder(6)]
            public DateTimeOffset CreateTimeOffset { get; set; }
        }

    }
}
