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

namespace Librame.Extensions.Data.Sharding;

class InternalShardingManager : IShardingManager
{
    private readonly List<IShardingStrategy> _strategies = new();


    public InternalShardingManager(IOptionsMonitor<DataExtensionOptions> dataOptions,
        IOptionsMonitor<CoreExtensionOptions> coreOptions)
    {
        if (_strategies.Count < 1)
        {
            _strategies.Add(new DateTimeShardingStrategy(coreOptions.CurrentValue.Clock));
            _strategies.Add(new DateTimeOffsetShardingStrategy(coreOptions.CurrentValue.Clock));
            _strategies.Add(new CultureInfoShardingStrategy());

            if (dataOptions.CurrentValue.ShardingStrategies.Count > 0)
            {
                _strategies.AddRange(dataOptions.CurrentValue.ShardingStrategies);
                _strategies = _strategies.DistinctBy(ks => ks.StrategyType.FullName).ToList();
            }
        }
    }


    public IShardingStrategy? GetStrategy(Type strategyType)
        => _strategies.FirstOrDefault(s => s.StrategyType.SameType(strategyType));

}
