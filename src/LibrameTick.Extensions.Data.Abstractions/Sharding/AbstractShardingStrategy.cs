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
/// 定义抽象实现 <see cref="IShardingStrategy"/>。
/// </summary>
public abstract class AbstractShardingStrategy : IShardingStrategy
{
    /// <summary>
    /// 默认键指示符。
    /// </summary>
    public const string DefaultKeyIndicator = "%";


    /// <summary>
    /// 策略类型。
    /// </summary>
    public virtual Type StrategyType
        => GetType();


    /// <summary>
    /// 启动分片（默认启用）。
    /// </summary>
    /// <param name="basis">给定的分片依据。</param>
    /// <returns>返回布尔值。</returns>
    public virtual bool Enabling(object? basis)
        => true;


    /// <summary>
    /// 格式化后缀。
    /// </summary>
    /// <param name="suffix">给定的后缀。</param>
    /// <param name="basis">给定的分片依据。</param>
    /// <returns>返回字符串。</returns>
    public virtual string FormatSuffix(string suffix, object? basis)
    {
        if (string.IsNullOrEmpty(suffix))
            return suffix;

        return FormatSuffixCore(suffix, basis);
    }

    /// <summary>
    /// 格式化后缀核心。
    /// </summary>
    /// <param name="suffix">给定的后缀。</param>
    /// <param name="basis">给定的分片依据。</param>
    /// <returns>返回字符串。</returns>
    protected abstract string FormatSuffixCore(string suffix, object? basis);


    /// <summary>
    /// 建立参数键。
    /// </summary>
    /// <param name="name">给定的名称。</param>
    /// <returns>返回字符串。</returns>
    public static string BuildParameterKey(string name)
        => $"{DefaultKeyIndicator}{name}";

}
