#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core.Storage;

class InternalFilePermission : AbstractFilePermission
{
    public InternalFilePermission(IOptionsMonitor<CoreExtensionOptions> options)
        : base(options.CurrentValue.WebRequest)
    {
    }

}
