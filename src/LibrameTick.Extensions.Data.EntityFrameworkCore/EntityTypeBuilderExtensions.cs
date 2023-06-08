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
    /// 返回可用于配置实体类型的分片的对象。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="builder">给定的 <see cref="EntityTypeBuilder{TEntity}"/>。</param>
    /// <param name="context">给定的 <see cref="IShardingContext"/>。</param>
    /// <returns>返回 <see cref="ShardingBuilder{TEntity}"/>。</returns>
    public static ShardingBuilder<TEntity> Sharding<TEntity>(this EntityTypeBuilder<TEntity> builder, IShardingContext context)
        where TEntity : class
        => new(builder, context);


    /// <summary>
    /// 通过带分片与复数化实体类型名称映射表名（此方法仅支持关系型数据库，同时要求实体添加 <see cref="ShardingAttribute"/> 特性）。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="builder">给定的 <see cref="EntityTypeBuilder{TEntity}"/>。</param>
    /// <param name="context">给定的 <see cref="IShardingContext"/>。</param>
    /// <returns>返回 <see cref="EntityTypeBuilder{TEntity}"/>。</returns>
    public static EntityTypeBuilder<TEntity> ToTableWithSharding<TEntity>(this EntityTypeBuilder<TEntity> builder,
        IShardingContext context)
        where TEntity : class
        => builder.ToTableWithSharding(context, schema: null, out _);

    /// <summary>
    /// 通过带分片与复数化实体类型名称映射表名（此方法仅支持关系型数据库，同时要求实体添加 <see cref="ShardingAttribute"/> 特性）。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="builder">给定的 <see cref="EntityTypeBuilder{TEntity}"/>。</param>
    /// <param name="context">给定的 <see cref="IShardingContext"/>。</param>
    /// <param name="schema">给定的架构。</param>
    /// <param name="descriptor">输出 <see cref="ShardingDescriptor"/>。</param>
    /// <returns>返回 <see cref="EntityTypeBuilder{TEntity}"/>。</returns>
    public static EntityTypeBuilder<TEntity> ToTableWithSharding<TEntity>(this EntityTypeBuilder<TEntity> builder,
        IShardingContext context, string? schema, out ShardingDescriptor descriptor)
        where TEntity : class
    {
        var defaultTableName = builder.Metadata.GetDefaultTableName();

        // 使用默认实体初始分片
        context.ShardTable(builder.Metadata.ClrType, entity: null, defaultTableName, out descriptor);

        if (schema is not null)
            builder.Metadata.SetSchema(schema);

        return builder;
    }

}
