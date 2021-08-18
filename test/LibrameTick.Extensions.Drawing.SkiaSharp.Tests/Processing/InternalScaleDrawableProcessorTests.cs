using Librame.Extensions.Core;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Librame.Extensions.Drawing.Processing
{
    public class InternalScaleDrawableProcessorTests
    {

        [Fact]
        public void DrawScaleTest()
        {
            var options = DrawingExtensionBuilderHelper.CurrentServices
                .GetRequiredService<DrawingExtensionOptions>();

            var processors = DrawingExtensionBuilderHelper.CurrentServices
                .GetRequiredService<IProcessorManager>();

            var imageFile = options.Directories.ResourceDirectory.CombinePath("win11_2160p.jpg");

            // Scale
            var bitmaps = processors.UseScalingProcessor().Process(imageFile.AsBitmaps());

            // Save
            processors.UseSavingProcessor().Process(bitmaps);

            var saveAsPath = bitmaps.First().SaveAsPath;
            Assert.True(saveAsPath?.FileExists());
            //saveAsPath?.FileDelete();
        }

    }
}
