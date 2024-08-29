#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Dependency;
using Librame.Extensions.Infrastructure;

namespace Librame.Extensions.Cryptography;

/// <summary>
/// 定义算法静态扩展。
/// </summary>
public static class AlgorithmExtensions
{
    /// <summary>
    /// 当前算法依赖。
    /// </summary>
    public static Lazy<IAlgorithmDependency> AlgorithmDependency
        => new(() => DependencyRegistration.InitializeDependency<Internal.AlgorithmDependencyInitializer, IAlgorithmDependency>());


    #region Encoding

    /// <summary>
    /// 转为字符编码字符串。
    /// </summary>
    /// <param name="bytes">给定的字节数组。</param>
    /// <param name="encoding">给定的字符编码（可选；默认为 <see cref="IDependencyContext.Encoding"/>）。</param>
    /// <returns>返回字符编码字符串。</returns>
    public static string AsEncodingString(this byte[] bytes, Encoding? encoding = null)
        => (encoding ?? DependencyRegistration.CurrentContext.Encoding).GetString(bytes);

    /// <summary>
    /// 还原字符编码字符串。
    /// </summary>
    /// <param name="plaintext">给定的明文。</param>
    /// <param name="encoding">给定的字符编码（可选；默认为 <see cref="IDependencyContext.Encoding"/>）。</param>
    /// <returns>返回字节数组。</returns>
    public static byte[] FromEncodingString(this string plaintext, Encoding? encoding = null)
        => (encoding ?? DependencyRegistration.CurrentContext.Encoding).GetBytes(plaintext);

    #endregion


    #region Base16String

    /// <summary>
    /// 转换 16 进制字符串。
    /// </summary>
    /// <param name="bytes">给定的字节数组。</param>
    /// <returns>返回字符串。</returns>
    public static string AsHexString(this byte[] bytes)
        => BitConverter.ToString(bytes).Replace("-", string.Empty);

    /// <summary>
    /// 还原 16 进制字符串。
    /// </summary>
    /// <exception cref="ArgumentException">
    /// Invalid hex string.
    /// </exception>
    /// <param name="hexString">给定的 16 进制字符串。</param>
    /// <returns>返回字节数组。</returns>
    public static byte[] FromHexString(this string hexString)
    {
        if (!hexString.Length.IsMultiples(2))
            throw new ArgumentException($"Invalid hex string '{hexString}'.");

        var length = hexString.Length / 2;
        var buffer = new byte[length];

        for (var i = 0; i < length; i++)
            buffer[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);

        return buffer;
    }

    #endregion


    #region Base32String

    private static readonly char[] _base32Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567".ToCharArray();


    /// <summary>
    /// 转为 BASE32 字符串。
    /// </summary>
    /// <param name="bytes">给定的字节数组。</param>
    /// <returns>返回字符串。</returns>
    public static string AsBase32String(this byte[] bytes)
    {
        var sb = new StringBuilder();

        byte index;
        var hi = 5;
        var currentByte = 0;

        while (currentByte < bytes.Length)
        {
            if (hi > 8)
            {
                index = (byte)(bytes[currentByte++] >> hi - 5);

                if (currentByte != bytes.Length)
                {
                    index = (byte)((byte)(bytes[currentByte] << 16 - hi) >> 3 | index);
                }

                hi -= 3;
            }
            else if (hi == 8)
            {
                index = (byte)(bytes[currentByte++] >> 3);
                hi -= 3;
            }
            else
            {
                index = (byte)((byte)(bytes[currentByte] << 8 - hi) >> 3);
                hi += 5;
            }

            sb.Append(_base32Chars[index]);
        }

        return sb.ToString();
    }

    /// <summary>
    /// 还原 BASE32 字符串。
    /// </summary>
    /// <param name="base32String">给定的 BASE32 字符串。</param>
    /// <returns>返回字节数组。</returns>
    public static byte[] FromBase32String(this string base32String)
    {
        var numBytes = base32String.Length * 5 / 8;
        var bytes = new byte[numBytes];

        if (base32String.Length < 3)
        {
            bytes[0] = (byte)(Array.IndexOf(_base32Chars, base32String[0]) | Array.IndexOf(_base32Chars, base32String[1]) << 5);
            return bytes;
        }

        int bitBuffer;
        int currentCharIndex;
        int bitsInBuffer;

        bitBuffer = Array.IndexOf(_base32Chars, base32String[0]) | Array.IndexOf(_base32Chars, base32String[1]) << 5;
        bitsInBuffer = 10;
        currentCharIndex = 2;

        for (var i = 0; i < bytes.Length; i++)
        {
            bytes[i] = (byte)bitBuffer;
            bitBuffer >>= 8;
            bitsInBuffer -= 8;

            while (bitsInBuffer < 8 && currentCharIndex < base32String.Length)
            {
                bitBuffer |= Array.IndexOf(_base32Chars, base32String[currentCharIndex++]) << bitsInBuffer;
                bitsInBuffer += 5;
            }
        }

        return bytes;
    }

    #endregion


    #region Base64String

    private static readonly string _imageDataKey = "data:";
    private static readonly string _imageBase64Separator = ";base64,";

    private static readonly FrozenDictionary<char, char> _base64SortChars = new Dictionary<char, char>()
    {
        {'A','$'},{'B','-'},{'C','0'},{'D','1'},{'E','2'},{'F','3'},{'G','4'},{'H','5'},{'I','6'},{'J','7'},{'K','8'},
        {'L','9'},{'M','A'},{'N','B'},{'O','C'},{'P','D'},{'Q','E'},{'R','F'},{'S','G'},{'T','H'},{'U','I'},{'V','J'},
        {'W','K'},{'X','L'},{'Y','M'},{'Z','N'},{'a','O'},{'b','P'},{'c','Q'},{'d','R'},{'e','S'},{'f','T'},{'g','U'},
        {'h','V'},{'i','W'},{'j','X'},{'k','Y'},{'l','Z'},{'m','a'},{'n','b'},{'o','c'},{'p','d'},{'q','e'},{'r','f'},
        {'s','g'},{'t','h'},{'u','i'},{'v','j'},{'w','k'},{'x','l'},{'y','m'},{'z','n'},{'0','o'},{'1','p'},{'2','q'},
        {'3','r'},{'4','s'},{'5','t'},{'6','u'},{'7','v'},{'8','w'},{'9','x'},{'+','y'},{'/','z'},{'=','!'}
    }.ToFrozenDictionary();


    /// <summary>
    /// 转为 BASE64 字符串。
    /// </summary>
    /// <param name="bytes">给定的字节数组。</param>
    /// <returns>返回字符串。</returns>
    public static string AsBase64String(this byte[] bytes)
        => Convert.ToBase64String(bytes);

    /// <summary>
    /// 还原 BASE64 字符串。
    /// </summary>
    /// <param name="base64String">给定的 BASE64 字符串。</param>
    /// <returns>返回字节数组。</returns>
    public static byte[] FromBase64String(this string base64String)
        => Convert.FromBase64String(base64String);


    /// <summary>
    /// 将图像字节数组转换为 BASE64 字符串形式。
    /// </summary>
    /// <param name="bytes">给定的图像字节数组。</param>
    /// <param name="contentType">给定的图像内容类型（如：image/jpeg）。</param>
    /// <returns>返回字符串。</returns>
    public static string AsImageBase64String(this byte[] bytes, string contentType)
        => $"{_imageDataKey}{contentType}{_imageBase64Separator}{bytes.AsBase64String()}";

    /// <summary>
    /// 从 BASE64 字符串还原图像类型与字节数组。
    /// </summary>
    /// <param name="base64String">给定的图像 BASE64 字符串。</param>
    /// <returns>返回包含图像类型与字节数组的元组。</returns>
    /// <exception cref="ArgumentException">
    /// Invalid image base64 format string <paramref name="base64String"/>.
    /// </exception>
    public static (string imgType, byte[] imgBytes) FromImageBase64String(this string base64String)
    {
        if (!base64String.StartsWith(_imageDataKey) || !base64String.TrySplitPair(_imageBase64Separator, out var pair))
        {
            throw new ArgumentException($"Invalid image base64 format string '{base64String}'.");
        }

        return (pair.Key.TrimStart(_imageDataKey), pair.Value.FromBase64String());
    }


