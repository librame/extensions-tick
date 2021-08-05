using Librame.Extensions.Core;
using System;

namespace Librame.Extensions.Data
{
    static class TestExtensionBuilderExtensions
    {
        public static TestExtensionBuilder AddTest(this IExtensionBuilder parent,
            Action<TestExtensionOptions>? setupAction = null)
        {
            var options = new TestExtensionOptions(parent.Options);
            setupAction?.Invoke(options);

            return new TestExtensionBuilder(parent, options);
        }

    }
}
