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

internal sealed class InternalShardingSettingProvider : AbstractShardingSettingProvider
{
    public InternalShardingSettingProvider(ISettingProvider<ShardingDatabaseSettingRoot> databaseSettingProvider,
        ISettingProvider<ShardingTableSettingRoot> tableSettingProvider)
        : base(databaseSettingProvider, tableSettingProvider)
    {
    }

}
