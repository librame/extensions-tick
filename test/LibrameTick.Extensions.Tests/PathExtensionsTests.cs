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
        public void CombineRelativePathTest()
        {
            var str = nameof(PathExtensionsTests);
            Assert.Equal($"{str}{Path.DirectorySeparatorChar}", str.CombineRelativePath());
            Assert.Equal($"{Path.DirectorySeparatorChar}{str}", str.CombineRelativePath(pathSeparatorForward: true));
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


        #region TrimRelativePath

        [Fact]
        public void TrimDevelopmentRelativePathTest()
        {
            var path = PathExtensions.CurrentDirectory;
            Assert.NotEqual(path, path.TrimDevelopmentRelativePath());
        }

        #endregion

    }
}
