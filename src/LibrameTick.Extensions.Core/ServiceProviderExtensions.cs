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
        /// <param name="activateAction">给定的激活动作。</param>
        /// <returns>返回 <see cref="IServiceProvider"/>。</returns>
        public static IServiceProvider UseServiceInitializer(this IServiceProvider services,
            Action<ServiceInitializerActivator> activateAction)
        {
            var activator = services.GetRequiredService<CoreExtensionBuilder>().InitializerActivator;

            using (var scope = services.CreateScope())
            {
                activator.ApplyServiceProvider(scope.ServiceProvider);

                activateAction.Invoke(activator);
            }

            return services;
        }

    }
}
