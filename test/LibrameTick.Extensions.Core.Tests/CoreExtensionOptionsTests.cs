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
                TestSaveOptions(opts!);
            };
        }

        private static void TestSaveOptions(IExtensionOptions options)
        {
            var filePath = options.SaveOptionsAsJson();
            Assert.True(filePath.FileExists());
            //filePath.FileDelete();
        }


        [Fact]
        public void AbstractTest()
        {
            Assert.NotNull(_options.ExtensionName);
            Assert.NotNull(_options.ExtensionType);

            Assert.NotEmpty(_options.Directories.BaseDirectory);
            Assert.NotEmpty(_options.Directories.ConfigDirectory);
            Assert.NotEmpty(_options.Directories.ReportDirectory);
            Assert.NotEmpty(_options.Directories.ResourceDirectory);

            TestSaveOptions(_options);
        }

    }
}
