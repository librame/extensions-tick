#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Data.Sharding;

namespace Librame.Extensions.Setting;

/// <summary>
/// 定义分片设置提供程序接口。
/// </summary>
public interface IShardingSettingProvider
{
    /// <summary>
    /// 数据库设置提供程序。
    /// </summary>
    ISettingProvider<ShardingDatabaseSettingRoot> DatabaseProvider { get; }

    /// <summary>
    /// 数据表设置提供程序。
    /// </summary>
    ISettingProvider<ShardingTableSettingRoot> TableProvider { get; }


    /// <summary>
    /// 分库设置根。
    /// </summary>
    ShardingDatabaseSettingRoot DatabaseRoot { get; }

    /// <summary>
    /// 分表设置根。
    /// </summary>
    ShardingTableSettingRoot TableRoot { get; }


    /// <summary>
    /// 保存分库设置根。
    /// </summary>
    /// <returns>返回 <see cref="ShardingDatabaseSettingRoot"/>。</returns>
    ShardingDatabaseSettingRoot SaveDatabaseRoot();

    /// <summary>
    /// 保存分库设置根。
    /// </summary>
    /// <returns>返回 <see cref="ShardingTableSettingRoot"/>。</returns>
    ShardingTableSettingRoot SaveTableRoot();


    /// <summary>
    /// 获取或创建分库设置并自动保存。
    /// </summary>
    /// <param name="descriptor">给定的 <see cref="ShardingDescriptor"/>。</param>
    /// <param name="shardedName">给定的分库名称。</param>
    /// <param name="sourceId">给定的来源标识。</param>
    /// <param name="source">给定的来源（暂不支持持久化）。</param>
    /// <returns>返回包含 <see cref="ShardingDatabaseSetting"/> 与 <see cref="ShardingItemSetting"/> 的元组。</returns>
    (ShardingDatabaseSetting databaseSetting, ShardingItemSetting itemSetting) GetOrCreateDatabase(
        ShardingDescriptor descriptor, string shardedName, string? sourceId, object? source);

    /// <summary>
    /// 获取或创建分表设置并自动保存（不推荐用于遍历集合）。
    /// </summary>
    /// <param name="descriptor">给定的 <see cref="ShardingDescriptor"/>。</param>
    /// <param name="shardedName">给定的分表名称。</param>
    /// <param name="sourceId">给定的来源标识。</param>
    /// <param name="source">给定的来源（暂不支持持久化）。</param>
    /// <returns>返回包含 <see cref="ShardingTableSetting"/> 与 <see cref="ShardingItemSetting"/> 的元组。</returns>
    (ShardingTableSetting tableSetting, ShardingItemSetting itemSetting) GetOrCreateTable(
        ShardingDescriptor descriptor, string shardedName, string? sourceId, object? source);


    /// <summary>
    /// 尝试添加分库设置集合并保存。
    /// </summary>
    /// <param name="databases">给定的 <see cref="ShardingDatabaseSetting"/> 数组。</param>
    /// <returns>返回是否添加的布尔值。</returns>
    bool TryAddDatabases(params ShardingDatabaseSetting[] databases);

    /// <summary>
    /// 尝试添加分库设置集合并保存。
    /// </summary>
    /// <param name="databases">给定的 <see cref="IEnumerable{ShardingDatabaseSetting}"/>。</param>
    /// <returns>返回是否添加的布尔值。</returns>
    bool TryAddDatabases(IEnumerable<ShardingDatabaseSetting> databases);


    /// <summary>
    /// 尝试添加分表设置集合并保存。
    /// </summary>
    /// <param name="tables">给定的 <see cref="ShardingTableSetting"/> 数组。</param>
    /// <returns>返回是否添加的布尔值。</returns>
    bool TryAddTables(params ShardingTableSetting[] tables);

    /// <summary>
    /// 尝试添加分表设置集合并保存。
    /// </summary>
    /// <param name="tables">给定的 <see cref="IEnumerable{ShardingTableSetting}"/>。</param>
    /// <returns>返回是否添加的布尔值。</returns>
    bool TryAddTables(IEnumerable<ShardingTableSetting> tables);
}
