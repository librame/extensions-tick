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
/// 定义用于分片的描述符。
/// </summary>
/// <param name="strategyProvider">给定的 <see cref="IShardingStrategyProvider"/>。</param>
/// <param name="attribute">给定的 <see cref="ShardingAttribute"/>。</param>
/// <param name="initialShardingValues">给定的初始 <see cref="IReadOnlyList{IShardingValue}"/>（可选）。</param>
public sealed class ShardingDescriptor(IShardingStrategyProvider strategyProvider, ShardingAttribute attribute,
    IReadOnlyList<IShardingValue>? initialShardingValues = null) : IEquatable<ShardingDescriptor>
{
    /// <summary>
    /// 策略提供程序。
    /// </summary>
    public IShardingStrategyProvider StrategyProvider { get; init; } = strategyProvider;

    /// <summary>
    /// 分片特性。
    /// </summary>
    public ShardingAttribute Attribute { get; init; } = attribute.Validate();

    /// <summary>
    /// 初始分片值集合。
    /// </summary>
    public IReadOnlyList<IShardingValue>? InitialShardingValues { get; set; } = initialShardingValues;

    /// <summary>
    /// 分片策略集合。
    /// </summary>
    public IReadOnlyList<IShardingStrategy> Strategies
        => Attribute.StrategyTypes.Select(StrategyProvider.GetStrategy).ToList();

    /// <summary>
    /// 来源类型。
    /// </summary>
    public Type SourceType
    {
        get
        {
            ArgumentNullException.ThrowIfNull(Attribute.SourceType);
            return Attribute.SourceType;
        }
    }


    /// <summary>
    /// 生成分片名称。
    /// </summary>
    /// <param name="obj">给定的对象。</param>
    /// <returns>返回分片名称字符串。</returns>
    public string GenerateShardingName(object obj)
    {
        var newShardingValues = obj.GetImplementedShardingValues().ToArray();

        return GenerateShardingName(newShardingValues);
    }

    /// <summary>
    /// 生成分片名称。
    /// </summary>
    /// <param name="newShardingValues">给定的新分片值集合（如果构造实例时已指定 <see cref="InitialShardingValues"/>，则此参数可选）。</param>
    /// <returns>返回分片名称字符串。</returns>
    public string GenerateShardingName(IReadOnlyList<IShardingValue>? newShardingValues)
    {
        var formatter = ToString();

        if (InitialShardingValues is null && newShardingValues is null)
            return formatter;

        var shardingValues = InitialShardingValues;
        if (shardingValues?.Count > 0 && newShardingValues?.Count > 0)
        {
            shardingValues = shardingValues.Union(newShardingValues).ToList();
        }
        else
        {
            shardingValues ??= newShardingValues;
        }
        
        if (shardingValues is null) return formatter;

        foreach (var strategy in Strategies)
        {
            foreach (var shardingValue in shardingValues)
            {
                formatter = strategy.Format(formatter, shardingValue);
            }
        }

        return formatter;
    }


    /// <summary>
    /// 通过比较分片特性的 <see cref="IShardingInfo"/> 来判定指定分片描述符的相等性。
    /// </summary>
    /// <param name="other">给定要比较的 <see cref="ShardingDescriptor"/>。</param>
    /// <returns>返回是否相等的布尔值。</returns>
    public bool Equals([NotNullWhen(true)] ShardingDescriptor? other)
        => ((IShardingInfo)Attribute).Equals(other?.Attribute);

    /// <summary>
    /// 通过比较分片特性的 <see cref="IShardingInfo"/> 来判定指定分片描述符的相等性。
    /// </summary>
    /// <param name="obj">给定的对象。</param>
    /// <returns>返回是否相等的布尔值。</returns>
    public override bool Equals([NotNullWhen(true)] object? obj)
        => Equals(obj as ShardingDescriptor);


    /// <summary>
    /// 获取哈希码。
    /// </summary>
    /// <returns>返回哈希码整数值。</returns>
    public override int GetHashCode()
        => ((IShardingInfo)Attribute).GetKey().GetHashCode();


    /// <summary>
    /// 转为带后缀格式器结尾的字符串。
    /// </summary>
    /// <returns>返回字符串。</returns>
    public override string ToString()
        => Attribute.ToString();


    /// <summary>
    /// 通过比较 <see cref="ShardingAttribute"/> 来判定指定分片描述符的相等性。
    /// </summary>
    /// <param name="a">给定的 <see cref="ShardingDescriptor"/>。</param>
    /// <param name="b">给定要比较的 <see cref="ShardingDescriptor"/>。</param>
    /// <returns>返回是否相等的布尔值。</returns>
    public static bool operator ==([NotNullWhen(true)] ShardingDescriptor? a, [NotNullWhen(true)] ShardingDescriptor? b)
        => a?.Equals(b) == true;

    /// <summary>
    /// 通过比较 <see cref="ShardingAttribute"/> 来判定指定分片描述符的不等性。
    /// </summary>
    /// <param name="a">给定的 <see cref="ShardingDescriptor"/>。</param>
    /// <param name="b">给定要比较的 <see cref="ShardingDescriptor"/>。</param>
    /// <returns>返回是否不等的布尔值。</returns>
    public static bool operator !=([NotNullWhen(true)] ShardingDescriptor? a, [NotNullWhen(true)] ShardingDescriptor? b)
        => !(a == b);

}
