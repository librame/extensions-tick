#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Compression;

namespace Librame.Extensions.Infrastructure;

/// <summary>
/// 定义 <see cref="FluentFilePath"/> 压缩器静态扩展。
/// </summary>
public static class FluentFilePathCompressorExtensions
{

    /// <summary>
    /// 压缩文件。
    /// </summary>
    /// <param name="originalFile">给定的原始 <see cref="FluentFilePath"/>。</param>
    /// <param name="compressedFile">给定的压缩 <see cref="FluentFilePath"/>。</param>
    /// <param name="options">给定的 <see cref="CompressionOptions"/>（可选；默认使用 <see cref="StaticDefaultInitializer{CompressionOptions}.Default"/>）。</param>
    /// <param name="algorithm">给定的 <see cref="CompressorAlgorithm"/>（可选；默认使用 <see cref="CompressionOptions.Algorithm"/>）。</param>
    public static void Compress(this FluentFilePath originalFile, FluentFilePath compressedFile,
        CompressionOptions? options = null, CompressorAlgorithm? algorithm = null)
    {
        using var originalStream = originalFile.ReadStream();

        originalStream.CompressFile(options, algorithm, compressor => compressedFile.WriteStream());
    }

    /// <summary>
    /// 压缩文件。
    /// </summary>
    /// <param name="originalFile">给定的原始 <see cref="FluentFilePath"/>。</param>
    /// <param name="options">给定的 <see cref="CompressionOptions"/>（可选；默认使用 <see cref="StaticDefaultInitializer{CompressionOptions}.Default"/>）。</param>
    /// <param name="algorithm">给定的 <see cref="CompressorAlgorithm"/>（可选；默认使用 <see cref="CompressionOptions.Algorithm"/>）。</param>
    /// <returns>返回压缩 <see cref="FluentFilePath"/>。</returns>
    public static FluentFilePath Compress(this FluentFilePath originalFile,
        CompressionOptions? options = null, CompressorAlgorithm? algorithm = null)
    {
        using var originalStream = originalFile.ReadStream();

        var compressedFile = originalStream.CompressFile(options, algorithm);

        return new(compressedFile, originalFile.Dependency, originalFile.Encoding);
    }


    /// <summary>
    /// 解压文件。
    /// </summary>
    /// <param name="compressedFile">给定的压缩文件。</param>
    /// <param name="decompressedFile">给定的解压文件。</param>
    /// <param name="options">给定的 <see cref="CompressionOptions"/>（可选；默认使用 <see cref="StaticDefaultInitializer{CompressionOptions}.Default"/>）。</param>
    /// <param name="algorithm">给定的 <see cref="CompressorAlgorithm"/>（可选；默认使用 <see cref="CompressionOptions.Algorithm"/>）。</param>
    public static void Decompress(this FluentFilePath compressedFile, FluentFilePath decompressedFile,
        CompressionOptions? options = null, CompressorAlgorithm? algorithm = null)
    {
        using var compressedStream = compressedFile.ReadStream();

        compressedStream.DecompressFile(options, algorithm, compressor => decompressedFile.WriteStream());
    }

    /// <summary>
    /// 解压文件。
    /// </summary>
    /// <param name="compressedFile">给定的压缩文件。</param>
    /// <param name="options">给定的 <see cref="CompressionOptions"/>（可选；默认使用 <see cref="StaticDefaultInitializer{CompressionOptions}.Default"/>）。</param>
    /// <param name="algorithm">给定的 <see cref="CompressorAlgorithm"/>（可选；默认使用 <see cref="CompressionOptions.Algorithm"/>）。</param>
    /// <returns>返回解压 <see cref="FluentFilePath"/>。</returns>
    public static FluentFilePath Decompress(this FluentFilePath compressedFile,
        CompressionOptions? options = null, CompressorAlgorithm? algorithm = null)
    {
        using var compressedStream = compressedFile.ReadStream();

        var decompressedFile = compressedStream.DecompressFile(options, algorithm);

        return new(decompressedFile, compressedFile.Dependency, compressedFile.Encoding);
    }

}
