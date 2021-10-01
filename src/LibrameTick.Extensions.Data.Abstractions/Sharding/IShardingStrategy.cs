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
    /// 启用分片。
    /// </summary>
    /// <returns>返回布尔值。</returns>
    bool Enabling();

    /// <summary>
    /// 格式化后缀。
    /// </summary>
    /// <param name="suffix">给定的后缀。</param>
    /// <returns>返回字符串。</returns>
    string FormatSuffix(string suffix);
}
