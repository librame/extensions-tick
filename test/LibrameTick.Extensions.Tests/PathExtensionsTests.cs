using System;
using System.IO;
using System.Linq;
using Xunit;

namespace Librame.Extensions
{
    public class BinaryInfo
    {
        private int _field1;

        internal int Field2;

        public int Property1 { get; set; }

        protected int Property2 { get; set; }

        public string? Name { get; set; }


        public void SetField1(int value)
            => _field1 = value;

        public void SetField2(int value)
            => Field2 = value;

        public void SetProperty2(int value)
            => Property2 = value;


        public bool Field1Equals(BinaryInfo other)
            => _field1 == other._field1;

        public bool Property2Equals(BinaryInfo other)
            => Property2 == other.Property2;
    }


    public class PathExtensionsTests
    {

        [Fact]
        public void CreateDirectoryTest()
        {
            var path = "subdir".SetBasePath();
            path.CreateDirectory();

            Assert.True(Directory.Exists(path));

            Directory.Delete(path);
        }

        [Fact]
        public void SetBasePathTest()
        {
            var str = "relativePath";
            Assert.NotEqual(str, str.SetBasePath());
        }


        #region BinaryFileRead & BinaryFileWrite

        [Fact]
        public void BinaryFileReadAndBinaryWriteTest()
        {
            var info = new BinaryInfo();
            info.SetField1(1);
            info.SetField2(2);
            info.SetProperty2(4);
            info.Name = nameof(BinaryInfo);

            var path = "binary_test.dat".SetBasePath();
            path.BinaryFileWrite(info);

            var info1 = path.BinaryFileRead<BinaryInfo>();
            Assert.True(info.Field1Equals(info1));
            Assert.True(info.Property2Equals(info1));
            Assert.Equal(info.Field2, info1.Field2);
            Assert.Equal(info.Property1, info1.Property1);
            Assert.Equal(info.Name, info1.Name);

            path.FileDelete();
        }

        #endregion


        #region FileRead & FileWrite

        [Fact]
        public void FileReadAndWriteTest()
        {
            var byteArray = Guid.NewGuid().ToByteArray();
            var path = "binary_test.bin".SetBasePath();

            path.FileWrite(byteArray);
            Assert.True(path.FileExists());

            var buffer = path.FileRead();
            Assert.True(byteArray.SequenceEqual(buffer));

            path.FileDelete();
        }

        #endregion


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
