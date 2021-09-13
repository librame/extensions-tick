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

namespace Librame.Extensions.Core
{
    /// <summary>
    /// <see cref="IServiceProvider"/> 与 <see cref="AssemblyAutoloaderActivator"/> 静态扩展。
    /// </summary>
    public static class ServiceProviderAutoloaderActivatorExtensions
    {

        /// <summary>
        /// 使用 <see cref="IAutoloader"/> 激活器。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceProvider"/>。</param>
        /// <param name="activateAction">给定的激活动作。</param>
        /// <returns>返回 <see cref="IServiceProvider"/>。</returns>
        public static IServiceProvider UseAutoloaderActivator(this IServiceProvider services,
            Action<AssemblyAutoloaderActivator> activateAction)
        {
            var activator = services.GetRequiredService<CoreExtensionBuilder>().AutoloaderActivator;
            if (activator == null)
                throw new ArgumentException($"The {nameof(CoreExtensionBuilder)}.{nameof(CoreExtensionBuilder.AutoloaderActivator)} is null. You need enable ${nameof(CoreExtensionOptions)}.{nameof(CoreExtensionOptions.EnableAutoloaderActivator)}.");
            
            activator.ApplyServiceProvider(services);

            activateAction.Invoke(activator);

            return services;
        }

    }
}
