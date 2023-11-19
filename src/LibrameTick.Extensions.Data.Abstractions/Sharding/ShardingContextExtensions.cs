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
using Librame.Extensions.Setting;

namespace Librame.Extensions.Data.Sharding;

/// <summary>
/// 定义 <see cref="IShardingContext"/> 静态扩展。
/// </summary>
public static class ShardingContextExtensions
{

    /// <summary>
    /// 对存取器的数据库分片。
    /// </summary>
    /// <param name="context">给定的 <see cref="IShardingContext"/>。</param>
    /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
    /// <param name="shardedAction">给定已分片的动作（可选）。</param>
    /// <returns>返回 <see cref="ShardingDatabaseSetting"/>。</returns>
    public static ShardingDatabaseSetting ShardDatabase(this IShardingContext context,
        IAccessor accessor, Action<ShardingDescriptor, ShardingDatabaseSetting>? shardedAction = null)
        => context.ShardDatabase(accessor, out _, shardedAction);

    ///// <summary>
    ///// 异步对存取器的数据库分片。
    ///// </summary>
    ///// <param name="context">给定的 <see cref="IShardingContext"/>。</param>
    ///// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
    ///// <param name="shardedAction">给定已分片的动作（可选）。</param>
    ///// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    ///// <returns>返回包含 <see cref="ShardingDatabaseSetting"/> 的异步操作。</returns>
    //public static async Task<ShardingDatabaseSetting> ShardDatabaseAsync(this IShardingContext context,
    //    IAccessor accessor, Action<ShardingDescriptor, ShardingDatabaseSetting>? shardedAction = null,
    //    CancellationToken cancellationToken = default)
    //{
    //    var (databaseSetting, _) = await context.ShardDatabaseAsync(accessor, shardedAction, cancellationToken);
    //    return databaseSetting;
    //}


    /// <summary>
    /// 对实体的数据表分片。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="context">给定的 <see cref="IShardingContext"/>。</param>
    /// <param name="entity">给定的 <typeparamref name="TEntity"/>。</param>
    /// <param name="shardedAction">给定已分片的动作（可选）。</param>
    /// <returns>返回 <see cref="ShardingTableSetting"/>。</returns>
    public static ShardingTableSetting ShardTable<TEntity>(this IShardingContext context,
        TEntity? entity, Action<ShardingDescriptor, ShardingTableSetting>? shardedAction = null)
        => context.ShardTable(entity, defaultTableName: null, out _, shardedAction);

    /// <summary>
    /// 对实体的数据表分片。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="context">给定的 <see cref="IShardingContext"/>。</param>
    /// <param name="entity">给定的 <typeparamref name="TEntity"/>。</param>
    /// <param name="descriptor">输出 <see cref="ShardingDescriptor"/>。</param>
    /// <param name="shardedAction">给定已分片的动作（可选）。</param>
    /// <returns>返回 <see cref="ShardingTableSetting"/>。</returns>
    public static ShardingTableSetting ShardTable<TEntity>(this IShardingContext context,
        TEntity? entity, out ShardingDescriptor descriptor,
        Action<ShardingDescriptor, ShardingTableSetting>? shardedAction = null)
        => context.ShardTable(typeof(TEntity), entity, defaultTableName: null, out descriptor, shardedAction);

    /// <summary>
    /// 对实体的数据表分片。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="context">给定的 <see cref="IShardingContext"/>。</param>
    /// <param name="entity">给定的 <typeparamref name="TEntity"/>。</param>
    /// <param name="defaultTableName">给定的默认表名。</param>
    /// <param name="shardedAction">给定已分片的动作（可选）。</param>
    /// <returns>返回 <see cref="ShardingTableSetting"/>。</returns>
    public static ShardingTableSetting ShardTable<TEntity>(this IShardingContext context,
        TEntity? entity, string? defaultTableName,
        Action<ShardingDescriptor, ShardingTableSetting>? shardedAction = null)
        => context.ShardTable(entity, defaultTableName, out _, shardedAction);

    /// <summary>
    /// 对实体的数据表分片。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="context">给定的 <see cref="IShardingContext"/>。</param>
    /// <param name="entity">给定的 <typeparamref name="TEntity"/>。</param>
    /// <param name="defaultTableName">给定的默认表名。</param>
    /// <param name="descriptor">输出 <see cref="ShardingDescriptor"/>。</param>
    /// <param name="shardedAction">给定已分片的动作（可选）。</param>
    /// <returns>返回 <see cref="ShardingTableSetting"/>。</returns>
    public static ShardingTableSetting ShardTable<TEntity>(this IShardingContext context,
        TEntity? entity, string? defaultTableName, out ShardingDescriptor descriptor,
        Action<ShardingDescriptor, ShardingTableSetting>? shardedAction = null)
        => context.ShardTable(typeof(TEntity), entity, defaultTableName, out descriptor, shardedAction);


    /// <summary>
    /// 对实体的数据表分片。
    /// </summary>
    /// <param name="context">给定的 <see cref="IShardingContext"/>。</param>
    /// <param name="entityType">给定的实体类型。</param>
    /// <param name="entity">给定的实体对象。</param>
    /// <param name="shardedAction">给定已分片的动作（可选）。</param>
    /// <returns>返回 <see cref="ShardingTableSetting"/>。</returns>
    public static ShardingTableSetting ShardTable(this IShardingContext context,
        Type entityType, object? entity,
        Action<ShardingDescriptor, ShardingTableSetting>? shardedAction = null)
        => context.ShardTable(entityType, entity, defaultTableName: null, out _, shardedAction);

    /// <summary>
    /// 对实体的数据表分片。
    /// </summary>
    /// <param name="context">给定的 <see cref="IShardingContext"/>。</param>
    /// <param name="entityType">给定的实体类型。</param>
    /// <param name="entity">给定的实体对象。</param>
    /// <param name="descriptor">输出 <see cref="ShardingDescriptor"/>。</param>
    /// <param name="shardedAction">给定已分片的动作（可选）。</param>
    /// <returns>返回 <see cref="ShardingTableSetting"/>。</returns>
    public static ShardingTableSetting ShardTable(this IShardingContext context,
        Type entityType, object? entity, out ShardingDescriptor descriptor,
        Action<ShardingDescriptor, ShardingTableSetting>? shardedAction = null)
        => context.ShardTable(entityType, entity, defaultTableName: null, out descriptor, shardedAction);

    /// <summary>
    /// 对实体的数据表分片。
    /// </summary>
    /// <param name="context">给定的 <see cref="IShardingContext"/>。</param>
    /// <param name="entityType">给定的实体类型。</param>
    /// <param name="entity">给定的实体对象。</param>
    /// <param name="defaultTableName">给定的默认表名。</param>
    /// <param name="shardedAction">给定已分片的动作（可选）。</param>
    /// <returns>返回 <see cref="ShardingTableSetting"/>。</returns>
    public static ShardingTableSetting ShardTable(this IShardingContext context,
        Type entityType, object? entity, string? defaultTableName,
        Action<ShardingDescriptor, ShardingTableSetting>? shardedAction = null)
        => context.ShardTable(entityType, entity, defaultTableName, out _, shardedAction);

}
