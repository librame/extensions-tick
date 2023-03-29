﻿#region License

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
/// 定义针对库或表的分片描述符。
/// </summary>
public class ShardedDescriptor : IEquatable<ShardedDescriptor>, IEquatable<string>
{
    /// <summary>
    /// 默认后缀连接符。
    /// </summary>
    public const string DefaultSuffixConnector = "_";


    /// <summary>
    /// 构造一个 <see cref="ShardedDescriptor"/>。
    /// </summary>
    /// <param name="baseName">给定的基础名称。</param>
    /// <param name="suffix">给定的后缀。</param>
    public ShardedDescriptor(string baseName, string suffix)
        : this(baseName, suffix, suffixConnector: null)
    {
    }

    /// <summary>
    /// 构造一个 <see cref="ShardedDescriptor"/>。
    /// </summary>
    /// <param name="baseName">给定的基础名称。</param>
    /// <param name="suffix">给定的后缀。</param>
    /// <param name="suffixConnector">给定的后缀连接符（可空；默认使用 <see cref="DefaultSuffixConnector"/>）。</param>
    public ShardedDescriptor(string baseName, string suffix, string? suffixConnector)
    {
        BaseName = baseName;
        Suffix = suffix;
        SuffixConnector = suffixConnector ?? DefaultSuffixConnector;
    }


    /// <summary>
    /// 基础名称。
    /// </summary>
    public string BaseName { get; init; }

    /// <summary>
    /// 后缀。
    /// </summary>
    public string Suffix { get; set; }

    /// <summary>
    /// 后缀连接符。
    /// </summary>
    public string SuffixConnector { get; init; }

    /// <summary>
    /// 默认策略。
    /// </summary>
    public IShardingStrategy? DefaultStrategy { get; set; }

    /// <summary>
    /// 分片实体。
    /// </summary>
    public ShardingEntity? Entity { get; set; }

    /// <summary>
    /// 引用类型。
    /// </summary>
    public Type? ReferenceType { get; set; }

    /// <summary>
    /// 引用名称。
    /// </summary>
    public string? ReferenceName { get; set; }

    /// <summary>
    /// 引用值。
    /// </summary>
    public object? ReferenceValue { get; set; }


    /// <summary>
    /// 修改实体。
    /// </summary>
    /// <param name="newEntity">给定的新 <see cref="ShardingEntity"/>。</param>
    /// <returns>返回 <see cref="ShardedDescriptor"/>。</returns>
    public ShardedDescriptor ChangeEntity(ShardingEntity newEntity)
    {
        Entity = newEntity;

        return this;
    }

    /// <summary>
    /// 修改引用。
    /// </summary>
    /// <param name="newReferenceValue">给定的新引用值。</param>
    /// <param name="newReferenceName">给定的新引用名称（可选）。</param>
    /// <returns>返回 <see cref="ShardedDescriptor"/>。</returns>
    public ShardedDescriptor ChangeReference(object newReferenceValue, string? newReferenceName = null)
    {
        ReferenceValue = newReferenceValue;
        ReferenceType = newReferenceValue.GetType();

        if (!string.IsNullOrEmpty(newReferenceName))
            ReferenceName = newReferenceName;

        return this;
    }


    /// <summary>
    /// 带指定新后缀的新 <see cref="ShardedDescriptor"/>。
    /// </summary>
    /// <param name="newSuffix">给定的新后缀。</param>
    /// <returns>返回 <see cref="ShardedDescriptor"/>。</returns>
    public ShardedDescriptor WithSuffix(string newSuffix)
        => new ShardedDescriptor(BaseName, newSuffix, SuffixConnector);


    /// <summary>
    /// 为数据库连接字符串验证是否需要分片。
    /// </summary>
    /// <param name="connectionString">给定的连接字符串。</param>
    /// <param name="newConnectionString">输出的新连接字符串。</param>
    /// <returns>返回是否需要分片的布尔值。</returns>
    public bool IsNeedShardingForConnectionString(string connectionString,
        [MaybeNullWhen(false)] out string? newConnectionString)
    {
        var database = connectionString.ParseDatabaseFromConnectionString();
        var sharding = ToString();

        if (!sharding.Equals(database, StringComparison.Ordinal))
        {
            newConnectionString = connectionString.Replace(database, sharding);
            return true;
        }

        newConnectionString = null;
        return false;
    }

    /// <summary>
    /// 为实体定义的分片特性验证是否需要分片。
    /// </summary>
    /// <param name="entityType">给定的实体类型。</param>
    /// <param name="newTableName">输出的新表名。</param>
    /// <returns>返回是否需要分片的布尔值。</returns>
    public bool IsNeedShardingForEntity(Type entityType,
        [MaybeNullWhen(false)] out string? newTableName)
    {
        var attribute = ShardedAttribute.ParseFromEntity(entityType, tableName: null);
        var sharding = ToString();

        if (!sharding.Equals(attribute.BaseName, StringComparison.Ordinal))
        {
            newTableName = sharding;
            return true;
        }

        newTableName = null;
        return false;
    }


    /// <summary>
    /// 比较相等。
    /// </summary>
    /// <param name="other">给定的 <see cref="ShardedDescriptor"/>。</param>
    /// <returns>返回布尔值。</returns>
    public bool Equals(ShardedDescriptor? other)
        => other is not null && ToString() == other.ToString();

    /// <summary>
    /// 比较相等。
    /// </summary>
    /// <param name="other">给定的字符串。</param>
    /// <returns>返回布尔值。</returns>
    public bool Equals(string? other)
        => ToString() == other;


    /// <summary>
    /// 获取哈希码。
    /// </summary>
    /// <returns>返回 32 位整数。</returns>
    public override int GetHashCode()
        => ToString().GetHashCode();


    /// <summary>
    /// 转为字符串。
    /// </summary>
    /// <returns>返回字符串。</returns>
    public override string ToString()
    {
        if (!string.IsNullOrEmpty(Suffix))
            return $"{BaseName}{SuffixConnector}{Suffix}";

        return BaseName ?? string.Empty;
    }


    /// <summary>
    /// 隐式转为字符串。
    /// </summary>
    /// <param name="descriptor">给定的 <see cref="ShardedDescriptor"/>。</param>
    public static implicit operator string(ShardedDescriptor descriptor)
        => descriptor.ToString();


    /// <summary>
    /// 从字符串中解析 <see cref="ShardedDescriptor"/>。
    /// </summary>
    /// <param name="sharded">给定的分片字符串。</param>
    /// <returns>返回 <see cref="ShardedDescriptor"/>。</returns>
    public static ShardedDescriptor Parse(string sharded)
        => Parse(sharded, DefaultSuffixConnector);

    /// <summary>
    /// 从字符串中解析 <see cref="ShardedDescriptor"/>。
    /// </summary>
    /// <param name="sharded">给定的分片字符串。</param>
    /// <param name="suffixConnector">给定的后缀连接符。</param>
    /// <returns>返回 <see cref="ShardedDescriptor"/>。</returns>
    public static ShardedDescriptor Parse(string sharded, string suffixConnector)
    {
        if (sharded.TrySplitPair(suffixConnector, out var pair))
            return new(pair.Key, pair.Value, suffixConnector);

        return new(sharded, string.Empty, suffixConnector);
    }

}
