#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Core;
using Librame.Extensions.Data.Sharding;

namespace Microsoft.EntityFrameworkCore;

/// <summary>
/// <see cref="PropertyBuilder"/> 静态扩展。
/// </summary>
public static class ShardingPropertyBuilderExtensions
{

    /// <summary>
    /// 转为属性键。
    /// </summary>
    /// <param name="propertyType">给定的 <see cref="IMutableTypeBase"/>。</param>
    /// <returns>返回 <see cref="TypeNamedKey"/>。</returns>
    public static TypeNamedKey AsPropertyKey(this IMutableTypeBase propertyType)
        => new(propertyType.ClrType, propertyType.Name);

    /// <summary>
    /// 转为分片实体。
    /// </summary>
    /// <param name="builder">给定的 <see cref="PropertyBuilder"/>。</param>
    /// <param name="strategy">给定的分片策略。</param>
    /// <returns>返回 <see cref="ShardingEntity"/>。</returns>
    public static ShardingEntity AsShardingEntity(this PropertyBuilder builder,
        IShardingStrategy strategy)
    {
        var entity = new ShardingEntity(builder.Metadata.DeclaringEntityType.ClrType);
        var propertyKey = builder.Metadata.DeclaringType.AsPropertyKey();

        return entity.AddOrSetProperty(propertyKey, strategy);
    }

    /// <summary>
    /// 转为分片属性。
    /// </summary>
    /// <param name="propertyType">给定的 <see cref="IMutableTypeBase"/>。</param>
    /// <param name="strategy">给定的 <see cref="IShardingStrategy"/>。</param>
    /// <returns>返回 <see cref="ShardingProperty"/>。</returns>
    public static ShardingProperty AsShardingProperty(this IMutableTypeBase propertyType,
        IShardingStrategy strategy)
        => new(propertyType.AsPropertyKey(), strategy);


    /// <summary>
    /// 配置分片属性。
    /// </summary>
    /// <typeparam name="TStrategy">指定要引用当前集成的策略类型。</typeparam>
    /// <param name="builder">给定的 <see cref="PropertyBuilder"/>。</param>
    /// <param name="manager">给定的 <see cref="IShardingManager"/>。</param>
    /// <returns>返回 <see cref="PropertyBuilder"/>。</returns>
    public static PropertyBuilder Sharding<TStrategy>(this PropertyBuilder builder,
        IShardingManager manager)
        => Sharding(builder, manager, typeof(TStrategy));

    /// <summary>
    /// 配置分片属性。
    /// </summary>
    /// <param name="builder">给定的 <see cref="PropertyBuilder"/>。</param>
    /// <param name="manager">给定的 <see cref="IShardingManager"/>。</param>
    /// <param name="referStrategyType">给定要引用当前集成的策略类型。</param>
    /// <returns>返回 <see cref="PropertyBuilder"/>。</returns>
    public static PropertyBuilder Sharding(this PropertyBuilder builder,
        IShardingManager manager, Type referStrategyType)
    {
        var entity = new ShardingEntity(builder.Metadata.DeclaringEntityType.ClrType);
        var propertyKey = builder.Metadata.DeclaringType.AsPropertyKey();
        var strategy = manager.GetStrategy(referStrategyType);

        manager.AddOrSetEntity(entity.AddOrSetProperty(propertyKey, strategy));
        return builder;
    }

    /// <summary>
    /// 配置分片属性。
    /// </summary>
    /// <param name="builder">给定的 <see cref="PropertyBuilder"/>。</param>
    /// <param name="manager">给定的 <see cref="IShardingManager"/>。</param>
    /// <param name="strategy">给定的 <see cref="IShardingStrategy"/>。</param>
    /// <returns>返回 <see cref="PropertyBuilder"/>。</returns>
    public static PropertyBuilder Sharding(this PropertyBuilder builder,
        IShardingManager manager, IShardingStrategy strategy)
    {
        manager.AddOrSetEntity(builder.AsShardingEntity(strategy));
        return builder;
    }

}
