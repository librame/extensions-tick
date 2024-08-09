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
/// 定义当前支持的压缩器算法枚举。
/// </summary>
public enum CompressorAlgorithm
{
    /// <summary>
    /// 未知。
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// Brotli 算法。
    /// </summary>
    Brotli = 1,

    /// <summary>
    /// Deflate 算法。
    /// </summary>
    Deflate = 2,

    /// <summary>
    /// GZip 算法。
    /// </summary>
    GZip = 4,

    /// <summary>
    /// ZLib 算法。
    /// </summary>
    ZLib = 8
}
