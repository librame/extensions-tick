using Xunit;

namespace Librame.Extensions.Core
{
    public class CoreExtensionOptionsTests
    {

        [Fact]
        public void AllTest()
        {
            var options = new CoreExtensionOptions();

            Assert.NotNull(options.ExtensionName);
            Assert.NotNull(options.ExtensionType);

            Assert.NotEmpty(options.Directories.BaseDirectory);
            Assert.NotEmpty(options.Directories.ConfigDirectory);
            Assert.NotEmpty(options.Directories.ReportDirectory);
            Assert.NotEmpty(options.Directories.ResourceDirectory);

            var filePath = options.ExportAsJson();
            Assert.True(filePath.FileExists());
            filePath.FileDelete();
        }

    }
}
