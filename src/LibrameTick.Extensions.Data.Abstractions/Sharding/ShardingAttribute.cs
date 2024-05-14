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
/// 定义用于分片的特性（命名构成方式为：BaseName+Connector+SuffixFormatter，可用于分库、分表操作）。
/// </summary>
/// <remarks>
/// 构造一个 <see cref="ShardingAttribute"/>。
/// </remarks>
/// <param name="kind">给定的分片种类。</param>
/// <param name="baseName">给定用于分片的基础名称。</param>
/// <param name="suffixFormatter">给定带分片策略参数的后缀格式化器（支持的参数可参考指定的分片策略类型）。</param>
/// <param name="strategyTypes">给定要引用的分片策略类型集合。</param>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
public class ShardingAttribute(ShardingKind kind, string baseName
    , string suffixFormatter, params Type[] strategyTypes) : Attribute, IShardingInfo, IValidation<ShardingAttribute>
{
    /// <summary>
    /// 默认连接符。
    /// </summary>
    public const string DefaultConnector = "_";


    private ShardingKey? _key;


    /// <summary>
    /// 分片种类。
    /// </summary>
    public ShardingKind Kind { get; init; } = kind;

    /// <summary>
    /// 基础名称。
    /// </summary>
    public string BaseName { get; private set; } = baseName;

    /// <summary>
    /// 带分片策略参数的后缀格式化器。
    /// </summary>
    public string SuffixFormatter { get; init; } = suffixFormatter;

    /// <summary>
    /// 分片策略类型集合。
    /// </summary>
    public List<Type> StrategyTypes { get; init; } = [.. strategyTypes];

    /// <summary>
    /// 来源类型。
    /// </summary>
    public Type? SourceType { get; set; }

    /// <summary>
    /// 连接符（默认使用 <see cref="DefaultConnector"/>）。
    /// </summary>
    public string Connector { get; set; } = DefaultConnector;


    /// <summary>
    /// 获取分片键。
    /// </summary>
    /// <returns>返回 <see cref="ShardingKey"/>。</returns>
    public virtual ShardingKey GetKey()
    {
        _key ??= new(this);
        return _key;
    }


    /// <summary>
    /// 改变基础名称。
    /// </summary>
    /// <param name="func">给定的新基础名称方法。</param>
    /// <returns>返回 <see cref="ShardingAttribute"/>。</returns>
    public virtual ShardingAttribute ChangeBaseName(Func<string, string> func)
        => ChangeBaseName(func(BaseName));

    /// <summary>
    /// 改变基础名称。
    /// </summary>
    /// <param name="newBaseName">给定的新基础名称。</param>
    /// <returns>返回 <see cref="ShardingAttribute"/>。</returns>
    public virtual ShardingAttribute ChangeBaseName(string newBaseName)
    {
        BaseName = newBaseName;
        return this;
    }


    /// <summary>
    /// 验证基础名称与来源类型是否有效。
    /// </summary>
    /// <returns>返回布尔值。</returns>
    public virtual bool IsValidated()
        => !string.IsNullOrEmpty(BaseName) && SourceType is not null;

    /// <summary>
    /// 验证基础名称与来源类型有效性。
    /// </summary>
    /// <remarks>
    /// 默认验证 <see cref="BaseName"/> 与 <see cref="SourceType"/>。
    /// </remarks>
    /// <returns>返回 <see cref="ShardingAttribute"/>。</returns>
    /// <exception cref="ArgumentNullException">
    /// <see cref="BaseName"/> or <see cref="SourceType"/> is null.
    /// </exception>
    public virtual ShardingAttribute Validate()
    {
        ArgumentException.ThrowIfNullOrEmpty(BaseName);
        ArgumentNullException.ThrowIfNull(SourceType);
        
        return this;
    }


    /// <summary>
    /// 转为格式为“BaseName+Connector+SuffixFormatter”的字符串。
    /// </summary>
    /// <returns>返回字符串。</returns>
    public override string ToString()
    {
        if (string.IsNullOrEmpty(BaseName))
            return SuffixFormatter;

        return $"{BaseName}{Connector}{SuffixFormatter}";
    }


    /// <summary>
    /// 从已标记的类型解析分片特性。
    /// </summary>
    /// <typeparam name="TSource">指定已标记的来源类型。</typeparam>
    /// <param name="baseNameFormatter">给定的基础名称格式化器（可选）。</param>
    /// <returns>返回 <see cref="ShardingAttribute"/> 或 NULL。</returns>
    public static ShardingAttribute? Get<TSource>(Func<string, string>? baseNameFormatter = null)
        => Get(typeof(TSource), baseNameFormatter);

    /// <summary>
    /// 从已标记的类型解析分片特性。
    /// </summary>
    /// <param name="sourceType">给定已标记的来源类型。</param>
    /// <param name="baseNameFormatter">给定的基础名称格式化器（可选）。</param>
    /// <returns>返回 <see cref="ShardingAttribute"/> 或 NULL。</returns>
    public static ShardingAttribute? Get(Type sourceType, Func<string, string>? baseNameFormatter = null)
    {
        if (!sourceType.TryGetAttribute<ShardingAttribute>(out var attribute))
            return attribute;

        if (baseNameFormatter is not null)
            attribute.ChangeBaseName(baseNameFormatter);

        attribute.SourceType ??= sourceType;

        return attribute;
    }

}
