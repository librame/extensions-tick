#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Configuration;
using Librame.Extensions.Dependency;

namespace Librame.Extensions.Compression;

/// <summary>
/// 定义压缩器静态扩展。
/// </summary>
public static class CompressorExtensions
{

    /// <summary>
    /// 压缩文件。
    /// </summary>
    /// <param name="originalFile">给定的原始 <see cref="IPathConfigurable"/>。</param>
    /// <param name="compressedFile">给定的压缩 <see cref="IPathConfigurable"/>。</param>
    /// <param name="algorithm">给定的 <see cref="CompressorAlgorithm"/>。</param>
    /// <param name="options">给定的 <see cref="CompressionOptions"/>（可选；默认使用 <see cref="StaticDefaultInitializer{CompressionOptions}.Default"/>）。</param>
    public static void CompressFile(this IPathConfigurable originalFile, IPathConfigurable compressedFile,
        CompressorAlgorithm algorithm, CompressionOptions? options = null)
        => originalFile.ToString().CompressFile(compressedFile.ToString(), algorithm, options);

    /// <summary>
    /// 压缩文件。
    /// </summary>
    /// <param name="originalFile">给定的原始文件。</param>
    /// <param name="compressedFile">给定的压缩文件。</param>
    /// <param name="algorithm">给定的 <see cref="CompressorAlgorithm"/>。</param>
    /// <param name="options">给定的 <see cref="CompressionOptions"/>（可选；默认使用 <see cref="StaticDefaultInitializer{CompressionOptions}.Default"/>）。</param>
    public static void CompressFile(this string originalFile, string compressedFile, CompressorAlgorithm algorithm,
        CompressionOptions? options = null)
    {
        using var originalStream = File.OpenRead(originalFile);
        using var compressedStream = File.Create(compressedFile);

        originalStream.Compress(compressedStream, algorithm, options);
    }

    /// <summary>
    /// 解压文件。
    /// </summary>
    /// <param name="compressedFile">给定的压缩 <see cref="IPathConfigurable"/>。</param>
    /// <param name="originalFile">给定的原始 <see cref="IPathConfigurable"/>。</param>
    /// <param name="algorithm">给定的 <see cref="CompressorAlgorithm"/>。</param>
    /// <param name="options">给定的 <see cref="CompressionOptions"/>（可选；默认使用 <see cref="StaticDefaultInitializer{CompressionOptions}.Default"/>）。</param>
    public static void DecompressFile(this IPathConfigurable compressedFile, IPathConfigurable originalFile,
        CompressorAlgorithm algorithm, CompressionOptions? options = null)
        => compressedFile.ToString().DecompressFile(originalFile.ToString(), algorithm, options);

    /// <summary>
    /// 解压文件。
    /// </summary>
    /// <param name="compressedFile">给定的压缩文件。</param>
    /// <param name="originalFile">给定的原始文件。</param>
    /// <param name="algorithm">给定的 <see cref="CompressorAlgorithm"/>。</param>
    /// <param name="options">给定的 <see cref="CompressionOptions"/>（可选；默认使用 <see cref="StaticDefaultInitializer{CompressionOptions}.Default"/>）。</param>
    public static void DecompressFile(this string compressedFile, string originalFile, CompressorAlgorithm algorithm,
        CompressionOptions? options = null)
    {
        using var compressedStream = File.OpenRead(compressedFile);
        using var originalStream = File.Create(originalFile);

        compressedStream.Decompress(originalStream, algorithm, options);
    }


    /// <summary>
    /// 压缩字节数组。
    /// </summary>
    /// <param name="original">给定的原始字节数组。</param>
    /// <param name="algorithm">给定的 <see cref="CompressorAlgorithm"/>。</param>
    /// <param name="options">给定的 <see cref="CompressionOptions"/>（可选；默认使用 <see cref="StaticDefaultInitializer{CompressionOptions}.Default"/>）。</param>
    /// <returns>返回字节数组。</returns>
    public static byte[] Compress(this byte[] original, CompressorAlgorithm algorithm,
        CompressionOptions? options = null)
    {
        using var originalStream = DependencyRegistration.CurrentContext.MemoryStreams.GetStream(original);
        using var compressedStream = DependencyRegistration.CurrentContext.MemoryStreams.GetStream();

        originalStream.Compress(compressedStream, algorithm, options);

        return compressedStream.GetReadOnlySequence().ToArray();
    }

    /// <summary>
    /// 解压字节数组。
    /// </summary>
    /// <param name="compressed">给定的已压缩字节数组。</param>
    /// <param name="algorithm">给定的 <see cref="CompressorAlgorithm"/>。</param>
    /// <param name="options">给定的 <see cref="CompressionOptions"/>（可选；默认使用 <see cref="StaticDefaultInitializer{CompressionOptions}.Default"/>）。</param>
    /// <returns>返回经过解压的字节数组。</returns>
    public static byte[] Decompress(this byte[] compressed, CompressorAlgorithm algorithm,
        CompressionOptions? options = null)
    {
        using var compressedStream = DependencyRegistration.CurrentContext.MemoryStreams.GetStream(compressed);
        using var originalStream = DependencyRegistration.CurrentContext.MemoryStreams.GetStream();

        compressedStream.Decompress(originalStream, algorithm, options);

        return originalStream.GetReadOnlySequence().ToArray();
    }


    /// <summary>
    /// 压缩流。
    /// </summary>
    /// <param name="original">给定的原始 <see cref="Stream"/>。</param>
    /// <param name="compressed">给定的压缩 <see cref="Stream"/>。</param>
    /// <param name="algorithm">给定的 <see cref="CompressorAlgorithm"/>。</param>
    /// <param name="options">给定的 <see cref="CompressionOptions"/>（可选；默认使用 <see cref="StaticDefaultInitializer{CompressionOptions}.Default"/>）。</param>
    public static void Compress(this Stream original, Stream compressed, CompressorAlgorithm algorithm,
        CompressionOptions? options = null)
    {
        options ??= CompressionOptions.Default;

        var compressor = options.GetCompressor(algorithm);

        compressor.Compress(original, compressed);
    }

    /// <summary>
    /// 解压流。
    /// </summary>
    /// <param name="compressed">给定的压缩 <see cref="Stream"/>。</param>
    /// <param name="original">给定的原始 <see cref="Stream"/>。</param>
    /// <param name="algorithm">给定的 <see cref="CompressorAlgorithm"/>。</param>
    /// <param name="options">给定的 <see cref="CompressionOptions"/>（可选；默认使用 <see cref="StaticDefaultInitializer{CompressionOptions}.Default"/>）。</param>
    public static void Decompress(this Stream compressed, Stream original, CompressorAlgorithm algorithm,
        CompressionOptions? options = null)
    {
        options ??= CompressionOptions.Default;

        var compressor = options.GetCompressor(algorithm);

        compressor.Decompress(compressed, original);
    }

}
