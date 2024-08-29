#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Infrastructure;

/// <summary>
/// 定义 <see cref="FluentStream"/> 静态扩展。
/// </summary>
public static class FluentStreamExtensions
{

    /// <summary>
    /// 基于当前的流畅文件路径读取流畅字节序列流。
    /// </summary>
    /// <param name="filePath">给定的 <see cref="FluentFilePath"/>。</param>
    /// <param name="useBufferedStream">是否使用缓冲流（可选；默认使用 <see cref="BufferedStream"/> 处理此文件流，除非文件流本身已是 <see cref="BufferedStream"/>）。</param>
    /// <returns>返回 <see cref="FluentStream"/>。</returns>
    public static FluentStream AsReadStream(this FluentFilePath filePath, bool useBufferedStream = true)
        => new(filePath.ReadStream(), useBufferedStream);

    /// <summary>
    /// 基于当前的流畅文件路径写入流畅字节序列流。
    /// </summary>
    /// <param name="filePath">给定的 <see cref="FluentFilePath"/>。</param>
    /// <param name="useBufferedStream">是否使用缓冲流（可选；默认使用 <see cref="BufferedStream"/> 处理此文件流，除非文件流本身已是 <see cref="BufferedStream"/>）。</param>
    /// <returns>返回 <see cref="FluentStream"/>。</returns>
    public static FluentStream AsWriteStream(this FluentFilePath filePath, bool useBufferedStream = true)
        => new(filePath.WriteStream(), useBufferedStream);

}
