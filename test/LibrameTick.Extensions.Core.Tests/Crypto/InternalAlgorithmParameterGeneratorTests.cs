using Librame.Extensions.Core;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Librame.Extensions.Infrastructure
{
    public class InternalAlgorithmParameterGeneratorTests
    {
        private IAlgorithmParameterGenerator _generator;


        public InternalAlgorithmParameterGeneratorTests()
        {
            _generator = CoreExtensionBuilderHelper.CurrentServices.GetRequiredService<IAlgorithmParameterGenerator>();
        }


        [Fact]
        public void GenerateKeyTest()
        {
            var buffer = _generator.GenerateKey(8);
            Assert.NotEmpty(buffer);
        }

        [Fact]
        public void GenerateNonceTest()
        {
            var buffer = _generator.GenerateNonce(8);
            Assert.NotEmpty(buffer);
        }

        [Fact]
        public void GenerateTagTest()
        {
            var buffer = _generator.GenerateTag(8);
            Assert.NotEmpty(buffer);
        }

    }
}
