using Xunit;

namespace Librame.Extensions.Core
{
    public class CoreExtensionBuilderTests
    {
        [Fact]
        public void AbstractTest()
        {
            Assert.NotNull(CoreExtensionBuilderHelper.CurrentBuilder.InfoType);
            Assert.Null(CoreExtensionBuilderHelper.CurrentBuilder.ParentInfo);
            Assert.NotEmpty(CoreExtensionBuilderHelper.CurrentBuilder.Name);

            Assert.NotNull(CoreExtensionBuilderHelper.CurrentBuilder.Services);
            Assert.NotNull(CoreExtensionBuilderHelper.CurrentBuilder.Options);
        }

    }
}
