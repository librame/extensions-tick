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

namespace Librame.Extensions.Device;

internal class InternalDeviceLoader : AbstractDeviceLoader
{
    public InternalDeviceLoader(IOptionsMonitor<CoreExtensionOptions> optionsMonitor)
        : base(optionsMonitor.CurrentValue.DeviceLoad)
    {
    }

}
