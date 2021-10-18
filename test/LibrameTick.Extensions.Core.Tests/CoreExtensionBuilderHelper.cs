using Librame.Extensions.Core.Storage;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Librame.Extensions.Core
{
    public static class CoreExtensionBuilderHelper
    {
        private static CoreExtensionBuilder _builder;
        private static IServiceProvider _services;


        static CoreExtensionBuilderHelper()
        {
            if (_builder is null)
            {
                var fileProvider = new PhysicalStorageFileProvider(PathExtensions.CurrentDirectoryWithoutDevelopmentRelativeSubpath);

                var services = new ServiceCollection();
                _builder = services.AddLibrameCore(opts => opts.WebRequest.FileProviders.Add(fileProvider));
            }

            if (_services is null)
            {
                _services = _builder.Services.BuildServiceProvider();
            }
        }


        public static CoreExtensionBuilder CurrentBuilder
            => _builder;

        public static IServiceProvider CurrentServices
            => _services;

    }
}
