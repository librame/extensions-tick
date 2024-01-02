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

namespace Librame.Extensions.Setting;

//public class JsonFileSettingManager : ISettingManager
//{
//    public InternalShardingSettingManager(IOptionsMonitor<CoreExtensionOptions> options)
//        : base(InitialDatabaseSettingProvider(options.CurrentValue))
//    {
//    }


//    private static ISettingProvider<ShardingDatabaseSetting> InitialDatabaseSettingProvider(DataExtensionOptions options)
//    {
//        var fileName = options.Setting.ShardingDatabaseFileName.SetBasePath(options.ShardingDirectory);

//        return new BaseSettingProvider<ShardingDatabaseSetting>(settingName: null, builder =>
//        {
//            builder.AddJsonFile(fileName)
//                .SetBasePath(PathExtensions.CurrentDirectoryWithoutDevelopmentRelativeSubpath);
//        });
//    }
//}
