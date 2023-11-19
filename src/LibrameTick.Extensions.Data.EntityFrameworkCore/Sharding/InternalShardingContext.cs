#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Dispatchers;
using Librame.Extensions.Setting;

namespace Librame.Extensions.Data.Sharding;

internal sealed class InternalShardingContext : AbstractShardingContext
{
    public InternalShardingContext(IDispatcherFactory dispatcherFactory,
        IShardingSettingProvider settingProvider,
        IShardingStrategyProvider strategyProvider,
        IShardingTracker tracker)
        : base(dispatcherFactory, settingProvider, strategyProvider, tracker)
    {
    }

}
