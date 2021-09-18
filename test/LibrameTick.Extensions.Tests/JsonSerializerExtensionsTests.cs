using Xunit;

namespace Librame.Extensions
{
    public class JsonSerializerExtensionsTests
    {
        public class TestOptions
        {
            public int Id { get; set; }

            public string? Name { get; set; }
        }


        #region ReadJson and WriteJson

        [Fact]
        public void ReadJsonAndWriteJsonTest()
        {
            var options = new TestOptions
            {
                Id = 1,
                Name = nameof(TestOptions)
            };
            var path = "test.json".SetBasePath();

            var json = path.WriteJson(options);
            Assert.NotEmpty(json);

            options = path.ReadJson<TestOptions>();
            Assert.NotNull(options);

            path.FileDelete();
        }

        #endregion

    }
}
