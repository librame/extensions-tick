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
/// 定义抽象实现 <see cref="ICompressor{TCompressed}"/> 的泛型压缩器接口。
/// </summary>
/// <typeparam name="TAdapted">指定的被适配类型。</typeparam>
/// <typeparam name="TCompressed">指定的被压缩类型。</typeparam>
public abstract class Compressor<TAdapted, TCompressed> : ICompressor<TCompressed>
{
    /// <summary>
    /// 构造一个 <see cref="Compressor{TAdapted, TCompressed}"/>。
    /// </summary>
    /// <param name="options">给定的 <see cref="CompressionOptions"/>。</param>
    /// <exception cref="ArgumentException">
    /// Cannot resolve compressor adapter for '<see cref="CompressionAdapter{TAdapted, TCompressed}"/>'.
    /// </exception>
    public Compressor(CompressionOptions options)
    {
        var apdater = options.AdapterResolver.Resolve<TAdapted, TCompressed>();
        if (apdater is null)
        {
            throw new ArgumentException($"Cannot resolve compressor adapter for '{typeof(CompressionAdapter<TAdapted, TCompressed>).Name}'.");
        }

        Options = options;
        Adapter = apdater;
    }


    /// <summary>
    /// 获取压缩器选项。
    /// </summary>
    /// <value>
    /// 返回 <see cref="CompressionOptions"/>。
    /// </value>
    public CompressionOptions Options { get; init; }

    /// <summary>
    /// 获取压缩器适配器。
    /// </summary>
    /// <value>
    /// 返回 <see cref="CompressionAdapter{TAdapted, TCompressed}"/>。
    /// </value>
    public CompressionAdapter<TAdapted, TCompressed> Adapter { get; init; }


    /// <summary>
    /// 压缩 <typeparamref name="TCompressed"/>。
    /// </summary>
    /// <param name="original">给定原始的 <typeparamref name="TCompressed"/>。</param>
    /// <param name="compressed">给定已压缩的 <typeparamref name="TCompressed"/>。</param>
    public abstract void Compress(TCompressed original, TCompressed compressed);

    /// <summary>
    /// 解压 <typeparamref name="TCompressed"/>。
    /// </summary>
    /// <param name="compressed">给定已压缩的 <typeparamref name="TCompressed"/>。</param>
    /// <param name="original">给定原始的 <typeparamref name="TCompressed"/>。</param>
    public abstract void Decompress(TCompressed compressed, TCompressed original);
}