    /// <summary>
    /// 将 BASE64 字符串转为可排序字符串。
    /// </summary>
    /// <remarks>
    /// 此格式的优点：
    ///     <para>1、可直接利用 ASCII 码表的次序比较大小进行排序，对数据库索引友好。</para>
    ///     <para>2、URL 和文件名安全。</para>
    /// </remarks>
    /// <param name="base64String">给定的 BASE64 字符串。</param>
    /// <returns>返回可排序字符串。</returns>
    public static string AsSortableBase64String(this string base64String)
    {
        var chars = base64String.ToArray();

        for (var i = 0; i < chars.Length; i++)
        {
            // 置换为排序字符
            chars[i] = _base64SortChars[chars[i]];
        }

        return new string(chars);
    }

    /// <summary>
    /// 将可排序字符串还原为 BASE64 字符串。
    /// </summary>
    /// <remarks>
    /// 此格式的优点：
    ///     <para>1、可直接利用 ASCII 码表的次序比较大小进行排序，对数据库索引友好。</para>
    ///     <para>2、URL 和文件名安全。</para>
    /// </remarks>
    /// <param name="sortableBase64String">给定的可排序字符串。</param>
    /// <returns>返回 BASE64 字符串。</returns>
    public static string FromSortableBase64String(this string sortableBase64String)
    {
        var chars = sortableBase64String.ToArray();

        for (var i = 0; i < chars.Length; i++)
        {
            // 还原为 BASE64 字符
            var base64Index = _base64SortChars.Values.IndexOf(chars[i]);
            chars[i] = _base64SortChars.Keys[base64Index];
        }

        return new string(chars);
    }

    #endregion


    #region Hash

    /// <summary>
    /// 计算 MD5 哈希值，并返回 BASE64 字符串形式。
    /// </summary>
    /// <param name="plaintext">给定的明文。</param>
    /// <param name="encoding">给定的字符编码（可选；默认为 <see cref="IDependencyContext.Encoding"/>）。</param>
    /// <returns>返回经过 BASE64 编码的加密字符串。</returns>
    public static string AsMd5Base64String(this string plaintext, Encoding? encoding = null)
        => plaintext.FromEncodingString(encoding).AsMd5().AsBase64String();

    /// <summary>
    /// 计算 SHA1 哈希值，并返回 BASE64 字符串形式。
    /// </summary>
    /// <param name="plaintext">给定的明文。</param>
    /// <param name="encoding">给定的字符编码（可选；默认为 <see cref="IDependencyContext.Encoding"/>）。</param>
    /// <returns>返回经过 BASE64 编码的加密字符串。</returns>
    public static string AsSha1Base64String(this string plaintext, Encoding? encoding = null)
        => plaintext.FromEncodingString(encoding).AsSha1().AsBase64String();

    /// <summary>
    /// 计算 SHA256 哈希值，并返回 BASE64 字符串形式。
    /// </summary>
    /// <param name="plaintext">给定的明文。</param>
    /// <param name="encoding">给定的字符编码（可选；默认为 <see cref="IDependencyContext.Encoding"/>）。</param>
    /// <returns>返回经过 BASE64 编码的加密字符串。</returns>
    public static string AsSha256Base64String(this string plaintext, Encoding? encoding = null)
        => plaintext.FromEncodingString(encoding).AsSha256().AsBase64String();

    /// <summary>
    /// 计算 SHA384 哈希值，并返回 BASE64 字符串形式。
    /// </summary>
    /// <param name="plaintext">给定的明文。</param>
    /// <param name="encoding">给定的字符编码（可选；默认为 <see cref="IDependencyContext.Encoding"/>）。</param>
    /// <returns>返回经过 BASE64 编码的加密字符串。</returns>
    public static string AsSha384Base64String(this string plaintext, Encoding? encoding = null)
        => plaintext.FromEncodingString(encoding).AsSha384().AsBase64String();

    /// <summary>
    /// 计算 SHA512 哈希值，并返回 BASE64 字符串形式。
    /// </summary>
    /// <param name="plaintext">给定的明文。</param>
    /// <param name="encoding">给定的字符编码（可选；默认为 <see cref="IDependencyContext.Encoding"/>）。</param>
    /// <returns>返回经过 BASE64 编码的加密字符串。</returns>
    public static string AsSha512Base64String(this string plaintext, Encoding? encoding = null)
        => plaintext.FromEncodingString(encoding).AsSha512().AsBase64String();


    /// <summary>
    /// 计算 MD5 哈希值。
    /// </summary>
    /// <param name="buffer">给定要计算的字节数组。</param>
    /// <returns>返回哈希字节数组。</returns>
    public static byte[] AsMd5(this byte[] buffer)
    {
        using var md5 = AlgorithmDependency.Value.LazyMd5.Value;
        return md5.ComputeHash(buffer);
    }

    /// <summary>
    /// 计算 MD5 哈希值。
    /// </summary>
    /// <param name="stream">给定要计算的 <see cref="Stream"/>。</param>
    /// <returns>返回哈希字节数组。</returns>
    public static byte[] AsMd5(this Stream stream)
    {
        using var md5 = AlgorithmDependency.Value.LazyMd5.Value;
        return md5.ComputeHash(stream);
    }

    /// <summary>
    /// 异步计算 MD5 哈希值。
    /// </summary>
    /// <param name="stream">给定要计算的 <see cref="Stream"/>。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含哈希字节数组的异步操作。</returns>
    public static async Task<byte[]> AsMd5Async(Stream stream, CancellationToken cancellationToken = default)
    {
        using var md5 = AlgorithmDependency.Value.LazyMd5.Value;
        return await md5.ComputeHashAsync(stream, cancellationToken).ConfigureAwait(false);
    }


    /// <summary>
    /// 计算 SHA1 哈希值。
    /// </summary>
    /// <param name="buffer">给定要计算的字节数组。</param>
    /// <returns>返回哈希字节数组。</returns>
    public static byte[] AsSha1(this byte[] buffer)
    {
        using var sha1 = AlgorithmDependency.Value.LazySha1.Value;
        return sha1.ComputeHash(buffer);
    }

    /// <summary>
    /// 计算 SHA1 哈希值。
    /// </summary>
    /// <param name="stream">给定要计算的 <see cref="Stream"/>。</param>
    /// <returns>返回哈希字节数组。</returns>
    public static byte[] AsSha1(this Stream stream)
    {
        using var sha1 = AlgorithmDependency.Value.LazySha1.Value;
        return sha1.ComputeHash(stream);
    }

    /// <summary>
    /// 异步计算 SHA1 哈希值。
    /// </summary>
    /// <param name="stream">给定要计算的 <see cref="Stream"/>。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含哈希字节数组的异步操作。</returns>
    public static async Task<byte[]> AsSha1Async(Stream stream, CancellationToken cancellationToken = default)
    {
        using var sha1 = AlgorithmDependency.Value.LazySha1.Value;
        return await sha1.ComputeHashAsync(stream, cancellationToken).ConfigureAwait(false);
    }


    /// <summary>
    /// 计算 SHA256 哈希值。
    /// </summary>
    /// <param name="buffer">给定要计算的字节数组。</param>
    /// <returns>返回哈希字节数组。</returns>
    public static byte[] AsSha256(this byte[] buffer)
    {
        using var sha256 = AlgorithmDependency.Value.LazySha256.Value;
        return sha256.ComputeHash(buffer);
    }

    /// <summary>
    /// 计算 SHA256 哈希值。
    /// </summary>
    /// <param name="stream">给定要计算的 <see cref="Stream"/>。</param>
    /// <returns>返回哈希字节数组。</returns>
    public static byte[] AsSha256(this Stream stream)
    {
        using var sha256 = AlgorithmDependency.Value.LazySha256.Value;
        return sha256.ComputeHash(stream);
    }

    /// <summary>
    /// 异步计算 SHA256 哈希值。
    /// </summary>
    /// <param name="stream">给定要计算的 <see cref="Stream"/>。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含哈希字节数组的异步操作。</returns>
    public static async Task<byte[]> AsSha256Async(Stream stream, CancellationToken cancellationToken = default)
    {
        using var sha256 = AlgorithmDependency.Value.LazySha256.Value;
        return await sha256.ComputeHashAsync(stream, cancellationToken).ConfigureAwait(false);
    }


    /// <summary>
    /// 计算 SHA384 哈希值。
    /// </summary>
    /// <param name="buffer">给定要计算的字节数组。</param>
    /// <returns>返回哈希字节数组。</returns>
    public static byte[] AsSha384(this byte[] buffer)
    {
        using var sha384 = AlgorithmDependency.Value.LazySha384.Value;
        return sha384.ComputeHash(buffer);
    }

    /// <summary>
    /// 计算 SHA384 哈希值。
    /// </summary>
    /// <param name="stream">给定要计算的 <see cref="Stream"/>。</param>
    /// <returns>返回哈希字节数组。</returns>
    public static byte[] AsSha384(this Stream stream)
    {
        using var sha384 = AlgorithmDependency.Value.LazySha384.Value;
        return sha384.ComputeHash(stream);
    }

