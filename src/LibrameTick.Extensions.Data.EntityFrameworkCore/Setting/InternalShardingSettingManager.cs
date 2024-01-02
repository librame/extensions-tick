#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Data;

namespace Librame.Extensions.Setting;

internal sealed class InternalShardingSettingManager : AbstractShardingSettingManager
{
    public InternalShardingSettingManager(IOptionsMonitorCache<ShardingDatabaseSetting> databaseCache,
        IOptionsMonitorCache<ShardingTableSetting> tableCache, IOptionsMonitor<DataExtensionOptions> options)
        : base(InitialDatabaseSettingProvider(databaseCache, options.CurrentValue),
            InitialTableSettingProvider(tableCache, options.CurrentValue))
    {
    }


    private static BaseSettingProvider<ShardingDatabaseSetting> InitialDatabaseSettingProvider(
        IOptionsMonitorCache<ShardingDatabaseSetting> cache, DataExtensionOptions options)
    {
        var fileName = options.Setting.ShardingDatabaseFileName.SetBasePath(options.ShardingDirectory);

        return new BaseSettingProvider<ShardingDatabaseSetting>(cache, builder =>
        {
            builder.AddJsonFile(fileName)
                .SetBasePath(PathExtensions.CurrentDirectoryWithoutDevelopmentRelativeSubpath);
        });
    }

    private static BaseSettingProvider<ShardingTableSetting> InitialTableSettingProvider(
        IOptionsMonitorCache<ShardingTableSetting> cache, DataExtensionOptions options)
    {
        var fileName = options.Setting.ShardingDatabaseFileName.SetBasePath(options.ShardingDirectory);

        return new BaseSettingProvider<ShardingTableSetting>(cache, builder =>
        {
            builder.AddJsonFile(fileName)
                .SetBasePath(PathExtensions.CurrentDirectoryWithoutDevelopmentRelativeSubpath);
        });
    }

}
