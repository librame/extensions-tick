using Xunit;

namespace Librame.Extensions.Core
{
    public class CoreExtensionBuilderTests
    {
        [Fact]
        public void AbstractTest()
        {
            Assert.NotNull(CoreExtensionBuilderHelper.CurrentBuilder.CurrentType);
            Assert.Null(CoreExtensionBuilderHelper.CurrentBuilder.Parent);
            Assert.NotEmpty(CoreExtensionBuilderHelper.CurrentBuilder.Name);

            Assert.NotNull(CoreExtensionBuilderHelper.CurrentBuilder.Services);
            Assert.NotNull(CoreExtensionBuilderHelper.CurrentBuilder.Options);
        }

    }
}
