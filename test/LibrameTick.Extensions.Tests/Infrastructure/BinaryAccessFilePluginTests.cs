using System;
using Xunit;

namespace Librame.Extensions.Infrastructure.Storage
{
    public class BinaryAccessFilePluginTests
    {

        [Fact]
        public void AllTest()
        {
            var filePlugin = "binary_access_file_plugin.bin".SetFileBasePath().UseBinaryAccessFilePlugin();

            var byteArray = Guid.NewGuid().ToByteArray();

            filePlugin.Write(byteArray);

            Assert.True(filePlugin.Path.Exists());

            var buffer = filePlugin.Read();

            Assert.True(byteArray.SequenceEqualByReadOnlySpan(buffer));

            filePlugin.Path.Delete();
        }

    }
}