    /// <summary>
    /// 异步计算 SHA384 哈希值。
    /// </summary>
    /// <param name="stream">给定要计算的 <see cref="Stream"/>。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含哈希字节数组的异步操作。</returns>
    public static async Task<byte[]> AsSha384Async(Stream stream, CancellationToken cancellationToken = default)
    {
        using var sha384 = AlgorithmDependency.Value.LazySha384.Value;
        return await sha384.ComputeHashAsync(stream, cancellationToken).ConfigureAwait(false);
    }


    /// <summary>
    /// 计算 SHA512 哈希值。
    /// </summary>
    /// <param name="buffer">给定要计算的字节数组。</param>
    /// <returns>返回哈希字节数组。</returns>
    public static byte[] AsSha512(this byte[] buffer)
    {
        using var sha512 = AlgorithmDependency.Value.LazySha512.Value;
        return sha512.ComputeHash(buffer);
    }

    /// <summary>
    /// 计算 SHA512 哈希值。
    /// </summary>
    /// <param name="stream">给定要计算的 <see cref="Stream"/>。</param>
    /// <returns>返回哈希字节数组。</returns>
    public static byte[] AsSha512(this Stream stream)
    {
        using var sha512 = AlgorithmDependency.Value.LazySha512.Value;
        return sha512.ComputeHash(stream);
    }

    /// <summary>
    /// 异步计算 SHA512 哈希值。
    /// </summary>
    /// <param name="stream">给定要计算的 <see cref="Stream"/>。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含哈希字节数组的异步操作。</returns>
    public static async Task<byte[]> AsSha512Async(Stream stream, CancellationToken cancellationToken = default)
    {
        using var sha512 = AlgorithmDependency.Value.LazySha512.Value;
        return await sha512.ComputeHashAsync(stream, cancellationToken).ConfigureAwait(false);
    }

    #endregion


    #region HMAC Hash

    /// <summary>
    /// 计算 HMACMD5 哈希值，并返回 BASE64 字符串形式。
    /// </summary>
    /// <param name="plaintext">给定的明文。</param>
    /// <param name="encoding">给定的字符编码（可选；默认为 <see cref="IDependencyContext.Encoding"/>）。</param>
    /// <returns>返回经过 BASE64 编码的加密字符串。</returns>
    public static string AsHmacMd5Base64String(this string plaintext, Encoding? encoding = null)
        => plaintext.FromEncodingString(encoding).AsHmacMd5().AsBase64String();

    /// <summary>
    /// 计算 HMACSHA1 哈希值，并返回 BASE64 字符串形式。
    /// </summary>
    /// <param name="plaintext">给定的明文。</param>
    /// <param name="encoding">给定的字符编码（可选；默认为 <see cref="IDependencyContext.Encoding"/>）。</param>
    /// <returns>返回经过 BASE64 编码的加密字符串。</returns>
    public static string AsHmacSha1Base64String(this string plaintext, Encoding? encoding = null)
        => plaintext.FromEncodingString(encoding).AsHmacSha1().AsBase64String();

    /// <summary>
    /// 计算 HMACSHA256 哈希值，并返回 BASE64 字符串形式。
    /// </summary>
    /// <param name="plaintext">给定的明文。</param>
    /// <param name="encoding">给定的字符编码（可选；默认为 <see cref="IDependencyContext.Encoding"/>）。</param>
    /// <returns>返回经过 BASE64 编码的加密字符串。</returns>
    public static string AsHmacSha256Base64String(this string plaintext, Encoding? encoding = null)
        => plaintext.FromEncodingString(encoding).AsHmacSha256().AsBase64String();

    /// <summary>
    /// 计算 HMACSHA384 哈希值，并返回 BASE64 字符串形式。
    /// </summary>
    /// <param name="plaintext">给定的明文。</param>
    /// <param name="encoding">给定的字符编码（可选；默认为 <see cref="IDependencyContext.Encoding"/>）。</param>
    /// <returns>返回经过 BASE64 编码的加密字符串。</returns>
    public static string AsHmacSha384Base64String(this string plaintext, Encoding? encoding = null)
        => plaintext.FromEncodingString(encoding).AsHmacSha384().AsBase64String();

    /// <summary>
    /// 计算 HMACSHA512 哈希值，并返回 BASE64 字符串形式。
    /// </summary>
    /// <param name="plaintext">给定的明文。</param>
    /// <param name="encoding">给定的字符编码（可选；默认为 <see cref="IDependencyContext.Encoding"/>）。</param>
    /// <returns>返回经过 BASE64 编码的加密字符串。</returns>
    public static string AsHmacSha512Base64String(this string plaintext, Encoding? encoding = null)
        => plaintext.FromEncodingString(encoding).AsHmacSha512().AsBase64String();


    /// <summary>
    /// 计算 HMACMD5 哈希值。
    /// </summary>
    /// <param name="buffer">给定要计算的字节数组。</param>
    /// <returns>返回哈希字节数组。</returns>
    public static byte[] AsHmacMd5(this byte[] buffer)
    {
        using var hmacMd5 = AlgorithmDependency.Value.LazyHmacMd5.Value;
        return hmacMd5.ComputeHash(buffer);
    }

    /// <summary>
    /// 计算 HMACMD5 哈希值。
    /// </summary>
    /// <param name="stream">给定要计算的 <see cref="Stream"/>。</param>
    /// <returns>返回哈希字节数组。</returns>
    public static byte[] AsHmacMd5(this Stream stream)
    {
        using var hmacMd5 = AlgorithmDependency.Value.LazyHmacMd5.Value;
        return hmacMd5.ComputeHash(stream);
    }

    /// <summary>
    /// 异步计算 HMACMD5 哈希值。
    /// </summary>
    /// <param name="stream">给定要计算的 <see cref="Stream"/>。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含哈希字节数组的异步操作。</returns>
    public static async Task<byte[]> AsHmacMd5Async(Stream stream, CancellationToken cancellationToken = default)
    {
        using var hmacMd5 = AlgorithmDependency.Value.LazyHmacMd5.Value;
        return await hmacMd5.ComputeHashAsync(stream, cancellationToken).ConfigureAwait(false);
    }


    /// <summary>
    /// 计算 HMACSHA1 哈希值。
    /// </summary>
    /// <param name="buffer">给定要计算的字节数组。</param>
    /// <returns>返回哈希字节数组。</returns>
    public static byte[] AsHmacSha1(this byte[] buffer)
    {
        using var hmacSha1 = AlgorithmDependency.Value.LazyHmacSha1.Value;
        return hmacSha1.ComputeHash(buffer);
    }

    /// <summary>
    /// 计算 HMACSHA1 哈希值。
    /// </summary>
    /// <param name="stream">给定要计算的 <see cref="Stream"/>。</param>
    /// <returns>返回哈希字节数组。</returns>
    public static byte[] AsHmacSha1(this Stream stream)
    {
        using var hmacSha1 = AlgorithmDependency.Value.LazyHmacSha1.Value;
        return hmacSha1.ComputeHash(stream);
    }

    /// <summary>
    /// 异步计算 HMACSHA1 哈希值。
    /// </summary>
    /// <param name="stream">给定要计算的 <see cref="Stream"/>。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含哈希字节数组的异步操作。</returns>
    public static async Task<byte[]> AsHmacSha1Async(Stream stream, CancellationToken cancellationToken = default)
    {
        using var hmacSha1 = AlgorithmDependency.Value.LazyHmacSha1.Value;
        return await hmacSha1.ComputeHashAsync(stream, cancellationToken).ConfigureAwait(false);
    }


    /// <summary>
    /// 计算 HMACSHA256 哈希值。
    /// </summary>
    /// <param name="buffer">给定要计算的字节数组。</param>
    /// <returns>返回哈希字节数组。</returns>
    public static byte[] AsHmacSha256(this byte[] buffer)
    {
        using var hmacSha256 = AlgorithmDependency.Value.LazyHmacSha256.Value;
        return hmacSha256.ComputeHash(buffer);
    }

    /// <summary>
    /// 计算 HMACSHA256 哈希值。
    /// </summary>
    /// <param name="stream">给定要计算的 <see cref="Stream"/>。</param>
    /// <returns>返回哈希字节数组。</returns>
    public static byte[] AsHmacSha256(this Stream stream)
    {
        using var hmacSha256 = AlgorithmDependency.Value.LazyHmacSha256.Value;
        return hmacSha256.ComputeHash(stream);
    }

