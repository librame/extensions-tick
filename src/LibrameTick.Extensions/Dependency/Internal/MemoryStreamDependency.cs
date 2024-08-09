#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.IO;

namespace Librame.Extensions.Dependency.Internal;

internal sealed class MemoryStreamDependency : IMemoryStreamDependency
{
    private readonly RecyclableMemoryStreamManager _manager = new();


    public RecyclableMemoryStream GetStream()
        => _manager.GetStream();

    public RecyclableMemoryStream GetStream(byte[] buffer)
        => _manager.GetStream(buffer);

    public RecyclableMemoryStream GetStream(Func<RecyclableMemoryStreamManager, RecyclableMemoryStream> func)
        => func(_manager);

}
