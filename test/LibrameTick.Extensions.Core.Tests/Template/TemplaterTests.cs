using Microsoft.Extensions.Configuration;
using Xunit;

namespace Librame.Extensions.Template
{
    public class TemplaterTests
    {
        [Fact]
        public void AllTests()
        {
            var configuration = ConfigurationBuilderExtensions.GetConfiguration(builder =>
            {
                builder.AddAppSettingsJsonFile();
            });

            var finder = Templating.GetConfigurationTemplateKeyFinder();
            Assert.NotEmpty(finder.AllNames);

            foreach (var section in configuration.GetChildren())
            {
                // 支持键引用
                var result = finder.Format(section.Key, out var replaced);
                if (replaced)
                    Assert.NotEqual(section.Key, result);
                else
                    Assert.Equal(section.Key, result);

                // 支持值引用
                result = finder.Format(section.Value, out replaced);

                if (replaced)
                    Assert.NotEqual(section.Value, result);
                else
                    Assert.Equal(section.Value, result);
            }
        }
        
    }
}
