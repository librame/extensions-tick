#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data.Storing;

/// <summary>
/// <see cref="IServiceProvider"/>、<see cref="IStore{T}"/> 静态扩展。
/// </summary>
public static class ServiceProviderStoreExtensions
{

    /// <summary>
    /// 获取指定实体类型的泛型存储。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="services">给定的 <see cref="IServiceProvider"/>。</param>
    /// <returns>返回 <see cref="IStore{TEntity}"/>。</returns>
    public static IStore<TEntity> GetStore<TEntity>(this IServiceProvider services)
        => services.GetRequiredService<IStore<TEntity>>();

}
