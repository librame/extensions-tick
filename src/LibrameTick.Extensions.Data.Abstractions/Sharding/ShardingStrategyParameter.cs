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
/// 定义分片策略参数。
/// </summary>
/// <typeparam name="TValue">指定的值类型。</typeparam>
public class ShardingStrategyParameter<TValue>
{
    /// <summary>
    /// 默认键指示符。
    /// </summary>
    public static readonly string DefaultKeyIndicator = "%";


    /// <summary>
    /// 构造一个 <see cref="ShardingStrategyParameter{TValue}"/>。
    /// </summary>
    /// <param name="name">给定的名称。</param>
    /// <param name="valueFormatter">给定的值格式化器。</param>
    /// <param name="defaultValue">给定的默认值（可选；默认为 <see cref="string.Empty"/>）。</param>
    /// <param name="keyIndicator">给定的键指示符（可选；默认为 <see cref="DefaultKeyIndicator"/>）。</param>
    public ShardingStrategyParameter(string name, Func<TValue, string> valueFormatter,
        string? defaultValue = null, string? keyIndicator = null)
    {
        KeyIndicator = keyIndicator ?? DefaultKeyIndicator;
        Key = BuildKey(KeyIndicator, name);
        ValueFormatter = valueFormatter;
        DefaultValue = defaultValue ?? string.Empty;
        Name = name;
    }


    /// <summary>
    /// 键名（通常包含键指示符与名称）。
    /// </summary>
    public string Key { get; init; }

    /// <summary>
    /// 键指示符（用于替换名称的导引符）。
    /// </summary>
    public string KeyIndicator { get; init; }

    /// <summary>
    /// 名称。
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// 值格式化器。
    /// </summary>
    public Func<TValue, string> ValueFormatter { get; init; }

    /// <summary>
    /// 默认值（当使用值格式化器未成功时，默认使用此值替换键名）。
    /// </summary>
    public string DefaultValue { get; init; }

    /// <summary>
    /// 值类型。
    /// </summary>
    public Type ValueType
        => typeof(TValue);


    /// <summary>
    /// 建立键名。
    /// </summary>
    /// <param name="keyIndicator">给定的键指示符。</param>
    /// <param name="name">给定的名称。</param>
    /// <returns>返回字符串。</returns>
    public static string BuildKey(string keyIndicator, string name)
        => $"{keyIndicator}{name}";

}
