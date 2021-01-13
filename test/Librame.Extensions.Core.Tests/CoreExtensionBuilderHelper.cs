using Microsoft.Extensions.DependencyInjection;
using System;

namespace Librame.Extensions.Core
{
    public static class CoreExtensionBuilderHelper
    {
        private static object _locker = new object();

#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
        private static CoreExtensionBuilder _builder;
#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。

#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
        private static IServiceProvider _services;
#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。

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
