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
/// 定义 <see cref="FluentFilePath"/> 的读写静态扩展。
/// </summary>
public static class FluentFilePathReadWriteExtensions
{

    #region Read

    /// <summary>
    /// 读取文件流。
    /// </summary>
    /// <param name="filePath">给定的 <see cref="FluentFilePath"/>。</param>
    /// <returns>返回 <see cref="FileStream"/>。</returns>
    public static FileStream ReadStream(this FluentFilePath filePath)
        => File.OpenRead(filePath.CurrentValue);


    /// <summary>
    /// 读取当前文件路径的字节数组。
    /// </summary>
    /// <param name="filePath">给定的 <see cref="FluentFilePath"/>。</param>
    /// <returns>返回字节数组。</returns>
    public static byte[] ReadAllBytes(this FluentFilePath filePath)
        => File.ReadAllBytes(filePath.CurrentValue);

    /// <summary>
    /// 异步读取当前文件路径的字节数组。
    /// </summary>
    /// <param name="filePath">给定的 <see cref="FluentFilePath"/>。</param>
    /// <param name="bytes">给定的字节数组。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含字节数组的异步操作。</returns>
    public static Task<byte[]> ReadAllBytesAsync(this FluentFilePath filePath, byte[] bytes,
        CancellationToken cancellationToken = default)
        => File.ReadAllBytesAsync(filePath.CurrentValue, cancellationToken);


    /// <summary>
    /// 读取当前文件路径的行内容集合。
    /// </summary>
    /// <param name="filePath">给定的 <see cref="FluentFilePath"/>。</param>
    /// <param name="encoding">给定的 <see cref="System.Text.Encoding"/>（可选；默认使用 <see cref="Encoding"/>）。</param>
    /// <returns>返回行内容字符串数组。</returns>
    public static string[] ReadAllLines(this FluentFilePath filePath, Encoding? encoding = null)
        => File.ReadAllLines(filePath.CurrentValue, encoding ?? filePath.Encoding);

    /// <summary>
    /// 异步读取当前文件路径的行内容集合。
    /// </summary>
    /// <param name="filePath">给定的 <see cref="FluentFilePath"/>。</param>
    /// <param name="encoding">给定的 <see cref="System.Text.Encoding"/>（可选；默认使用 <see cref="Encoding"/>）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含行内容字符串数组的异步操作。</returns>
    public static Task<string[]> ReadAllLinesAsync(this FluentFilePath filePath, Encoding? encoding = null,
        CancellationToken cancellationToken = default)
        => File.ReadAllLinesAsync(filePath.CurrentValue, encoding ?? filePath.Encoding, cancellationToken);


    /// <summary>
    /// 读取当前文件路径的所有文本。
    /// </summary>
    /// <param name="filePath">给定的 <see cref="FluentFilePath"/>。</param>
    /// <param name="encoding">给定的 <see cref="System.Text.Encoding"/>（可选；默认使用 <see cref="Encoding"/>）。</param>
    /// <returns>返回字符串。</returns>
    public static string ReadAllText(this FluentFilePath filePath, Encoding? encoding = null)
        => File.ReadAllText(filePath.CurrentValue, encoding ?? filePath.Encoding);

    /// <summary>
    /// 异步读取当前文件路径的所有文本。
    /// </summary>
    /// <param name="filePath">给定的 <see cref="FluentFilePath"/>。</param>
    /// <param name="encoding">给定的 <see cref="System.Text.Encoding"/>（可选；默认使用 <see cref="Encoding"/>）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含字符串的异步操作。</returns>
    public static Task<string> ReadAllTextAsync(this FluentFilePath filePath, Encoding? encoding = null,
        CancellationToken cancellationToken = default)
        => File.ReadAllTextAsync(filePath.CurrentValue, encoding ?? filePath.Encoding, cancellationToken);

    #endregion


    #region Write

