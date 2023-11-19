#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Core;

namespace Librame.Extensions.Data.Sharding;

/// <summary>
/// 定义用于分片的命名特性（构成方式为：BaseName+Connector+Suffix，可用于分库、分表操作）。
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
public class ShardingAttribute : Attribute, IValidatable<ShardingAttribute>
{
    /// <summary>
    /// 构造一个 <see cref="ShardingAttribute"/>。
    /// </summary>
    /// <param name="suffixFormatter">给定带分片策略参数的后缀格式化器（支持的参数可参考指定的分片策略类型）。</param>
    /// <param name="strategyTypes">给定要引用的分片策略类型集合。</param>
    public ShardingAttribute(string suffixFormatter, params Type[] strategyTypes)
    {
        SuffixFormatter = suffixFormatter;
        StrategyTypes = new(strategyTypes);
    }


    /// <summary>
    /// 带分片策略参数的后缀格式化器。
    /// </summary>
    public string SuffixFormatter { get; init; }

    /// <summary>
    /// 分片策略类型集合。
    /// </summary>
    public List<Type> StrategyTypes { get; init; }

    /// <summary>
    /// 基础名称。
    /// </summary>
    public string? BaseName { get; private set; }

    /// <summary>
    /// 来源类型。
    /// </summary>
    public Type? SourceType { get; private set; }

    /// <summary>
    /// 分片种类。
    /// </summary>
    /// <remarks>
    /// 默认 <see cref="ShardingKind.Unspecified"/>。
    /// </remarks>
    public ShardingKind Kind { get; private set; } = ShardingKind.Unspecified;

    /// <summary>
    /// 连接符。
    /// </summary>
    public string? Connector { get; set; }

    /// <summary>
    /// 分片配置。
    /// </summary>
    public IConfiguration? Configuration { get; set; }


    /// <summary>
    /// 修改如果为空的基础名称。
    /// </summary>
    /// <param name="newBaseNameFunc">给定新基础名称的方法。</param>
    /// <returns>返回 <see cref="ShardingAttribute"/>。</returns>
    public virtual ShardingAttribute ChangeBaseNameIfEmpty(Func<string> newBaseNameFunc)
        => ChangeBaseNameIfEmpty(newBaseNameFunc());

    /// <summary>
    /// 修改如果为空的基础名称。
    /// </summary>
    /// <param name="newBaseName">给定的新基础名称。</param>
    /// <returns>返回 <see cref="ShardingAttribute"/>。</returns>
    public virtual ShardingAttribute ChangeBaseNameIfEmpty(string newBaseName)
    {
        if (string.IsNullOrEmpty(BaseName))
            BaseName = newBaseName;

        return this;
    }


    /// <summary>
    /// 修改来源类型。
    /// </summary>
    /// <param name="newSourceType">给定的新来源类型。</param>
    /// <returns>返回 <see cref="ShardingAttribute"/>。</returns>
    public virtual ShardingAttribute ChangeSourceType(Type newSourceType)
    {
        SourceType = newSourceType;
        return this;
    }

    /// <summary>
    /// 修改分片种类。
    /// </summary>
    /// <param name="newKind">给定的新 <see cref="ShardingKind"/>。</param>
    /// <returns>返回 <see cref="ShardingAttribute"/>。</returns>
    public virtual ShardingAttribute ChangeKind(ShardingKind newKind)
    {
        Kind = newKind;
        return this;
    }


    /// <summary>
    /// 验证是否有效。
    /// </summary>
    /// <returns>返回布尔值。</returns>
    public bool IsValidated()
        => !string.IsNullOrEmpty(BaseName) && SourceType is not null;

    /// <summary>
    /// 验证有效性。
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
    /// 转为字符串。
    /// </summary>
    /// <returns>返回字符串。</returns>
    public override string ToString()
    {
        if (string.IsNullOrEmpty(BaseName))
            return SuffixFormatter;

        return $"{BaseName}{Connector}{SuffixFormatter}";
    }


