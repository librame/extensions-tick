#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Data.Specifications;

namespace Librame.Extensions.Data.Storing;

/// <summary>
/// <see cref="IStore{TEntity}"/> 静态扩展。
/// </summary>
public static class DataStoreExtensions
{

    /// <summary>
    /// 获取指定实体类型的泛型商店。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="store">给定的 <see cref="IStore{TEntity}"/>。</param>
    /// <param name="specification">给定的 <see cref="IAccessorSpecification"/>（可选；默认使用 <see cref="ReadAccessorSpecification"/> 规约）。</param>
    /// <returns>返回 <see cref="DbSet{TEntity}"/>。</returns>
    public static DbSet<TEntity> GetSet<TEntity>(this IStore<TEntity> store, IAccessorSpecification? specification = null)
        where TEntity : class
    {
        if (specification is null)
        {
            var context = (DbContext)store.Accessors.GetReadAccessor();
            return context.Set<TEntity>();
        }

        return ((DbContext)store.Accessors.GetAccessor(specification)).Set<TEntity>();
    }

}
