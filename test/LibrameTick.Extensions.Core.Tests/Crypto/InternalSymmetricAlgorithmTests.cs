using Librame.Extensions.Core;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using Xunit;

namespace Librame.Extensions.Crypto
{
    public class InternalSymmetricAlgorithmTests
    {
        private static Encoding _encoding
            = Encoding.UTF8;

        private static byte[] _buffer
            = _encoding.GetBytes(nameof(InternalSymmetricAlgorithmTests));

        private ISymmetricAlgorithm _algorithm;


        public InternalSymmetricAlgorithmTests()
        {
            _algorithm = CoreExtensionBuilderHelper.CurrentServices.GetRequiredService<ISymmetricAlgorithm>();
        }


        [Fact]
        public void AesTest()
        {
            var buffer = _algorithm.EncryptAes(_buffer);

            buffer = _algorithm.DecryptAes(buffer);

            Assert.NotEmpty(_encoding.GetString(buffer));
        }

        [Fact]
        public void AesCcmTest()
        {
            var buffer = _algorithm.EncryptAesCcm(_buffer);

            buffer = _algorithm.DecryptAesCcm(buffer);

            Assert.NotEmpty(_encoding.GetString(buffer));
        }

        [Fact]
        public void AesGcmTest()
        {
            var buffer = _algorithm.EncryptAesGcm(_buffer);

            buffer = _algorithm.DecryptAesGcm(buffer);

            Assert.NotEmpty(_encoding.GetString(buffer));
        }

    }
}
