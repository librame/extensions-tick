using Librame.Extensions.Core;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Librame.Extensions.Data
{
    public static class DataExtensionBuilderHelper
    {
        private static DataExtensionBuilder _builder;
        private static IServiceProvider _services;


        static DataExtensionBuilderHelper()
        {
            if (_builder is null)
            {
                var services = new ServiceCollection();
                _builder = services.AddLibrame().AddData();
            }

            if (_services is null)
            {
                _services = _builder.Services.BuildServiceProvider();
            }
        }


        public static DataExtensionBuilder CurrentBuilder
            => _builder;

        public static IServiceProvider CurrentServices
            => _services;

    }
}
