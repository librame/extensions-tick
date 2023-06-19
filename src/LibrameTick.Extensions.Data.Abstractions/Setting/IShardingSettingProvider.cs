#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Setting;

/// <summary>
/// 定义分片设置提供程序接口。
/// </summary>
public interface IShardingSettingProvider
{
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
    /// 添加分库设置集合并保存。
    /// </summary>
    /// <param name="settings">给定的 <see cref="ShardingDatabaseSetting"/> 数组。</param>
    /// <returns>返回 <see cref="IShardingSettingProvider"/>。</returns>
    IShardingSettingProvider AddDatabaseSettings(params ShardingDatabaseSetting[] settings);

    /// <summary>
    /// 添加分库设置集合并保存。
    /// </summary>
    /// <param name="settings">给定的 <see cref="IEnumerable{ShardingDatabaseSetting}"/>。</param>
    /// <returns>返回 <see cref="IShardingSettingProvider"/>。</returns>
    IShardingSettingProvider AddDatabaseSettings(IEnumerable<ShardingDatabaseSetting> settings);


    /// <summary>
    /// 添加分表设置集合并保存。
    /// </summary>
    /// <param name="settings">给定的 <see cref="ShardingTableSetting"/> 数组。</param>
    /// <returns>返回 <see cref="IShardingSettingProvider"/>。</returns>
    IShardingSettingProvider AddTableSettings(params ShardingTableSetting[] settings);

    /// <summary>
    /// 添加分表设置集合并保存。
    /// </summary>
    /// <param name="settings">给定的 <see cref="IEnumerable{ShardingTableSetting}"/>。</param>
    /// <returns>返回 <see cref="IShardingSettingProvider"/>。</returns>
    IShardingSettingProvider AddTableSettings(IEnumerable<ShardingTableSetting> settings);
}
