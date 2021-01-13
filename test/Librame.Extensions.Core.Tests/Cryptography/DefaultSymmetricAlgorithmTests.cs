using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Librame.Extensions.Core.Cryptography
{
    public class DefaultSymmetricAlgorithmTests
    {
        private IAlgorithmParameterGenerator _generator;


#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
        public DefaultSymmetricAlgorithmTests()
#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
        {
#pragma warning disable CS8601 // 可能的 null 引用赋值。
            _generator = CoreExtensionBuilderHelper.CurrentServices.GetService<IAlgorithmParameterGenerator>();
#pragma warning restore CS8601 // 可能的 null 引用赋值。
        }


        [Fact]
        public void GenerateKeyTest()
        {
            var buffer = _generator.GenerateKey(8);
        }

    }
}