    /// <summary>
    /// 写入文件流。
    /// </summary>
    /// <param name="filePath">给定的 <see cref="FluentFilePath"/>。</param>
    /// <returns>返回 <see cref="FileStream"/>。</returns>
    public static FileStream WriteStream(this FluentFilePath filePath)
    {
        filePath.CreateDirectory();

        return File.Create(filePath.CurrentValue);
    }


    /// <summary>
    /// 将字节数组写入当前文件路径。
    /// </summary>
    /// <param name="filePath">给定的 <see cref="FluentFilePath"/>。</param>
    /// <param name="bytes">给定的字节数组。</param>
    public static void WriteAllBytes(this FluentFilePath filePath, byte[] bytes)
    {
        filePath.CreateDirectory();

        File.WriteAllBytes(filePath.CurrentValue, bytes);
    }

    /// <summary>
    /// 异步将字节数组写入当前文件路径。
    /// </summary>
    /// <param name="filePath">给定的 <see cref="FluentFilePath"/>。</param>
    /// <param name="bytes">给定的字节数组。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个异步操作。</returns>
    public static Task WriteAllBytesAsync(this FluentFilePath filePath, byte[] bytes,
        CancellationToken cancellationToken = default)
    {
        filePath.CreateDirectory();

        return File.WriteAllBytesAsync(filePath.CurrentValue, bytes, cancellationToken);
    }


    /// <summary>
    /// 将行内容集合写入当前文件路径。
    /// </summary>
    /// <param name="filePath">给定的 <see cref="FluentFilePath"/>。</param>
    /// <param name="contents">给定的行内容集合。</param>
    /// <param name="encoding">给定的 <see cref="System.Text.Encoding"/>（可选；默认使用 <see cref="Encoding"/>）。</param>
    public static void WriteAllLines(this FluentFilePath filePath, IEnumerable<string> contents,
        Encoding? encoding = null)
    {
        filePath.CreateDirectory();

        File.WriteAllLines(filePath.CurrentValue, contents, encoding ?? filePath.Encoding);
    }

    /// <summary>
    /// 异步将行内容集合写入当前文件路径。
    /// </summary>
    /// <param name="filePath">给定的 <see cref="FluentFilePath"/>。</param>
    /// <param name="contents">给定的行内容集合。</param>
    /// <param name="encoding">给定的 <see cref="System.Text.Encoding"/>（可选；默认使用 <see cref="Encoding"/>）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个异步操作。</returns>
    public static Task WriteAllLinesAsync(this FluentFilePath filePath, IEnumerable<string> contents,
        Encoding? encoding = null, CancellationToken cancellationToken = default)
    {
        filePath.CreateDirectory();

        return File.WriteAllLinesAsync(filePath.CurrentValue, contents, encoding ?? filePath.Encoding, cancellationToken);
    }


    /// <summary>
    /// 将所有文本写入当前文件路径。
    /// </summary>
    /// <param name="filePath">给定的 <see cref="FluentFilePath"/>。</param>
    /// <param name="contents">给定的所有文本。</param>
    /// <param name="encoding">给定的 <see cref="System.Text.Encoding"/>（可选；默认使用 <see cref="Encoding"/>）。</param>
    public static void WriteAllText(this FluentFilePath filePath, string? contents, Encoding? encoding = null)
    {
        filePath.CreateDirectory();

        File.WriteAllText(filePath.CurrentValue, contents, encoding ?? filePath.Encoding);
    }

    /// <summary>
    /// 异步将所有文本写入当前文件路径。
    /// </summary>
    /// <param name="filePath">给定的 <see cref="FluentFilePath"/>。</param>
    /// <param name="contents">给定的所有文本。</param>
    /// <param name="encoding">给定的 <see cref="System.Text.Encoding"/>（可选；默认使用 <see cref="Encoding"/>）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个异步操作。</returns>
    public static Task WriteAllTextAsync(this FluentFilePath filePath, string? contents, Encoding? encoding = null,
        CancellationToken cancellationToken = default)
    {
        filePath.CreateDirectory();

        return File.WriteAllTextAsync(filePath.CurrentValue, contents, encoding ?? filePath.Encoding, cancellationToken);
    }

    #endregion

}
