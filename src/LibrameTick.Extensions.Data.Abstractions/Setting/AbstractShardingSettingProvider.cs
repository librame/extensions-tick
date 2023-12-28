#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Bootstraps;
using Librame.Extensions.Data.Sharding;

namespace Librame.Extensions.Setting;

/// <summary>
/// 定义抽象实现 <see cref="IShardingSettingProvider"/> 的分片设置提供程序。
/// </summary>
public abstract class AbstractShardingSettingProvider : IShardingSettingProvider
{
    private readonly ISettingProvider<ShardingDatabaseSettingRoot> _databaseProvider;
    private readonly ISettingProvider<ShardingTableSettingRoot> _tableProvider;


    /// <summary>
    /// 构造一个 <see cref="AbstractShardingSettingProvider"/>。
    /// </summary>
    /// <param name="databaseProvider">给定的 <see cref="ISettingProvider{ShardingDatabaseSetting}"/>。</param>
    /// <param name="tableProvider">给定的 <see cref="ISettingProvider{ShardingTableSetting}"/>。</param>
    protected AbstractShardingSettingProvider(ISettingProvider<ShardingDatabaseSettingRoot> databaseProvider,
        ISettingProvider<ShardingTableSettingRoot> tableProvider)
    {
        _databaseProvider = databaseProvider;
        _tableProvider = tableProvider;
    }


    /// <summary>
    /// 数据库设置提供程序。
    /// </summary>
    public ISettingProvider<ShardingDatabaseSettingRoot> DatabaseProvider
        => _databaseProvider;

    /// <summary>
    /// 数据表设置提供程序。
    /// </summary>
    public ISettingProvider<ShardingTableSettingRoot> TableProvider
        => _tableProvider;


    /// <summary>
    /// 分库设置根。
    /// </summary>
    public ShardingDatabaseSettingRoot DatabaseRoot
        => _databaseProvider.LoadOrSave();

    /// <summary>
    /// 分表设置根。
    /// </summary>
    public ShardingTableSettingRoot TableRoot
        => _tableProvider.LoadOrSave();


    /// <summary>
    /// 保存分库设置根。
    /// </summary>
    /// <returns>返回 <see cref="ShardingDatabaseSettingRoot"/>。</returns>
    public virtual ShardingDatabaseSettingRoot SaveDatabaseRoot()
        => _databaseProvider.Save(DatabaseRoot);

    /// <summary>
    /// 保存分库设置根。
    /// </summary>
    /// <returns>返回 <see cref="ShardingTableSettingRoot"/>。</returns>
    public virtual ShardingTableSettingRoot SaveTableRoot()
        => _tableProvider.Save(TableRoot);


    /// <summary>
    /// 获取或创建分库设置并自动保存。
    /// </summary>
    /// <param name="descriptor">给定的 <see cref="ShardingDescriptor"/>。</param>
    /// <param name="shardedName">给定的分库名称。</param>
    /// <param name="sourceId">给定的来源标识。</param>
    /// <param name="source">给定的来源（暂不支持持久化）。</param>
    /// <returns>返回包含 <see cref="ShardingDatabaseSetting"/> 与 <see cref="ShardingItemSetting"/> 的元组。</returns>
    public virtual (ShardingDatabaseSetting databaseSetting, ShardingItemSetting itemSetting) GetOrCreateDatabase(
        ShardingDescriptor descriptor, string shardedName, string? sourceId, object? source)
    {
        return Bootstrapper.GetLocker().Lock(index =>
        {
            return DatabaseRoot.GetOrCreate(descriptor, shardedName, sourceId, source, () => SaveDatabaseRoot());
        });
    }

    /// <summary>
    /// 获取或创建分表设置并自动保存（不推荐用于遍历集合）。
    /// </summary>
    /// <param name="descriptor">给定的 <see cref="ShardingDescriptor"/>。</param>
    /// <param name="shardedName">给定的分表名称。</param>
    /// <param name="sourceId">给定的来源标识。</param>
    /// <param name="source">给定的来源（暂不支持持久化）。</param>
    /// <returns>返回包含 <see cref="ShardingTableSetting"/> 与 <see cref="ShardingItemSetting"/> 的元组。</returns>
    public virtual (ShardingTableSetting tableSetting, ShardingItemSetting itemSetting) GetOrCreateTable(
        ShardingDescriptor descriptor, string shardedName, string? sourceId, object? source)
    {
        return Bootstrapper.GetLocker().Lock(index =>
        {
            return TableRoot.GetOrCreate(descriptor, shardedName, sourceId, source, () => SaveTableRoot());
        });
    }


    /// <summary>
    /// 尝试添加分库设置集合并保存。
    /// </summary>
    /// <param name="databases">给定的 <see cref="ShardingDatabaseSetting"/> 数组。</param>
    /// <returns>返回是否添加的布尔值。</returns>
    public virtual bool TryAddDatabases(params ShardingDatabaseSetting[] databases)
        => TryAddDatabases((IEnumerable<ShardingDatabaseSetting>)databases);

    /// <summary>
    /// 尝试添加分库设置集合并保存。
    /// </summary>
    /// <param name="databases">给定的 <see cref="IEnumerable{ShardingDatabaseSetting}"/>。</param>
    /// <returns>返回是否添加的布尔值。</returns>
    public virtual bool TryAddDatabases(IEnumerable<ShardingDatabaseSetting> databases)
    {
        return Bootstrapper.GetLocker().Lock(index =>
        {
            var isAdded = false;

            foreach (var database in databases)
            {
                if (DatabaseRoot.TryAdd(database))
                {
                    if (!isAdded) isAdded = true;
                }
            }

            if (isAdded)
                SaveDatabaseRoot();

            return isAdded;
        });
    }


    /// <summary>
    /// 尝试添加分表设置集合并保存。
    /// </summary>
    /// <param name="tables">给定的 <see cref="ShardingTableSetting"/> 数组。</param>
    /// <returns>返回是否添加的布尔值。</returns>
    public virtual bool TryAddTables(params ShardingTableSetting[] tables)
        => TryAddTables((IEnumerable<ShardingTableSetting>)tables);

    /// <summary>
    /// 尝试添加分表设置集合并保存。
    /// </summary>
    /// <param name="tables">给定的 <see cref="IEnumerable{ShardingTableSetting}"/>。</param>
    /// <returns>返回是否添加的布尔值。</returns>
    public virtual bool TryAddTables(IEnumerable<ShardingTableSetting> tables)
    {
        return Bootstrapper.GetLocker().Lock(index =>
        {
            var isAdded = false;

            foreach (var table in tables)
            {
                if (TableRoot.TryAdd(table))
                {
                    if (!isAdded) isAdded = true;
                }
            }

            if (isAdded)
                SaveTableRoot();

            return isAdded;
        });
    }

}
