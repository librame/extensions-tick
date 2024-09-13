#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Infrastructure;
using InternalCompressors = Librame.Extensions.Compression.Internal.CompressionAdapters;
using InternalCompressorAdapterResolver = Librame.Extensions.Compression.Internal.CompressionAdapterResolver;

namespace Librame.Extensions.Compression;

/// <summary>
/// 定义压缩选项。
/// </summary>
public sealed class CompressionOptions : StaticDefaultInitializer<CompressionOptions>, IReferencable
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
    /// 获取或设置当前是否已启用此功能引用。
    /// </summary>
    /// <remarks>
    /// 主要用于控制外部引用此压缩功能是否已启用。默认不启用。
    /// </remarks>
    public bool IsRefEnabled { get; set; }

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
    /// 获取或设置是否使用缓冲流进行读写操作。
    /// </summary>
    /// <remarks>
    /// 默认不使用缓冲流。推荐在大数据 IO 中使用缓冲流，以减少内存占用，提高读写性能。
    /// </remarks>
    /// <value>
    /// 返回是否使用缓冲流的布尔值。
    /// </value>
    public bool UseBufferedStream { get; set; }

    /// <summary>
    /// 获取或设置压缩器算法。
    /// </summary>
    /// <remarks>
    /// 默认使用 <see cref="CompressorAlgorithm.GZip"/>。
    /// </remarks>
    /// <value>
    /// 返回 <see cref="CompressorAlgorithm"/>。
    /// </value>
    public CompressorAlgorithm Algorithm { get; set; } = CompressorAlgorithm.GZip;

    /// <summary>
    /// 获取或设置压缩文件扩展名方法。
    /// </summary>
    /// <remarks>
    /// 默认在当前文件扩展名后附加压缩扩展名。
    /// </remarks>
    /// <value>
    /// 返回方法委托。
    /// </value>
    public Func<string, string>? CompressedFileExtensionFunc { get; set; }

    /// <summary>
    /// 获取或设置解压文件扩展名方法。
    /// </summary>
    /// <remarks>
    /// 默认在当前压缩文件扩展名后附加解压扩展名“.decomp”。
    /// </remarks>
    /// <value>
    /// 返回方法委托。
    /// </value>
    public Func<string, string> DecompressedFileExtensionFunc { get; set; }
        = compExt => $"{compExt}.decomp";

    /// <summary>
    /// 获取或设置解压流的前置动作。
    /// </summary>
    /// <remarks>
    /// 传入参数根据上下文环境可能是 <see cref="FileStream"/> 或 <see cref="BufferedStream"/>。
    /// </remarks>
    /// <value>
    /// 返回动作委托。
    /// </value>
    public Action<Stream>? DecompressBeforeAction { get; set; }


    /// <summary>
    /// 构建解压文件扩展名。
    /// </summary>
    /// <param name="adapter">给定的 <see cref="ICompressionAdapter"/>。</param>
    /// <returns>返回扩展名字符串。</returns>
    public string? BuildDecompressedFileExtension(ICompressionAdapter adapter)
        => adapter.BeAdaptedFileExtension is null ? null : DecompressedFileExtensionFunc(adapter.BeAdaptedFileExtension);

    /// <summary>
    /// 获取流式算法压缩器。
    /// </summary>
    /// <param name="algorithm">给定的 <see cref="CompressorAlgorithm"/>（可选；默认使用 <see cref="Algorithm"/>）。</param>
    /// <returns>返回 <see cref="ICompressor{TCompressed}"/>。</returns>
    public ICompressor<Stream> GetCompressor(CompressorAlgorithm? algorithm = null)
    {
        algorithm ??= Algorithm;

        return algorithm switch
        {
            CompressorAlgorithm.Brotli => new StreamCompressor<BrotliStream>(this),
            CompressorAlgorithm.Deflate => new StreamCompressor<DeflateStream>(this),
            CompressorAlgorithm.GZip => new StreamCompressor<GZipStream>(this),
            CompressorAlgorithm.ZLib => new StreamCompressor<ZLibStream>(this),
            _ => throw new NotSupportedException($"The '{algorithm}' compressor is not supported.")
        };
    }

}
