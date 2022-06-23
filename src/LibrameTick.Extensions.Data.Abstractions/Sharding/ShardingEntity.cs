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

namespace Librame.Extensions.Data.Sharding;

/// <summary>
/// 定义一个分片实体。
/// </summary>
public class ShardingEntity : IEquatable<ShardingEntity>
{
    private readonly ConcurrentDictionary<TypeNamedKey, ShardingProperty> _properties = new();


    /// <summary>
    /// 构造一个 <see cref="ShardingEntity"/>。
    /// </summary>
    /// <param name="entityType">给定的实体类型。</param>
    public ShardingEntity(Type entityType)
    {
        EntityType = entityType;
    }


    /// <summary>
    /// 实体类型。
    /// </summary>
    public Type EntityType { get; init; }


    /// <summary>
    /// 获取指定属性键的策略。
    /// </summary>
    /// <param name="propertyKey">给定的属性键。</param>
    /// <returns>返回该属性包含的 <see cref="ShardingProperty"/>。</returns>
    public ShardingProperty this[TypeNamedKey propertyKey]
        => _properties[propertyKey];

    /// <summary>
    /// 属性键集合。
    /// </summary>
    public ICollection<TypeNamedKey> PropertyKeys
        => _properties.Keys;

    /// <summary>
    /// 属性策略集合。
    /// </summary>
    public ICollection<ShardingProperty> Properties
        => _properties.Values;

    /// <summary>
    /// 属性集合数。
    /// </summary>
    public int PropertiesCount
        => _properties.Count;


    /// <summary>
    /// 添加或设置分片属性。
    /// </summary>
    /// <typeparam name="TProperty">指定的属性类型。</typeparam>
    /// <param name="propertyName">给定的属性名称。</param>
    /// <param name="strategy">给定的分片策略。</param>
    /// <returns>返回 <see cref="ShardingEntity"/>。</returns>
    public ShardingEntity AddOrSetProperty<TProperty>(string propertyName, IShardingStrategy strategy)
        => AddOrSetProperty(typeof(TProperty), propertyName, strategy);

    /// <summary>
    /// 添加或设置分片属性。
    /// </summary>
    /// <param name="propertyType">给定的属性类型。</param>
    /// <param name="propertyName">给定的属性名称。</param>
    /// <param name="strategy">给定的分片策略。</param>
    /// <returns>返回 <see cref="ShardingEntity"/>。</returns>
    public ShardingEntity AddOrSetProperty(Type propertyType,
        string propertyName, IShardingStrategy strategy)
        => AddOrSetProperty(new(propertyType, propertyName), strategy);

    /// <summary>
    /// 添加或设置分片属性。
    /// </summary>
    /// <param name="propertyKey">给定的属性键。</param>
    /// <param name="strategy">给定的分片策略。</param>
    /// <returns>返回 <see cref="ShardingEntity"/>。</returns>
    public ShardingEntity AddOrSetProperty(TypeNamedKey propertyKey, IShardingStrategy strategy)
        => AddOrSetProperty(new(propertyKey, strategy));

    /// <summary>
    /// 添加或设置分片属性。
    /// </summary>
    /// <param name="property">给定的 <see cref="ShardingProperty"/>。</param>
    /// <returns>返回 <see cref="ShardingEntity"/>。</returns>
    public ShardingEntity AddOrSetProperty(ShardingProperty property)
    {
        _properties.AddOrUpdate(property.Key, property, (key, value) => property);
        return this;
    }

    /// <summary>
    /// 添加或设置分片属性集合。
    /// </summary>
    /// <param name="properties">给定的 <see cref="IEnumerable{ShardingProperty}"/>。</param>
    /// <returns>返回 <see cref="ShardingEntity"/>。</returns>
    public ShardingEntity AddOrSetProperties(IEnumerable<ShardingProperty> properties)
    {
        foreach (var property in properties)
        {
            AddOrSetProperty(property);
        }

        return this;
    }


    /// <summary>
    /// 是否相等。
    /// </summary>
    /// <param name="other">给定的 <see cref="ShardingEntity"/>。</param>
    /// <returns>返回布尔值。</returns>
    public bool Equals(ShardingEntity? other)
        => EntityType == other?.EntityType;

}
