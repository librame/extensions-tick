using System;
using Xunit;

namespace Librame.Extensions.Core
{
    public class FileSizeDescriptionTests
    {

        [Fact]
        public void AllTest()
        {
            var fileSize = DateTime.Now.Ticks;
            var unitString = fileSize.FormatSizeStringWithUnit(FileSizeSystem.Binary, FileSizeUnit.PeByte);
            Assert.NotEmpty(unitString);

            var adapterUnitString = fileSize.FormatSizeStringWithAdaptionUnit(FileSizeSystem.Binary);
            Assert.Equal(unitString, adapterUnitString);
        }

    }
}
