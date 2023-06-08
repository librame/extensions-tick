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

namespace Librame.Extensions.Setting;

/// <summary>
/// 定义抽象实现 <see cref="IShardingSettingProvider"/> 的分片设置提供程序。
/// </summary>
public abstract class AbstractShardingSettingProvider : IShardingSettingProvider
{
    private ISettingProvider<ShardingDatabaseSettingRoot> _databaseSettingProvider;
    private ISettingProvider<ShardingTableSettingRoot> _tableSettingProvider;


    /// <summary>
    /// 构造一个 <see cref="AbstractShardingSettingProvider"/>。
    /// </summary>
    /// <param name="databaseSettingProvider">给定的 <see cref="ISettingProvider{ShardingDatabaseSetting}"/>。</param>
    /// <param name="tableSettingProvider">给定的 <see cref="ISettingProvider{ShardingTableSetting}"/>。</param>
    protected AbstractShardingSettingProvider(ISettingProvider<ShardingDatabaseSettingRoot> databaseSettingProvider,
        ISettingProvider<ShardingTableSettingRoot> tableSettingProvider)
    {
        _databaseSettingProvider = databaseSettingProvider;
        _tableSettingProvider = tableSettingProvider;
    }


    /// <summary>
    /// 分库设置根。
    /// </summary>
    public ShardingDatabaseSettingRoot DatabaseSettings
        => _databaseSettingProvider.LoadOrSave();

    /// <summary>
    /// 分表设置根。
    /// </summary>
    public ShardingTableSettingRoot TableSettings
        => _tableSettingProvider.LoadOrSave();


    /// <summary>
    /// 添加分库设置集合并保存。
    /// </summary>
    /// <param name="settings">给定的 <see cref="ShardingDatabaseSetting"/> 数组。</param>
    /// <returns>返回 <see cref="IShardingSettingProvider"/>。</returns>
    public virtual IShardingSettingProvider AddDatabaseSettings(params ShardingDatabaseSetting[] settings)
        => AddDatabaseSettings((IEnumerable<ShardingDatabaseSetting>)settings);

    /// <summary>
    /// 添加分库设置集合并保存。
    /// </summary>
    /// <param name="settings">给定的 <see cref="IEnumerable{ShardingDatabaseSetting}"/>。</param>
    /// <returns>返回 <see cref="IShardingSettingProvider"/>。</returns>
    public virtual IShardingSettingProvider AddDatabaseSettings(IEnumerable<ShardingDatabaseSetting> settings)
    {
        Bootstrapper.GetLocker().Lock(index =>
        {
            var isAdded = false;

            foreach (var setting in settings)
            {
                if (!DatabaseSettings.Shardeds.Contains(setting))
                {
                    DatabaseSettings.Shardeds.Add(setting);

                    if (!isAdded)
                        isAdded = true;
                }
            }

            if (isAdded)
                _databaseSettingProvider.Save(DatabaseSettings);
        });

        return this;
    }


    /// <summary>
    /// 添加分表设置集合并保存。
    /// </summary>
    /// <param name="settings">给定的 <see cref="ShardingTableSetting"/> 数组。</param>
    /// <returns>返回 <see cref="IShardingSettingProvider"/>。</returns>
    public virtual IShardingSettingProvider AddTableSettings(params ShardingTableSetting[] settings)
        => AddTableSettings((IEnumerable<ShardingTableSetting>)settings);

    /// <summary>
    /// 添加分表设置集合并保存。
    /// </summary>
    /// <param name="settings">给定的 <see cref="IEnumerable{ShardingTableSetting}"/>。</param>
    /// <returns>返回 <see cref="IShardingSettingProvider"/>。</returns>
    public virtual IShardingSettingProvider AddTableSettings(IEnumerable<ShardingTableSetting> settings)
    {
        Bootstrapper.GetLocker().Lock(index =>
        {
            var isAdded = false;

            foreach (var setting in settings)
            {
                if (!TableSettings.Shardeds.Contains(setting))
                {
                    TableSettings.Shardeds.Add(setting);

                    if (!isAdded)
                        isAdded = true;
                }
            }

            if (isAdded)
                _tableSettingProvider.Save(TableSettings);
        });

        return this;
    }

}
