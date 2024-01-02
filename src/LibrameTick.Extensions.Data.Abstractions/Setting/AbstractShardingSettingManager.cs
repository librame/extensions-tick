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
/// 定义抽象实现 <see cref="IShardingSettingManager"/> 的分片设置管理器。
/// </summary>
public abstract class AbstractShardingSettingManager : IShardingSettingManager
{
    private readonly ISettingProvider<ShardingDatabaseSetting> _databaseProvider;
    private readonly ISettingProvider<ShardingTableSetting> _tableProvider;


    /// <summary>
    /// 构造一个 <see cref="AbstractShardingSettingManager"/>。
    /// </summary>
    /// <param name="databaseProvider">给定的 <see cref="ISettingProvider{ShardingDatabaseSetting}"/>。</param>
    /// <param name="tableProvider">给定的 <see cref="ISettingProvider{ShardingTableSetting}"/>。</param>
    protected AbstractShardingSettingManager(ISettingProvider<ShardingDatabaseSetting> databaseProvider,
        ISettingProvider<ShardingTableSetting> tableProvider)
    {
        _databaseProvider = databaseProvider;
        _tableProvider = tableProvider;
    }


    /// <summary>
    /// 数据库设置提供程序。
    /// </summary>
    public ISettingProvider<ShardingDatabaseSetting> DatabaseProvider
        => _databaseProvider;

    /// <summary>
    /// 数据表设置提供程序。
    /// </summary>
    public ISettingProvider<ShardingTableSetting> TableProvider
        => _tableProvider;


    /// <summary>
    /// 获取或创建分库设置并自动保存。
    /// </summary>
    /// <param name="descriptor">给定的 <see cref="ShardingDescriptor"/>。</param>
    /// <param name="shardedName">给定的分库名称。</param>
    /// <param name="sourceId">给定的来源标识。</param>
    /// <param name="source">给定的来源（暂不支持持久化）。</param>
    /// <returns>返回包含 <see cref="ShardingSetting"/> 与 <see cref="ShardingItemSetting"/> 的元组。</returns>
    public virtual (ShardingSetting databaseSetting, ShardingItemSetting itemSetting) GetOrCreateDatabase(
        ShardingDescriptor descriptor, string shardedName, string? sourceId, object? source)
    {
        return DatabaseProvider.CurrentSetting.GetOrCreate(descriptor, shardedName, sourceId, source,
            createdAction: () => DatabaseProvider.SaveChanges());
    }

    /// <summary>
    /// 获取或创建分表设置并自动保存（不推荐用于遍历集合）。
    /// </summary>
    /// <param name="descriptor">给定的 <see cref="ShardingDescriptor"/>。</param>
    /// <param name="shardedName">给定的分表名称。</param>
    /// <param name="sourceId">给定的来源标识。</param>
    /// <param name="source">给定的来源（暂不支持持久化）。</param>
    /// <returns>返回包含 <see cref="ShardingSetting"/> 与 <see cref="ShardingItemSetting"/> 的元组。</returns>
    public virtual (ShardingSetting tableSetting, ShardingItemSetting itemSetting) GetOrCreateTable(
        ShardingDescriptor descriptor, string shardedName, string? sourceId, object? source)
    {
        return TableProvider.CurrentSetting.GetOrCreate(descriptor, shardedName, sourceId, source,
            createdAction: () => TableProvider.SaveChanges());
    }


    /// <summary>
    /// 尝试添加分库设置集合并保存。
    /// </summary>
    /// <param name="databases">给定的 <see cref="ShardingSetting"/> 数组。</param>
    /// <returns>返回是否添加的布尔值。</returns>
    public virtual bool TryAddDatabases(params ShardingSetting[] databases)
        => TryAddDatabases((IEnumerable<ShardingSetting>)databases);

    /// <summary>
    /// 尝试添加分库设置集合并保存。
    /// </summary>
    /// <param name="databases">给定的 <see cref="IEnumerable{ShardingSetting}"/>。</param>
    /// <returns>返回是否添加的布尔值。</returns>
    public virtual bool TryAddDatabases(IEnumerable<ShardingSetting> databases)
    {
        var setting = TableProvider.CurrentSetting;
        var isAdded = false;

        foreach (var database in databases)
        {
            if (setting.TryAdd(database))
            {
                if (!isAdded) isAdded = true;
            }
        }

        if (isAdded) DatabaseProvider.SaveChanges();

        return isAdded;
    }


    /// <summary>
    /// 尝试添加分表设置集合并保存。
    /// </summary>
    /// <param name="tables">给定的 <see cref="ShardingSetting"/> 数组。</param>
    /// <returns>返回是否添加的布尔值。</returns>
    public virtual bool TryAddTables(params ShardingSetting[] tables)
        => TryAddTables((IEnumerable<ShardingSetting>)tables);

    /// <summary>
    /// 尝试添加分表设置集合并保存。
    /// </summary>
    /// <param name="tables">给定的 <see cref="IEnumerable{ShardingSetting}"/>。</param>
    /// <returns>返回是否添加的布尔值。</returns>
    public virtual bool TryAddTables(IEnumerable<ShardingSetting> tables)
    {
        var setting = TableProvider.CurrentSetting;
        var isAdded = false;

        foreach (var table in tables)
        {
            if (setting.TryAdd(table))
            {
                if (!isAdded) isAdded = true;
            }
        }

        if (isAdded) TableProvider.SaveChanges();

        return isAdded;
    }

}
