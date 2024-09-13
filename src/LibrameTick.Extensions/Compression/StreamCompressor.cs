#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Compression;

/// <summary>
/// 定义实现 <see cref="AbstractCompressor{TAdapted, Stream}"/> 的流式压缩器接口。
/// </summary>
/// <typeparam name="TStreamAdapted">指定的流式适配类型。</typeparam>
/// <remarks>
/// 构造一个 <see cref="StreamCompressor{TStreamAdapted}"/>。
/// </remarks>
/// <param name="options">给定的 <see cref="CompressionOptions"/>。</param>
public class StreamCompressor<TStreamAdapted>(CompressionOptions options)
    : AbstractCompressor<TStreamAdapted, Stream>(options)
    where TStreamAdapted : Stream
{

    /// <summary>
    /// 压缩 <see cref="Stream"/>。
    /// </summary>
    /// <param name="original">给定原始的 <see cref="Stream"/>。</param>
    /// <param name="compressed">给定已压缩的 <see cref="Stream"/>。</param>
    public override void Compress(Stream original, Stream compressed)
    {
        using var compressor = RealAdapter.GetCompress(compressed, Options);

        original.CopyTo(compressor);
    }

    /// <summary>
    /// 解压 <see cref="Stream"/>。
    /// </summary>
    /// <param name="compressed">给定已压缩的 <see cref="Stream"/>。</param>
    /// <param name="decompressed">给定的解压 <see cref="Stream"/>。</param>
    public override void Decompress(Stream compressed, Stream decompressed)
    {
        using var decompressor = RealAdapter.GetDecompress(compressed, Options);

        decompressor.CopyTo(decompressed);
    }

}
