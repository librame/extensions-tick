using Xunit;

namespace Librame.Extensions
{
    public class FileSerializationExtensionsTests
    {
        public class JsonOptions
        {
            public int Id { get; set; }

            public string? Name { get; set; }
        }


        public class BinaryInfo
        {
            private int _field1;

            internal int Field2;

            public int Property1 { get; set; }

            protected int Property2 { get; set; }

            public string? Name { get; set; }


            public void SetField1(int value)
                => _field1 = value;

            public void SetField2(int value)
                => Field2 = value;

            public void SetProperty2(int value)
                => Property2 = value;


            public bool Field1Equals(BinaryInfo other)
                => _field1 == other._field1;

            public bool Property2Equals(BinaryInfo other)
                => Property2 == other.Property2;
        }


        #region Binary

        [Fact]
        public void SerializeBinaryFileTest()
        {
            var info = new BinaryInfo();
            info.SetField1(1);
            info.SetField2(2);
            info.SetProperty2(4);
            info.Name = nameof(BinaryInfo);

            var path = "serialize_binary.dat".SetBasePath();
            path.SerializeBinaryFile(info);

            var info1 = path.DeserializeBinaryFile<BinaryInfo>();
            Assert.True(info.Field1Equals(info1));
            Assert.True(info.Property2Equals(info1));
            Assert.Equal(info.Field2, info1.Field2);
            Assert.Equal(info.Property1, info1.Property1);
            Assert.Equal(info.Name, info1.Name);

            path.FileDelete();
        }

        #endregion


        #region Json

        [Fact]
        public void SerializeJsonFileTest()
        {
            var options = new JsonOptions
            {
                Id = 1,
                Name = nameof(JsonOptions)
            };
            var path = "serialize_json.json".SetBasePath();

            var json = path.SerializeJsonFile(options);
            Assert.NotEmpty(json);

            options = path.DeserializeJsonFile<JsonOptions>();
            Assert.NotNull(options);

            path.FileDelete();
        }

        #endregion

    }
}
