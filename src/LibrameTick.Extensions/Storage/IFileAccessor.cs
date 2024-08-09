#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Storage;

/// <summary>
/// 定义读写文件的存取器接口。
/// </summary>
public interface IFileAccessor
{
    /// <summary>
    /// 文件路径。
    /// </summary>
    string Path { get; }


    #region No Buffer

    /// <summary>
    /// 一次性读取小文件。
    /// </summary>
    /// <param name="offset">给定的读取偏移量（可选；默认从头开始读取）。</param>
    /// <returns>返回字节数组。</returns>
    byte[] Read(long offset = 0L);

    /// <summary>
    /// 一次性写入文件。
    /// </summary>
    /// <param name="bytes">给定的字节数组。</param>
    /// <param name="offset">给定的写入偏移量（可选；默认从头开始写入）。</param>
    void Write(byte[] bytes, long offset = 0L);


    /// <summary>
    /// 异步一次性读取小文件。
    /// </summary>
    /// <param name="offset">给定的读取偏移量（可选；默认从头开始读取）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回包含字节数组的异步操作。</returns>
    Task<byte[]> ReadAsync(long offset = 0L, CancellationToken cancellationToken = default);

    /// <summary>
    /// 异步一次性写入文件。
    /// </summary>
    /// <param name="bytes">给定的字节数组。</param>
    /// <param name="offset">给定的写入偏移量（可选；默认从头开始写入）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回异步操作。</returns>
    ValueTask WriteAsync(byte[] bytes, long offset = 0L, CancellationToken cancellationToken = default);

    #endregion


    #region Buffer

    /// <summary>
    /// 使用缓冲区读取大文件。
    /// </summary>
    /// <param name="bufferAction">给定的字节缓冲区动作。</param>
    /// <param name="offset">给定的读取偏移量（可选；默认从头开始读取）。</param>
    /// <param name="bufferSize">给定的缓冲区大小（可选；默认为 512）。</param>
    /// <returns>返回实际读取的文件长度。</returns>
    long BufferRead(Action<byte[]> bufferAction, long offset = 0L, int bufferSize = 512);

    /// <summary>
    /// 使用缓冲区写入大文件。
    /// </summary>
    /// <param name="bufferFunc">给定的缓冲区（传入参数为当前偏移量）。</param>
    /// <param name="offset">给定的读取偏移量（可选；默认从头开始读取）。</param>
    /// <returns>返回实际写入的文件长度。</returns>
    long BufferWrite(Func<long, byte[]?> bufferFunc, long offset = 0L);


    /// <summary>
    /// 异步使用缓冲区读取大文件。
    /// </summary>
    /// <param name="bufferAction">给定的字节缓冲区动作。</param>
    /// <param name="offset">给定的读取偏移量（可选；默认从头开始读取）。</param>
    /// <param name="bufferSize">给定的缓冲区大小（可选；默认为 512）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回包含实际读取文件长度的异步操作。</returns>
    ValueTask<long> BufferReadAsync(Action<byte[]> bufferAction, long offset = 0L, int bufferSize = 512,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 异步使用缓冲区写入大文件。
    /// </summary>
    /// <param name="bufferFunc">给定的缓冲区（传入参数为当前偏移量）。</param>
    /// <param name="offset">给定的读取偏移量（可选；默认从头开始读取）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回包含实际写入文件长度的异步操作。</returns>
    ValueTask<long> BufferWriteAsync(Func<long, byte[]?> bufferFunc, long offset = 0L,
        CancellationToken cancellationToken = default);

    #endregion


    #region Equals

    /// <summary>
    /// 比较两个文件路径是否相等。默认自动根据不同操作系统使用不同的路径字符串比较方法。
    /// </summary>
    /// <param name="otherPath">给定要比较的文件路径。</param>
    /// <returns>返回是否相等的布尔值。</returns>
    bool PathEquals(string otherPath);


    /// <summary>
    /// 比较两个文件是否相等（支持比较文件路径、大小、内容等）。
    /// </summary>
    /// <param name="otherPath">给定要比较的文件路径。</param>
    /// <param name="comparePath">是否比较文件路径（可选；默认不启用，如果启用将使用 <see cref="PathEquals(string)"/> 比较）。</param>
    /// <param name="compareSize">是否比较文件大小（可选；默认不启用，也不推荐启用，除非你能确定文件大小相等就表示是同一个文件）。</param>
    /// <param name="bufferSize">给定的缓冲区大小（可选；默认为 512）。</param>
    /// <returns>返回是否相等的布尔值。</returns>
    bool Equals(string otherPath, bool comparePath = false, bool compareSize = false, int bufferSize = 512);

    /// <summary>
    /// 比较两个文件是否相等（支持比较文件路径、大小、内容等）。
    /// </summary>
    /// <param name="otherPath">给定要比较的文件路径。</param>
    /// <param name="comparePath">是否比较文件路径（可选；默认不启用，如果启用将使用 <see cref="PathEquals(string)"/> 比较）。</param>
    /// <param name="compareSize">是否比较文件大小（可选；默认不启用，也不推荐启用，除非你能确定文件大小相等就表示是同一个文件）。</param>
    /// <param name="bufferSize">给定的缓冲区大小（可选；默认为 512）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回是否相等的布尔值。</returns>
    ValueTask<bool> EqualsAsync(string otherPath, bool comparePath = false, bool compareSize = false,
        int bufferSize = 512, CancellationToken cancellationToken = default);

    #endregion

}
