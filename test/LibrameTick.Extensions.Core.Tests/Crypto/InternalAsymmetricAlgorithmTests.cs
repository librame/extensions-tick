using Librame.Extensions.Core;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using Xunit;

namespace Librame.Extensions.Infrastructure
{
    public class InternalAsymmetricAlgorithmTests
    {
        private static Encoding _encoding
            = Encoding.UTF8;

        private static byte[] _buffer
            = _encoding.GetBytes(nameof(InternalAsymmetricAlgorithmTests));

        private IAsymmetricAlgorithm _algorithm;


        public InternalAsymmetricAlgorithmTests()
        {
            _algorithm = CoreExtensionBuilderHelper.CurrentServices.GetRequiredService<IAsymmetricAlgorithm>();
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
