#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Dependency;
using Librame.Extensions.Infrastructure;

namespace Librame.Extensions.Compression;

/// <summary>
/// 定义压缩器静态扩展。
/// </summary>
public static class CompressorExtensions
{

    #region FileStream

    /// <summary>
    /// 压缩文件。
    /// </summary>
    /// <param name="originalFile">给定的原始文件。</param>
    /// <param name="compressedFile">给定的压缩文件。</param>
    /// <param name="options">给定的 <see cref="CompressionOptions"/>（可选；默认使用 <see cref="StaticDefaultInitializer{CompressionOptions}.Default"/>）。</param>
    /// <param name="algorithm">给定的 <see cref="CompressorAlgorithm"/>（可选；默认使用 <see cref="CompressionOptions.Algorithm"/>）。</param>
    public static void CompressFile(this string originalFile, string compressedFile,
        CompressionOptions? options = null, CompressorAlgorithm? algorithm = null)
    {
        using var originalStream = File.OpenRead(originalFile);

        originalStream.CompressFile(options, algorithm,
            compressor => File.Create(compressedFile));
    }

    /// <summary>
    /// 压缩文件。
    /// </summary>
    /// <param name="originalFile">给定的原始文件。</param>
    /// <param name="options">给定的 <see cref="CompressionOptions"/>（可选；默认使用 <see cref="StaticDefaultInitializer{CompressionOptions}.Default"/>）。</param>
    /// <param name="algorithm">给定的 <see cref="CompressorAlgorithm"/>（可选；默认使用 <see cref="CompressionOptions.Algorithm"/>）。</param>
    /// <returns>返回压缩的文件路径。</returns>
    public static string CompressFile(this string originalFile,
        CompressionOptions? options = null, CompressorAlgorithm? algorithm = null)
    {
        using var originalStream = File.OpenRead(originalFile);

        return originalStream.CompressFile(options, algorithm);
    }

    /// <summary>
    /// 压缩文件。
    /// </summary>
    /// <param name="originalStream">给定的原始 <see cref="FileStream"/>。</param>
    /// <param name="options">给定的 <see cref="CompressionOptions"/>（可选；默认使用 <see cref="StaticDefaultInitializer{CompressionOptions}.Default"/>）。</param>
    /// <param name="algorithm">给定的 <see cref="CompressorAlgorithm"/>（可选；默认使用 <see cref="CompressionOptions.Algorithm"/>）。</param>
    /// <param name="compressedStreamFunc">给定创建压缩 <see cref="FileStream"/> 的方法（可选；默认自动基于原始文件名附加压缩扩展名）。</param>
    /// <returns>返回压缩的文件路径。</returns>
    public static string CompressFile(this FileStream originalStream,
        CompressionOptions? options = null, CompressorAlgorithm? algorithm = null,
        Func<ICompressor, FileStream>? compressedStreamFunc = null)
    {
        options ??= CompressionOptions.Default;
        compressedStreamFunc ??= LocalCompressedStream;

        var compressor = options.GetCompressor(algorithm);
        using var compressedStream = compressedStreamFunc(compressor);

        if (!options.UseBufferedStream)
        {
            compressor.Compress(originalStream, compressedStream);
            return compressedStream.Name;
        }

        using var bufferedOriginalStream = new BufferedStream(originalStream);
        using var bufferedCompressedStream = new BufferedStream(compressedStream);

        compressor.Compress(bufferedOriginalStream, bufferedCompressedStream);
        return compressedStream.Name;


        FileStream LocalCompressedStream(ICompressor compressor)
        {
            var compressFile = originalStream.Name.AppendExtension(compressor.Adapter.BeAdaptedFileExtension,
                options.CompressedFileExtensionFunc);

            return File.Create(compressFile);
        }
    }


    /// <summary>
    /// 解压文件。
    /// </summary>
    /// <param name="compressedFile">给定的压缩文件。</param>
    /// <param name="decompressedFile">给定的解压文件。</param>
    /// <param name="options">给定的 <see cref="CompressionOptions"/>（可选；默认使用 <see cref="StaticDefaultInitializer{CompressionOptions}.Default"/>）。</param>
    /// <param name="algorithm">给定的 <see cref="CompressorAlgorithm"/>（可选；默认使用 <see cref="CompressionOptions.Algorithm"/>）。</param>
    public static void DecompressFile(this string compressedFile, string decompressedFile,
        CompressionOptions? options = null, CompressorAlgorithm? algorithm = null)
    {
        using var compressedStream = File.OpenRead(compressedFile);

        compressedStream.DecompressFile(options, algorithm,
            Compressor => File.Create(decompressedFile));
    }

    /// <summary>
    /// 解压文件。
    /// </summary>
    /// <param name="compressedFile">给定的压缩文件。</param>
    /// <param name="options">给定的 <see cref="CompressionOptions"/>（可选；默认使用 <see cref="StaticDefaultInitializer{CompressionOptions}.Default"/>）。</param>
    /// <param name="algorithm">给定的 <see cref="CompressorAlgorithm"/>（可选；默认使用 <see cref="CompressionOptions.Algorithm"/>）。</param>
    /// <returns>返回解压的文件路径。</returns>
    public static string DecompressFile(this string compressedFile,
        CompressionOptions? options = null, CompressorAlgorithm? algorithm = null)
    {
        using var compressedStream = File.OpenRead(compressedFile);

        return compressedStream.DecompressFile(options, algorithm);
    }