    /// <summary>
    /// 从分片策略类型、后缀与数据库连接字符串包含的数据库键值解析特性。
    /// </summary>
    /// <param name="dbContextType">给定的数据库上下文类型。</param>
    /// <param name="suffixFormatter">给定带分片策略参数的后缀格式化器（支持的参数可参考指定的分片策略类型）。</param>
    /// <param name="strategyTypes">给定要引用的分片策略类型集合。</param>
    /// <param name="connectionString">给定的连接字符串。</param>
    /// <returns>返回 <see cref="ShardingAttribute"/>。</returns>
    /// <exception cref="ArgumentException">
    /// A matching supported database keys was not found from the current connection string, Please specify the database key.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// The database key for the current connection string is null or empty.
    /// </exception>
    public static ShardingAttribute ParseFromConnectionString(Type dbContextType,
        string suffixFormatter, Type[] strategyTypes, string? connectionString)
        => ParseFromConnectionString(dbContextType, suffixFormatter, strategyTypes, connectionString, out _, out _);

    /// <summary>
    /// 从分片策略类型、后缀与数据库连接字符串包含的数据库键值解析特性。
    /// </summary>
    /// <param name="dbContextType">给定的数据库上下文类型。</param>
    /// <param name="suffixFormatter">给定带分片策略参数的后缀格式化器（支持的参数可参考指定的分片策略类型）。</param>
    /// <param name="strategyTypes">给定要引用的分片策略类型集合。</param>
    /// <param name="connectionString">给定的连接字符串。</param>
    /// <param name="segments">输出连接字符串的键值对集合。</param>
    /// <param name="isFileDatabase">输出是否为文件型数据源。</param>
    /// <param name="keyValueSeparator">给定的键值对分隔符（可选；默认为等号）。</param>
    /// <param name="pairDelimiter">给定的键值对集合界定符（可选；默认为分号）。</param>
    /// <param name="databaseKey">给定的数据库键（可选；默认使用 <see cref="ConnectionStringExtensions.DefaultSupportedDatabaseKeys"/>）。</param>
    /// <returns>返回 <see cref="ShardingAttribute"/>。</returns>
    public static ShardingAttribute ParseFromConnectionString(Type dbContextType,
        string suffixFormatter, Type[] strategyTypes, string? connectionString,
        out Dictionary<string, string>? segments, out bool isFileDatabase,
        string keyValueSeparator = "=", string pairDelimiter = ";", string? databaseKey = null)
    {
        var database = connectionString.ParseDatabaseFromConnectionString(out segments, out isFileDatabase,
            keyValueSeparator, pairDelimiter, databaseKey);
        
        var attribute = new ShardingAttribute(suffixFormatter, strategyTypes);
        attribute.ChangeBaseNameIfEmpty(database);
        attribute.ChangeSourceType(dbContextType);

        return attribute.ChangeKind(ShardingKind.Database);
    }


    /// <summary>
    /// 从已标记分片特性的实体解析分片特性。
    /// </summary>
    /// <param name="entityType">给定的实体类型。</param>
    /// <param name="defaultTableName">给定当 <paramref name="entityType"/> 标注特性 <see cref="ShardingAttribute"/> 的基础名称为空时的默认表名（如果此参数也为空，则使用实体类型名称的复数形式命名）。</param>
    /// <returns>返回 <see cref="ShardingAttribute"/>。</returns>
    /// <exception cref="NotSupportedException">
    /// Unsupported entity type. You need to label entity type with attribute.
    /// </exception>
    public static ShardingAttribute ParseFromEntity(Type entityType, string? defaultTableName)
    {
        // 分表的实体类型必须标注特性
        if (!entityType.TryGetAttribute<ShardingAttribute>(out var attribute))
            throw new NotSupportedException($"Unsupported entity type '{entityType}'. You need to label entity type with attribute '[{nameof(ShardingAttribute)}]'.");

        // 忽略已指定基础名称的情况
        attribute.ChangeBaseNameIfEmpty(() => defaultTableName.IfNullOrEmpty(entityType.Name.AsPluralize));
        attribute.ChangeSourceType(entityType);

        return attribute.ChangeKind(ShardingKind.Table);
    }

}
