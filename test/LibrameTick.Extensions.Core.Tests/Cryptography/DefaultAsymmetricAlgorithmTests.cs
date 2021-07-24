using Microsoft.Extensions.DependencyInjection;
using System.Text;
using Xunit;

namespace Librame.Extensions.Core.Cryptography
{
    public class DefaultAsymmetricAlgorithmTests
    {
        private static Encoding _encoding
            = Encoding.UTF8;

        private static byte[] _buffer
            = _encoding.GetBytes(nameof(DefaultAsymmetricAlgorithmTests));

        private IAsymmetricAlgorithm _algorithm;


        public DefaultAsymmetricAlgorithmTests()
        {
            _algorithm = CoreExtensionBuilderHelper.CurrentServices.GetRequiredService<IAsymmetricAlgorithm>()!;
        }


        [Fact]
        public void AesTest()
        {
            var buffer = _algorithm.EncryptRsa(_buffer);

            buffer = _algorithm.DecryptRsa(buffer);

            Assert.NotEmpty(_encoding.GetString(buffer));
        }

    }
}
