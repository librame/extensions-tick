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
public abstract class AbstractShardingSetting : IEquatable<AbstractShardingSetting>
{
    /// <summary>
    /// 构造一个 <see cref="AbstractShardingSetting"/>。
    /// </summary>
    protected AbstractShardingSetting()
    {
        BaseName = string.Empty;
        SuffixFormatter = string.Empty;
        Connector = string.Empty;

        Items = new();
        StrategyTypes = new();
    }

    /// <summary>
    /// 使用 <see cref="ShardingDescriptor"/> 构造一个 <see cref="AbstractShardingSetting"/>。
    /// </summary>
    /// <param name="descriptor">给定的 <see cref="ShardingDescriptor"/>。</param>
    protected AbstractShardingSetting(ShardingDescriptor descriptor)
    {
        BaseName = descriptor.BaseName;
        SuffixFormatter = descriptor.SuffixFormatter;
        Connector = descriptor.Connector;
        SourceType = descriptor.SourceType;

        Items = new();
        StrategyTypes = descriptor.StrategyTypes;
    }

    /// <summary>
    /// 使用 <see cref="AbstractShardingSetting"/> 构造一个 <see cref="AbstractShardingSetting"/>。
    /// </summary>
    /// <param name="setting">给定的 <see cref="AbstractShardingSetting"/>。</param>
    protected AbstractShardingSetting(AbstractShardingSetting setting)
    {
        BaseName = setting.BaseName;
        SuffixFormatter = setting.SuffixFormatter;
        Connector = setting.Connector;
        SourceType = setting.SourceType;

        Items = setting.Items;
        StrategyTypes = setting.StrategyTypes;
    }


    /// <summary>
    /// 基础名称。
    /// </summary>
    public string BaseName { get; set; }

    /// <summary>
    /// 带分片策略参数的后缀格式化器。
    /// </summary>
    public string SuffixFormatter { get; set; }

    /// <summary>
    /// 连接符。
    /// </summary>
    public string Connector { get; set; }

    /// <summary>
    /// 来源类型。
    /// </summary>
    public Type? SourceType { get; set; }

    /// <summary>
    /// 分片策略类型集合。
    /// </summary>
    public List<Type> StrategyTypes { get; set; }
    
    /// <summary>
    /// 分片项集合。
    /// </summary>
    public List<ShardingItemSetting> Items { get; set; }


    /// <summary>
    /// 尝试获取指定分片名称项设置。
    /// </summary>
    /// <param name="shardedName">给定的分片名称。</param>
    /// <param name="result">输出 <see cref="ShardingItemSetting"/>。</param>
    /// <returns>返回布尔值。</returns>
    public virtual bool TryGetItem(string shardedName,
        [MaybeNullWhen(false)] out ShardingItemSetting result)
    {
        result = Items.SingleOrDefault(p => p.ShardedName == shardedName);
        return result is not null;
    }


    /// <summary>
    /// 比较相等。
    /// </summary>
    /// <remarks>
    /// 主要比较 <see cref="BaseName"/> 与 <see cref="SuffixFormatter"/> 是否相等，区别大小写。
    /// </remarks>
    /// <param name="other">给定的 <see cref="AbstractShardingSetting"/>。</param>
    /// <returns>返回布尔值。</returns>
    public virtual bool Equals(AbstractShardingSetting? other)
    {
        var comparison = StringComparison.Ordinal;

        return other?.BaseName.Equals(BaseName, comparison) == true
            && other.SuffixFormatter.Equals(SuffixFormatter, comparison);
    }

    /// <summary>
    /// 比较相等，通过调用 <see cref="Equals(AbstractShardingSetting?)"/> 实现。
    /// </summary>
    /// <param name="obj">给定的对象。</param>
    /// <returns>返回布尔值。</returns>
    public override bool Equals(object? obj)
        => Equals(obj as AbstractShardingSetting);


    /// <summary>
    /// 获取哈希码。
    /// </summary>
    /// <returns>返回整数。</returns>
    public override int GetHashCode()
        => HashCode.Combine(BaseName, SuffixFormatter);


    /// <summary>
    /// 转为字符串。
    /// </summary>
    /// <returns>返回字符串。</returns>
    public override string ToString()
        => $"{nameof(BaseName)}={BaseName},{nameof(SuffixFormatter)}={SuffixFormatter}";

}
