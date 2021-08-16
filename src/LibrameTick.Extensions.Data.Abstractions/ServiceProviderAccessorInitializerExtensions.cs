#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Data.Accessing;
using Microsoft.Extensions.DependencyInjection;

namespace Librame.Extensions.Data
{
    /// <summary>
    /// <see cref="IServiceProvider"/> 与 <see cref="IAccessorInitializer"/> 静态扩展。
    /// </summary>
    public static class ServiceProviderAccessorInitializerExtensions
    {

        /// <summary>
        /// 使用 <see cref="IAccessorInitializer"/>。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceProvider"/>。</param>
        /// <returns>返回 <see cref="IServiceProvider"/>。</returns>
        public static IServiceProvider UseAccessorInitializer(this IServiceProvider services)
        {
            var initializers = services.GetRequiredService<IEnumerable<IAccessorInitializer>>();
            initializers.ForEach(a => a.Initialize(services));

            return services;
        }

    }
}
