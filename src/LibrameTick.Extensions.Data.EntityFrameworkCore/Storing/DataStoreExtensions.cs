#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Specifications;

namespace Librame.Extensions.Data.Storing;

/// <summary>
/// <see cref="IStore{TEntity}"/> 静态扩展。
/// </summary>
public static class DataStoreExtensions
{

    /// <summary>
    /// 获取指定实体类型泛型商店的数据集。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="store">给定的 <see cref="IStore{TEntity}"/>。</param>
    /// <param name="specification">给定的 <see cref="AccessorSpec"/>（可选；默认使用 <see cref="ReadAccessorSpec"/> 规约）。</param>
    /// <returns>返回 <see cref="DbSet{TEntity}"/>。</returns>
    public static DbSet<TEntity> GetSet<TEntity>(this IStore<TEntity> store,
        AccessorSpec? specification = null)
        where TEntity : class
    {
        var accessor = specification is null
            ? store.Accessors.GetReadAccessor()
            : store.Accessors.GetAccessor(specification);

        return ((DbContext)accessor).Set<TEntity>();
    }

    /// <summary>
    /// 获取指定实体类型泛型商店的架构。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="store">给定的 <see cref="IStore{TEntity}"/>。</param>
    /// <param name="specification">给定的 <see cref="AccessorSpec"/>（可选；默认使用 <see cref="ReadAccessorSpec"/> 规约）。</param>
    /// <returns>返回表名字符串。</returns>
    public static string? GetSchema<TEntity>(this IStore<TEntity> store,
        AccessorSpec? specification = null)
    {
        var accessor = specification is null
            ? store.Accessors.GetReadAccessor()
            : store.Accessors.GetAccessor(specification);

        return ((DbContext)accessor).Model.FindEntityType(typeof(TEntity))?.GetSchema();
    }

    /// <summary>
    /// 获取指定实体类型泛型商店的表名。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="store">给定的 <see cref="IStore{TEntity}"/>。</param>
    /// <param name="specification">给定的 <see cref="AccessorSpec"/>（可选；默认使用 <see cref="ReadAccessorSpec"/> 规约）。</param>
    /// <returns>返回表名字符串。</returns>
    public static string? GetTableName<TEntity>(this IStore<TEntity> store,
        AccessorSpec? specification = null)
    {
        var accessor = specification is null
            ? store.Accessors.GetReadAccessor()
            : store.Accessors.GetAccessor(specification);

        return ((DbContext)accessor).Model.FindEntityType(typeof(TEntity))?.GetTableName();
    }

}
