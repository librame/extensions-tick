#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.DependencyInjection;
using System;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// <see cref="IServiceProvider"/> 静态扩展。
    /// </summary>
    public static class ServiceProviderExtensions
    {

        /// <summary>
        /// 使用服务初始化器。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceProvider"/>。</param>
        /// <param name="setupAction">给定的激活器安装动作。</param>
        /// <param name="loadingOptions">给定的 <see cref="AssemblyLoadingOptions"/>（可选）。</param>
        /// <returns>返回 <see cref="IServiceProvider"/>。</returns>
        public static IServiceProvider UseServiceInitializer(this IServiceProvider services,
            Action<ServiceInitializerActivator> setupAction,
            AssemblyLoadingOptions? loadingOptions = null)
        {
            services.NotNull(nameof(services));

            using (var scope = services.CreateScope())
            {
                var supplier = new ServiceInitializerActivator(scope.ServiceProvider, loadingOptions);
                setupAction.Invoke(supplier);
            }

            return services;
        }

    }
}
