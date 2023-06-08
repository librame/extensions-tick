#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Data.Accessing;
using Librame.Extensions.Setting;

namespace Librame.Extensions.Data.Sharding;

/// <summary>
/// 定义针对库或表的分片描述符。
/// </summary>
public class ShardingDescriptor : IEquatable<ShardingDescriptor>, IEquatable<string>
{
    /// <summary>
    /// 默认连接符。
    /// </summary>
    public static readonly string DefaultConnector = "_";


    //private readonly ShardingAttribute _attribute;
    //private readonly Func<Type, IShardingStrategy> _createStrategyFunc;


    /// <summary>
    /// 使用 <see cref="ShardingAttribute"/> 构造一个 <see cref="ShardingDescriptor"/>。
    /// </summary>
    /// <param name="attribute">给定的 <see cref="ShardingAttribute"/>。</param>
    /// <param name="createStrategyFunc">给定的创建策略方法。</param>
    /// <param name="connector">给定的连接符（可选；默认使用 <see cref="DefaultConnector"/>）。</param>
    public ShardingDescriptor(ShardingAttribute attribute, Func<Type, IShardingStrategy> createStrategyFunc, string? connector = null)
    {
        attribute.Validate();

        BaseName = attribute.BaseName!;
        SuffixFormatter = attribute.SuffixFormatter;
        StrategyTypes = attribute.StrategyTypes;
        Strategies = attribute.StrategyTypes.Select(createStrategyFunc).ToList();
        SourceType = attribute.SourceType;
        Kind = attribute.Kind;
        Connector = connector ?? DefaultConnector;

        //_attribute = attribute;
        //_createStrategyFunc = createStrategyFunc;
    }


    /// <summary>
    /// 基础名称。
    /// </summary>
    public string BaseName { get; init; }

    /// <summary>
    /// 带分片策略参数的后缀格式化器。
    /// </summary>
    public string SuffixFormatter { get; init; }

    /// <summary>
    /// 分片策略类型集合。
    /// </summary>
    public List<Type> StrategyTypes { get; init; }

    /// <summary>
    /// 分片策略集合。
    /// </summary>
    public List<IShardingStrategy>? Strategies { get; init; }

    /// <summary>
    /// 连接符。
    /// </summary>
    public string Connector { get; init; }

    /// <summary>
    /// 来源类型。
    /// </summary>
    public Type? SourceType { get; set; }

    /// <summary>
    /// 分片种类。
    /// </summary>
    public ShardingKind? Kind { get; set; } = ShardingKind.Unspecified;

    /// <summary>
    /// 经过格式化的后缀。
    /// </summary>
    public string? FormattedSuffix { get; set; }


    /// <summary>
    /// 使用默认策略格式化后缀。
    /// </summary>
    /// <param name="shardingValue">给定的 <see cref="IShardingValue"/>。</param>
    /// <returns>返回 <see cref="ShardingDescriptor"/>。</returns>
    public virtual ShardingDescriptor FormatSuffix(IShardingValue? shardingValue)
    {
        if (Strategies is null)
            return this;

        var formatter = SuffixFormatter;

        foreach (var strategy in Strategies)
        {
            formatter = strategy.Format(formatter, shardingValue);
        }

        FormattedSuffix = formatter;
        return this;
    }


    /// <summary>
    /// 为数据库连接字符串验证是否需要分片。
    /// </summary>
    /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
    /// <param name="databaseSetting">输出 <see cref="ShardingDatabaseSetting"/>。</param>
    /// <param name="newConnectionString">输出新连接字符串。</param>
    /// <returns>返回是否需要分片的布尔值。</returns>
    public virtual bool IsNeedShardingForConnectionString(IAccessor accessor,
        out ShardingDatabaseSetting databaseSetting,
        [MaybeNullWhen(false)] out string? newConnectionString)
    {
        FormatSuffix(accessor);

        // 从数据库连接字符串提取数据库名称（不一定是原始名称）
        var connectionString = accessor.CurrentConnectionString!;
        var database = connectionString.ParseDatabaseFromConnectionString();

        var shardedName = ToString();

        databaseSetting = ShardingDatabaseSetting.Create(this);
        databaseSetting.ChangeShardedName(shardedName);

        // 分库使用访问器标识作用引用标识
        databaseSetting.ChangeReferenceId(accessor.AccessorId);

        // 与当前分片名称对比
        if (!shardedName.Equals(database, StringComparison.Ordinal))
        {
            newConnectionString = connectionString.Replace(database, shardedName);
            return true;
        }

        newConnectionString = null;
        return false;
    }

    /// <summary>
    /// 为实体定义的分片特性验证是否需要分片。
    /// </summary>
    /// <param name="entity">给定的 <see cref="IShardingValue"/>。</param>
    /// <param name="tableSetting">输出 <see cref="ShardingTableSetting"/>。</param>
    /// <returns>返回是否需要分片的布尔值。</returns>
    public virtual bool IsNeedShardingForEntity(IShardingValue? entity, out ShardingTableSetting tableSetting)
    {
        FormatSuffix(entity);

        var shardedName = ToString();

        tableSetting = ShardingTableSetting.Create(this);
        tableSetting.ChangeShardedName(shardedName);

        return !shardedName.Equals(BaseName, StringComparison.Ordinal);
    }


    /// <summary>
    /// 比较相等。
    /// </summary>
    /// <param name="other">给定的字符串。</param>
    /// <returns>返回布尔值。</returns>
    public virtual bool Equals(string? other)
        => ToString() == other;

    /// <summary>
    /// 比较相等。
    /// </summary>
    /// <param name="other">给定的 <see cref="ShardingDescriptor"/>。</param>
    /// <returns>返回布尔值。</returns>
    public virtual bool Equals(ShardingDescriptor? other)
        => other is not null && ToString() == other.ToString();

    /// <summary>
    /// 比较相等。
    /// </summary>
    /// <param name="obj">给定的对象。</param>
    /// <returns>返回布尔值。</returns>
    public override bool Equals(object? obj)
        => Equals(obj as ShardingDescriptor);


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
        if (!string.IsNullOrEmpty(FormattedSuffix))
            return $"{BaseName}{Connector}{FormattedSuffix}";

        return BaseName ?? string.Empty;
    }


    /// <summary>
    /// 隐式转为字符串。
    /// </summary>
    /// <param name="descriptor">给定的 <see cref="ShardingDescriptor"/>。</param>
    public static implicit operator string(ShardingDescriptor descriptor)
        => descriptor.ToString();

}
