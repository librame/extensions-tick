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
/// 定义抽象实现 <see cref="ICompressionAdapter"/> 的压缩适配器。
/// </summary>
/// <typeparam name="TAdapted">指定的被适配类型。</typeparam>
/// <typeparam name="TCompressed">指定的被压缩类型。</typeparam>
/// <param name="adapterFunc">给定的流式适配器方法。</param>
/// <param name="beAdaptedFileExtension">给定的被适配文件扩展名。</param>
public class CompressionAdapter<TAdapted, TCompressed>(
    Func<TCompressed, CompressionMode, CompressionOptions, TAdapted> adapterFunc,
    string? beAdaptedFileExtension) : ICompressionAdapter
{
    /// <summary>
    /// 获取被适配文件扩展名。
    /// </summary>
    /// <value>
    /// 返回文件扩展名字符串。
    /// </value>
    public string? BeAdaptedFileExtension { get; init; } = beAdaptedFileExtension;

    /// <summary>
    /// 获取被适配类型。
    /// </summary>
    /// <value>
    /// 返回 <see cref="Type"/>。
    /// </value>
    public virtual Type BeAdaptedType => typeof(TAdapted);

    /// <summary>
    /// 获取被压缩类型。
    /// </summary>
    /// <value>
    /// 返回 <see cref="Type"/>。
    /// </value>
    public virtual Type BeCompressedType => typeof(TCompressed);

    /// <summary>
    /// 获取适配器类型。
    /// </summary>
    /// <value>
    /// 返回 <see cref="Type"/>。
    /// </value>
    public virtual Type AdapterType => GetType();

    /// <summary>
    /// 获取适配器名称。
    /// </summary>
    /// <value>
    /// 返回名称字符串。
    /// </value>
    public virtual string AdapterName => AdapterType.Name;


    /// <summary>
    /// 获取压缩适配器。
    /// </summary>
    /// <param name="original">给定的原始 <typeparamref name="TCompressed"/>。</param>
    /// <param name="options">给定的 <see cref="CompressionOptions"/>。</param>
    /// <returns>返回 <typeparamref name="TAdapted"/>。</returns>
    public virtual TAdapted GetCompress(TCompressed original, CompressionOptions options)
        => adapterFunc(original, CompressionMode.Compress, options);

    /// <summary>
    /// 获取解压适配器。
    /// </summary>
    /// <param name="compressed">给定的 <typeparamref name="TCompressed"/>。</param>
    /// <param name="options">给定的 <see cref="CompressionOptions"/>。</param>
    /// <returns>返回 <typeparamref name="TAdapted"/>。</returns>
    public virtual TAdapted GetDecompress(TCompressed compressed, CompressionOptions options)
        => adapterFunc(compressed, CompressionMode.Decompress, options);

}
