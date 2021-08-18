using Xunit;

namespace Librame.Extensions
{
    public class PathExtensionsTests
    {

        [Fact]
        public void CreateDirectory()
        {
            var path = "subdir".SetBasePath();
            path.CreateDirectory();

            Assert.True(Directory.Exists(path));

            Directory.Delete(path);
        }

        [Fact]
        public void SetBasePath()
        {
            var str = "relativePath";
            Assert.NotEqual(str, str.SetBasePath());
        }


        #region CombinePath

        [Fact]
        public void CombinePathTest()
        {
            var root = PathExtensions.CurrentDirectory;
            var str = nameof(PathExtensionsTests);
            var path = root.CombinePath(str);

            Assert.StartsWith(root, path);
            Assert.EndsWith(str, path);
        }

        [Fact]
        public void CombineRelativeSubpathTest()
        {
            var separator = Path.DirectorySeparatorChar;

            var folderName = nameof(PathExtensionsTests);
            Assert.Equal($"{folderName}{separator}", folderName.CombineRelativeSubpath());
            Assert.Equal($"{separator}{folderName}", folderName.CombineRelativeSubpath(pathSeparatorForward: true));

            var folderNames = new string[] { folderName, "test" };
            var relativeSubdirectory = folderNames.CombineRelativeSubpath();
            Assert.StartsWith($"{folderName}{separator}", relativeSubdirectory);
            Assert.EndsWith($"{"test"}{separator}", relativeSubdirectory);
        }

        #endregion


        #region InvalidPathChars

        [Fact]
        public void HasInvalidPathCharsTest()
        {
            var str = nameof(PathExtensionsTests);
            Assert.False(str.HasInvalidPathChars());

            str = str.Append(Path.GetInvalidPathChars().JoinString());
            Assert.True(str.HasInvalidPathChars());
        }

        [Fact]
        public void HasInvalidFileNameCharsTest()
        {
            var str = nameof(PathExtensionsTests);
            Assert.False(str.HasInvalidFileNameChars());

            str = str.Append(Path.GetInvalidFileNameChars().JoinString());
            Assert.True(str.HasInvalidFileNameChars());
        }

        #endregion


        #region TrimRelativeSubpath

        [Fact]
        public void TrimDevelopmentRelativeSubpathTest()
        {
            var path = PathExtensions.CurrentDirectory;
            Assert.NotEqual(path, path.TrimDevelopmentRelativeSubpath());
        }

        #endregion

    }
}
