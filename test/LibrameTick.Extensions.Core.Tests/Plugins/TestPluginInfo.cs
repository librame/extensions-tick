using Microsoft.Extensions.Localization;

namespace Librame.Extensions.Core.Plugins
{
    public class TestPluginInfo : AbstractLibramePluginInfo<TestPluginInfo>
    {
        public override string Name
            => nameof(TestPluginInfo);

        public override IStringLocalizer? Localizer
            => new PluginResourceStringLocalizer<TestPluginResource>();
    }
}
