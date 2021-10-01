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

namespace Librame.Extensions.Data.Sharding;

/// <summary>
/// <see cref="IShardingManager"/> 静态扩展。
/// </summary>
public static class ShardingManagerExtensions
{

    #region GetDescriptor

    /// <summary>
    /// 默认已支持的数据库键集合。
    /// </summary>
    public static readonly string[] DefaultSupportedKeys
        = new string[] { "Database", "Initial Catalog", "Data Source" };

    /// <summary>
    /// 默认键值对的分隔符。
    /// </summary>
    public const string DefaultSeparator = "=";


    /// <summary>
    /// 从连接字符串获取数据库分片命名描述符。
    /// </summary>
    /// <param name="manager">给定的 <see cref="IShardingManager"/>。</param>
    /// <param name="connectionString">给定的连接字符串。</param>
    /// <param name="attribute">给定的 <see cref="ShardedAttribute"/>。</param>
    /// <param name="separator">给定的数据库键值连接符（可选；默认使用 <see cref="DefaultSeparator"/>）。</param>
    /// <param name="supportedDatabaseKeys">给定支持的数据库键集合（可选；默认使用 <see cref="DefaultSupportedKeys"/>）。</param>
    /// <returns>返回 <see cref="ShardDescriptor"/>。</returns>
    public static ShardDescriptor GetDescriptorFromConnectionString(this IShardingManager manager,
        string connectionString, ShardedAttribute attribute,
        string? separator = null, string[]? supportedDatabaseKeys = null)
        => manager.GetDescriptorFromConnectionString(connectionString, attribute,
            out _, separator, supportedDatabaseKeys);

    /// <summary>
    /// 从连接字符串获取数据库分片命名描述符。
    /// </summary>
    /// <param name="manager">给定的 <see cref="IShardingManager"/>。</param>
    /// <param name="connectionString">给定的连接字符串。</param>
    /// <param name="attribute">给定的 <see cref="ShardedAttribute"/>。</param>
    /// <param name="segments">输出连接字符串各部分的字典集合。</param>
    /// <param name="separator">给定的数据库键值连接符（可选；默认使用 <see cref="DefaultSeparator"/>）。</param>
    /// <param name="supportedDatabaseKeys">给定支持的数据库键集合（可选；默认使用 <see cref="DefaultSupportedKeys"/>）。</param>
    /// <returns>返回 <see cref="ShardDescriptor"/>。</returns>
    public static ShardDescriptor GetDescriptorFromConnectionString(this IShardingManager manager,
        string connectionString, ShardedAttribute attribute,
        [MaybeNullWhen(false)] out Dictionary<string, string>? segments,
        string? separator = null, string[]? supportedDatabaseKeys = null)
    {
        if (string.IsNullOrEmpty(attribute.BaseName))
        {
            if (separator is null)
                separator = DefaultSeparator;

            if (supportedDatabaseKeys is null)
                supportedDatabaseKeys = DefaultSupportedKeys;

            var pairs = connectionString
                .Split(';')
                .Select(part =>
                {
                    var pairPart = part.Split(separator);
                    return new KeyValuePair<string, string>(pairPart[0], pairPart[pairPart.Length - 1]);
                });

            var currentKey = string.Empty;
            var currentName = string.Empty;

            segments = new Dictionary<string, string>(pairs);
            foreach (var key in supportedDatabaseKeys)
            {
                if (segments.TryGetValue(key, out var value))
                {
                    currentKey = key;
                    currentName = value;
                }
            }

            if (string.IsNullOrEmpty(currentKey) || string.IsNullOrEmpty(currentName))
                throw new ArgumentException($"A matching supported database keys '{supportedDatabaseKeys.JoinString(',')}' was not found from the current connection string '{connectionString}'.");

            // 修剪可能存在的路径和文件扩展名
            if (currentName.Contains('.') || currentName.Contains(Path.DirectorySeparatorChar))
                currentName = Path.GetFileNameWithoutExtension(currentName);

            attribute.BaseName = currentName;
        }
        else
        {
            segments = null;
        }
            
        return manager.GetDescriptor(attribute);
    }

    /// <summary>
    /// 从实体获取分片描述符。
    /// </summary>
    /// <param name="manager">给定的 <see cref="IShardingManager"/>。</param>
    /// <param name="entityType">给定的实体类型。</param>
    /// <returns>返回 <see cref="ShardDescriptor"/>。</returns>
    public static ShardDescriptor GetDescriptorFromEntity(this IShardingManager manager, Type entityType)
    {
        if (!entityType.TryGetAttribute<ShardedAttribute>(out var attribute))
            throw new NotSupportedException($"Unsupported entity type '{entityType}'. You need to label entity type with attribute '[{nameof(ShardedAttribute)}]'.");

        if (string.IsNullOrEmpty(attribute.BaseName))
            attribute.BaseName = entityType.Name.AsPluralize();

        return manager.GetDescriptor(attribute);
    }

    /// <summary>
    /// 获取分片描述符。
    /// </summary>
    /// <param name="manager">给定的 <see cref="IShardingManager"/>。</param>
    /// <param name="attribute">给定的 <see cref="ShardedAttribute"/>。</param>
    /// <returns>返回 <see cref="ShardDescriptor"/>。</returns>
    public static ShardDescriptor GetDescriptor(this IShardingManager manager, ShardedAttribute attribute)
    {
        if (string.IsNullOrEmpty(attribute.BaseName))
            throw new ArgumentException($"The {nameof(attribute)}.{nameof(attribute.BaseName)} is null or empty.");

        var strategy = manager.GetStrategy(attribute.StrategyType);
        if (strategy is null)
            throw new NotSupportedException($"Unsupported sharding strategy type '{attribute.StrategyType}'.");

        var suffix = attribute.Suffix;
        if (strategy.Enabling())
            suffix = strategy.FormatSuffix(suffix);

        return new ShardDescriptor(attribute.BaseName, suffix, attribute.SuffixConnector);
    }

    #endregion


    /// <summary>
    /// 对访问器进行分库。
    /// </summary>
    /// <param name="shardingManager">给定的 <see cref="IShardingManager"/>。</param>
    /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
    /// <returns>返回 <see cref="IAccessor"/>。</returns>
    public static IAccessor ShardDatabase(this IShardingManager shardingManager, IAccessor accessor)
    {
        if (accessor.AccessorDescriptor?.Sharded is null || string.IsNullOrEmpty(accessor.CurrentConnectionString))
            return accessor;

        var shardDescriptor = shardingManager.GetDescriptorFromConnectionString(accessor.CurrentConnectionString,
            accessor.AccessorDescriptor.Sharded);

        var shardName = shardDescriptor.ToString();
        if (!shardName.Equals(shardDescriptor.BaseName))
        {
            var newConnectionString = accessor.CurrentConnectionString.Replace(shardDescriptor.BaseName!, shardName);
            accessor.ChangeConnection(newConnectionString);
        }

        return accessor;
    }

}
