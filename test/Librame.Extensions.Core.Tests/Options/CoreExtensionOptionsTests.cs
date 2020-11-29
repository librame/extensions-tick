using Xunit;

namespace Librame.Extensions.Core.Options
{
    public class CoreExtensionOptionsTests
    {
        private CoreExtensionOptions _options;


        public CoreExtensionOptionsTests()
        {
            _options = new CoreExtensionOptions();
        }


        [Fact]
        public void AbstractTest()
        {
            Assert.NotNull(_options.CurrentType);
            Assert.Null(_options.Parent);
            Assert.NotEmpty(_options.Name);

            Assert.NotEmpty(_options.BaseDirectory);
            Assert.NotEmpty(_options.ConfigDirectory);
            Assert.NotEmpty(_options.ReportDirectory);
            Assert.NotEmpty(_options.ResourceDirectory);
        }

    }
}
