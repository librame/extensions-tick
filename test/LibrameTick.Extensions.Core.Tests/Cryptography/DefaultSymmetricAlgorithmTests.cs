using Microsoft.Extensions.DependencyInjection;
using System.Text;
using Xunit;

namespace Librame.Extensions.Core.Cryptography
{
    public class DefaultSymmetricAlgorithmTests
    {
        private static Encoding _encoding
            = Encoding.UTF8;
        private static byte[] _buffer
            = _encoding.GetBytes(nameof(DefaultSymmetricAlgorithmTests));

        private ISymmetricAlgorithm? _algorithm;


        public DefaultSymmetricAlgorithmTests()
        {
            _algorithm = CoreExtensionBuilderHelper.CurrentServices.GetService<ISymmetricAlgorithm>();
        }


        [Fact]
        public void AesTest()
        {
            var buffer = _algorithm?.EncryptAes(_buffer);

#pragma warning disable CS8604 // 可能的 null 引用参数。
            buffer = _algorithm?.DecryptAes(buffer);
#pragma warning restore CS8604 // 可能的 null 引用参数。

#pragma warning disable CS8604 // 可能的 null 引用参数。
            Assert.NotEmpty(_encoding.GetString(buffer));
#pragma warning restore CS8604 // 可能的 null 引用参数。
        }

        [Fact]
        public void AesCcmTest()
        {
            var buffer = _algorithm?.EncryptAesCcm(_buffer);

#pragma warning disable CS8604 // 可能的 null 引用参数。
            buffer = _algorithm?.DecryptAesCcm(buffer);
#pragma warning restore CS8604 // 可能的 null 引用参数。

#pragma warning disable CS8604 // 可能的 null 引用参数。
            Assert.NotEmpty(_encoding.GetString(buffer));
#pragma warning restore CS8604 // 可能的 null 引用参数。
        }

        [Fact]
        public void AesGcmTest()
        {
            var buffer = _algorithm?.EncryptAesGcm(_buffer);

#pragma warning disable CS8604 // 可能的 null 引用参数。
            buffer = _algorithm?.DecryptAesGcm(buffer);
#pragma warning restore CS8604 // 可能的 null 引用参数。

#pragma warning disable CS8604 // 可能的 null 引用参数。
            Assert.NotEmpty(_encoding.GetString(buffer));
#pragma warning restore CS8604 // 可能的 null 引用参数。
        }

    }
}
