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
/// 定义通过 <see cref="RandomAccess"/> 以线程安全的方式实现 <see cref="IBinaryAccessFilePlugin"/> 文件的二进制存取插件。
/// </summary>
/// <param name="path">给定的 <see cref="FluentFilePath"/>。</param>
public class BinaryAccessFilePlugin(FluentFilePath path)
    : FilePlugin(path), IBinaryAccessFilePlugin
{

    #region No Buffer

    /// <summary>
    /// 一次性读取小文件。
    /// </summary>
    /// <param name="offset">给定的读取偏移量（可选；默认从头开始读取）。</param>
    /// <returns>返回字节数组。</returns>
    public byte[] Read(long offset = 0L)
    {
        using var handle = File.OpenHandle(Path);

        var length = RandomAccess.GetLength(handle);
        var buffer = new byte[length - offset];

        RandomAccess.Read(handle, buffer, offset);
        return buffer;
    }

    /// <summary>
    /// 一次性写入文件。
    /// </summary>
    /// <param name="bytes">给定的字节数组。</param>
    /// <param name="offset">给定的写入偏移量（可选；默认从头开始写入）。</param>
    public void Write(byte[] bytes, long offset = 0L)
    {
        using var handle = File.OpenHandle(Path, FileMode.Create, FileAccess.Write, FileShare.Read);

        RandomAccess.Write(handle, bytes, offset);
    }


    /// <summary>
    /// 异步一次性读取小文件。
    /// </summary>
    /// <param name="offset">给定的读取偏移量（可选；默认从头开始读取）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回包含字节数组的异步操作。</returns>
    public async Task<byte[]> ReadAsync(long offset = 0L, CancellationToken cancellationToken = default)
    {
        using var handle = File.OpenHandle(Path);

        var length = RandomAccess.GetLength(handle);
        var buffer = new byte[length - offset];

        await RandomAccess.ReadAsync(handle, buffer, offset, cancellationToken).ConfigureAwait(false);
        return buffer;
    }

    /// <summary>
    /// 异步一次性写入文件。
    /// </summary>
    /// <param name="bytes">给定的字节数组。</param>
    /// <param name="offset">给定的写入偏移量（可选；默认从头开始写入）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回异步操作。</returns>
    public async ValueTask WriteAsync(byte[] bytes, long offset = 0L, CancellationToken cancellationToken = default)
    {
        using var handle = File.OpenHandle(Path, FileMode.Create, FileAccess.Write, FileShare.Read);

        await RandomAccess.WriteAsync(handle, bytes, offset, cancellationToken).ConfigureAwait(false);
    }

    #endregion


    #region Buffer

    /// <summary>
    /// 使用缓冲区读取大文件。
    /// </summary>
    /// <param name="bufferAction">给定的字节缓冲区动作。</param>
    /// <param name="offset">给定的读取偏移量（可选；默认从头开始读取）。</param>
    /// <param name="bufferSize">给定的缓冲区大小（可选；默认为 512）。</param>
    /// <returns>返回实际读取的文件长度。</returns>
    public long BufferRead(Action<byte[]> bufferAction, long offset = 0L, int bufferSize = 512)
    {
        using (var handle = File.OpenHandle(Path))
        {
            var buffer = ArrayPool<byte>.Shared.Rent(bufferSize);

            var currentOffset = offset;
            while (true)
            {
                var readLength = RandomAccess.Read(handle, buffer, currentOffset);
                if (readLength == 0)
                    break;

                bufferAction(buffer);
                currentOffset += readLength;
            }

            ArrayPool<byte>.Shared.Return(buffer, clearArray: true);

            return currentOffset - offset;
        }
    }

    /// <summary>
    /// 使用缓冲区写入大文件。
    /// </summary>
    /// <param name="bufferFunc">给定的缓冲区（传入参数为当前偏移量）。</param>
    /// <param name="offset">给定的读取偏移量（可选；默认从头开始读取）。</param>
    /// <returns>返回实际写入的文件长度。</returns>
    public long BufferWrite(Func<long, byte[]?> bufferFunc, long offset = 0L)
    {
        using var handle = File.OpenHandle(Path, FileMode.Create, FileAccess.Write, FileShare.Read);

        var currentOffset = offset;
        while (true)
        {
            var buffer = bufferFunc(currentOffset);
            if (buffer is null || buffer.Length == 0)
                break;

            RandomAccess.Write(handle, buffer, currentOffset);

            currentOffset += buffer.Length;
        }

        return currentOffset - offset;
    }


    /// <summary>
    /// 异步使用缓冲区读取大文件。
    /// </summary>
    /// <param name="bufferAction">给定的字节缓冲区动作。</param>
    /// <param name="offset">给定的读取偏移量（可选；默认从头开始读取）。</param>
    /// <param name="bufferSize">给定的缓冲区大小（可选；默认为 512）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回包含实际读取文件长度的异步操作。</returns>
    public async ValueTask<long> BufferReadAsync(Action<byte[]> bufferAction, long offset = 0L, int bufferSize = 512,
        CancellationToken cancellationToken = default)
    {
        using var handle = File.OpenHandle(Path);

        var buffer = ArrayPool<byte>.Shared.Rent(bufferSize);

        var currentOffset = offset;
        while (true)
        {
            var readLength = await RandomAccess.ReadAsync(handle, buffer, currentOffset, cancellationToken)
                .ConfigureAwait(false);

            if (readLength == 0) break;

            bufferAction(buffer);
            currentOffset += readLength;
        }

        ArrayPool<byte>.Shared.Return(buffer, clearArray: true);

        return currentOffset - offset;
    }

    /// <summary>
    /// 异步使用缓冲区写入大文件。
    /// </summary>
    /// <param name="bufferFunc">给定的缓冲区（传入参数为当前偏移量）。</param>
    /// <param name="offset">给定的读取偏移量（可选；默认从头开始读取）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回包含实际写入文件长度的异步操作。</returns>
    public async ValueTask<long> BufferWriteAsync(Func<long, byte[]?> bufferFunc, long offset = 0L,
        CancellationToken cancellationToken = default)
    {
        using var handle = File.OpenHandle(Path, FileMode.Create, FileAccess.Write, FileShare.Read);

        var currentOffset = offset;
        while (true)
        {
            var buffer = bufferFunc(currentOffset);

            if (buffer is null || buffer.Length == 0) break;

            await RandomAccess.WriteAsync(handle, buffer, currentOffset, cancellationToken).ConfigureAwait(false);

            currentOffset += buffer.Length;
        }

        return currentOffset - offset;
    }

    #endregion


    #region Equals

    /// <summary>
    /// 比较两个文件路径是否相等。默认自动根据不同操作系统使用不同的路径字符串比较方法。
    /// </summary>
    /// <param name="otherPath">给定要比较的文件路径。</param>
    /// <returns>返回是否相等的布尔值。</returns>
    public bool PathEquals(string otherPath)
        => PathEqualityComparer.Default.Equals(Path, otherPath);


    /// <summary>
    /// 比较两个文件是否相等（支持比较文件路径、大小、内容等）。
    /// </summary>
    /// <param name="otherPath">给定要比较的文件路径。</param>
    /// <param name="comparePath">是否比较文件路径（可选；默认不启用，如果启用将使用 <see cref="PathEquals(string)"/> 比较）。</param>
    /// <param name="compareSize">是否比较文件大小（可选；默认不启用，也不推荐启用，除非你能确定文件大小相等就表示是同一个文件）。</param>
    /// <param name="bufferSize">给定的缓冲区大小（可选；默认为 512）。</param>
    /// <returns>返回是否相等的布尔值。</returns>
    public bool Equals(string otherPath, bool comparePath = false, bool compareSize = false, int bufferSize = 512)
    {
        // 文件路径相等直接返回相等
        if (comparePath && PathEquals(otherPath)) return true;

        using var handle1 = File.OpenHandle(Path);
        using var handle2 = File.OpenHandle(otherPath);

        var length1 = RandomAccess.GetLength(handle1);
        var length2 = RandomAccess.GetLength(handle2);

        // 如果启用比较文件大小，则大小相等表示文件相等
        if (compareSize && length1 == length2) return true;

        var buffer1 = ArrayPool<byte>.Shared.Rent(bufferSize);
        var buffer2 = ArrayPool<byte>.Shared.Rent(bufferSize);

        var offset1 = 0L;
        var offset2 = 0L;

        while (true)
        {
            var curLength1 = RandomAccess.Read(handle1, buffer1, offset1);
            var curLength2 = RandomAccess.Read(handle2, buffer2, offset2);

            // 如果同顺序读取指定长度内容不相同，则直接返回不相等
            if (!buffer1.SequenceEqualByReadOnlySpan(buffer2))
            {
                ArrayPool<byte>.Shared.Return(buffer1, clearArray: true);
                ArrayPool<byte>.Shared.Return(buffer2, clearArray: true);

                return false;
            }

            if (curLength1 == 0 || curLength2 == 0) break;

            offset1 += curLength1;
            offset2 += curLength2;
        }

        ArrayPool<byte>.Shared.Return(buffer1, clearArray: true);
        ArrayPool<byte>.Shared.Return(buffer2, clearArray: true);

        return true;
    }

    /// <summary>
    /// 比较两个文件是否相等（支持比较文件路径、大小、内容等）。
    /// </summary>
    /// <param name="otherPath">给定要比较的文件路径。</param>
    /// <param name="comparePath">是否比较文件路径（可选；默认不启用，如果启用将使用 <see cref="PathEquals(string)"/> 比较）。</param>
    /// <param name="compareSize">是否比较文件大小（可选；默认不启用，也不推荐启用，除非你能确定文件大小相等就表示是同一个文件）。</param>
    /// <param name="bufferSize">给定的缓冲区大小（可选；默认为 512）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回是否相等的布尔值。</returns>
    public async ValueTask<bool> EqualsAsync(string otherPath, bool comparePath = false, bool compareSize = false,
        int bufferSize = 512, CancellationToken cancellationToken = default)
    {
        // 文件路径相等直接返回相等
        if (comparePath && PathEquals(otherPath)) return true;

        using var handle1 = File.OpenHandle(Path);
        using var handle2 = File.OpenHandle(otherPath);

        var length1 = RandomAccess.GetLength(handle1);
        var length2 = RandomAccess.GetLength(handle2);

        // 如果启用比较文件大小，则大小相等表示文件相等
        if (compareSize && length1 == length2) return true;

        var buffer1 = ArrayPool<byte>.Shared.Rent(bufferSize);
        var buffer2 = ArrayPool<byte>.Shared.Rent(bufferSize);

        var offset1 = 0L;
        var offset2 = 0L;

        while (true)
        {
            var curLength1 = await RandomAccess.ReadAsync(handle1, buffer1, offset1, cancellationToken).ConfigureAwait(false);
            var curLength2 = await RandomAccess.ReadAsync(handle2, buffer2, offset2, cancellationToken).ConfigureAwait(false);

            // 如果同顺序读取指定长度内容不相同，则直接返回不相等
            if (!buffer1.SequenceEqualByReadOnlySpan(buffer2))
            {
                ArrayPool<byte>.Shared.Return(buffer1, clearArray: true);
                ArrayPool<byte>.Shared.Return(buffer2, clearArray: true);

                return false;
            }

            if (curLength1 == 0 || curLength2 == 0) break;

            offset1 += curLength1;
            offset2 += curLength2;
        }

        ArrayPool<byte>.Shared.Return(buffer1, clearArray: true);
        ArrayPool<byte>.Shared.Return(buffer2, clearArray: true);

        return true;
    }

    #endregion

}
