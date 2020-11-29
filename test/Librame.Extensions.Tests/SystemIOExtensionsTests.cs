using System;
using System.IO;
using Xunit;

namespace Librame.Extensions
{
    public class SystemIOExtensionsTests
    {

        #region Path

        [Fact]
        public void CombineRelativePathTest()
        {
            var str = nameof(SystemIOExtensionsTests);
            Assert.Equal($"{str}{Path.DirectorySeparatorChar}", str.CombineRelativePath());
        }


        [Fact]
        public void HasInvalidPathCharsTest()
        {
            var str = nameof(SystemIOExtensionsTests);
            Assert.False(str.HasInvalidPathChars());

            str = str.Append(string.Join(string.Empty, Path.GetInvalidPathChars()));
            Assert.True(str.HasInvalidPathChars());
        }

        [Fact]
        public void HasInvalidFileNameCharsTest()
        {
            var str = nameof(SystemIOExtensionsTests);
            Assert.False(str.HasInvalidFileNameChars());

            str = str.Append(string.Join(string.Empty, Path.GetInvalidFileNameChars()));
            Assert.True(str.HasInvalidFileNameChars());
        }


        [Fact]
        public void TrimDevelopmentRelativePathTest()
        {
            var path = Environment.CurrentDirectory;
            Assert.NotEqual(path, path.TrimDevelopmentRelativePath());
        }

        #endregion

    }
}
