using Librame.Extensions.Core;
using Librame.Extensions.Data.Accessors;
using Microsoft.Extensions.DependencyInjection;

namespace Librame.Extensions.Data
{
    public class TestExtensionBuilder : AbstractExtensionBuilder<TestExtensionOptions>
    {
        public TestExtensionBuilder(IExtensionBuilder parentBuilder, TestExtensionOptions options)
            : base(parentBuilder, options)
        {
            Services.AddSingleton(this);

            // Accessors
            AddOrReplaceByCharacteristic<TestAccessorSeeder>();
            AddOrReplaceByCharacteristic<TestReadAccessorInitializer>();
            AddOrReplaceByCharacteristic<TestWriteAccessorInitializer>();
        }

    }
}
