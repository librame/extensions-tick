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
using Librame.Extensions.Specification;

namespace Librame.Extensions.Data.Storing;

/// <summary>
/// <see cref="IStore{TEntity}"/> 规约静态扩展。
/// </summary>
public static class StoreSpecificationExtensions
{

    /// <summary>
    /// 获取指定实体类型泛型存储的数据集。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="store">给定的 <see cref="IStore{TEntity}"/>。</param>
    /// <param name="specification">给定的 <see cref="AccessAccessorSpecification"/>（可选；默认使用 <see cref="ReadAccessAccessorSpecification"/> 规约）。</param>
    /// <returns>返回 <see cref="DbSet{TEntity}"/>。</returns>
    public static DbSet<TEntity> GetSet<TEntity>(this IStore<TEntity> store,
        AccessAccessorSpecification? specification = null)
        where TEntity : class
    {
        var accessor = specification is null
            ? store.AccessorContext.GetReadAccessors()
            : store.AccessorContext.GetAccessors(specification);

        return ((DbContext)accessor).Set<TEntity>();
    }

    /// <summary>
    /// 获取指定实体类型泛型存储的架构。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="store">给定的 <see cref="IStore{TEntity}"/>。</param>
    /// <param name="specification">给定的 <see cref="AccessAccessorSpecification"/>（可选；默认使用 <see cref="ReadAccessAccessorSpecification"/> 规约）。</param>
    /// <returns>返回表名字符串。</returns>
    public static string? GetSchema<TEntity>(this IStore<TEntity> store,
        AccessAccessorSpecification? specification = null)
    {
        var accessor = specification is null
            ? store.AccessorContext.GetReadAccessors()
            : store.AccessorContext.GetAccessors(specification);

        return ((DbContext)accessor).Model.FindEntityType(typeof(TEntity))?.GetSchema();
    }

    /// <summary>
    /// 获取指定实体类型泛型存储的表名。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="store">给定的 <see cref="IStore{TEntity}"/>。</param>
    /// <param name="specification">给定的 <see cref="AccessAccessorSpecification"/>（可选；默认使用 <see cref="ReadAccessAccessorSpecification"/> 规约）。</param>
    /// <returns>返回表名字符串。</returns>
    public static string? GetTableName<TEntity>(this IStore<TEntity> store,
        AccessAccessorSpecification? specification = null)
    {
        var accessor = specification is null
            ? store.AccessorContext.GetReadAccessors()
            : store.AccessorContext.GetAccessors(specification);

        return ((DbContext)accessor).Model.FindEntityType(typeof(TEntity))?.GetTableName();
    }

}
