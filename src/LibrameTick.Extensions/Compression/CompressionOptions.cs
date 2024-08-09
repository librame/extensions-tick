#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using InternalCompressors = Librame.Extensions.Compression.Internal.CompressionAdapters;
using InternalCompressorAdapterResolver = Librame.Extensions.Compression.Internal.CompressionAdapterResolver;

namespace Librame.Extensions.Compression;

/// <summary>
/// 定义压缩选项。
/// </summary>
public class CompressionOptions : StaticDefaultInitializer<CompressionOptions>
{
    /// <summary>
    /// 构造一个 <see cref="CompressionOptions"/>。
    /// </summary>
    public CompressionOptions()
    {
        Adapters = InternalCompressors.InitializeAdapters();
        AdapterResolver = new InternalCompressorAdapterResolver(this);
    }

    /// <summary>
    /// 使用指定的 <see cref="CompressionOptions"/> 构造一个 <see cref="CompressionOptions"/>。
    /// </summary>
    /// <param name="options">给定的 <see cref="CompressionOptions"/>。</param>
    public CompressionOptions(CompressionOptions options)
    {
        Adapters = new(options.Adapters);
        AdapterResolver = options.AdapterResolver;
    }


    /// <summary>
    /// 获取压缩适配器列表。
    /// </summary>
    /// <value>
    /// 返回 <see cref="List{ICompressorAdapter}"/>。
    /// </value>
    public List<ICompressionAdapter> Adapters { get; init; }

    /// <summary>
    /// 获取或设置 <see cref="ICompressionAdapter"/> 解析器。
    /// </summary>
    /// <value>
    /// 返回 <see cref="ICompressionAdapterResolver"/>。
    /// </value>
    public ICompressionAdapterResolver AdapterResolver { get; set; }


    /// <summary>
    /// 获取流式算法压缩器。
    /// </summary>
    /// <param name="algorithm">给定的 <see cref="CompressorAlgorithm"/>。</param>
    /// <returns>返回 <see cref="ICompressor{TCompressed}"/>。</returns>
    public ICompressor<Stream> GetCompressor(CompressorAlgorithm algorithm)
    {
        return algorithm switch
        {
            CompressorAlgorithm.Brotli => new StreamCompressor<BrotliStream>(this),
            CompressorAlgorithm.Deflate => new StreamCompressor<DeflateStream>(this),
            CompressorAlgorithm.GZip => new StreamCompressor<GZipStream>(this),
            CompressorAlgorithm.ZLib => new StreamCompressor<ZLibStream>(this),
            _ => throw new NotSupportedException($"The '{algorithm}' compressor is not supported")
        };
    }

}
