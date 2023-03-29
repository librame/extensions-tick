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
    /// <param name="suffix">给定的分片策略参数后缀（支持的参数可参考指定的分片策略类型）。</param>
    public ShardedAttribute(string suffix)
        : this(suffix, defaultStrategyType: null, baseName: null)
    {
    }

    /// <summary>
    /// 构造一个 <see cref="ShardedAttribute"/>。
    /// </summary>
    /// <param name="suffix">给定的分片策略参数后缀（支持的参数可参考指定的分片策略类型）。</param>
    /// <param name="defaultStrategyType">给定的默认分片策略类型（可空）。</param>
    public ShardedAttribute(string suffix, Type? defaultStrategyType)
        : this(suffix, defaultStrategyType, baseName: null)
    {
    }

    /// <summary>
    /// 构造一个 <see cref="ShardedAttribute"/>。
    /// </summary>
    /// <param name="suffix">给定的分片策略参数后缀（支持的参数可参考指定的分片策略类型）。</param>
    /// <param name="defaultStrategyType">给定的默认分片策略类型（可空）。</param>
    /// <param name="baseName">给定的基础名称（可空；针对分表默认使用实体名称复数，针对数据库默认使用数据库名）。</param>
    public ShardedAttribute(string suffix, Type? defaultStrategyType, string? baseName)
    {
        Suffix = suffix;
        DefaultStrategyType = defaultStrategyType;
        BaseName = baseName;
    }


    /// <summary>
    /// 分片策略参数后缀。
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
    /// 从分片策略类型、后缀与数据库连接字符串包含的数据库键值解析特性。
    /// </summary>
    /// <param name="strategyType">给定的分片策略类型。</param>
    /// <param name="suffix">给定的分片策略参数后缀。</param>
    /// <param name="connectionString">给定的连接字符串。</param>
    /// <returns>返回 <see cref="ShardedAttribute"/>。</returns>
    /// <exception cref="ArgumentException">
    /// A matching supported database keys was not found from the current connection string, Please specify the database key.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// The database key for the current connection string is null or empty.
    /// </exception>
    public static ShardedAttribute ParseFromConnectionString(Type strategyType, string suffix, string? connectionString)
        => ParseFromConnectionString(strategyType, suffix, connectionString, out _, out _);

    /// <summary>
    /// 从分片策略类型、后缀与数据库连接字符串包含的数据库键值解析特性。
    /// </summary>
    /// <param name="strategyType">给定的分片策略类型。</param>
    /// <param name="suffix">给定的分片策略参数后缀。</param>
    /// <param name="connectionString">给定的连接字符串。</param>
    /// <param name="segments">输出连接字符串的键值对集合。</param>
    /// <param name="isFileDatabase">输出是否为文件型数据源。</param>
    /// <param name="keyValueSeparator">给定的键值对分隔符（可选；默认为等号）。</param>
    /// <param name="pairDelimiter">给定的键值对集合界定符（可选；默认为分号）。</param>
    /// <param name="databaseKey">给定的数据库键（可选；默认使用 <see cref="ConnectionStringExtensions.DefaultSupportedDatabaseKeys"/>）。</param>
    /// <returns>返回 <see cref="ShardedAttribute"/>。</returns>
    public static ShardedAttribute ParseFromConnectionString(Type strategyType, string suffix, string? connectionString,
        out Dictionary<string, string>? segments, out bool isFileDatabase,
        string keyValueSeparator = "=", string pairDelimiter = ";", string? databaseKey = null)
    {
        var database = connectionString.ParseDatabaseFromConnectionString(out segments, out isFileDatabase,
            keyValueSeparator, pairDelimiter, databaseKey);

        var attribute = new ShardedAttribute(suffix, strategyType, database);

        ArgumentException.ThrowIfNullOrEmpty(attribute.BaseName);

        return attribute;
    }


    /// <summary>
    /// 从已标记分片特性的实体解析分片特性。
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

        ArgumentException.ThrowIfNullOrEmpty(attribute.BaseName);

        return attribute;
    }

}
