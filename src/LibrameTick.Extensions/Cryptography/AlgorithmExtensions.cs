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


    #region SharedArray

    /// <summary>
    /// 使用共享字节数组动作。
    /// </summary>
    /// <remarks>
    /// 请求租用的最小字节数组长度不等于实际的字节数组长度。如果对租用的数组长度有精确的要求，则不适用此方法。
    /// </remarks>
    /// <param name="minLength">给定要租用的最小字节数组长度。</param>
    /// <param name="clearArrayIfReturn">给定归还字节数组后是否需要清除字节数组内容。</param>
    /// <param name="resultAction">给定使用字节数组的动作。</param>
    public static void SharedByteArrayAction(this int minLength,
        bool clearArrayIfReturn, Action<byte[]> resultAction)
        => minLength.SharedArrayAction(clearArrayIfReturn, resultAction);

    /// <summary>
    /// 使用共享数组动作。
    /// </summary>
    /// <remarks>
    /// 请求租用的最小字节数组长度不等于实际的字节数组长度。如果对租用的数组长度有精确的要求，则不适用此方法。
    /// </remarks>
    /// <typeparam name="TArray">指定的数组类型。</typeparam>
    /// <param name="minLength">给定要租用的最小数组长度。</param>
    /// <param name="clearArrayIfReturn">给定归还数组后是否需要清除数组内容。</param>
    /// <param name="resultAction">给定使用数组的动作。</param>
    public static void SharedArrayAction<TArray>(this int minLength,
        bool clearArrayIfReturn, Action<TArray[]> resultAction)
    {
        var buffer = ArrayPool<TArray>.Shared.Rent(minLength);

        resultAction(buffer);

        ArrayPool<TArray>.Shared.Return(buffer, clearArrayIfReturn);
    }


    /// <summary>
    /// 使用共享字节数组方法。
    /// </summary>
    /// <remarks>
    /// 请求租用的最小字节数组长度不等于实际的字节数组长度。如果对租用的数组长度有精确的要求，则不适用此方法。
    /// </remarks>
    /// <typeparam name="TResult">指定的结果类型。</typeparam>
    /// <param name="minLength">给定要租用的最小字节数组长度。</param>
    /// <param name="clearArrayIfReturn">给定归还字节数组后是否需要清除字节数组内容。</param>
    /// <param name="resultFunc">给定使用字节数组的方法。</param>
    /// <returns>返回 <typeparamref name="TResult"/>。</returns>
    public static TResult SharedByteArrayFunc<TResult>(this int minLength,
        bool clearArrayIfReturn, Func<byte[], TResult> resultFunc)
        => minLength.SharedArrayFunc(clearArrayIfReturn, resultFunc);

    /// <summary>
    /// 使用共享数组方法。
    /// </summary>
    /// <remarks>
    /// 请求租用的最小字节数组长度不等于实际的字节数组长度。如果对租用的数组长度有精确的要求，则不适用此方法。
    /// </remarks>
    /// <typeparam name="TArray">指定的数组类型。</typeparam>
    /// <typeparam name="TResult">指定的结果类型。</typeparam>
    /// <param name="minLength">给定要租用的最小数组长度。</param>
    /// <param name="clearArrayIfReturn">给定归还数组后是否需要清除数组内容。</param>
    /// <param name="resultFunc">给定使用数组的方法。</param>
    /// <returns>返回 <typeparamref name="TResult"/>。</returns>
    public static TResult SharedArrayFunc<TArray, TResult>(this int minLength,
        bool clearArrayIfReturn, Func<TArray[], TResult> resultFunc)
    {
        var buffer = ArrayPool<TArray>.Shared.Rent(minLength);

        var value = resultFunc(buffer);

        ArrayPool<TArray>.Shared.Return(buffer, clearArrayIfReturn);

        return value;
    }

    #endregion


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
    /// <param name="imageType">给定的图像类型（如：image/jpeg）。</param>
    /// <returns>返回字符串。</returns>
    public static string AsImageBase64String(this byte[] bytes, string imageType)
        => $"data:{imageType};base64,{bytes.AsBase64String()}";

    /// <summary>
    /// 从 BASE64 字符串还原图像类型与字节数组。
    /// </summary>
    /// <param name="base64String">给定的图像 BASE64 字符串。</param>
    /// <returns>返回包含图像类型与字节数组的元组。</returns>
    /// <exception cref="ArgumentException">
    /// Invalid image base64 format string <paramref name="base64String"/>.
    /// </exception>
    public static (string imageType, byte[] bytes) FromImageBase64String(this string base64String)
    {
        if (!base64String.TrySplitPair(';', out var pair))
            throw new ArgumentException($"Invalid image base64 format string '{base64String}'.");

        return (pair.Key.TrimStart("data:"), pair.Value.TrimStart("base64,").FromBase64String());
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
    /// <returns>返回经过计算的字节数组。</returns>
    public static byte[] AsMd5(this byte[] buffer)
        => AlgorithmDependency.Value.LazyMd5.Value.ComputeHash(buffer);

    /// <summary>
    /// 计算 SHA1 哈希值。
    /// </summary>
    /// <param name="buffer">给定要计算的字节数组。</param>
    /// <returns>返回经过计算的字节数组。</returns>
    public static byte[] AsSha1(this byte[] buffer)
        => AlgorithmDependency.Value.LazySha1.Value.ComputeHash(buffer);

    /// <summary>
    /// 计算 SHA256 哈希值。
    /// </summary>
    /// <param name="buffer">给定要计算的字节数组。</param>
    /// <returns>返回经过计算的字节数组。</returns>
    public static byte[] AsSha256(this byte[] buffer)
        => AlgorithmDependency.Value.LazySha256.Value.ComputeHash(buffer);

    /// <summary>
    /// 计算 SHA384 哈希值。
    /// </summary>
    /// <param name="buffer">给定要计算的字节数组。</param>
    /// <returns>返回经过计算的字节数组。</returns>
    public static byte[] AsSha384(this byte[] buffer)
        => AlgorithmDependency.Value.LazySha384.Value.ComputeHash(buffer);

    /// <summary>
    /// 计算 SHA512 哈希值。
    /// </summary>
    /// <param name="buffer">给定要计算的字节数组。</param>
    /// <returns>返回经过计算的字节数组。</returns>
    public static byte[] AsSha512(this byte[] buffer)
        => AlgorithmDependency.Value.LazySha512.Value.ComputeHash(buffer);

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
    /// <returns>返回经过计算的字节数组。</returns>
    public static byte[] AsHmacMd5(this byte[] buffer)
        => AlgorithmDependency.Value.LazyHmacMd5.Value.ComputeHash(buffer);

    /// <summary>
    /// 计算 HMACSHA1 哈希值。
    /// </summary>
    /// <param name="buffer">给定要计算的字节数组。</param>
    /// <returns>返回经过计算的字节数组。</returns>
    public static byte[] AsHmacSha1(this byte[] buffer)
        => AlgorithmDependency.Value.LazyHmacSha1.Value.ComputeHash(buffer);

    /// <summary>
    /// 计算 HMACSHA256 哈希值。
    /// </summary>
    /// <param name="buffer">给定要计算的字节数组。</param>
    /// <returns>返回经过计算的字节数组。</returns>
    public static byte[] AsHmacSha256(this byte[] buffer)
        => AlgorithmDependency.Value.LazyHmacSha256.Value.ComputeHash(buffer);

    /// <summary>
    /// 计算 HMACSHA384 哈希值。
    /// </summary>
    /// <param name="buffer">给定要计算的字节数组。</param>
    /// <returns>返回经过计算的字节数组。</returns>
    public static byte[] AsHmacSha384(this byte[] buffer)
        => AlgorithmDependency.Value.LazyHmacSha384.Value.ComputeHash(buffer);

    /// <summary>
    /// 计算 HMACSHA512 哈希值。
    /// </summary>
    /// <param name="buffer">给定要计算的字节数组。</param>
    /// <returns>返回经过计算的字节数组。</returns>
    public static byte[] AsHmacSha512(this byte[] buffer)
        => AlgorithmDependency.Value.LazyHmacSha512.Value.ComputeHash(buffer);

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
    /// <returns>返回经过加密的字节数组。</returns>
    public static byte[] As3Des(this byte[] plaintext)
    {
        var transform = AlgorithmDependency.Value.Lazy3Des.Value.CreateEncryptor();
        return transform.TransformFinalBlock(plaintext, 0, plaintext.Length);
    }

    /// <summary>
    /// TripleDES 解密。
    /// </summary>
    /// <param name="ciphertext">给定的密文。</param>
    /// <returns>返回经过解密的字节数组。</returns>
    public static byte[] From3Des(this byte[] ciphertext)
    {
        var transform = AlgorithmDependency.Value.Lazy3Des.Value.CreateDecryptor();
        return transform.TransformFinalBlock(ciphertext, 0, ciphertext.Length);
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
    /// <returns>返回经过加密的字节数组。</returns>
    public static byte[] AsAes(this byte[] plaintext)
    {
        var transform = AlgorithmDependency.Value.LazyAes.Value.CreateEncryptor();
        return transform.TransformFinalBlock(plaintext, 0, plaintext.Length);
    }

    /// <summary>
    /// AES 解密。
    /// </summary>
    /// <param name="ciphertext">给定的密文。</param>
    /// <returns>返回经过解密的字节数组。</returns>
    public static byte[] FromAes(this byte[] ciphertext)
    {
        var transform = AlgorithmDependency.Value.LazyAes.Value.CreateDecryptor();
        return transform.TransformFinalBlock(ciphertext, 0, ciphertext.Length);
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
        var aesCcm = AlgorithmDependency.Value.Keyring.AesCcm;

        AlgorithmDependency.Value.LazyAesCcm.Value.Encrypt(aesCcm.Nonce, plaintext, ciphertext, aesCcm.Tag);

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
        var aesCcm = AlgorithmDependency.Value.Keyring.AesCcm;

        AlgorithmDependency.Value.LazyAesCcm.Value.Decrypt(aesCcm.Nonce, ciphertext, aesCcm.Tag, plaintext);

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
        var aesGcm = AlgorithmDependency.Value.Keyring.AesGcm;

        AlgorithmDependency.Value.LazyAesGcm.Value.Encrypt(aesGcm.Nonce, plaintext, ciphertext, aesGcm.Tag);

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
        var aesGcm = AlgorithmDependency.Value.Keyring.AesGcm;

        AlgorithmDependency.Value.LazyAesGcm.Value.Decrypt(aesGcm.Nonce, ciphertext, aesGcm.Tag, plaintext);

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
    /// 数字证书 RSA 私钥签名 BASE64 字符串形式。
    /// </summary>
    /// <param name="plaintext">给定的明文。</param>
    /// <param name="encoding">给定的字符编码（可选；默认为 <see cref="IDependencyContext.Encoding"/>）。</param>
    /// <returns>返回经过 BASE64 编码的加密字符串。</returns>
    public static string SignDataPrivateRsaWithBase64String(this string plaintext, Encoding? encoding = null)
        => plaintext.FromEncodingString(encoding).SignDataPrivateRsa().AsBase64String();

    /// <summary>
    /// 数字证书 RSA 公钥验证 BASE64 字符串形式。
    /// </summary>
    /// <param name="ciphertext">给定的密文。</param>
    /// <param name="plaintext">给定的明文。</param>
    /// <param name="encoding">给定的字符编码（可选；默认为 <see cref="IDependencyContext.Encoding"/>）。</param>
    /// <returns>返回经过 BASE64 解码的解密字符串。</returns>
    public static bool VerifyDataPublicRsaWithBase64String(this string ciphertext, string plaintext, Encoding? encoding = null)
        => ciphertext.FromBase64String().VerifyDataPublicRsa(plaintext.FromEncodingString(encoding));


    /// <summary>
    /// RSA 公钥加密。
    /// </summary>
    /// <param name="plaintext">给定的明文。</param>
    /// <param name="padding">给定的 <see cref="RSAEncryptionPadding"/>（可选；默认为 <see cref="RSAEncryptionPadding.Pkcs1"/>）。</param>
    /// <returns>返回经过加密的字节数组。</returns>
    public static byte[] AsPublicRsa(this byte[] plaintext, RSAEncryptionPadding? padding = null)
    {
        var rsa = AlgorithmDependency.Value.LazyRsaPair.Value.PublicAlgo;

        // RSA 单次加密受支持 Key 的模长(modulus)-n 长度限制，超出限制需分段加密
        // 此处默认超过 128 就开始分段加密
        var bufferSize = 128;

        if (plaintext.Length <= bufferSize)
        {
            return rsa.Encrypt(plaintext, padding ?? RSAEncryptionPadding.Pkcs1);
        }

        var ciphertext = new List<byte>();

        var encryptLength = 0;
        var surplusLength = plaintext.Length;

        while (encryptLength < plaintext.Length)
        {
            var curLength = bufferSize;
            if (surplusLength < bufferSize)
            {
                curLength = surplusLength;
            }

            curLength.SharedByteArrayAction(clearArrayIfReturn: true, buffer =>
            {
                Array.Copy(plaintext, encryptLength, buffer, 0, curLength);

                // encryptBuffer.Length != bufferSize
                var encryptBuffer = rsa.Encrypt(buffer, padding ?? RSAEncryptionPadding.Pkcs1);

                ciphertext.AddRange(encryptBuffer);
            });

            encryptLength += bufferSize;
            surplusLength -= bufferSize;
        }

        return [.. ciphertext];
    }

    /// <summary>
    /// RSA 私钥解密。
    /// </summary>
    /// <param name="ciphertext">给定的密文。</param>
    /// <param name="padding">给定的 <see cref="RSAEncryptionPadding"/>（可选；默认为 <see cref="RSAEncryptionPadding.Pkcs1"/>）。</param>
    /// <returns>返回经过解密的字节数组。</returns>
    public static byte[] FromPrivateRsa(this byte[] ciphertext, RSAEncryptionPadding? padding = null)
    {
        var rsa = AlgorithmDependency.Value.LazyRsaPair.Value.PrivateAlgo;

        // 解密时分段长度需使用密钥大小对应的字节长度
        var keySizeInBytes = rsa.KeySize / 8;

        if (ciphertext.Length <= keySizeInBytes)
        {
            return rsa.Decrypt(ciphertext, padding ?? RSAEncryptionPadding.Pkcs1);
        }

        var plaintext = new List<byte>();

        var decryptLength = 0;
        var surplusLength = ciphertext.Length;

        while (decryptLength < ciphertext.Length)
        {
            var curLength = keySizeInBytes;
            if (surplusLength < keySizeInBytes)
            {
                curLength = surplusLength;
            }

            curLength.SharedByteArrayAction(clearArrayIfReturn: true, buffer =>
            {
                Array.Copy(ciphertext, decryptLength, buffer, 0, curLength);

                // decryptBuffer.Length != keySizeInBytes
                var decryptBuffer = rsa.Decrypt(buffer, padding ?? RSAEncryptionPadding.Pkcs1);

                plaintext.AddRange(decryptBuffer);
            });

            decryptLength += keySizeInBytes;
            surplusLength -= keySizeInBytes;
        }

        return [.. plaintext];
    }


    /// <summary>
    /// 数字证书 RSA 私钥签名。
    /// </summary>
    /// <param name="plaintext">给定的明文。</param>
    /// <param name="hashName">给定的 <see cref="HashAlgorithmName"/>（可选；默认为 <see cref="HashAlgorithmName.SHA256"/>）。</param>
    /// <param name="padding">给定的 <see cref="RSASignaturePadding"/>（可选；默认为 <see cref="RSASignaturePadding.Pkcs1"/>）</param>
    /// <returns>返回经过加密的字节数组。</returns>
    public static byte[] SignDataPrivateRsa(this byte[] plaintext, HashAlgorithmName? hashName = null, RSASignaturePadding? padding = null)
        => AlgorithmDependency.Value.LazyRsaPair.Value.PrivateAlgo.SignData(plaintext, hashName ?? HashAlgorithmName.SHA256,
            padding ?? RSASignaturePadding.Pkcs1);

    /// <summary>
    /// 数字证书 RSA 公钥验证。
    /// </summary>
    /// <param name="ciphertext">给定待验证的签名密文。</param>
    /// <param name="signed">给定明确的签名数据。</param>
    /// <param name="hashName">给定的 <see cref="HashAlgorithmName"/>（可选；默认为 <see cref="HashAlgorithmName.SHA256"/>）。</param>
    /// <param name="padding">给定的 <see cref="RSASignaturePadding"/>（可选；默认为 <see cref="RSASignaturePadding.Pkcs1"/>）</param>
    /// <returns>返回经过解密的字节数组。</returns>
    public static bool VerifyDataPublicRsa(this byte[] ciphertext, byte[] signed, HashAlgorithmName? hashName = null, RSASignaturePadding? padding = null)
        => AlgorithmDependency.Value.LazyRsaPair.Value.PublicAlgo.VerifyData(signed, ciphertext, hashName ?? HashAlgorithmName.SHA256,
            padding ?? RSASignaturePadding.Pkcs1);


    /// <summary>
    /// 数字证书 RSA 私钥签名。
    /// </summary>
    /// <param name="hash">给定的明文。</param>
    /// <param name="hashName">给定的 <see cref="HashAlgorithmName"/>（可选；默认为 <see cref="HashAlgorithmName.SHA256"/>）。</param>
    /// <param name="padding">给定的 <see cref="RSASignaturePadding"/>（可选；默认为 <see cref="RSASignaturePadding.Pkcs1"/>）</param>
    /// <returns>返回经过加密的字节数组。</returns>
    public static byte[] SignHashPrivateRsa(this byte[] hash, HashAlgorithmName? hashName = null, RSASignaturePadding? padding = null)
        => AlgorithmDependency.Value.LazyRsaPair.Value.PrivateAlgo.SignHash(hash, hashName ?? HashAlgorithmName.SHA256,
            padding ?? RSASignaturePadding.Pkcs1);

    /// <summary>
    /// 数字证书 RSA 公钥验证。
    /// </summary>
    /// <param name="ciphertext">给定待验证的签名密文。</param>
    /// <param name="hash">给定明确的签名数据。</param>
    /// <param name="hashName">给定的 <see cref="HashAlgorithmName"/>（可选；默认为 <see cref="HashAlgorithmName.SHA256"/>）。</param>
    /// <param name="padding">给定的 <see cref="RSASignaturePadding"/>（可选；默认为 <see cref="RSASignaturePadding.Pkcs1"/>）</param>
    /// <returns>返回经过解密的字节数组。</returns>
    public static bool VerifyHashPublicRsa(this byte[] ciphertext, byte[] hash, HashAlgorithmName? hashName = null, RSASignaturePadding? padding = null)
        => AlgorithmDependency.Value.LazyRsaPair.Value.PublicAlgo.VerifyHash(hash, ciphertext, hashName ?? HashAlgorithmName.SHA256,
            padding ?? RSASignaturePadding.Pkcs1);

    #endregion


    #region ECDSA

    /// <summary>
    /// 数字证书 ECDSA 私钥签名 BASE64 字符串形式。
    /// </summary>
    /// <param name="plaintext">给定的明文。</param>
    /// <param name="encoding">给定的字符编码（可选；默认为 <see cref="IDependencyContext.Encoding"/>）。</param>
    /// <returns>返回经过 BASE64 编码的加密字符串。</returns>
    public static string SignDataPrivateEcdsaWithBase64String(this string plaintext, Encoding? encoding = null)
        => plaintext.FromEncodingString(encoding).SignDataPrivateEcdsa().AsBase64String();

    /// <summary>
    /// 数字证书 ECDSA 公钥验证 BASE64 字符串形式。
    /// </summary>
    /// <param name="ciphertext">给定的密文。</param>
    /// <param name="plaintext">给定的明文。</param>
    /// <param name="encoding">给定的字符编码（可选；默认为 <see cref="IDependencyContext.Encoding"/>）。</param>
    /// <returns>返回经过 BASE64 解码的解密字符串。</returns>
    public static bool VerifyDataPublicEcdsaWithBase64String(this string ciphertext, string plaintext, Encoding? encoding = null)
        => ciphertext.FromBase64String().VerifyDataPublicEcdsa(plaintext.FromEncodingString(encoding));


    /// <summary>
    /// 数字证书 ECDSA 私钥签名。
    /// </summary>
    /// <param name="plaintext">给定的明文。</param>
    /// <returns>返回经过加密的字节数组。</returns>
    public static byte[] SignDataPrivateEcdsa(this byte[] plaintext)
        => AlgorithmDependency.Value.LazyEcdsaPair.Value.PrivateAlgo.SignData(plaintext);

    /// <summary>
    /// 数字证书 ECDSA 公钥验证。
    /// </summary>
    /// <param name="ciphertext">给定待验证的签名密文。</param>
    /// <param name="signed">给定明确的签名数据。</param>
    /// <returns>返回经过解密的字节数组。</returns>
    public static bool VerifyDataPublicEcdsa(this byte[] ciphertext, byte[] signed)
        => AlgorithmDependency.Value.LazyEcdsaPair.Value.PublicAlgo.VerifyData(signed, ciphertext);


    /// <summary>
    /// 数字证书 ECDSA 私钥签名。
    /// </summary>
    /// <param name="hash">给定的明文。</param>
    /// <returns>返回经过加密的字节数组。</returns>
    public static byte[] SignHashPrivateEcdsa(this byte[] hash)
        => AlgorithmDependency.Value.LazyEcdsaPair.Value.PrivateAlgo.SignHash(hash);

    /// <summary>
    /// 数字证书 ECDSA 公钥验证。
    /// </summary>
    /// <param name="ciphertext">给定待验证的签名密文。</param>
    /// <param name="hash">给定明确的签名数据。</param>
    /// <returns>返回经过解密的字节数组。</returns>
    public static bool VerifyHashPublicEcdsa(this byte[] ciphertext, byte[] hash)
        => AlgorithmDependency.Value.LazyEcdsaPair.Value.PublicAlgo.VerifyHash(hash, ciphertext);

    #endregion


    #region Password

    /// <summary>
    /// 计算密码哈希 BASE64 排序字符串。
    /// </summary>
    /// <param name="plaintext">给定的密码明文。</param>
    /// <param name="encoding">给定的字符编码（可选；默认为 <see cref="IDependencyContext.Encoding"/>）。</param>
    /// <param name="hashFunc">给定计算哈希值的方法（可选；默认使用 <see cref="AsSha384(byte[])"/>）。</param>
    /// <param name="encryptFunc">给定的哈希值加密方法（可选；默认使用 <see cref="AsAes(byte[])"/>）。</param>
    /// <returns>返回经过编码的字符串。</returns>
    public static string AsPassword(this string plaintext, Encoding? encoding = null,
        Func<byte[], byte[]>? hashFunc = null, Func<byte[], byte[]>? encryptFunc = null)
    {
        hashFunc ??= AsSha384;
        encryptFunc ??= AsAes;

        return AlgorithmDependency.Value.FluentProcess(algo =>
        {
            var buffer = plaintext.FromEncodingString(encoding);

            buffer = hashFunc(buffer);
            buffer = encryptFunc(buffer);

            return buffer.AsBase64String();
        });
    }

    /// <summary>
    /// 验证密码哈希 BASE64 排序字符串。
    /// </summary>
    /// <param name="passwordHash">给定的密码哈希字符串。</param>
    /// <param name="plaintext">给定的密码明文。</param>
    /// <param name="encoding">给定的字符编码（可选；默认为 <see cref="IDependencyContext.Encoding"/>）。</param>
    /// <param name="hashFunc">给定计算哈希值的方法（可选；默认使用 <see cref="AsSha384(byte[])"/>）。</param>
    /// <param name="decryptFunc">给定的哈希值解密方法（可选；默认使用 <see cref="FromAes(byte[])"/>）。</param>
    /// <returns>返回密码是否相等的布尔值。</returns>
    public static bool VerifyPassword(this string passwordHash, string plaintext, Encoding? encoding = null,
        Func<byte[], byte[]>? hashFunc = null, Func<byte[], byte[]>? decryptFunc = null)
    {
        hashFunc ??= AsSha384;
        decryptFunc ??= FromAes;

        return AlgorithmDependency.Value.FluentProcess(algo =>
        {
            var plaintextBuffer = plaintext.FromEncodingString(encoding);
            plaintextBuffer = hashFunc(plaintextBuffer);

            var hashBuffer = passwordHash.FromBase64String();
            hashBuffer = decryptFunc(hashBuffer);

            // 使用固定时间比较字符串相等，以防范诸如破解密钥的计时攻击
            return CryptographicOperations.FixedTimeEquals(plaintextBuffer, hashBuffer);
        });
    }

    #endregion

}
