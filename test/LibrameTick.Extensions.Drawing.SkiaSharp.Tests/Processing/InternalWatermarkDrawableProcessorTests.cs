using Librame.Extensions.Core;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Librame.Extensions.Drawing.Processing
{
    public class InternalWatermarkDrawableProcessorTests
    {

        [Fact]
        public void DrawWatermarkTest()
        {
            var options = DrawingExtensionBuilderHelper.CurrentServices
                .GetRequiredService<DrawingExtensionOptions>();

            var processors = DrawingExtensionBuilderHelper.CurrentServices
                .GetRequiredService<IProcessorManager>();

            var imageFile = options.Directories.ResourceDirectory.CombinePath("win11_1200p.jpg");

            // Watermark
            var bitmaps = processors.UseWatermarkProcessor().Process(imageFile.AsBitmaps());

            // Save
            processors.UseSavingProcessor().Process(bitmaps);

            var saveAsPath = bitmaps.First().SaveAsPath;
            Assert.True(saveAsPath?.FileExists());
            //saveAsPath?.FileDelete();
        }

    }
}
