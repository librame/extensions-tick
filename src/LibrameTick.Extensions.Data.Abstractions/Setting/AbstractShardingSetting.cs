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
        SuffixFormat = descriptor.SuffixFormatter;
        FormattedSuffix = descriptor.FormattedSuffix;
        Connector = descriptor.Connector;
        ShardedStrategyTypes = descriptor.StrategyTypes;
        ReferenceType = descriptor.SourceType;
        //ReferenceName = descriptor.ReferenceName;
    }

    /// <summary>
    /// 使用 <see cref="AbstractShardingSetting"/> 构造一个 <see cref="AbstractShardingSetting"/>。
    /// </summary>
    /// <param name="setting">给定的 <see cref="AbstractShardingSetting"/>。</param>
    protected AbstractShardingSetting(AbstractShardingSetting setting)
    {
        BaseName = setting.BaseName;
        SuffixFormat = setting.SuffixFormat;
        FormattedSuffix = setting.FormattedSuffix;
        Connector = setting.Connector;
        ShardedName = setting.ShardedName;
        ShardedStrategyTypes = setting.ShardedStrategyTypes;
        ReferenceType = setting.ReferenceType;
        //ReferenceName = setting.ReferenceName;
        ReferenceId = setting.ReferenceId;
    }


    /// <summary>
    /// 基础名称。
    /// </summary>
    public string? BaseName { get; set; }

    /// <summary>
    /// 后缀格式。
    /// </summary>
    public string? SuffixFormat { get; set; }

    /// <summary>
    /// 经过格式化的后缀。
    /// </summary>
    public string? FormattedSuffix { get; set; }

    /// <summary>
    /// 连接符。
    /// </summary>
    public string? Connector { get; set; }

    /// <summary>
    /// 当前分片名称。
    /// </summary>
    public string? ShardedName { get; set; }

    /// <summary>
    /// 当前分片策略类型集合。
    /// </summary>
    public List<Type>? ShardedStrategyTypes { get; set; }

    /// <summary>
    /// 引用类型。
    /// </summary>
    public Type? ReferenceType { get; set; }

    ///// <summary>
    ///// 引用名称。
    ///// </summary>
    //public string? ReferenceName { get; set; }

    /// <summary>
    /// 引用标识。
    /// </summary>
    public string? ReferenceId { get; set; }


    /// <summary>
    /// 修改分片名称。
    /// </summary>
    /// <param name="shardedName">给定的分片名称。</param>
    /// <returns>返回当前 <see cref="AbstractShardingSetting"/>。</returns>
    public virtual AbstractShardingSetting ChangeShardedName(string shardedName)
    {
        ShardedName = shardedName;
        return this;
    }

    /// <summary>
    /// 修改引用标识。
    /// </summary>
    /// <param name="referenceId">给定的引用标识。</param>
    /// <returns>返回当前 <see cref="AbstractShardingSetting"/>。</returns>
    public virtual AbstractShardingSetting ChangeReferenceId(string referenceId)
    {
        ReferenceId = referenceId;
        return this;
    }


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

        if (ReferenceId is not null && !ReferenceId.Equals(other?.ReferenceId, comparison))
            return false;

        if (ShardedName is not null && !ShardedName.Equals(other?.ShardedName, comparison))
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
        if (ReferenceId is not null)
            return ReferenceId.GetHashCode();

        if (ShardedName is not null)
            return ShardedName.GetHashCode();

        return BaseName?.GetHashCode() ?? -1;
    }


    /// <summary>
    /// 转为字符串。
    /// </summary>
    /// <returns>返回字符串。</returns>
    public override string ToString()
        => $"{nameof(BaseName)}={BaseName},{nameof(ShardedName)}={ShardedName},{nameof(ReferenceId)}={ReferenceId}";

}