    /// <summary>
    /// 异步计算 HMACSHA256 哈希值。
    /// </summary>
    /// <param name="stream">给定要计算的 <see cref="Stream"/>。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含哈希字节数组的异步操作。</returns>
    public static async Task<byte[]> AsHmacSha256Async(Stream stream, CancellationToken cancellationToken = default)
    {
        using var hmacSha256 = AlgorithmDependency.Value.LazyHmacSha256.Value;
        return await hmacSha256.ComputeHashAsync(stream, cancellationToken).ConfigureAwait(false);
    }


    /// <summary>
    /// 计算 HMACSHA384 哈希值。
    /// </summary>
    /// <param name="buffer">给定要计算的字节数组。</param>
    /// <returns>返回哈希字节数组。</returns>
    public static byte[] AsHmacSha384(this byte[] buffer)
    {
        using var hmacSha384 = AlgorithmDependency.Value.LazyHmacSha384.Value;
        return hmacSha384.ComputeHash(buffer);
    }

    /// <summary>
    /// 计算 HMACSHA384 哈希值。
    /// </summary>
    /// <param name="stream">给定要计算的 <see cref="Stream"/>。</param>
    /// <returns>返回哈希字节数组。</returns>
    public static byte[] AsHmacSha384(this Stream stream)
    {
        using var hmacSha384 = AlgorithmDependency.Value.LazyHmacSha384.Value;
        return hmacSha384.ComputeHash(stream);
    }

    /// <summary>
    /// 异步计算 HMACSHA384 哈希值。
    /// </summary>
    /// <param name="stream">给定要计算的 <see cref="Stream"/>。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含哈希字节数组的异步操作。</returns>
    public static async Task<byte[]> AsHmacSha384Async(Stream stream, CancellationToken cancellationToken = default)
    {
        using var hmacSha384 = AlgorithmDependency.Value.LazyHmacSha384.Value;
        return await hmacSha384.ComputeHashAsync(stream, cancellationToken).ConfigureAwait(false);
    }


    /// <summary>
    /// 计算 HMACSHA512 哈希值。
    /// </summary>
    /// <param name="buffer">给定要计算的字节数组。</param>
    /// <returns>返回哈希字节数组。</returns>
    public static byte[] AsHmacSha512(this byte[] buffer)
    {
        using var hmacSha512 = AlgorithmDependency.Value.LazyHmacSha512.Value;
        return hmacSha512.ComputeHash(buffer);
    }

    /// <summary>
    /// 计算 HMACSHA512 哈希值。
    /// </summary>
    /// <param name="stream">给定要计算的 <see cref="Stream"/>。</param>
    /// <returns>返回哈希字节数组。</returns>
    public static byte[] AsHmacSha512(this Stream stream)
    {
        using var hmacSha512 = AlgorithmDependency.Value.LazyHmacSha512.Value;
        return hmacSha512.ComputeHash(stream);
    }

    /// <summary>
    /// 异步计算 HMACSHA512 哈希值。
    /// </summary>
    /// <param name="stream">给定要计算的 <see cref="Stream"/>。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含哈希字节数组的异步操作。</returns>
    public static async Task<byte[]> AsHmacSha512Async(Stream stream, CancellationToken cancellationToken = default)
    {
        using var hmacSha512 = AlgorithmDependency.Value.LazyHmacSha512.Value;
        return await hmacSha512.ComputeHashAsync(stream, cancellationToken).ConfigureAwait(false);
    }

    #endregion


    #region DES

    /// <summary>
    /// TripleDES 加密 BASE64 字符串形式。
    /// </summary>
    /// <param name="plaintext">给定的明文。</param>
    /// <param name="encoding">给定的字符编码（可选；默认为 <see cref="IDependencyContext.Encoding"/>）。</param>
    /// <returns>返回经过 BASE64 编码的加密字符串。</returns>
    public static string As3DesWithBase64String(this string plaintext, Encoding? encoding = null)
        => plaintext.FromEncodingString(encoding).As3Des().AsBase64String();

    /// <summary>
    /// TripleDES 解密 BASE64 字符串形式。
    /// </summary>
    /// <param name="ciphertext">给定的密文。</param>
    /// <param name="encoding">给定的字符编码（可选；默认为 <see cref="IDependencyContext.Encoding"/>）。</param>
    /// <returns>返回经过 BASE64 解码的解密字符串。</returns>
    public static string From3DesWithBase64String(this string ciphertext, Encoding? encoding = null)
        => ciphertext.FromBase64String().From3Des().AsEncodingString(encoding);


    /// <summary>
    /// TripleDES 加密。
    /// </summary>
    /// <param name="plaintext">给定的明文。</param>
    /// <param name="rgbKey">给定的密钥（可选）。</param>
    /// <param name="rgbIV">给定的初始化向量（可选）。</param>
    /// <returns>返回加密字节数组。</returns>
    public static byte[] As3Des(this byte[] plaintext, byte[]? rgbKey = null, byte[]? rgbIV = null)
    {
        using var encryptor = Create3DesEncryptor(rgbKey, rgbIV);

        return encryptor.TransformFinalBlock(plaintext, 0, plaintext.Length);
    }

    /// <summary>
    /// TripleDES 加密。
    /// </summary>
    /// <param name="plaintext">给定的明文 <see cref="Stream"/>。</param>
    /// <param name="encrypted">给定的加密 <see cref="Stream"/>。</param>
    /// <param name="rgbKey">给定的密钥（可选）。</param>
    /// <param name="rgbIV">给定的初始化向量（可选）。</param>
    public static void As3Des(this Stream plaintext, Stream encrypted, byte[]? rgbKey = null, byte[]? rgbIV = null)
    {
        using var encryptor = Create3DesEncryptor(rgbKey, rgbIV);

        using var crypto = new CryptoStream(encrypted, encryptor, CryptoStreamMode.Write);
        plaintext.CopyTo(crypto);
    }

    private static ICryptoTransform Create3DesEncryptor(byte[]? rgbKey, byte[]? rgbIV)
    {
        using var des = AlgorithmDependency.Value.Lazy3Des.Value;

        return rgbKey is null || rgbIV is null
            ? des.CreateEncryptor()
            : des.CreateEncryptor(rgbKey, rgbIV);
    }

    /// <summary>
    /// TripleDES 加密。
    /// </summary>
    /// <param name="plaintext">给定的明文 <see cref="Stream"/>。</param>
    /// <param name="rgbKey">给定的密钥（可选）。</param>
    /// <param name="rgbIV">给定的初始化向量（可选）。</param>
    /// <returns>返回加密字节数组。</returns>
    public static byte[] As3DesBytes(this Stream plaintext, byte[]? rgbKey = null, byte[]? rgbIV = null)
    {
        using var memory = DependencyRegistration.CurrentContext.MemoryStreams.GetStream();
        plaintext.As3Des(memory, rgbKey, rgbIV);

        return memory.GetReadOnlySequence().ToArray();
    }


    /// <summary>
    /// TripleDES 解密。
    /// </summary>
    /// <param name="ciphertext">给定的密文。</param>
    /// <param name="rgbKey">给定的密钥（可选）。</param>
    /// <param name="rgbIV">给定的初始化向量（可选）。</param>
    /// <returns>返回经过解密的字节数组。</returns>
    public static byte[] From3Des(this byte[] ciphertext, byte[]? rgbKey = null, byte[]? rgbIV = null)
    {
        using var decryptor = Create3DesDecryptor(rgbKey, rgbIV);

        return decryptor.TransformFinalBlock(ciphertext, 0, ciphertext.Length);
    }

    /// <summary>
    /// TripleDES 解密。
    /// </summary>
    /// <param name="encrypted">给定的加密 <see cref="Stream"/>。</param>
    /// <param name="decrypted">给定的解密 <see cref="Stream"/>。</param>
    /// <param name="rgbKey">给定的密钥（可选）。</param>
    /// <param name="rgbIV">给定的初始化向量（可选）。</param>
    public static void From3Des(this Stream encrypted, Stream decrypted, byte[]? rgbKey = null, byte[]? rgbIV = null)
    {
        using var decryptor = Create3DesDecryptor(rgbKey, rgbIV);

        using var crypto = new CryptoStream(encrypted, decryptor, CryptoStreamMode.Read);
        crypto.CopyTo(decrypted);
    }

    private static ICryptoTransform Create3DesDecryptor(byte[]? rgbKey, byte[]? rgbIV)
    {
        using var des = AlgorithmDependency.Value.Lazy3Des.Value;

        return rgbKey is null || rgbIV is null
            ? des.CreateDecryptor()
            : des.CreateDecryptor(rgbKey, rgbIV);
    }

