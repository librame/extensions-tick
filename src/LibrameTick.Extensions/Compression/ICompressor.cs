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
public interface ICompressor
{
    /// <summary>
    /// 获取压缩适配器。
    /// </summary>
    /// <value>
    /// 返回 <see cref="ICompressionAdapter"/>。
    /// </value>
    ICompressionAdapter Adapter { get; }

    /// <summary>
    /// 获取压缩选项。
    /// </summary>
    /// <value>
    /// 返回 <see cref="CompressionOptions"/>。
    /// </value>
    CompressionOptions Options { get; }

}
