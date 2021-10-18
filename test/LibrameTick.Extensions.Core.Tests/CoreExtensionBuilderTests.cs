using Xunit;

namespace Librame.Extensions.Core
{
    public class CoreExtensionBuilderTests
    {

        [Fact]
        public void AllTest()
        {
            Assert.NotNull(CoreExtensionBuilderHelper.CurrentBuilder.ExtensionName);
            Assert.NotNull(CoreExtensionBuilderHelper.CurrentBuilder.ExtensionType);
            Assert.NotNull(CoreExtensionBuilderHelper.CurrentBuilder.Services);
        }

    }
}
