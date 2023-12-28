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
/// 定义一个分片信息。
/// </summary>
public interface IShardingInfo : IEquatable<IShardingInfo>
{
    /// <summary>
    /// 分片种类。
    /// </summary>
    ShardingKind Kind { get; }

    /// <summary>
    /// 基础名称。
    /// </summary>
    string BaseName { get; }

    /// <summary>
    /// 带分片策略参数的后缀格式化器。
    /// </summary>
    string SuffixFormatter { get; }

    /// <summary>
    /// 分片策略类型集合。
    /// </summary>
    List<Type> StrategyTypes { get; }

    /// <summary>
    /// 来源类型。
    /// </summary>
    Type? SourceType { get; }

    /// <summary>
    /// 连接符。
    /// </summary>
    string Connector { get; }


    /// <summary>
    /// 通过比较 <see cref="ShardingKey"/> 来判定指定分片信息的相等性。
    /// </summary>
    /// <param name="other">给定要比较的 <see cref="IShardingInfo"/>。</param>
    /// <returns>返回是否相等的布尔值。</returns>
    bool IEquatable<IShardingInfo>.Equals([NotNullWhen(true)] IShardingInfo? other)
        => GetKey() == other?.GetKey();


    /// <summary>
    /// 获取分片键。
    /// </summary>
    /// <returns>返回 <see cref="ShardingKey"/>。</returns>
    ShardingKey GetKey();

}
