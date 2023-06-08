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
/// 定义实现 <see cref="IShardingStrategy"/> 的泛型分片策略接口。
/// </summary>
/// <typeparam name="TValue">指定的分片值类型。</typeparam>
public interface IShardingStrategy<TValue> : IShardingStrategy
{
    /// <summary>
    /// 默认值。
    /// </summary>
    Lazy<TValue> DefaultValue { get; }

    /// <summary>
    /// 所有键名集合。
    /// </summary>
    ICollection<string> AllKeys { get; }

    /// <summary>
    /// 获取指定键名的参数。
    /// </summary>
    /// <param name="key">给定的键名。</param>
    /// <returns>返回 <see cref="ShardingStrategyParameter{TValue}"/>。</returns>
    ShardingStrategyParameter<TValue> this[string key] { get; }

    /// <summary>
    /// 获取指定索引的参数。
    /// </summary>
    /// <param name="index">给定的索引。</param>
    /// <returns>返回 <see cref="ShardingStrategyParameter{TValue}"/>。</returns>
    ShardingStrategyParameter<TValue> this[int index] { get; }


    /// <summary>
    /// 策略类型。
    /// </summary>
    Type IShardingStrategy.StrategyType
        => GetType();

    /// <summary>
    /// 值类型。
    /// </summary>
    Type IShardingStrategy.ValueType
        => typeof(TValue);
}


/// <summary>
/// 定义分片策略接口。
/// </summary>
public interface IShardingStrategy
{
    /// <summary>
    /// 策略类型。
    /// </summary>
    Type StrategyType { get; }

    /// <summary>
    /// 分片值类型。
    /// </summary>
    Type ValueType { get; }


    /// <summary>
    /// 包含任何可格式化的键名。
    /// </summary>
    /// <param name="formatter">给定带分片策略参数的后缀格式化器。</param>
    /// <returns>返回布尔值。</returns>
    bool ContainsKey(string formatter);

    /// <summary>
    /// 格式化分片。
    /// </summary>
    /// <param name="formatter">给定带分片策略参数的后缀格式化器。</param>
    /// <param name="shardingValue">给定的 <see cref="IShardingValue"/>。</param>
    /// <returns>返回字符串。</returns>
    string Format(string formatter, IShardingValue? shardingValue);
}
