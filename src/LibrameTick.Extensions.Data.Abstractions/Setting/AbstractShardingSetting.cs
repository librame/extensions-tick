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

namespace Librame.Extensions.Setting;

/// <summary>
/// 定义抽象分片设置。
/// </summary>
public abstract class AbstractShardingSetting : IShardingInfo, IEquatable<AbstractShardingSetting>
{
    private ShardingKey? _key;


    /// <summary>
    /// 构造一个 <see cref="AbstractShardingSetting"/> 用于反序列化。
    /// </summary>
    protected AbstractShardingSetting()
    {
        BaseName = string.Empty;
        SuffixFormatter = string.Empty;
        StrategyTypes = [];
        Kind = ShardingKind.Unspecified;
        SourceType = null;
        Connector = string.Empty;
    }

    /// <summary>
    /// 使用 <see cref="ShardingDescriptor"/> 构造一个 <see cref="AbstractShardingSetting"/>。
    /// </summary>
    /// <param name="descriptor">给定的 <see cref="ShardingDescriptor"/>。</param>
    protected AbstractShardingSetting(ShardingDescriptor descriptor)
        : this(descriptor.Attribute)
    {
    }

    /// <summary>
    /// 使用 <see cref="AbstractShardingSetting"/> 构造一个 <see cref="AbstractShardingSetting"/>。
    /// </summary>
    /// <param name="setting">给定的 <see cref="AbstractShardingSetting"/>。</param>
    protected AbstractShardingSetting(AbstractShardingSetting setting)
        : this((IShardingInfo)setting)
    {
        Items = setting.Items;
    }

    private AbstractShardingSetting(IShardingInfo info)
    {
        Kind = info.Kind;
        BaseName = info.BaseName;
        SuffixFormatter = info.SuffixFormatter;
        StrategyTypes = info.StrategyTypes;
        SourceType = info.SourceType;
        Connector = info.Connector;
    }


    /// <summary>
    /// 分片种类。
    /// </summary>
    public ShardingKind Kind { get; set; }

    /// <summary>
    /// 基础名称。
    /// </summary>
    public string BaseName { get; set; }

    /// <summary>
    /// 带分片策略参数的后缀格式化器。
    /// </summary>
    public string SuffixFormatter { get; set; }

    /// <summary>
    /// 分片策略类型集合。
    /// </summary>
    public List<Type> StrategyTypes { get; set; }

    /// <summary>
    /// 来源类型。
    /// </summary>
    public Type? SourceType { get; set; }

    /// <summary>
    /// 连接符。
    /// </summary>
    public string Connector { get; set; }

    /// <summary>
    /// 分片项集合。
    /// </summary>
    public List<ShardingItemSetting> Items { get; set; } = [];


    /// <summary>
    /// 获取分片键。
    /// </summary>
    /// <returns>返回 <see cref="ShardingKey"/>。</returns>
    public virtual ShardingKey GetKey()
    {
        _key ??= new(this);
        return _key;
    }


    /// <summary>
    /// 添加分片项设置。
    /// </summary>
    /// <param name="item">给定的 <see cref="ShardingItemSetting"/>。</param>
    /// <returns>返回是否添加的布尔值。</returns>
    public virtual bool TryAdd(ShardingItemSetting item)
    {
        if (Items.Any(p => p.CurrentName == item.CurrentName))
            return false;

        Items.Add(item);
        return true;
    }

    /// <summary>
    /// 尝试获取指定分片名称项设置。
    /// </summary>
    /// <param name="shardedName">给定的分片名称。</param>
    /// <param name="result">输出 <see cref="ShardingItemSetting"/>。</param>
    /// <returns>返回是否存在的布尔值。</returns>
    public virtual bool TryGetItem(string shardedName,
        [NotNullWhen(true)] out ShardingItemSetting? result)
    {
        result = Items.SingleOrDefault(p => p.CurrentName == shardedName);
        return result is not null;
    }


    /// <summary>
    /// 通过比较 <see cref="IShardingInfo"/> 与后缀格式化器来判定指定分片设置的相等性。
    /// </summary>
    /// <param name="other">给定要比较的 <see cref="AbstractShardingSetting"/>。</param>
    /// <returns>返回是否相等的布尔值。</returns>
    public virtual bool Equals([NotNullWhen(true)] AbstractShardingSetting? other)
        => ((IShardingInfo)this).Equals(other)
        && SuffixFormatter.Equals(other.SuffixFormatter, StringComparison.Ordinal);

    /// <summary>
    /// 比较相等，通过调用 <see cref="Equals(AbstractShardingSetting?)"/> 实现。
    /// </summary>
    /// <param name="obj">给定要比较的对象。</param>
    /// <returns>返回是否相等的布尔值。</returns>
    public override bool Equals([NotNullWhen(true)] object? obj)
        => Equals(obj as AbstractShardingSetting);


    /// <summary>
    /// 获取哈希码。
    /// </summary>
    /// <returns>返回哈希码整数。</returns>
    public override int GetHashCode()
        => HashCode.Combine(((IShardingInfo)this).GetKey(), SuffixFormatter);


    /// <summary>
    /// 转为字符串。
    /// </summary>
    /// <returns>返回字符串。</returns>
    public override string ToString()
        => $"{SuffixFormatter};{((IShardingInfo)this).GetKey()}";

}