    /// <summary>
    /// TripleDES 解密。
    /// </summary>
    /// <param name="ciphertext">给定的密文。</param>
    /// <param name="decrypted">给定的解密 <see cref="Stream"/>。</param>
    /// <param name="rgbKey">给定的密钥（可选）。</param>
    /// <param name="rgbIV">给定的初始化向量（可选）。</param>
    /// <returns>返回解密字节数组。</returns>
    public static void From3DesBytes(this byte[] ciphertext, Stream decrypted, byte[]? rgbKey = null, byte[]? rgbIV = null)
    {
        using var encrypted = DependencyRegistration.CurrentContext.MemoryStreams.GetStream(ciphertext);
        encrypted.From3Des(decrypted, rgbKey, rgbIV);
    }

    #endregion


    #region AES

    /// <summary>
    /// AES 加密 BASE64 字符串形式。
    /// </summary>
    /// <param name="plaintext">给定的明文。</param>
    /// <param name="encoding">给定的字符编码（可选；默认为 <see cref="IDependencyContext.Encoding"/>）。</param>
    /// <returns>返回经过 BASE64 编码的加密字符串。</returns>
    public static string AsAesWithBase64String(this string plaintext, Encoding? encoding = null)
        => plaintext.FromEncodingString(encoding).AsAes().AsBase64String();

    /// <summary>
    /// AES 解密 BASE64 字符串形式。
    /// </summary>
    /// <param name="ciphertext">给定的密文。</param>
    /// <param name="encoding">给定的字符编码（可选；默认为 <see cref="IDependencyContext.Encoding"/>）。</param>
    /// <returns>返回经过 BASE64 解码的解密字符串。</returns>
    public static string FromAesWithBase64String(this string ciphertext, Encoding? encoding = null)
        => ciphertext.FromBase64String().FromAes().AsEncodingString(encoding);


    /// <summary>
    /// AES 加密。
    /// </summary>
    /// <param name="plaintext">给定的明文。</param>
    /// <param name="rgbKey">给定的密钥（可选）。</param>
    /// <param name="rgbIV">给定的初始化向量（可选）。</param>
    /// <returns>返回经过加密的字节数组。</returns>
    public static byte[] AsAes(this byte[] plaintext, byte[]? rgbKey = null, byte[]? rgbIV = null)
    {
        using var encryptor = CreateAesEncryptor(rgbKey, rgbIV);

        return encryptor.TransformFinalBlock(plaintext, 0, plaintext.Length);
    }

    /// <summary>
    /// AES 加密。
    /// </summary>
    /// <param name="plaintext">给定的明文 <see cref="Stream"/>。</param>
    /// <param name="encrypted">给定的加密 <see cref="Stream"/>。</param>
    /// <param name="rgbKey">给定的密钥（可选）。</param>
    /// <param name="rgbIV">给定的初始化向量（可选）。</param>
    public static void AsAes(this Stream plaintext, Stream encrypted, byte[]? rgbKey = null, byte[]? rgbIV = null)
    {
        using var encryptor = CreateAesEncryptor(rgbKey, rgbIV);

        using var crypto = new CryptoStream(encrypted, encryptor, CryptoStreamMode.Write);
        plaintext.CopyTo(crypto);
    }

    private static ICryptoTransform CreateAesEncryptor(byte[]? rgbKey, byte[]? rgbIV)
    {
        using var des = AlgorithmDependency.Value.LazyAes.Value;

        return rgbKey is null || rgbIV is null
            ? des.CreateEncryptor()
            : des.CreateEncryptor(rgbKey, rgbIV);
    }

    /// <summary>
    /// AES 加密。
    /// </summary>
    /// <param name="plaintext">给定的明文 <see cref="Stream"/>。</param>
    /// <param name="rgbKey">给定的密钥（可选）。</param>
    /// <param name="rgbIV">给定的初始化向量（可选）。</param>
    /// <returns>返回加密字节数组。</returns>
    public static byte[] AsAesBytes(this Stream plaintext, byte[]? rgbKey = null, byte[]? rgbIV = null)
    {
        using var memory = DependencyRegistration.CurrentContext.MemoryStreams.GetStream();
        plaintext.AsAes(memory, rgbKey, rgbIV);

        return memory.GetReadOnlySequence().ToArray();
    }


    /// <summary>
    /// AES 解密。
    /// </summary>
    /// <param name="ciphertext">给定的密文。</param>
    /// <param name="rgbKey">给定的密钥（可选）。</param>
    /// <param name="rgbIV">给定的初始化向量（可选）。</param>
    /// <returns>返回经过解密的字节数组。</returns>
    public static byte[] FromAes(this byte[] ciphertext, byte[]? rgbKey = null, byte[]? rgbIV = null)
    {
        using var decryptor = CreateAesDecryptor(rgbKey, rgbIV);

        return decryptor.TransformFinalBlock(ciphertext, 0, ciphertext.Length);
    }

    /// <summary>
    /// AES 解密。
    /// </summary>
    /// <param name="encrypted">给定的加密 <see cref="Stream"/>。</param>
    /// <param name="decrypted">给定的解密 <see cref="Stream"/>。</param>
    /// <param name="rgbKey">给定的密钥（可选）。</param>
    /// <param name="rgbIV">给定的初始化向量（可选）。</param>
    public static void FromAes(this Stream encrypted, Stream decrypted, byte[]? rgbKey = null, byte[]? rgbIV = null)
    {
        using var decryptor = CreateAesDecryptor(rgbKey, rgbIV);

        using var crypto = new CryptoStream(encrypted, decryptor, CryptoStreamMode.Read);
        crypto.CopyTo(decrypted);
    }

    private static ICryptoTransform CreateAesDecryptor(byte[]? rgbKey, byte[]? rgbIV)
    {
        using var des = AlgorithmDependency.Value.LazyAes.Value;

        return rgbKey is null || rgbIV is null
            ? des.CreateDecryptor()
            : des.CreateDecryptor(rgbKey, rgbIV);
    }

    /// <summary>
    /// AES 解密。
    /// </summary>
    /// <param name="ciphertext">给定的密文。</param>
    /// <param name="decrypted">给定的解密 <see cref="Stream"/>。</param>
    /// <param name="rgbKey">给定的密钥（可选）。</param>
    /// <param name="rgbIV">给定的初始化向量（可选）。</param>
    /// <returns>返回解密字节数组。</returns>
    public static void FromAesBytes(this byte[] ciphertext, Stream decrypted, byte[]? rgbKey = null, byte[]? rgbIV = null)
    {
        using var encrypted = DependencyRegistration.CurrentContext.MemoryStreams.GetStream(ciphertext);
        encrypted.FromAes(decrypted, rgbKey, rgbIV);
    }

    #endregion


    #region AES-CCM

    /// <summary>
    /// AES-CCM 加密 BASE64 字符串形式。
    /// </summary>
    /// <param name="plaintext">给定的明文。</param>
    /// <param name="encoding">给定的字符编码（可选；默认为 <see cref="IDependencyContext.Encoding"/>）。</param>
    /// <returns>返回经过 BASE64 编码的加密字符串。</returns>
    public static string AsAesCcmWithBase64String(this string plaintext, Encoding? encoding = null)
        => plaintext.FromEncodingString(encoding).AsAesCcm().AsBase64String();

    /// <summary>
    /// AES-CCM 解密 BASE64 字符串形式。
    /// </summary>
    /// <param name="ciphertext">给定的密文。</param>
    /// <param name="encoding">给定的字符编码（可选；默认为 <see cref="IDependencyContext.Encoding"/>）。</param>
    /// <returns>返回经过 BASE64 解码的解密字符串。</returns>
    public static string FromAesCcmWithBase64String(this string ciphertext, Encoding? encoding = null)
        => ciphertext.FromBase64String().FromAesCcm().AsEncodingString(encoding);


    /// <summary>
    /// AES-CCM 加密。
    /// </summary>
    /// <param name="plaintext">给定的明文。</param>
    /// <returns>返回经过加密的字节数组。</returns>
    public static byte[] AsAesCcm(this byte[] plaintext)
    {
        var ciphertext = new byte[plaintext.Length];
        var aesCcmKeyring = AlgorithmDependency.Value.Keyring.AesCcm;

        using var aesCcm = AlgorithmDependency.Value.LazyAesCcm.Value;
        aesCcm.Encrypt(aesCcmKeyring.Nonce, plaintext, ciphertext, aesCcmKeyring.Tag);

        return ciphertext;
    }

