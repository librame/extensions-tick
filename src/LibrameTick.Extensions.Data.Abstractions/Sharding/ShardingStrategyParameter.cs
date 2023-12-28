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
    /// 默认通用键指示符（如 %）。
    /// </summary>
    public const string DefaultKeyIndicator = "%";

    /// <summary>
    /// 默认前缀连接符（如 :）。
    /// </summary>
    public const string DefaultPrefixConnector = ":";


    /// <summary>
    /// 构造一个 <see cref="ShardingStrategyParameter{TValue}"/>。
    /// </summary>
    /// <param name="prefix">给定的策略前缀。</param>
    /// <param name="name">给定的参数名称。</param>
    /// <param name="valueFormatter">给定的值格式化器。</param>
    /// <param name="defaultValue">给定的默认值（可选；默认为 <see cref="string.Empty"/>）。</param>
    /// <param name="prefixConnector">给定的前缀连接符（可选；默认为 <see cref="DefaultPrefixConnector"/>）。</param>
    /// <param name="keyIndicator">给定的通用键指示符（可选；默认为 <see cref="DefaultKeyIndicator"/>）。</param>
    public ShardingStrategyParameter(string prefix, string name, Func<TValue, string> valueFormatter,
        string? defaultValue = null, string? prefixConnector = null, string? keyIndicator = null)
    {
        KeyIndicator = keyIndicator ?? DefaultKeyIndicator;
        PrefixConnector = prefixConnector ?? DefaultPrefixConnector;
        DefaultValue = defaultValue ?? string.Empty;
        ValueFormatter = valueFormatter;
        Name = name;
        Prefix = prefix;
    }


    /// <summary>
    /// 通用键指示符（用于替换名称的导引符）。
    /// </summary>
    public string KeyIndicator { get; init; }

    /// <summary>
    /// 前缀连接符（用于分隔各策略参数名称）。
    /// </summary>
    public string PrefixConnector { get; init; }

    /// <summary>
    /// 策略前缀。
    /// </summary>
    public string Prefix { get; init; }

    /// <summary>
    /// 参数名称。
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
    /// 构建参数键（通常格式为：%prefix:key）。
    /// </summary>
    /// <returns>返回键字符串。</returns>
    public string BuildKey()
        => CreateKey(KeyIndicator, Prefix, PrefixConnector, Name);


    /// <summary>
    /// 创建参数键（通常格式为：%prefix:key）。
    /// </summary>
    /// <param name="keyIndicator">给定的键指示符。</param>
    /// <param name="prefix">给定的策略前缀。</param>
    /// <param name="prefixConnector">给定的前缀连接符。</param>
    /// <param name="name">给定的名称。</param>
    /// <returns>返回键字符串。</returns>
    public static string CreateKey(string keyIndicator, string prefix, string prefixConnector, string name)
        => $"{keyIndicator}{prefix}{prefixConnector}{name}";

}
