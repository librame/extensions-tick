using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Linq;
using Xunit;

namespace Librame.Extensions.Core.Storage
{
    public class InternalStorableFileManagerTests
    {

        [Fact]
        public async void AllTest()
        {
            var manager = CoreExtensionBuilderHelper.CurrentServices.GetRequiredService<IStorableFileManager>();

            var now = DateTime.Now;
            var subdir = Directory.CreateDirectory(now.Ticks.ToString().SetBasePath());

            var file = Path.Combine(subdir.FullName, "test.txt");
            var text = $"Now: {now}";
            File.WriteAllText(file, text);

            var contents = await manager.GetDirectoryContentsAsync(subdir.Name).ConfigureAwait();
            Assert.NotEmpty(contents);

            var fileInfo = (IStorableFileInfo)contents.First();
            Assert.Equal(file, fileInfo.PhysicalPath);

            // file to copyFile
            var copyFile = Path.Combine(subdir.FullName, "copy_test.txt");

            using (var writeStream = new FileStream(copyFile, FileMode.Create))
            {
                await manager.ReadAsync(fileInfo, writeStream).ConfigureAwait();
            }
            Assert.Equal(text, File.ReadAllText(copyFile));

            File.Delete(file);

            // copyFile to file
            using (var readStream = new FileStream(copyFile, FileMode.Open))
            {
                await manager.WriteAsync(fileInfo, readStream).ConfigureAwait();
            }
            Assert.Equal(text, File.ReadAllText(fileInfo.PhysicalPath));

            File.Delete(copyFile);
            File.Delete(file);
            Directory.Delete(subdir.FullName);
        }

    }
}
