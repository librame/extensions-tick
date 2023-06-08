using Librame.Extensions.Core;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using Xunit;

namespace Librame.Extensions.Storage
{
    public class InternalWebStorableFileTransferTests
    {
        private IWebStorableFileTransfer _transmission;


        public InternalWebStorableFileTransferTests()
        {
            _transmission = CoreExtensionBuilderHelper.CurrentServices.GetRequiredService<IWebStorableFileTransfer>();
        }


        [Fact]
        public async void DownloadFileAsync()
        {
            var url = "https://www.baidu.com/img/baidu_jgylogo3.gif";
            var filePath = @"d:\baidu_jgylogo3.gif";
            filePath.FileDelete();

            var savePath = await _transmission.DownloadFileAsync(url, filePath).DiscontinueCapturedContext();
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
