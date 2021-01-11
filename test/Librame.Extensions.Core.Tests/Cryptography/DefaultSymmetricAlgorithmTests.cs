using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Librame.Extensions.Core.Cryptography
{
    public class DefaultSymmetricAlgorithmTests
    {
        private IAlgorithmParameterGenerator _generator;


        public DefaultSymmetricAlgorithmTests()
        {
            _generator = CoreExtensionBuilderHelper.CurrentServices.GetService<IAlgorithmParameterGenerator>();
        }


        [Fact]
        public void GenerateKeyTest()
        {
            var buffer = _generator.GenerateKey(8);
        }

    }
}
