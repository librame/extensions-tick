using Librame.Extensions.Core;
using Librame.Extensions.Data.Accessors;

namespace Librame.Extensions.Data
{
    public class TestExtensionOptions : AbstractExtensionOptions<TestExtensionOptions>
    {
        public TestExtensionOptions(IExtensionOptions parentOptions)
            : base(parentOptions, parentOptions?.Directories)
        {
            // Accessors
            ServiceCharacteristics.AddScope<TestAccessorSeeder>();
            ServiceCharacteristics.AddScope<TestReadAccessorInitializer>();
            ServiceCharacteristics.AddScope<TestWriteAccessorInitializer>();
        }

    }
}
