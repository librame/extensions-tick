using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Xunit;

namespace Librame.Extensions.Drawing.Drawers
{
    public class InternalWatermarkDrawerTests
    {

        [Fact]
        public void DrawWatermarkTest()
        {
            var services = DrawingExtensionBuilderHelper.CurrentServices;

            var options = services.GetRequiredService<DrawingExtensionOptions>();

            var watermarkDrawer = services.GetWatermarkDrawer();
            var savingDrawer = services.GetSavingDrawer();

            var imageFile = options.Directories.ResourceDirectory.CombinePath("win11_1200p.jpg");

            // Watermark
            var bitmaps = watermarkDrawer.Draw(imageFile.AsBitmaps());

            // Save
            savingDrawer.Draw(bitmaps);

            var saveAsPath = bitmaps.First().SaveAsPath;
            Assert.True(saveAsPath?.FileExists());
            //saveAsPath?.FileDelete();
        }

    }
}
