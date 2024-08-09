using Librame.Extensions.Configuration;
using System.IO;
using Xunit;

namespace Librame.Extensions.Compression
{
    public class CompressorExtensionsTests
    {

        [Fact]
        public void CompressFileTest()
        {
            var originalPath = "compression-original.txt".SetFileBasePath();
            var compressedPath = "compression-compressed.txt".SetFileBasePath();

            var testString = "This is a test string.".Repeat(10);
            originalPath.WriteAll();

            File.WriteAllText(originalPath.ToString(), testString);

            originalPath.CompressFile(compressedPath, CompressorAlgorithm.GZip);
        }

    }
}
