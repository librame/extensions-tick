using Librame.Extensions.Infrastructure;
using System.Text;
using Xunit;

namespace Librame.Extensions.Compression
{
    public class CompressorExtensionsTests
    {

        [Fact]
        public void CompressByteArrayTest()
        {
            var encoding = Encoding.UTF8;

            var testString = "This is a test string.".Repeat(10);
            var testBytes = encoding.GetBytes(testString);

            var compressedBytes = testBytes.Compress();
            var decompressedBytes = compressedBytes.Decompress();

            var decompressedString = encoding.GetString(decompressedBytes);

            Assert.Equal(testString, decompressedString);
        }

        [Fact]
        public void CompressFileTest()
        {
            var originalFile = "compression-original.txt".SetFileBasePath();
            var decompressedFile = "compression-decompressed.txt".SetFileBasePath();

            var testString = "This is a test string.".Repeat(10);

            originalFile.WriteAllText(testString);

            var options = new CompressionOptions()
            {
                Algorithm = CompressorAlgorithm.Brotli,
                UseBufferedStream = true
            };

            var compressedFile = originalFile.Compress(options);
            compressedFile.Decompress(decompressedFile, options);

            var decompressedString = decompressedFile.ReadAllText();
            Assert.Equal(testString, decompressedString);

            decompressedFile.Delete();
            compressedFile.Delete();
            originalFile.Delete();
        }

    }
}
