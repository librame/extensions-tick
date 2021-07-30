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
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data.Accessors
{
    /// <summary>
    /// <see cref="IAccessor"/> 初始化。
    /// </summary>
    public static class AccessorPopulatorServiceProviderExtensions
    {
        private static readonly Type _basePopulatorDefinition
            = typeof(AbstractAccessorPopulator<>);


        /// <summary>
        /// 初始化指定程序集数组内已实现 <see cref="AbstractAccessorPopulator{TAccessor}"/> 的填充器集合。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceProvider"/>。</param>
        /// <param name="assemblies">给定包含 <see cref="IAccessorPopulator"/> 填充器实现的程序集数组。</param>
        /// <returns>返回 <see cref="IServiceProvider"/>。</returns>
        public static IServiceProvider InitializePopulators(this IServiceProvider services,
            params Assembly[] assemblies)
        {
            services.NotNull(nameof(services));

            using (var scope = services.CreateScope())
            {
                var scopeServices = scope.ServiceProvider;

                var pairs = GetAccessorPopulatorTypes(assemblies);
                foreach (var pair in pairs)
                {
                    var accessor = (AbstractAccessor)scopeServices.GetRequiredService(pair.Key);

                    var populator = (IAccessorPopulator)Activator.CreateInstance(pair.Value, accessor)!;

                    populator.Populate(scopeServices);
                }
            }

            return services;
        }

        /// <summary>
        /// 异步初始化指定程序集数组内已实现 <see cref="AbstractAccessorPopulator{TAccessor}"/> 的填充器集合。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceProvider"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <param name="assemblies">给定包含 <see cref="IAccessorPopulator"/> 填充器实现的程序集数组。</param>
        /// <returns>返回一个包含 <see cref="IServiceProvider"/> 的异步操作。</returns>
        public static async Task<IServiceProvider> InitializePopulatorsAsync(this IServiceProvider services,
            CancellationToken cancellationToken = default, params Assembly[] assemblies)
        {
            services.NotNull(nameof(services));

            using (var scope = services.CreateAsyncScope())
            {
                var scopeServices = scope.ServiceProvider;

                var pairs = GetAccessorPopulatorTypes(assemblies);
                foreach (var pair in pairs)
                {
                    var accessor = (AbstractAccessor)scopeServices.GetRequiredService(pair.Key);

                    var populator = (IAccessorPopulator)Activator.CreateInstance(pair.Value, accessor)!;

                    await populator.PopulateAsync(scopeServices, cancellationToken);
                }
            }

            return services;
        }


        private static IReadOnlyDictionary<Type, Type> GetAccessorPopulatorTypes(IEnumerable<Assembly> assemblies)
        {
            var dictionary = new Dictionary<Type, Type>();

            foreach (var assembly in assemblies)
            {
                foreach (var type in assembly.ExportedTypes)
                {
                    if (type.IsImplementedType(_basePopulatorDefinition, out var resultType))
                    {
                        dictionary.Add(resultType!.GenericTypeArguments[0], type);
                    }
                }
            }

            return dictionary;
        }

    }
}
