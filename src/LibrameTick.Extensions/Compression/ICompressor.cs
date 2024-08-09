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
/// 定义流式压缩器接口。
/// </summary>
/// <typeparam name="TCompressed">指定的被压缩类型。</typeparam>
public interface ICompressor<TCompressed>
{
    /// <summary>
    /// 获取压缩器选项。
    /// </summary>
    /// <value>
    /// 返回 <see cref="CompressionOptions"/>。
    /// </value>
    CompressionOptions Options { get; }


    /// <summary>
    /// 压缩 <typeparamref name="TCompressed"/>。
    /// </summary>
    /// <param name="original">给定原始的 <typeparamref name="TCompressed"/>。</param>
    /// <param name="compressed">给定已压缩的 <typeparamref name="TCompressed"/>。</param>
    void Compress(TCompressed original, TCompressed compressed);

    /// <summary>
    /// 解压 <typeparamref name="TCompressed"/>。
    /// </summary>
    /// <param name="compressed">给定已压缩的 <typeparamref name="TCompressed"/>。</param>
    /// <param name="original">给定原始的 <typeparamref name="TCompressed"/>。</param>
    void Decompress(TCompressed compressed, TCompressed original);
}
