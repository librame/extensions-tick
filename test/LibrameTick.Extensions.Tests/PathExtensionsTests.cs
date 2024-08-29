using System;
using Xunit;

namespace Librame.Extensions
{
    public class PathExtensionsTests
    {

        [Fact]
        public void AppendExtensionTest()
        {
            var path = @"D:\123\456\789.txt.abcd.txt";

            var extension = path.GetExtension();
            Assert.NotNull(extension);

            var appendedPath = path.AppendExtension(".jpg");
            extension = appendedPath.GetExtension();
            Assert.Equal(".jpg", extension);

            appendedPath = path.AppendExtension(appendExtension: null);
            Assert.Equal(path, appendedPath);

            path = @"D:\123\456\789";
            appendedPath = path.AppendExtension(".jpg");
            extension = appendedPath.GetExtension();
            Assert.Equal(".jpg", extension);
        }

    }
}
