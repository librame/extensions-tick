using System;
using Xunit;

namespace Librame.Extensions.Storage
{
    public class FileSizeDescriptionTests
    {
        [Fact]
        public void AllTest()
        {
            var fileSize = DateTime.Now.Ticks;
            var unitString = fileSize.FormatSizeWithUnit(FileSizeSystem.Binary, FileSizeUnit.PeByte);
            Assert.NotEmpty(unitString);

            var adapterUnitString = fileSize.FormatSizeWithUnit(FileSizeSystem.Binary);
            Assert.Equal(unitString, adapterUnitString);
        }

    }
}
