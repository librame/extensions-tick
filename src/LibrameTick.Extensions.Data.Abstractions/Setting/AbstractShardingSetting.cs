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
    /// 构造一个默认的 <see cref="AbstractShardingSetting"/>。
    /// </summary>
    public AbstractShardingSetting()
    {
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
        StrategyTypes = descriptor.StrategyTypes;
        SourceType = descriptor.SourceType;
        //ReferenceName = descriptor.ReferenceName;
    }

    /// <summary>
    /// 使用 <see cref="AbstractShardingSetting"/> 构造一个 <see cref="AbstractShardingSetting"/>。
    /// </summary>
    /// <param name="setting">给定的 <see cref="AbstractShardingSetting"/>。</param>
    protected AbstractShardingSetting(AbstractShardingSetting setting)
    {
        BaseName = setting.BaseName;
        SuffixFormatter = setting.SuffixFormatter;
        //FormattedSuffix = setting.FormattedSuffix;
        Connector = setting.Connector;
        ShardedName = setting.ShardedName;
        StrategyTypes = setting.StrategyTypes;
        SourceType = setting.SourceType;
        //ReferenceName = setting.ReferenceName;
        SourceId = setting.SourceId;
    }


    /// <summary>
    /// 基础名称。
    /// </summary>
    public string? BaseName { get; set; }

    /// <summary>
    /// 带分片策略参数的后缀格式化器。
    /// </summary>
    public string? SuffixFormatter { get; set; }

    ///// <summary>
    ///// 经过格式化的后缀。
    ///// </summary>
    //public string? FormattedSuffix { get; set; }

    /// <summary>
    /// 连接符。
    /// </summary>
    public string? Connector { get; set; }

    /// <summary>
    /// 分片名称。
    /// </summary>
    public string? ShardedName { get; set; }

    /// <summary>
    /// 是否需要分片。
    /// </summary>
    public bool IsNeedSharding { get; set; }

    /// <summary>
    /// 分片策略类型集合。
    /// </summary>
    public List<Type>? StrategyTypes { get; set; }

    /// <summary>
    /// 来源类型。
    /// </summary>
    public Type? SourceType { get; set; }

    ///// <summary>
    ///// 引用名称。
    ///// </summary>
    //public string? ReferenceName { get; set; }

    /// <summary>
    /// 来源标识。
    /// </summary>
    public string? SourceId { get; set; }


    /// <summary>
    /// 比较相等。
    /// </summary>
    /// <remarks>
    /// <para>优先比较引用标识（如果标识不为空），其次比较分片名称（如果分片名称不为空），最后比较基础名称。</para>
    /// </remarks>
    /// <param name="other">给定的 <see cref="AbstractShardingSetting"/>。</param>
    /// <returns>返回布尔值。</returns>
    public virtual bool Equals(AbstractShardingSetting? other)
    {
        var comparison = StringComparison.Ordinal;

        if (ShardedName is not null && !ShardedName.Equals(other?.ShardedName, comparison))
            return false;

        if (SourceId is not null && !SourceId.Equals(other?.SourceId, comparison))
            return false;

        return BaseName is not null && BaseName.Equals(other?.BaseName, comparison);
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
    {
        if (ShardedName is not null)
            return ShardedName.GetHashCode();

        if (SourceId is not null)
            return SourceId.GetHashCode();

        return BaseName?.GetHashCode() ?? -1;
    }


    /// <summary>
    /// 转为字符串。
    /// </summary>
    /// <returns>返回字符串。</returns>
    public override string ToString()
        => $"{nameof(BaseName)}={BaseName},{nameof(ShardedName)}={ShardedName},{nameof(SourceId)}={SourceId}";

}
