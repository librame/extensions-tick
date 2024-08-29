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
/// 定义继承 <see cref="ICompressor"/> 的泛型压缩器接口。
/// </summary>
/// <typeparam name="TCompressed">指定的被压缩类型。</typeparam>
public interface ICompressor<TCompressed> : ICompressor
{
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
    /// <param name="decompressed">给定的解压 <typeparamref name="TCompressed"/>。</param>
    void Decompress(TCompressed compressed, TCompressed decompressed);
}
