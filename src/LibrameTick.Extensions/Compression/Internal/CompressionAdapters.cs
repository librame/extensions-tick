#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Compression.Internal;

internal static class CompressionAdapters
{

    public static List<ICompressionAdapter> InitializeAdapters()
    {
        var adapters = new List<ICompressionAdapter>
        {
            new CompressionAdapter<BrotliStream, Stream>(static (s, m, o) => new(s, m, leaveOpen: true), ".br"),
            new CompressionAdapter<DeflateStream, Stream>(static (s, m, o) => new(s, m, leaveOpen: true), ".gz"),
            new CompressionAdapter<GZipStream, Stream>(static (s, m, o) => new(s, m, leaveOpen: true), ".tar.gz"),
            new CompressionAdapter<ZLibStream, Stream>(static (s, m, o) => new(s, m, leaveOpen: true), ".z")
        };

        return adapters;
    }

}
