#region License

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
        var options = dataOptions.CurrentValue.Sharding;

        AddStrategy(new CultureInfoShardingStrategy(options.DefaultCultureInfoFactory));
        AddStrategy(new DateTimeShardingStrategy(options.DefaultDateTimeFactory));
        AddStrategy(new DateTimeOffsetShardingStrategy(options.DefaultDateTimeOffsetFactory));
        AddStrategy(new ModShardingStrategy());

        // 合并选项自定义策略集合
        if (options.AttachStrategies.Count > 0)
        {
            foreach (var strategy in options.AttachStrategies)
            {
                AddStrategy(strategy);
            }
        }
    }

}
