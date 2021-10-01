using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Xunit;

namespace Librame.Extensions.Drawing.Drawers
{
    public class InternalScaleDrawerTests
    {

        [Fact]
        public void DrawScaleTest()
        {
            var services = DrawingExtensionBuilderHelper.CurrentServices;

            var options = services.GetRequiredService<DrawingExtensionOptions>();

            var scalingDrawer = services.GetScalingDrawer();
            var savingDrawer = services.GetSavingDrawer();

            var imageFile = options.Directories.ResourceDirectory.CombinePath("win11_2160p.jpg");

            // Scale
            var bitmaps = scalingDrawer.Draw(imageFile.AsBitmaps());

            // Save
            savingDrawer.Draw(bitmaps);

            var saveAsPath = bitmaps.First().SaveAsPath;
            Assert.True(saveAsPath?.FileExists());
            //saveAsPath?.FileDelete();
        }

    }
}
