#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions;

/// <summary>
/// <see cref="Stream"/> 静态扩展。
/// </summary>
public static class StreamExtensions
{

    /// <summary>
    /// 使用缓冲流包装当前流。
    /// </summary>
    /// <param name="stream">给定的 <see cref="Stream"/>。</param>
    /// <returns>返回 <see cref="BufferedStream"/>。</returns>
    public static BufferedStream AsBufferedStream(this Stream stream)
    {
        if (stream is BufferedStream bufferedStream)
        {
            return bufferedStream;
        }

        return new(stream);
    }

    /// <summary>
    /// 重置流的原始位置（如果当前流位置不是 0）。
    /// </summary>
    /// <param name="stream">给定的 <see cref="Stream"/>。</param>
    /// <param name="offset">给定的偏移量（可选；默认为 0）。</param>
    public static void ResetOriginalPositionIfNotBegin(this Stream stream, long offset = 0)
    {
        if (stream.Position != 0)
        {
            stream.Seek(offset, SeekOrigin.Begin);
        }
    }

}
