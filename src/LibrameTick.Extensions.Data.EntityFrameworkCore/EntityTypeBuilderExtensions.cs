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
    /// <param name="context">给定的 <see cref="DbContext"/>。</param>
    /// <returns>返回 <see cref="EntityTypeBuilder{TEntity}"/>。</returns>
    public static EntityTypeBuilder<TEntity> ToTableWithSharding<TEntity>(this EntityTypeBuilder<TEntity> builder,
        IShardingManager shardingManager, DbContext context)
        where TEntity : class
        => builder.ToTableWithSharding(shardingManager, context, schema: null);

    /// <summary>
    /// 通过带分片与复数化实体类型名称映射表名（此方法仅支持关系型数据库，同时要求实体添加 <see cref="ShardedAttribute"/> 特性）。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="builder">给定的 <see cref="EntityTypeBuilder{TEntity}"/>。</param>
    /// <param name="shardingManager">给定的 <see cref="IShardingManager"/>。</param>
    /// <param name="context">给定的 <see cref="DbContext"/>。</param>
    /// <param name="schema">给定的架构。</param>
    /// <returns>返回 <see cref="EntityTypeBuilder{TEntity}"/>。</returns>
    public static EntityTypeBuilder<TEntity> ToTableWithSharding<TEntity>(this EntityTypeBuilder<TEntity> builder,
        IShardingManager shardingManager, DbContext context, string? schema)
        where TEntity : class
    {
        var entity = context.Set<TEntity>().FirstBySpecification();
        if (entity is not null)
        {
            var tableName = builder.Metadata.GetTableName();
            var descriptor = shardingManager.ShardEntity(typeof(TEntity), entity, tableName);

            if (!descriptor.BaseName.Equals(tableName, StringComparison.Ordinal))
                builder.Metadata.SetTableName(descriptor.BaseName);
        }

        builder.Metadata.SetSchema(schema);

        return builder;
    }

}