    /// <summary>
    /// AES-CCM 解密。
    /// </summary>
    /// <param name="ciphertext">给定的密文。</param>
    /// <returns>返回经过解密的字节数组。</returns>
    public static byte[] FromAesCcm(this byte[] ciphertext)
    {
        var plaintext = new byte[ciphertext.Length];
        var aesCcmKeyring = AlgorithmDependency.Value.Keyring.AesCcm;

        using var aesCcm = AlgorithmDependency.Value.LazyAesCcm.Value;
        aesCcm.Decrypt(aesCcmKeyring.Nonce, ciphertext, aesCcmKeyring.Tag, plaintext);

        return plaintext;
    }

    #endregion


    #region AES-GCM

    /// <summary>
    /// AES-GCM 加密 BASE64 字符串形式。
    /// </summary>
    /// <param name="plaintext">给定的明文。</param>
    /// <param name="encoding">给定的字符编码（可选；默认为 <see cref="IDependencyContext.Encoding"/>）。</param>
    /// <returns>返回经过 BASE64 编码的加密字符串。</returns>
    public static string AsAesGcmWithBase64String(this string plaintext, Encoding? encoding = null)
        => plaintext.FromEncodingString(encoding).AsAesGcm().AsBase64String();

    /// <summary>
    /// AES-GCM 解密 BASE64 字符串形式。
    /// </summary>
    /// <param name="ciphertext">给定的密文。</param>
    /// <param name="encoding">给定的字符编码（可选；默认为 <see cref="IDependencyContext.Encoding"/>）。</param>
    /// <returns>返回经过 BASE64 解码的解密字符串。</returns>
    public static string FromAesGcmWithBase64String(this string ciphertext, Encoding? encoding = null)
        => ciphertext.FromBase64String().FromAesGcm().AsEncodingString(encoding);


    /// <summary>
    /// AES-GCM 加密。
    /// </summary>
    /// <param name="plaintext">给定的明文。</param>
    /// <returns>返回经过加密的字节数组。</returns>
    public static byte[] AsAesGcm(this byte[] plaintext)
    {
        var ciphertext = new byte[plaintext.Length];
        var aesGcmKeyring = AlgorithmDependency.Value.Keyring.AesGcm;

        using var aesGcm = AlgorithmDependency.Value.LazyAesGcm.Value;
        aesGcm.Encrypt(aesGcmKeyring.Nonce, plaintext, ciphertext, aesGcmKeyring.Tag);

        return ciphertext;
    }

    /// <summary>
    /// AES-GCM 解密。
    /// </summary>
    /// <param name="ciphertext">给定的密文。</param>
    /// <returns>返回经过解密的字节数组。</returns>
    public static byte[] FromAesGcm(this byte[] ciphertext)
    {
        var plaintext = new byte[ciphertext.Length];
        var aesGcmKeyring = AlgorithmDependency.Value.Keyring.AesGcm;

        using var aesGcm = AlgorithmDependency.Value.LazyAesGcm.Value;
        aesGcm.Decrypt(aesGcmKeyring.Nonce, ciphertext, aesGcmKeyring.Tag, plaintext);

        return plaintext;
    }

    #endregion


    #region RSA

    /// <summary>
    /// RSA 公钥加密 BASE64 字符串形式。
    /// </summary>
    /// <param name="plaintext">给定的明文。</param>
    /// <param name="encoding">给定的字符编码（可选；默认为 <see cref="IDependencyContext.Encoding"/>）。</param>
    /// <param name="padding">给定的 <see cref="RSAEncryptionPadding"/>（可选；默认为 <see cref="RSAEncryptionPadding.Pkcs1"/>）。</param>
    /// <returns>返回经过 BASE64 编码的加密字符串。</returns>
    public static string AsPublicRsaWithBase64String(this string plaintext, Encoding? encoding = null, RSAEncryptionPadding? padding = null)
        => plaintext.FromEncodingString(encoding).AsPublicRsa(padding).AsBase64String();

    /// <summary>
    /// RSA 私钥解密 BASE64 字符串形式。
    /// </summary>
    /// <param name="ciphertext">给定的密文。</param>
    /// <param name="encoding">给定的字符编码（可选；默认为 <see cref="IDependencyContext.Encoding"/>）。</param>
    /// <param name="padding">给定的 <see cref="RSAEncryptionPadding"/>（可选；默认为 <see cref="RSAEncryptionPadding.Pkcs1"/>）。</param>
    /// <returns>返回经过 BASE64 解码的解密字符串。</returns>
    public static string FromPrivateRsaWithBase64String(this string ciphertext, Encoding? encoding = null, RSAEncryptionPadding? padding = null)
        => ciphertext.FromBase64String().FromPrivateRsa(padding).AsEncodingString(encoding);


    /// <summary>
    /// 数字证书 RSA 私钥数据签名并转为 BASE64 字符串形式。
    /// </summary>
    /// <param name="data">给定的数据。</param>
    /// <param name="encoding">给定的字符编码（可选；默认为 <see cref="IDependencyContext.Encoding"/>）。</param>
    /// <returns>返回经过 BASE64 编码的加密字符串。</returns>
    public static string SignDataPrivateRsaWithBase64String(this string data, Encoding? encoding = null)
        => data.FromEncodingString(encoding).SignDataPrivateRsa().AsBase64String();

    /// <summary>
    /// 数字证书 RSA 公钥验证数据签名的 BASE64 字符串形式。
    /// </summary>
    /// <param name="data">给定的数据。</param>
    /// <param name="signed">给定的数据签名。</param>
    /// <param name="encoding">给定的字符编码（可选；默认为 <see cref="IDependencyContext.Encoding"/>）。</param>
    /// <returns>返回经过 BASE64 解码的解密字符串。</returns>
    public static bool VerifyDataPublicRsaWithBase64String(this string data, string signed, Encoding? encoding = null)
        => data.FromEncodingString(encoding).VerifyDataPublicRsa(signed.FromBase64String());


    /// <summary>
    /// RSA 公钥加密。
    /// </summary>
    /// <param name="plaintext">给定的明文。</param>
    /// <param name="padding">给定的 <see cref="RSAEncryptionPadding"/>（可选；默认为 <see cref="RSAEncryptionPadding.Pkcs1"/>）。</param>
    /// <returns>返回经过加密的字节数组。</returns>
    public static byte[] AsPublicRsa(this byte[] plaintext, RSAEncryptionPadding? padding = null)
    {
        padding ??= RSAEncryptionPadding.Pkcs1;

        using var rsa = AlgorithmDependency.Value.LazyRsaPair.Value.PublicAlgo;

        // RSA 单次加密受支持 Key 的模长(modulus)-n 长度限制，超出限制需分段加密
        var bufferSize = rsa.KeySize / 8 - 11;

        if (plaintext.Length <= bufferSize)
        {
            return rsa.Encrypt(plaintext, padding);
        }

        var ciphertext = DependencyRegistration.CurrentContext.MemoryStreams.GetStream();

        var encryptLength = 0;
        var surplusLength = plaintext.Length;

        while (encryptLength < plaintext.Length)
        {
            var curLength = bufferSize;
            if (surplusLength < bufferSize)
            {
                curLength = surplusLength;
            }

            var buffer = new byte[curLength];

            // buffer.Length != curLength
            Array.Copy(plaintext, encryptLength, buffer, 0, curLength);

            // encryptBuffer.Length != bufferSize
            var encryptBuffer = rsa.Encrypt(buffer, padding);

            ciphertext.Write(encryptBuffer);

            encryptLength += bufferSize;
            surplusLength -= bufferSize;
        }

        return ciphertext.GetReadOnlySequence().ToArray();
    }

    /// <summary>
    /// RSA 公钥加密。
    /// </summary>
    /// <param name="plaintext">给定的明文 <see cref="Stream"/>。</param>
    /// <param name="encrypted">给定的加密 <see cref="Stream"/>。</param>
    /// <param name="padding">给定的 <see cref="RSAEncryptionPadding"/>（可选；默认为 <see cref="RSAEncryptionPadding.Pkcs1"/>）。</param>
    /// <param name="createPublicRsaFunc">给定创建公钥 RSA 的方法（可选；默认使用 <see cref="AlgorithmDependency"/>）。</param>
    public static void AsPublicRsa(this Stream plaintext, Stream encrypted,
        RSAEncryptionPadding? padding = null, Func<RSA>? createPublicRsaFunc = null)
    {
        padding ??= RSAEncryptionPadding.Pkcs1;
        createPublicRsaFunc ??= () => AlgorithmDependency.Value.LazyRsaPair.Value.PublicAlgo;

        using var rsa = createPublicRsaFunc();

        // RSA 单次加密受支持 Key 的模长(modulus)-n 长度限制，超出限制需分段加密
        var maxBlockSize = rsa.KeySize / 8 - 11;

        var maxBuffer = new byte[maxBlockSize];
        var readBlockSize = plaintext.Read(maxBuffer, 0, maxBlockSize);

        while (readBlockSize > 0)
        {
            var readBuffer = new byte[readBlockSize];
            Array.Copy(maxBuffer, 0, readBuffer, 0, readBlockSize);

            var encryptedBuffer = rsa.Encrypt(readBuffer, padding);
            encrypted.Write(encryptedBuffer, 0, encryptedBuffer.Length);

            readBlockSize = plaintext.Read(maxBuffer, 0, maxBlockSize);
        }
    }


