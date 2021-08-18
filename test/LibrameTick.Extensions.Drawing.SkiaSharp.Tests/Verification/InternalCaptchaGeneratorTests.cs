using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Librame.Extensions.Drawing.Verification
{
    public class InternalCaptchaGeneratorTests
    {

        [Fact]
        public void DrawFileTest()
        {
            var options = DrawingExtensionBuilderHelper.CurrentServices
                .GetRequiredService<DrawingExtensionOptions>();

            var generator = DrawingExtensionBuilderHelper.CurrentServices
                .GetRequiredService<ICaptchaGenerator>();
            
            var now = DateTime.Now;

            var captchas = new KeyValuePair<string, string>[]
            {
                new("captcha_year", now.ToString("yyyy")),
                new("captcha_time", now.ToString("HHmmss")),
                new("captcha_ticks", now.Ticks.ToString())
            };

            captchas.ForEach(captcha =>
            {
                var filePath = options.ImageDirectory.CombinePath(captcha.Key + options.ImageFormat.Leading('.'));
                generator.DrawFile(captcha.Value, filePath);

                Assert.True(filePath.FileExists());
            });
        }

    }
}
