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
/// 定义用于分片的特性（常用于分库、分表操作）。
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class ShardedAttribute : Attribute
{
    /// <summary>
    /// 构造一个 <see cref="ShardedAttribute"/>。
    /// </summary>
    /// <param name="strategyType">给定的分片策略类型。</param>
    /// <param name="suffix">给定的后缀（支持的参数可参考指定的分片策略类型）。</param>
    public ShardedAttribute(Type strategyType, string suffix)
        : this(strategyType, suffix, suffixConnector: null, baseName: null)
    {
    }

    /// <summary>
    /// 构造一个 <see cref="ShardedAttribute"/>。
    /// </summary>
    /// <param name="strategyType">给定的分片策略类型。</param>
    /// <param name="suffix">给定的后缀（支持的参数可参考指定的分片策略类型）。</param>
    /// <param name="suffixConnector">给定的后缀连接符（可空；默认使用 <see cref="ShardDescriptor.DefaultSuffixConnector"/>）。</param>
    /// <param name="baseName">给定的基础名称（可空；针对分表默认使用实体名称复数，针对数据库默认使用数据库名）。</param>
    public ShardedAttribute(Type strategyType, string suffix, string? suffixConnector, string? baseName)
    {
        StrategyType = strategyType;
        Suffix = suffix;
        SuffixConnector = suffixConnector;
        BaseName = baseName;
    }


    /// <summary>
    /// 策略类型。
    /// </summary>
    public Type StrategyType { get; set; }

    /// <summary>
    /// 后缀。
    /// </summary>
    public string Suffix { get; set; }

    /// <summary>
    /// 后缀连接符。
    /// </summary>
    public string? SuffixConnector { get; set; }

    /// <summary>
    /// 基础名称。
    /// </summary>
    public string? BaseName { get; set; }


    /// <summary>
    /// 获取哈希码。
    /// </summary>
    /// <returns>返回 32 位整数。</returns>
    public override int GetHashCode()
        => ToString().GetHashCode();

    /// <summary>
    /// 转换为字符串。
    /// </summary>
    /// <returns>返回字符串。</returns>
    public override string ToString()
        => $"{nameof(BaseName)}={BaseName};{nameof(StrategyType)}={StrategyType.Name};{nameof(SuffixConnector)}={SuffixConnector};{nameof(Suffix)}={Suffix}";

}
