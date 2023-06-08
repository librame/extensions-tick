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

namespace Librame.Extensions.Storage;

sealed internal class InternalWebFilePermission : AbstractWebFilePermission
{
    public InternalWebFilePermission(IOptionsMonitor<CoreExtensionOptions> options)
        : base(options.CurrentValue.WebFile)
    {
    }

}
