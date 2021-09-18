using Microsoft.Extensions.DependencyInjection;
using System.IO;
using Xunit;

namespace Librame.Extensions.Core.Storage
{
    public class InternalFileTransmissionTests
    {
        private IFileTransmission _transmission;


        public InternalFileTransmissionTests()
        {
            _transmission = CoreExtensionBuilderHelper.CurrentServices.GetRequiredService<IFileTransmission>();
        }


        [Fact]
        public async void DownloadFileAsync()
        {
            var url = "https://www.baidu.com/img/baidu_jgylogo3.gif";
            var filePath = @"d:\baidu_jgylogo3.gif";
            filePath.FileDelete();

            var savePath = await _transmission.DownloadFileAsync(url, filePath).ConfigureAwait();
            Assert.True(savePath.FileExists());
        }

        [Fact]
        public void UploadFileAsync()
        {
            var url = "https://domain.com/api/upload";
            var filePath = @"d:\_never.txt";

            Assert.ThrowsAsync<FileNotFoundException>(() =>
            {
                return _transmission.UploadFileAsync(url, filePath);
            });
        }

    }
}
