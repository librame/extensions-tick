using Xunit;

namespace Librame.Extensions.Configuration
{
    public class PathConfigurableTests
    {

        [Fact]
        public void AllTest()
        {
            var dirPath = "testdir".SetDirectoryBasePath();
            Assert.False(dirPath.Exists());

            dirPath.EnsureDirectory();
            Assert.True(dirPath.Exists());

            var filePath = "testfile.json".SetFileBasePath(dirPath);
            Assert.False(filePath.Exists());

            filePath.ConfigureFileExtension(".txt");
            Assert.False(filePath.Exists());

            dirPath.Delete();
            Assert.False(dirPath.Exists());
        }

    }
}
