using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Librame.Extensions.Tests
{
    [TestClass]
    public class SystemExtensionsTests
    {

        [TestMethod]
        public void AppendTest()
        {
            string str = null;
            Assert.AreEqual(string.Empty, str.Append(null));

            str = nameof(SystemExtensionsTests);
            Assert.AreEqual(str, str.Append(null));

            var append = "123456";
            Assert.AreEqual($"{str}{append}", str.Append(append));
        }

        [TestMethod]
        public void InsertTest()
        {
            string str = null;
            Assert.AreEqual(string.Empty, str.Insert(null));

            str = nameof(SystemExtensionsTests);
            Assert.AreEqual(str, str.Insert(null));

            var insert = "123456";
            Assert.AreEqual($"{insert}{str}", str.Insert(insert));

            Assert.AreEqual($"{str.Substring(0, 6)}{insert}{str.Substring(6)}",
                str.Insert(insert, startIndex: 6));

            Assert.AreEqual($"{str}{insert}", str.Insert(insert, startIndex: str.Length));
        }

    }
}
