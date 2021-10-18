using Microsoft.Extensions.DependencyInjection;
using System;

namespace Librame.Extensions.Drawing
{
    public static class DrawingExtensionBuilderHelper
    {
        private static DrawingExtensionBuilder _builder;
        private static IServiceProvider _services;


        static DrawingExtensionBuilderHelper()
        {
            if (_builder is null)
            {
                var services = new ServiceCollection();
                _builder = services.AddLibrameCore().AddDrawing();
            }

            if (_services is null)
            {
                _services = _builder.Services.BuildServiceProvider();
            }
        }


        public static DrawingExtensionBuilder CurrentBuilder
            => _builder;

        public static IServiceProvider CurrentServices
            => _services;

    }
}
