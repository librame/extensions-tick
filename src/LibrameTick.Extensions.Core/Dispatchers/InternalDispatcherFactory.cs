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

namespace Librame.Extensions.Dispatching;

internal sealed class InternalDispatcherFactory : AbstractDispatcherFactory
{
    public InternalDispatcherFactory(IOptionsMonitor<CoreExtensionOptions> optionsMonitor)
        : base(optionsMonitor.CurrentValue.Dispatching)
    {
    }

}
