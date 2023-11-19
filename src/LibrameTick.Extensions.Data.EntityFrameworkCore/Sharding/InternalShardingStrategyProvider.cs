﻿#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data.Sharding;

internal sealed class InternalShardingStrategyProvider : AbstractShardingStrategyProvider
{
    public InternalShardingStrategyProvider(IOptionsMonitor<DataExtensionOptions> dataOptions)
        : base()
    {
        AddStrategy(new DateTimeShardingStrategy());
        AddStrategy(new DateTimeOffsetShardingStrategy());
        AddStrategy(new CultureInfoShardingStrategy());
        AddStrategy(new ModShardingStrategy());

        // 合并选项自定义策略集合
        if (dataOptions.CurrentValue.ShardingStrategies.Count > 0)
        {
            foreach (var strategy in dataOptions.CurrentValue.ShardingStrategies)
            {
                AddStrategy(strategy);
            }
        }
    }

}
