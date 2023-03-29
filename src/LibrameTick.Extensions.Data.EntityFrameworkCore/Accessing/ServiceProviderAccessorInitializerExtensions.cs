#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Data.Sharding;

namespace Librame.Extensions.Data.Accessing;

/// <summary>
/// <see cref="IServiceProvider"/> 与 <see cref="IAccessorInitializer"/> 静态扩展。
/// </summary>
public static class ServiceProviderAccessorInitializerExtensions
{
    private static bool _isInitialized = false;


    /// <summary>
    /// 使用 <see cref="IAccessorInitializer"/>。
    /// </summary>
    /// <param name="services">给定的 <see cref="IServiceProvider"/>。</param>
    /// <returns>返回 <see cref="IServiceProvider"/>。</returns>
    public static IServiceProvider UseAccessorInitializer(this IServiceProvider services)
    {
        if (!_isInitialized)
        {
            var initializer = services.GetRequiredService<IAccessorInitializer>();
            var manager = services.GetRequiredService<IAccessorManager>();

            foreach (var accessor in manager.ResolvedAccessors)
            {
                // 初始尝试对所有存取器分库
                manager.ShardingManager.ShardDatabase(accessor);

                initializer.Initialize(accessor, services);
            }

            _isInitialized = true;
        }

        return services;
    }

}