    /// <summary>
    /// RSA 私钥解密。
    /// </summary>
    /// <param name="ciphertext">给定的密文。</param>
    /// <param name="padding">给定的 <see cref="RSAEncryptionPadding"/>（可选；默认为 <see cref="RSAEncryptionPadding.Pkcs1"/>）。</param>
    /// <returns>返回经过解密的字节数组。</returns>
    public static byte[] FromPrivateRsa(this byte[] ciphertext, RSAEncryptionPadding? padding = null)
    {
        using var rsa = AlgorithmDependency.Value.LazyRsaPair.Value.PrivateAlgo;

        // 解密时分段长度需使用密钥大小对应的字节长度
        var keySizeInBytes = rsa.KeySize / 8;

        if (ciphertext.Length <= keySizeInBytes)
        {
            return rsa.Decrypt(ciphertext, padding ?? RSAEncryptionPadding.Pkcs1);
        }

        var plaintext = DependencyRegistration.CurrentContext.MemoryStreams.GetStream();

        var decryptLength = 0;
        var surplusLength = ciphertext.Length;

        while (decryptLength < ciphertext.Length)
        {
            var curLength = keySizeInBytes;
            if (surplusLength < keySizeInBytes)
            {
                curLength = surplusLength;
            }

            var buffer = new byte[curLength];

            Array.Copy(ciphertext, decryptLength, buffer, 0, curLength);

            // decryptBuffer.Length != keySizeInBytes
            var decryptBuffer = rsa.Decrypt(buffer, padding ?? RSAEncryptionPadding.Pkcs1);

            plaintext.Write(decryptBuffer);

            decryptLength += keySizeInBytes;
            surplusLength -= keySizeInBytes;
        }

        return plaintext.GetReadOnlySequence().ToArray();
    }

    /// <summary>
    /// RSA 私钥解密。
    /// </summary>
    /// <param name="encrypted">给定的加密 <see cref="Stream"/>。</param>
    /// <param name="decrypted">给定的解密 <see cref="Stream"/>。</param>
    /// <param name="padding">给定的 <see cref="RSAEncryptionPadding"/>（可选；默认为 <see cref="RSAEncryptionPadding.Pkcs1"/>）。</param>
    /// <param name="createPrivateRsaFunc">给定创建私钥 RSA 的方法（可选；默认使用 <see cref="AlgorithmDependency"/>）。</param>
    public static void FromPrivateRsa(this Stream encrypted, Stream decrypted,
        RSAEncryptionPadding? padding = null, Func<RSA>? createPrivateRsaFunc = null)
    {
        padding ??= RSAEncryptionPadding.Pkcs1;
        createPrivateRsaFunc ??= () => AlgorithmDependency.Value.LazyRsaPair.Value.PrivateAlgo;

        using var rsa = createPrivateRsaFunc();

        // 解密时分段长度需使用密钥大小对应的字节长度
        var maxBlockSize = rsa.KeySize / 8;

        var maxBuffer = new byte[maxBlockSize];
        var readBlockSize = encrypted.Read(maxBuffer, 0, maxBlockSize);

        while (readBlockSize > 0)
        {
            var readBuffer = new byte[readBlockSize];
            Array.Copy(maxBuffer, 0, readBuffer, 0, readBlockSize);

            var decryptedBuffer = rsa.Decrypt(readBuffer, padding);
            decrypted.Write(decryptedBuffer, 0, decryptedBuffer.Length);

            readBlockSize = encrypted.Read(maxBuffer, 0, maxBlockSize);
        }
    }


    /// <summary>
    /// 数字证书 RSA 私钥数据签名。
    /// </summary>
    /// <param name="data">给定的字节数组。</param>
    /// <param name="hashName">给定的 <see cref="HashAlgorithmName"/>（可选；默认为 <see cref="HashAlgorithmName.SHA256"/>）。</param>
    /// <param name="padding">给定的 <see cref="RSASignaturePadding"/>（可选；默认为 <see cref="RSASignaturePadding.Pkcs1"/>）</param>
    /// <returns>返回经过加密的字节数组。</returns>
    public static byte[] SignDataPrivateRsa(this byte[] data, HashAlgorithmName? hashName = null,
        RSASignaturePadding? padding = null)
    {
        using var rsa = AlgorithmDependency.Value.LazyRsaPair.Value.PrivateAlgo;
        return rsa.SignData(data, hashName ?? HashAlgorithmName.SHA256, padding ?? RSASignaturePadding.Pkcs1);
    }

    /// <summary>
    /// 数字证书 RSA 私钥数据签名。
    /// </summary>
    /// <param name="data">给定的数据 <see cref="Stream"/>。</param>
    /// <param name="hashName">给定的 <see cref="HashAlgorithmName"/>（可选；默认为 <see cref="HashAlgorithmName.SHA256"/>）。</param>
    /// <param name="padding">给定的 <see cref="RSASignaturePadding"/>（可选；默认为 <see cref="RSASignaturePadding.Pkcs1"/>）</param>
    /// <returns>返回经过加密的字节数组。</returns>
    public static byte[] SignDataPrivateRsa(this Stream data, HashAlgorithmName? hashName = null,
        RSASignaturePadding? padding = null)
    {
        using var rsa = AlgorithmDependency.Value.LazyRsaPair.Value.PrivateAlgo;
        return rsa.SignData(data, hashName ?? HashAlgorithmName.SHA256, padding ?? RSASignaturePadding.Pkcs1);
    }

    /// <summary>
    /// 数字证书 RSA 公钥验证数据签名。
    /// </summary>
    /// <param name="data">给定的字节数组。</param>
    /// <param name="signed">给定数据签名。</param>
    /// <param name="hashName">给定的 <see cref="HashAlgorithmName"/>（可选；默认为 <see cref="HashAlgorithmName.SHA256"/>）。</param>
    /// <param name="padding">给定的 <see cref="RSASignaturePadding"/>（可选；默认为 <see cref="RSASignaturePadding.Pkcs1"/>）</param>
    /// <returns>返回经过解密的字节数组。</returns>
    public static bool VerifyDataPublicRsa(this byte[] data, byte[] signed, HashAlgorithmName? hashName = null,
        RSASignaturePadding? padding = null)
    {
        using var rsa = AlgorithmDependency.Value.LazyRsaPair.Value.PublicAlgo;
        return rsa.VerifyData(data, signed, hashName ?? HashAlgorithmName.SHA256, padding ?? RSASignaturePadding.Pkcs1);
    }

    /// <summary>
    /// 数字证书 RSA 公钥验证数据签名。
    /// </summary>
    /// <param name="data">给定的数据 <see cref="Stream"/>。</param>
    /// <param name="signed">给定数据签名。</param>
    /// <param name="hashName">给定的 <see cref="HashAlgorithmName"/>（可选；默认为 <see cref="HashAlgorithmName.SHA256"/>）。</param>
    /// <param name="padding">给定的 <see cref="RSASignaturePadding"/>（可选；默认为 <see cref="RSASignaturePadding.Pkcs1"/>）</param>
    /// <returns>返回经过解密的字节数组。</returns>
    public static bool VerifyDataPublicRsa(this Stream data, byte[] signed, HashAlgorithmName? hashName = null,
        RSASignaturePadding? padding = null)
    {
        using var rsa = AlgorithmDependency.Value.LazyRsaPair.Value.PublicAlgo;
        return rsa.VerifyData(data, signed, hashName ?? HashAlgorithmName.SHA256, padding ?? RSASignaturePadding.Pkcs1);
    }


