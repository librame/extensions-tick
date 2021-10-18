using Microsoft.Extensions.Localization;
using Xunit;

namespace Librame.Extensions.Resources
{
    public class ResourceDictionaryStringLocalizerTests
    {
        [Fact]
        public void AllTest()
        {
            var factory = new ResourceDictionaryFactory<TestResourceDictionary>();
            var localizer = new ResourceDictionaryStringLocalizer<TestResourceDictionary>(factory);

            var localizedString = localizer.GetString(p => p.TestName);
            Assert.False(localizedString.ResourceNotFound);
            Assert.NotEmpty(localizedString.Value);
        }

    }
}
