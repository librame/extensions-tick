#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data.Sharding;

/// <summary>
/// 定义用于 <see cref="EntityTypeBuilder{TEntity}"/> 的分片实体类型构建器。
/// </summary>
/// <typeparam name="TEntity">指定的类型。</typeparam>
/// <param name="entityBuilder">给定的 <see cref="EntityTypeBuilder{TEntity}"/>。</param>
/// <param name="attribute">给定的 <see cref="ShardingAttribute"/>。</param>
public class ShardingEntityTypeBuilder<TEntity>(EntityTypeBuilder<TEntity> entityBuilder, ShardingAttribute attribute)
    where TEntity : class
{
    private readonly List<IShardingValue> _shardingValues = [];

    private readonly Type _entityType = entityBuilder.Metadata.ClrType;
    private readonly ShardingAttribute _attribute = attribute;


    /// <summary>
    /// 实体类型。
    /// </summary>
    public Type EntityType
        => _entityType;


    /// <summary>
    /// 创建描述符。
    /// </summary>
    /// <param name="strategyProvider">给定的 <see cref="IShardingStrategyProvider"/>。</param>
    /// <returns>返回 <see cref="ShardingDescriptor"/>。</returns>
    internal ShardingDescriptor CreateDescriptor(IShardingStrategyProvider strategyProvider)
        => new(strategyProvider, _attribute, _shardingValues);


    /// <summary>
    /// 配置实体分片属性。
    /// </summary>
    /// <param name="action">给定的配置动作。</param>
    /// <returns>返回 <see cref="ShardingEntityTypeBuilder{TEntity}"/>。</returns>
    public virtual ShardingEntityTypeBuilder<TEntity> HasAttribute(Action<ShardingAttribute> action)
    {
        action(_attribute);
        return this;
    }

    /// <summary>
    /// 配置实体属性分片。
    /// </summary>
    /// <typeparam name="TProperty">指定的属性类型。</typeparam>
    /// <param name="propertyExpression">给定的属性表达式。</param>
    /// <returns>返回 <see cref="ShardingEntityTypeBuilder{TEntity}"/>。</returns>
    public virtual ShardingEntityTypeBuilder<TEntity> HasProperty<TProperty>(Expression<Func<TEntity, TProperty>> propertyExpression)
    {
        _shardingValues.Add(new ExpressionShardingProperty<TEntity, TProperty>(propertyExpression));
        return this;
    }

    /// <summary>
    /// 配置实体分片初始值。
    /// </summary>
    /// <typeparam name="TValue">指定的值类型。</typeparam>
    /// <param name="valueFactory">给定的值工厂方法。</param>
    /// <returns>返回 <see cref="ShardingEntityTypeBuilder{TEntity}"/>。</returns>
    public virtual ShardingEntityTypeBuilder<TEntity> HasValue<TValue>(Func<TValue> valueFactory)
        => HasShardingValue(new SingleShardingValue<TValue>(valueFactory));

    /// <summary>
    /// 配置实体分片值。
    /// </summary>
    /// <typeparam name="TValue">指定的值类型。</typeparam>
    /// <param name="shardingValue">给定的 <see cref="IShardingValue{TValue}"/>。</param>
    /// <returns>返回 <see cref="ShardingEntityTypeBuilder{TEntity}"/>。</returns>
    public virtual ShardingEntityTypeBuilder<TEntity> HasShardingValue<TValue>(IShardingValue<TValue> shardingValue)
    {
        _shardingValues.Add(shardingValue);
        return this;
    }

}