    /// <summary>
    /// 解压文件。
    /// </summary>
    /// <param name="compressedStream">给定的已压缩 <see cref="FileStream"/>。</param>
    /// <param name="options">给定的 <see cref="CompressionOptions"/>（可选；默认使用 <see cref="StaticDefaultInitializer{CompressionOptions}.Default"/>）。</param>
    /// <param name="algorithm">给定的 <see cref="CompressorAlgorithm"/>（可选；默认使用 <see cref="CompressionOptions.Algorithm"/>）。</param>
    /// <param name="decompressedStreamFunc">给定创建解压 <see cref="FileStream"/> 的方法（可选）。</param>
    /// <returns>返回解压的文件路径。</returns>
    public static string DecompressFile(this FileStream compressedStream,
        CompressionOptions? options = null, CompressorAlgorithm? algorithm = null,
        Func<ICompressor, FileStream>? decompressedStreamFunc = null)
    {
        options ??= CompressionOptions.Default;
        decompressedStreamFunc ??= LocalDecompressedStream;

        var compressor = options.GetCompressor(algorithm);
        using var decompressedStream = decompressedStreamFunc(compressor);

        if (!options.UseBufferedStream)
        {
            compressor.Decompress(compressedStream, decompressedStream);
            options.DecompressBeforeAction?.Invoke(decompressedStream);

            return decompressedStream.Name;
        }

        using var bufferedCompressedStream = new BufferedStream(compressedStream);
        using var bufferedDecompressedStream = new BufferedStream(decompressedStream);

        compressor.Decompress(bufferedCompressedStream, bufferedDecompressedStream);
        options.DecompressBeforeAction?.Invoke(bufferedDecompressedStream);

        return decompressedStream.Name;


        FileStream LocalDecompressedStream(ICompressor compressor)
        {
            var decompressedFileExtension = options.BuildDecompressedFileExtension(compressor.Adapter);
            var decompressedFile = compressedStream.Name.ChangeExtension(decompressedFileExtension);

            return File.Create(decompressedFile);
        }
    }

    #endregion


    #region MemoryStream

    /// <summary>
    /// 压缩字节数组。
    /// </summary>
    /// <param name="original">给定的原始字节数组。</param>
    /// <param name="options">给定的 <see cref="CompressionOptions"/>（可选；默认使用 <see cref="StaticDefaultInitializer{CompressionOptions}.Default"/>）。</param>
    /// <param name="algorithm">给定的 <see cref="CompressorAlgorithm"/>（可选；默认使用 <see cref="CompressionOptions.Algorithm"/>）。</param>
    /// <returns>返回字节数组。</returns>
    public static byte[] Compress(this byte[] original,
        CompressionOptions? options = null, CompressorAlgorithm? algorithm = null)
    {
        options ??= CompressionOptions.Default;
        algorithm ??= options.Algorithm;

        var memoryStreams = DependencyRegistration.CurrentContext.MemoryStreams;

        using var originalStream = memoryStreams.GetStream(original);
        using var compressedStream = memoryStreams.GetStream();

        originalStream.Compress(compressedStream, options, algorithm.Value);

        return compressedStream.GetReadOnlySequence().ToArray();
    }

    /// <summary>
    /// 解压字节数组。
    /// </summary>
    /// <param name="compressed">给定的已压缩字节数组。</param>
    /// <param name="options">给定的 <see cref="CompressionOptions"/>（可选；默认使用 <see cref="StaticDefaultInitializer{CompressionOptions}.Default"/>）。</param>
    /// <param name="algorithm">给定的 <see cref="CompressorAlgorithm"/>（可选；默认使用 <see cref="CompressionOptions.Algorithm"/>）。</param>
    /// <returns>返回经过解压的字节数组。</returns>
    public static byte[] Decompress(this byte[] compressed,
        CompressionOptions? options = null, CompressorAlgorithm? algorithm = null)
    {
        options ??= CompressionOptions.Default;
        algorithm ??= options.Algorithm;

        var memoryStreams = DependencyRegistration.CurrentContext.MemoryStreams;

        using var compressedStream = memoryStreams.GetStream(compressed);
        using var decompressedStream = memoryStreams.GetStream();

        compressedStream.Decompress(decompressedStream, options, algorithm.Value);

        return decompressedStream.GetReadOnlySequence().ToArray();
    }

    #endregion


    #region Stream

    /// <summary>
    /// 压缩流。
    /// </summary>
    /// <param name="original">给定的原始 <see cref="Stream"/>。</param>
    /// <param name="compressed">给定的压缩 <see cref="Stream"/>。</param>
    /// <param name="options">给定的 <see cref="CompressionOptions"/>。</param>
    /// <param name="algorithm">给定的 <see cref="CompressorAlgorithm"/>（可选；默认使用 <see cref="CompressionOptions.Algorithm"/>）。</param>
    public static void Compress(this Stream original, Stream compressed,
        CompressionOptions options, CompressorAlgorithm? algorithm = null)
    {
        var compressor = options.GetCompressor(algorithm);
        
        compressor.Compress(original, compressed);
    }

    /// <summary>
    /// 解压流。
    /// </summary>
    /// <param name="compressed">给定的压缩 <see cref="Stream"/>。</param>
    /// <param name="decompressed">给定的解压 <see cref="Stream"/>。</param>
    /// <param name="options">给定的 <see cref="CompressionOptions"/>。</param>
    /// <param name="algorithm">给定的 <see cref="CompressorAlgorithm"/>（可选；默认使用 <see cref="CompressionOptions.Algorithm"/>）。</param>
    public static void Decompress(this Stream compressed, Stream decompressed,
        CompressionOptions options, CompressorAlgorithm? algorithm = null)
    {
        var compressor = options.GetCompressor(algorithm);

        compressor.Decompress(compressed, decompressed);
    }

    #endregion

}
