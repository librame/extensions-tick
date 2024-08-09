using Librame.Extensions.Configuration;
using System;
using Xunit;

namespace Librame.Extensions.Infrastructure.Storage
{
    public class FileAccessorTests
    {

        [Fact]
        public void AllTest()
        {
            var path = "read_binary.bin".SetFileBasePath();
            var file = path.UseFileAccessor();

            var byteArray = Guid.NewGuid().ToByteArray();
            file.Write(byteArray);
            Assert.True(path.Exists());

            var buffer = file.Read();
            Assert.True(byteArray.SequenceEqualByReadOnlySpan(buffer));

            path.Delete();
        }

    }
}
