#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.Configuration;

namespace Librame.Extensions.Data.Sharding;

/// <summary>
/// 定义用于分片的命名特性（构成方式为：BaseName+Suffix，可用于分库、分表操作）。
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class ShardedAttribute : Attribute
{
    /// <summary>
    /// 构造一个 <see cref="ShardedAttribute"/>。
    /// </summary>
    /// <param name="suffix">给定的后缀（支持的参数可参考指定的分片策略类型）。</param>
    public ShardedAttribute(string suffix)
        : this(suffix, defaultStrategyType: null, baseName: null)
    {
    }

    /// <summary>
    /// 构造一个 <see cref="ShardedAttribute"/>。
    /// </summary>
    /// <param name="suffix">给定的后缀（支持的参数可参考指定的分片策略类型）。</param>
    /// <param name="defaultStrategyType">给定的默认分片策略类型（可空）。</param>
    public ShardedAttribute(string suffix, Type? defaultStrategyType)
        : this(suffix, defaultStrategyType, baseName: null)
    {
    }

    /// <summary>
    /// 构造一个 <see cref="ShardedAttribute"/>。
    /// </summary>
    /// <param name="suffix">给定的后缀（支持的参数可参考指定的分片策略类型）。</param>
    /// <param name="defaultStrategyType">给定的默认分片策略类型（可空）。</param>
    /// <param name="baseName">给定的基础名称（可空；针对分表默认使用实体名称复数，针对数据库默认使用数据库名）。</param>
    public ShardedAttribute(string suffix, Type? defaultStrategyType, string? baseName)
    {
        Suffix = suffix;
        DefaultStrategyType = defaultStrategyType;
        BaseName = baseName;
    }


    /// <summary>
    /// 命名后缀。
    /// </summary>
    public string Suffix { get; set; }

    /// <summary>
    /// 基础名称。
    /// </summary>
    public string? BaseName { get; set; }

    /// <summary>
    /// 默认策略类型。
    /// </summary>
    public Type? DefaultStrategyType { get; set; }

    /// <summary>
    /// 分片配置。
    /// </summary>
    public IConfiguration? Configuration { get; set; }


    /// <summary>
    /// 从已标记分片特性的实体解析实例。
    /// </summary>
    /// <param name="entityType">给定的实体类型。</param>
    /// <param name="tableName">给定的映射表名。</param>
    /// <returns>返回 <see cref="ShardedAttribute"/>。</returns>
    /// <exception cref="NotSupportedException">
    /// Unsupported entity type. You need to label entity type with attribute.
    /// </exception>
    public static ShardedAttribute ParseFromEntity(Type entityType, string? tableName)
    {
        if (!entityType.TryGetAttribute<ShardedAttribute>(out var attribute))
            throw new NotSupportedException($"Unsupported entity type '{entityType}'. You need to label entity type with attribute '[{nameof(ShardedAttribute)}]'.");

        // 忽略已指定基础名称的情况
        if (string.IsNullOrEmpty(attribute.BaseName))
        {
            if (string.IsNullOrEmpty(tableName))
                tableName = entityType.Name.AsPluralize();

            attribute.BaseName = tableName;
        }

        return attribute;
    }

}
