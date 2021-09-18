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

class InternalShardingManager : IShardingManager
{
    private readonly DataExtensionOptions _options;


    public InternalShardingManager(DataExtensionOptions options)
    {
        _options = options;
    }


    public IShardingStrategy? GetStrategy(Type strategyType)
        => _options.ShardingStrategies.FirstOrDefault(s => s.StrategyType == strategyType);

}
