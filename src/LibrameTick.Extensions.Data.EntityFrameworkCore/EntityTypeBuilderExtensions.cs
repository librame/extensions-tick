#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Data;
using Librame.Extensions.Data.Sharding;

namespace Microsoft.EntityFrameworkCore;

/// <summary>
/// <see cref="EntityTypeBuilder"/> 静态扩展。
/// </summary>
public static class EntityTypeBuilderExtensions
{

    /// <summary>
    /// 返回可用于配置实体类型的分片构建器。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="builder">给定的 <see cref="EntityTypeBuilder{TEntity}"/>。</param>
    /// <param name="baseName">给定用于分片的基础名称。</param>
    /// <param name="suffixFormatter">给定带分片策略参数的后缀格式化器（支持的参数可参考指定的分片策略类型）。</param>
    /// <param name="strategyTypes">给定要引用的分片策略类型集合。</param>
    /// <returns>返回 <see cref="ShardingEntityTypeBuilder{TEntity}"/>。</returns>
    public static ShardingEntityTypeBuilder<TEntity> Sharding<TEntity>(this EntityTypeBuilder<TEntity> builder,
        string baseName, string suffixFormatter, params Type[] strategyTypes)
        where TEntity : class
        => builder.Sharding(new ShardingAttribute(ShardingKind.Table, baseName, suffixFormatter, strategyTypes));

    /// <summary>
    /// 返回可用于配置实体类型的分片构建器。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="builder">给定的 <see cref="EntityTypeBuilder{TEntity}"/>。</param>
    /// <param name="attribute">给定的 <see cref="ShardingAttribute"/>。</param>
    /// <returns>返回 <see cref="ShardingEntityTypeBuilder{TEntity}"/>。</returns>
    public static ShardingEntityTypeBuilder<TEntity> Sharding<TEntity>(this EntityTypeBuilder<TEntity> builder,
        ShardingAttribute? attribute = null)
        where TEntity : class
    {
        attribute ??= builder.Metadata.GetShardingAttribute();
        ArgumentNullException.ThrowIfNull(attribute);

        var shardingBuilder = new ShardingEntityTypeBuilder<TEntity>(builder, attribute);
        builder.Metadata.SetShardingBuilder(shardingBuilder);

        return shardingBuilder;
    }


    ///// <summary>
    ///// 获取分片实体类型构建器。
    ///// </summary>
    ///// <typeparam name="TEntity">指定的实体类型。</typeparam>
    ///// <param name="builder">给定的 <see cref="EntityTypeBuilder{TEntity}"/>。</param>
    ///// <returns>返回 <see cref="ShardingEntityTypeBuilder{TEntity}"/>。</returns>
    //public static ShardingEntityTypeBuilder<TEntity>? GetShardingBuilder<TEntity>(this EntityTypeBuilder<TEntity> builder)
    //    where TEntity : class
    //{
    //    var shardingAnnotation = builder.Metadata.FindAnnotation(ShardingAnnotationName);
    //    return (ShardingEntityTypeBuilder<TEntity>?)shardingAnnotation?.Value;
    //}

    ///// <summary>
    ///// 设置分片实体类型构建器。
    ///// </summary>
    ///// <typeparam name="TEntity">指定的实体类型。</typeparam>
    ///// <param name="builder">给定的 <see cref="EntityTypeBuilder{TEntity}"/>。</param>
    ///// <param name="shardingBuilder">给定的 <see cref="ShardingEntityTypeBuilder{TEntity}"/>。</param>
    ///// <returns>返回 <see cref="ShardingEntityTypeBuilder{TEntity}"/>。</returns>
    //public static ShardingEntityTypeBuilder<TEntity>? SetShardingBuilder<TEntity>(this EntityTypeBuilder<TEntity> builder,
    //    ShardingEntityTypeBuilder<TEntity> shardingBuilder)
    //    where TEntity : class
    //{
    //    builder.Metadata.SetAnnotation(ShardingAnnotationName, shardingBuilder);
    //    return shardingBuilder;
    //}


    //public static ShardingBuilder<TEntity> Sharding<TEntity>(this EntityTypeBuilder<TEntity> builder, ShardingAttribute attribute)
    //{
    //    //string suffixFormatter, params Type[] strategyTypes
    //}


    ///// <summary>
    ///// 通过带分片与复数化实体类型名称映射表名（此方法仅支持关系型数据库，同时要求实体添加 <see cref="ShardingAttribute"/> 特性）。
    ///// </summary>
    ///// <typeparam name="TEntity">指定的实体类型。</typeparam>
    ///// <param name="builder">给定的 <see cref="EntityTypeBuilder{TEntity}"/>。</param>
    ///// <param name="context">给定的 <see cref="IShardingContext"/>。</param>
    ///// <returns>返回 <see cref="EntityTypeBuilder{TEntity}"/>。</returns>
    //public static EntityTypeBuilder<TEntity> ToTableWithSharding<TEntity>(this EntityTypeBuilder<TEntity> builder,
    //    IShardingContext context)
    //    where TEntity : class
    //    => builder.ToTableWithSharding(context, schema: null, out _);

    ///// <summary>
    ///// 通过带分片与复数化实体类型名称映射表名（此方法仅支持关系型数据库，同时要求实体添加 <see cref="ShardingAttribute"/> 特性）。
    ///// </summary>
    ///// <typeparam name="TEntity">指定的实体类型。</typeparam>
    ///// <param name="builder">给定的 <see cref="EntityTypeBuilder{TEntity}"/>。</param>
    ///// <param name="context">给定的 <see cref="IShardingContext"/>。</param>
    ///// <param name="schema">给定的架构。</param>
    ///// <param name="descriptor">输出 <see cref="ShardingDescriptor"/>。</param>
    ///// <returns>返回 <see cref="EntityTypeBuilder{TEntity}"/>。</returns>
    //public static EntityTypeBuilder<TEntity> ToTableWithSharding<TEntity>(this EntityTypeBuilder<TEntity> builder,
    //    IShardingContext context, string? schema, out ShardingDescriptor descriptor)
    //    where TEntity : class
    //{
    //    var defaultTableName = builder.Metadata.GetDefaultTableName();

    //    // 使用默认实体初始分片
    //    var shardingTable = context.ShardTable(builder.Metadata.ClrType, entity: null, defaultTableName, out descriptor);

    //    if (schema is not null)
    //        builder.Metadata.SetSchema(schema);

    //    return builder;
    //}

}
