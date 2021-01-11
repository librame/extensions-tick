using Microsoft.Extensions.DependencyInjection;
using System;

namespace Librame.Extensions.Core
{
    public static class CoreExtensionBuilderHelper
    {
        private static object _locker = new object();

        private static CoreExtensionBuilder _builder;

        private static IServiceProvider _services;

        public static CoreExtensionBuilder CurrentBuilder
        {
            get
            {
                if (_builder is null)
                {
                    lock (_locker)
                    {
                        if (_builder is null)
                        {
                            var services = new ServiceCollection();
                            _builder = services.AddLibrame();
                        }
                    }
                }

                return _builder;
            }
        }

        public static IServiceProvider CurrentServices
        {
            get
            {
                if (_services is null)
                {
                    lock (_locker)
                    {
                        if (_services is null)
                        {
                            _services = CurrentBuilder.BuildServiceProvider();
                        }
                    }
                }

                return _services;
            }
        }

    }
}
