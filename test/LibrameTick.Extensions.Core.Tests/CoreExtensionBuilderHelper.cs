using Librame.Extensions.Proxy;
using Librame.Extensions.Setting;
using Librame.Extensions.Storage;
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
                var fileProvider = new PhysicalStorableFileProvider(PathExtensions.CurrentDirectoryWithoutDevelopmentRelativeSubpath);

                var services = new ServiceCollection();

                _builder = services.AddLibrame(opts =>
                {
                    opts.WebFile.AccessToken = "Test access token.";
                    opts.WebFile.UserName = "Test user name.";
                    opts.WebFile.Password = "Test password.";
                    opts.WebFile.JwtToken = "Test jwt token.";
                    opts.WebFile.CookieName = "Test cookie name.";

                    opts.WebFile.FileProviders.Add(fileProvider);
                })
                .AddSettingProvider(typeof(TestJsonFileSettingProvider));

                _builder.Services.AddSingleton<ITestProxyService, TestProxyService>();
            }

            _services ??= _builder.Services.BuildServiceProvider();
        }


        public static CoreExtensionBuilder CurrentBuilder
            => _builder;

        public static IServiceProvider CurrentServices
            => _services;

    }
}
