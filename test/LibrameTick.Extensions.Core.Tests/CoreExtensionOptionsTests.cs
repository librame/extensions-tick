using Xunit;

namespace Librame.Extensions.Core
{
    public class CoreExtensionOptionsTests
    {
        private CoreExtensionOptions _options;


        public CoreExtensionOptionsTests()
        {
            _options = new CoreExtensionOptions();

            _options.PropertyChangedAction = (opts, e) =>
            {
                var filePath = opts.SaveOptionsAsJson().First().Key;
                Assert.True(File.Exists(filePath));
                File.Delete(filePath);
            };
        }


        [Fact]
        public void AbstractTest()
        {
            Assert.NotNull(_options.InfoType);
            Assert.NotEmpty(_options.Name);
            Assert.Null(_options.ParentOptions);

            Assert.NotNull(_options.Encoding);
            Assert.NotNull(_options.Algorithms);

            Assert.NotEmpty(_options.Directories.BaseDirectory);
            Assert.NotEmpty(_options.Directories.ConfigDirectory);
            Assert.NotEmpty(_options.Directories.ReportDirectory);
            Assert.NotEmpty(_options.Directories.ResourceDirectory);
        }

    }
}
