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

namespace Microsoft.EntityFrameworkCore;

/// <summary>
/// <see cref="EntityTypeBuilder"/> 静态扩展。
/// </summary>
public static class EntityTypeBuilderExtensions
{

    /// <summary>
    /// 通过带分片与复数化实体类型名称映射表名（此方法仅支持关系型数据库，同时要求实体添加 <see cref="ShardedAttribute"/> 特性）。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="builder">给定的 <see cref="EntityTypeBuilder{TEntity}"/>。</param>
    /// <param name="shardingManager">给定的 <see cref="IShardingManager"/>。</param>
    /// <returns>返回 <see cref="EntityTypeBuilder{TEntity}"/>。</returns>
    public static EntityTypeBuilder<TEntity> ToTableWithSharding<TEntity>(this EntityTypeBuilder<TEntity> builder,
        IShardingManager shardingManager)
        where TEntity : class
        => builder.ToTableWithSharding(shardingManager, schema: null);

    /// <summary>
    /// 通过带分片与复数化实体类型名称映射表名（此方法仅支持关系型数据库，同时要求实体添加 <see cref="ShardedAttribute"/> 特性）。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="builder">给定的 <see cref="EntityTypeBuilder{TEntity}"/>。</param>
    /// <param name="shardingManager">给定的 <see cref="IShardingManager"/>。</param>
    /// <param name="schema">给定的架构。</param>
    /// <returns>返回 <see cref="EntityTypeBuilder{TEntity}"/>。</returns>
    public static EntityTypeBuilder<TEntity> ToTableWithSharding<TEntity>(this EntityTypeBuilder<TEntity> builder,
        IShardingManager shardingManager, string? schema)
        where TEntity : class
    {
        var entityType = typeof(TEntity);
        var tableName = builder.Metadata.GetTableName(); // 可能是已分片表名

        // 在 OnModelCreating 不能使用 Query 查询
        var descriptor = shardingManager.GetShardedByEntity(entityType, entity: null, tableName);

        if (descriptor.IsNeedShardingForEntity(entityType, out var newTableName))
            builder.Metadata.SetTableName(newTableName);

        builder.Metadata.SetSchema(schema);

        return builder;
    }

}
