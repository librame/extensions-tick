#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

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
            var context = services.GetRequiredService<IAccessorContext>();

            foreach (var accessor in context.ResolvedAccessors)
            {
                // 初始尝试对所有存取器分库
                //context.ShardingContext.ShardDatabase(accessor);

                initializer.Initialize(accessor, services);
            }

            _isInitialized = true;
        }

        return services;
    }

    /// <summary>
    /// 异步使用 <see cref="IAccessorInitializer"/>。
    /// </summary>
    /// <param name="services">给定的 <see cref="IServiceProvider"/>。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回包含 <see cref="IServiceProvider"/> 的异步操作。</returns>
    public static async Task<IServiceProvider> UseAccessorInitializerAsync(this IServiceProvider services,
        CancellationToken cancellationToken = default)
    {
        if (!_isInitialized)
        {
            var initializer = services.GetRequiredService<IAccessorInitializer>();
            var context = services.GetRequiredService<IAccessorContext>();

            foreach (var accessor in context.ResolvedAccessors)
            {
                // 初始尝试对所有存取器分库
                //await context.ShardingContext.ShardDatabaseAsync(accessor, cancellationToken: cancellationToken);

                await initializer.InitializeAsync(accessor, services, cancellationToken);
            }

            _isInitialized = true;
        }

        return services;
    }

}
