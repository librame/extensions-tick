using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Librame.Extensions.Core.Builders
{
    using Options;

    public class CoreExtensionBuilderTests
    {
        private CoreExtensionBuilder _builder;


        public CoreExtensionBuilderTests()
        {
            _builder = new CoreExtensionBuilder(new ServiceCollection(), new CoreExtensionOptions());
        }


        [Fact]
        public void AbstractTest()
        {
            Assert.NotNull(_builder.CurrentType);
            Assert.Null(_builder.Parent);
            Assert.NotEmpty(_builder.Name);

            Assert.NotNull(_builder.Services);
            Assert.NotNull(_builder.Options);
        }

    }
}
