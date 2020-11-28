using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace Librame.Extensions.Tests
{
    [TestClass]
    public class SystemIOExtensionsTests
    {

        [TestMethod]
        public void HasInvalidPathCharsTest()
        {
            var str = nameof(SystemIOExtensionsTests);
            Assert.IsFalse(str.HasInvalidPathChars());

            str = str.Append(string.Join(string.Empty, Path.GetInvalidPathChars()));
            Assert.IsTrue(str.HasInvalidPathChars());
        }

        [TestMethod]
        public void HasInvalidFileNameCharsTest()
        {
            var str = nameof(SystemIOExtensionsTests);
            Assert.IsFalse(str.HasInvalidFileNameChars());

            str = str.Append(string.Join(string.Empty, Path.GetInvalidFileNameChars()));
            Assert.IsTrue(str.HasInvalidFileNameChars());
        }

        [TestMethod]
        public void CombineRelativePathTest()
        {
            var str = nameof(SystemIOExtensionsTests);
            Assert.AreEqual($"{str}{Path.DirectorySeparatorChar}", str.CombineRelativePath());
        }

        [TestMethod]
        public void TrimDevelopmentRelativePathTest()
        {
            var path = Environment.CurrentDirectory;
            Assert.AreNotEqual(path, path.TrimDevelopmentRelativePath());
        }

    }
}
