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
/// 定义用于 <see cref="EntityTypeBuilder{TEntity}"/> 的分片构建器。
/// </summary>
/// <remarks>
/// 适用于配置非属性的独立分片值。
/// </remarks>
/// <typeparam name="T">指定的类型。</typeparam>
public class ShardingBuilder<T>
    where T : class
{
    /// <summary>
    /// 构造一个 <see cref="ShardingBuilder{T}"/>。
    /// </summary>
    /// <param name="entityBuilder">给定的 <see cref="EntityTypeBuilder{TEntity}"/>。</param>
    /// <param name="context">给定的 <see cref="IShardingContext"/>。</param>
    public ShardingBuilder(EntityTypeBuilder<T> entityBuilder, IShardingContext context)
    {
        Context = context;
        EntityType = entityBuilder.Metadata.ClrType;
    }

    /// <summary>
    /// 构造一个 <see cref="ShardingBuilder{T}"/>。
    /// </summary>
    /// <param name="context">给定的 <see cref="IShardingContext"/>。</param>
    public ShardingBuilder(IShardingContext context)
    {
        Context = context;
        EntityType = typeof(T);
    }


    /// <summary>
    /// 分片上下文。
    /// </summary>
    protected IShardingContext Context { get; init; }

    /// <summary>
    /// 实体类型。
    /// </summary>
    protected Type EntityType { get; init; }


    /// <summary>
    /// 配置实体属性分片。
    /// </summary>
    /// <typeparam name="TProperty">指定的属性类型。</typeparam>
    /// <param name="propertyExpression">给定的属性表达式。</param>
    /// <returns>返回 <see cref="ShardingBuilder{T}"/>。</returns>
    public virtual ShardingBuilder<T> HasProperty<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
    {
        Context.Tracker.AddEntityValue(EntityType, new ExpressionShardingProperty<T, TProperty>(propertyExpression));
        return this;
    }

    /// <summary>
    /// 配置实体分片值。
    /// </summary>
    /// <typeparam name="TValue">指定的值类型。</typeparam>
    /// <param name="initialValue">给定的初始 <typeparamref name="TValue"/>。</param>
    /// <returns>返回 <see cref="ShardingBuilder{T}"/>。</returns>
    public virtual ShardingBuilder<T> HasValue<TValue>(TValue initialValue)
        => HasValue<TValue>(new SingleShardingValue<TValue>(initialValue));

    /// <summary>
    /// 配置实体分片值。
    /// </summary>
    /// <typeparam name="TValue">指定的值类型。</typeparam>
    /// <param name="value">给定的 <see cref="IShardingValue{TValue}"/>。</param>
    /// <returns>返回 <see cref="ShardingBuilder{T}"/>。</returns>
    public virtual ShardingBuilder<T> HasValue<TValue>(IShardingValue<TValue> value)
    {
        Context.Tracker.AddEntityValue(EntityType, value);
        return this;
    }

}
