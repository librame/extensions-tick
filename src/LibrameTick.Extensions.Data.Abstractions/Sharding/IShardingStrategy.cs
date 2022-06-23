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
/// 定义分片策略接口。
/// </summary>
public interface IShardingStrategy
{
    /// <summary>
    /// 策略类型。
    /// </summary>
    Type StrategyType { get; }


    /// <summary>
    /// 格式化分片后缀。
    /// </summary>
    /// <param name="sharded">给定的 <see cref="ShardedDescriptor"/>。</param>
    /// <returns>返回字符串。</returns>
    string FormatSuffix(ShardedDescriptor sharded);
}
