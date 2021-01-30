using Xunit;

namespace Librame.Extensions.Core
{
    public class CoreExtensionOptionsTests
    {
        private CoreExtensionOptions _options;


        public CoreExtensionOptionsTests()
        {
            _options = new CoreExtensionOptions();

            //_options.PropertyChangedAction = (opts, e) =>
            //{
            //    var filePath = opts.SaveAsJson();
            //    Assert.True(File.Exists(filePath));
            //};
        }


        [Fact]
        public void AbstractTest()
        {
            Assert.NotNull(_options.CurrentType);
            Assert.NotEmpty(_options.Name);
            Assert.Null(_options.Parent);

            Assert.NotNull(_options.Encoding);
            Assert.NotNull(_options.Algorithms);

            Assert.NotEmpty(_options.Directories.BaseDirectory);
            Assert.NotEmpty(_options.Directories.ConfigDirectory);
            Assert.NotEmpty(_options.Directories.ReportDirectory);
            Assert.NotEmpty(_options.Directories.ResourceDirectory);

            //Assert.Null(_options.Algorithms.Aes.Key);
            //_options.Algorithms.Aes.Key = AlgorithmUtility.RunAes(aes => aes.Key);
            //Assert.NotNull(_options.Algorithms.Aes.Key);
        }

    }
}
