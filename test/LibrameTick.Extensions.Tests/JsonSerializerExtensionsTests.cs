using System.IO;
using Xunit;

namespace Librame.Extensions.Tests
{
    public class JsonSerializerExtensionsTests
    {
        public class TestOptions
        {
            public int Id { get; set; }

            public string? Name { get; set; }
        }


        [Fact]
        public void ReadAndWriteJson()
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

            File.Delete(path);
        }

    }
}