    /// <summary>
    /// 数字证书 RSA 私钥签名。
    /// </summary>
    /// <param name="hash">给定的明文。</param>
    /// <param name="hashName">给定的 <see cref="HashAlgorithmName"/>（可选；默认为 <see cref="HashAlgorithmName.SHA256"/>）。</param>
    /// <param name="padding">给定的 <see cref="RSASignaturePadding"/>（可选；默认为 <see cref="RSASignaturePadding.Pkcs1"/>）</param>
    /// <returns>返回经过加密的字节数组。</returns>
    public static byte[] SignHashPrivateRsa(this byte[] hash, HashAlgorithmName? hashName = null, RSASignaturePadding? padding = null)
    {
        using var rsa = AlgorithmDependency.Value.LazyRsaPair.Value.PrivateAlgo;
        return rsa.SignHash(hash, hashName ?? HashAlgorithmName.SHA256, padding ?? RSASignaturePadding.Pkcs1);
    }

    /// <summary>
    /// 数字证书 RSA 公钥验证。
    /// </summary>
    /// <param name="ciphertext">给定待验证的签名密文。</param>
    /// <param name="hash">给定明确的签名数据。</param>
    /// <param name="hashName">给定的 <see cref="HashAlgorithmName"/>（可选；默认为 <see cref="HashAlgorithmName.SHA256"/>）。</param>
    /// <param name="padding">给定的 <see cref="RSASignaturePadding"/>（可选；默认为 <see cref="RSASignaturePadding.Pkcs1"/>）</param>
    /// <returns>返回经过解密的字节数组。</returns>
    public static bool VerifyHashPublicRsa(this byte[] ciphertext, byte[] hash, HashAlgorithmName? hashName = null, RSASignaturePadding? padding = null)
    {
        using var rsa = AlgorithmDependency.Value.LazyRsaPair.Value.PublicAlgo;
        return rsa.VerifyHash(hash, ciphertext, hashName ?? HashAlgorithmName.SHA256, padding ?? RSASignaturePadding.Pkcs1);
    }

    #endregion


    #region ECDSA

    /// <summary>
    /// 数字证书 ECDSA 私钥数据签名并转换为 BASE64 字符串形式。
    /// </summary>
    /// <param name="data">给定的数据。</param>
    /// <param name="encoding">给定的字符编码（可选；默认为 <see cref="IDependencyContext.Encoding"/>）。</param>
    /// <returns>返回经过 BASE64 编码的加密字符串。</returns>
    public static string SignDataPrivateEcdsaWithBase64String(this string data, Encoding? encoding = null)
        => data.FromEncodingString(encoding).SignDataPrivateEcdsa().AsBase64String();

    /// <summary>
    /// 数字证书 ECDSA 公钥验证数据签名的 BASE64 字符串形式。
    /// </summary>
    /// <param name="data">给定的数据。</param>
    /// <param name="signed">给定的数据签名。</param>
    /// <param name="encoding">给定的字符编码（可选；默认为 <see cref="IDependencyContext.Encoding"/>）。</param>
    /// <returns>返回签名是否有效的布尔值。</returns>
    public static bool VerifyDataPublicEcdsaWithBase64String(this string data, string signed, Encoding? encoding = null)
        => data.FromEncodingString(encoding).VerifyDataPublicEcdsa(signed.FromBase64String());


    /// <summary>
    /// 数字证书 ECDSA 私钥数据签名。
    /// </summary>
    /// <param name="data">给定的数据。</param>
    /// <returns>返回数字签名的字节数组。</returns>
    public static byte[] SignDataPrivateEcdsa(this byte[] data)
    {
        using var ecdsa = AlgorithmDependency.Value.LazyEcdsaPair.Value.PrivateAlgo;
        return ecdsa.SignData(data);
    }

    /// <summary>
    /// 数字证书 ECDSA 私钥数据签名。
    /// </summary>
    /// <param name="data">给定的数据 <see cref="Stream"/>。</param>
    /// <returns>返回数字签名的字节数组。</returns>
    public static byte[] SignDataPrivateEcdsa(this Stream data)
    {
        using var ecdsa = AlgorithmDependency.Value.LazyEcdsaPair.Value.PrivateAlgo;
        return ecdsa.SignData(data);
    }

    /// <summary>
    /// 数字证书 ECDSA 公钥验证数据签名。
    /// </summary>
    /// <param name="data">给定的数据。</param>
    /// <param name="signed">给定的数据签名。</param>
    /// <returns>返回签名是否有效的布尔值。</returns>
    public static bool VerifyDataPublicEcdsa(this byte[] data, byte[] signed)
    {
        using var ecdsa = AlgorithmDependency.Value.LazyEcdsaPair.Value.PublicAlgo;
        return ecdsa.VerifyData(data, signed);
    }

    /// <summary>
    /// 数字证书 ECDSA 公钥验证数据签名。
    /// </summary>
    /// <param name="data">给定的数据 <see cref="Stream"/>。</param>
    /// <param name="signed">给定的数据签名。</param>
    /// <returns>返回签名是否有效的布尔值。</returns>
    public static bool VerifyDataPublicEcdsa(this Stream data, byte[] signed)
    {
        using var ecdsa = AlgorithmDependency.Value.LazyEcdsaPair.Value.PublicAlgo;
        return ecdsa.VerifyData(data, signed);
    }


    /// <summary>
    /// 数字证书 ECDSA 私钥哈希值签名。
    /// </summary>
    /// <param name="hash">给定的哈希值。</param>
    /// <returns>返回数字签名的字节数组。</returns>
    public static byte[] SignHashPrivateEcdsa(this byte[] hash)
    {
        using var ecdsa = AlgorithmDependency.Value.LazyEcdsaPair.Value.PrivateAlgo;
        return ecdsa.SignHash(hash);
    }

    /// <summary>
    /// 数字证书 ECDSA 公钥验证哈希值签名。
    /// </summary>
    /// <param name="hash">给定的哈希值。</param>
    /// <param name="signed">给定的数据签名。</param>
    /// <returns>返回签名是否有效的布尔值。</returns>
    public static bool VerifyHashPublicEcdsa(this byte[] hash,  byte[] signed)
    {
        using var ecdsa = AlgorithmDependency.Value.LazyEcdsaPair.Value.PublicAlgo;
        return ecdsa.VerifyHash(hash, signed);
    }

    #endregion


    #region PasswordHash

    /// <summary>
    /// 计算密码哈希 BASE64 排序字符串。
    /// </summary>
    /// <param name="plaintext">给定的密码明文。</param>
    /// <param name="encoding">给定的字符编码（可选；默认为 <see cref="IDependencyContext.Encoding"/>）。</param>
    /// <param name="hashFunc">给定计算哈希值的方法（可选；默认使用 <see cref="AsSha384(byte[])"/>）。</param>
    /// <param name="encryptFunc">给定的哈希值加密方法（可选；默认使用 <see cref="AsAes(byte[], byte[], byte[])"/>）。</param>
    /// <returns>返回经过编码的字符串。</returns>
    public static string AsPasswordHash(this string plaintext, Encoding? encoding = null,
        Func<byte[], byte[]>? hashFunc = null, Func<byte[], byte[]>? encryptFunc = null)
    {
        hashFunc ??= AsSha384;
        encryptFunc ??= EncryptData;

        return plaintext.EncodePlaintextBuffer(encoding)
            .Switch(buffer => encryptFunc(hashFunc(buffer)))
            .ToBase64String();


        static byte[] EncryptData(byte[] data)
            => AsAes(data);
    }

    /// <summary>
    /// 验证密码哈希 BASE64 排序字符串。
    /// </summary>
    /// <param name="plaintext">给定的密码明文。</param>
    /// <param name="passwordHash">给定的密码哈希字符串。</param>
    /// <param name="encoding">给定的字符编码（可选；默认为 <see cref="IDependencyContext.Encoding"/>）。</param>
    /// <param name="hashFunc">给定计算哈希值的方法（可选；默认使用 <see cref="AsSha384(byte[])"/>）。</param>
    /// <param name="decryptFunc">给定的哈希值解密方法（可选；默认使用 <see cref="FromAes(byte[], byte[], byte[])"/>）。</param>
    /// <returns>返回密码是否相等的布尔值。</returns>
    public static bool VerifyPasswordHash(this string plaintext, string passwordHash, Encoding? encoding = null,
        Func<byte[], byte[]>? hashFunc = null, Func<byte[], byte[]>? decryptFunc = null)
    {
        hashFunc ??= AsSha384;
        decryptFunc ??= DecryptData;

        return plaintext.EncodePlaintextBuffer(encoding)
            .Switch(buffer => hashFunc(buffer))
            .FixedTimeEquals(decryptFunc(passwordHash.FromBase64String()));


        static byte[] DecryptData(byte[] data)
            => FromAes(data);
    }

    #endregion

}
