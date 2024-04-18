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
/// 算法静态扩展。
/// </summary>
public static class AlgorithmExtensions
{
    /// <summary>
    /// 当前扩展依赖。
    /// </summary>
    public static Dependencies.IExtensionsDependency Dependency
        => Dependencies.ExtensionsDependencyInstantiator.Instance;


    #region Encoding

    /// <summary>
    /// 转为字符编码字符串。
    /// </summary>
    /// <param name="bytes">给定的字节数组。</param>
    /// <param name="encoding">给定的字符编码（可选；默认为 <see cref="Dependencies.IExtensionsDependency.Encoding"/>）。</param>
    /// <returns>返回字符编码字符串。</returns>
    public static string AsEncodingString(this byte[] bytes, Encoding? encoding = null)
        => (encoding ?? Dependency.Encoding).GetString(bytes);

    /// <summary>
    /// 还原字符编码字符串。
    /// </summary>
    /// <param name="plaintext">给定的明文。</param>
    /// <param name="encoding">给定的字符编码（可选；默认为 <see cref="Dependencies.IExtensionsDependency.Encoding"/>）。</param>
    /// <returns>返回字节数组。</returns>
    public static byte[] FromEncodingString(this string plaintext, Encoding? encoding = null)
        => (encoding ?? Dependency.Encoding).GetBytes(plaintext);

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
                index = (byte)(bytes[currentByte++] >> (hi - 5));

                if (currentByte != bytes.Length)
                {
                    index = (byte)(((byte)(bytes[currentByte] << (16 - hi)) >> 3) | index);
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
                index = (byte)((byte)(bytes[currentByte] << (8 - hi)) >> 3);
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
    /// <param name="encoding">给定的字符编码（可选；默认为 <see cref="Dependencies.IExtensionsDependency.Encoding"/>）。</param>
    /// <returns>返回经过 BASE64 编码的加密字符串。</returns>
    public static string AsMd5Base64String(this string plaintext, Encoding? encoding = null)
        => plaintext.FromEncodingString(encoding).AsMd5().AsBase64String();

    /// <summary>
    /// 计算 SHA1 哈希值，并返回 BASE64 字符串形式。
    /// </summary>
    /// <param name="plaintext">给定的明文。</param>
    /// <param name="encoding">给定的字符编码（可选；默认为 <see cref="Dependencies.IExtensionsDependency.Encoding"/>）。</param>
    /// <returns>返回经过 BASE64 编码的加密字符串。</returns>
    public static string AsSha1Base64String(this string plaintext, Encoding? encoding = null)
        => plaintext.FromEncodingString(encoding).AsSha1().AsBase64String();

    /// <summary>
    /// 计算 SHA256 哈希值，并返回 BASE64 字符串形式。
    /// </summary>
    /// <param name="plaintext">给定的明文。</param>
    /// <param name="encoding">给定的字符编码（可选；默认为 <see cref="Dependencies.IExtensionsDependency.Encoding"/>）。</param>
    /// <returns>返回经过 BASE64 编码的加密字符串。</returns>
    public static string AsSha256Base64String(this string plaintext, Encoding? encoding = null)
        => plaintext.FromEncodingString(encoding).AsSha256().AsBase64String();

    /// <summary>
    /// 计算 SHA384 哈希值，并返回 BASE64 字符串形式。
    /// </summary>
    /// <param name="plaintext">给定的明文。</param>
    /// <param name="encoding">给定的字符编码（可选；默认为 <see cref="Dependencies.IExtensionsDependency.Encoding"/>）。</param>
    /// <returns>返回经过 BASE64 编码的加密字符串。</returns>
    public static string AsSha384Base64String(this string plaintext, Encoding? encoding = null)
        => plaintext.FromEncodingString(encoding).AsSha384().AsBase64String();

    /// <summary>
    /// 计算 SHA512 哈希值，并返回 BASE64 字符串形式。
    /// </summary>
    /// <param name="plaintext">给定的明文。</param>
    /// <param name="encoding">给定的字符编码（可选；默认为 <see cref="Dependencies.IExtensionsDependency.Encoding"/>）。</param>
    /// <returns>返回经过 BASE64 编码的加密字符串。</returns>
    public static string AsSha512Base64String(this string plaintext, Encoding? encoding = null)
        => plaintext.FromEncodingString(encoding).AsSha512().AsBase64String();


    /// <summary>
    /// 计算 MD5 哈希值。
    /// </summary>
    /// <param name="buffer">给定要计算的字节数组。</param>
    /// <returns>返回经过计算的字节数组。</returns>
    public static byte[] AsMd5(this byte[] buffer)
        => Dependency.LazyAlgorithmManager.Value.LazyMd5.Value.ComputeHash(buffer);

    /// <summary>
    /// 计算 SHA1 哈希值。
    /// </summary>
    /// <param name="buffer">给定要计算的字节数组。</param>
    /// <returns>返回经过计算的字节数组。</returns>
    public static byte[] AsSha1(this byte[] buffer)
        => Dependency.LazyAlgorithmManager.Value.LazySha1.Value.ComputeHash(buffer);

    /// <summary>
    /// 计算 SHA256 哈希值。
    /// </summary>
    /// <param name="buffer">给定要计算的字节数组。</param>
    /// <returns>返回经过计算的字节数组。</returns>
    public static byte[] AsSha256(this byte[] buffer)
        => Dependency.LazyAlgorithmManager.Value.LazySha256.Value.ComputeHash(buffer);

    /// <summary>
    /// 计算 SHA384 哈希值。
    /// </summary>
    /// <param name="buffer">给定要计算的字节数组。</param>
    /// <returns>返回经过计算的字节数组。</returns>
    public static byte[] AsSha384(this byte[] buffer)
        => Dependency.LazyAlgorithmManager.Value.LazySha384.Value.ComputeHash(buffer);

    /// <summary>
    /// 计算 SHA512 哈希值。
    /// </summary>
    /// <param name="buffer">给定要计算的字节数组。</param>
    /// <returns>返回经过计算的字节数组。</returns>
    public static byte[] AsSha512(this byte[] buffer)
        => Dependency.LazyAlgorithmManager.Value.LazySha512.Value.ComputeHash(buffer);

    #endregion


    #region HMAC Hash

    /// <summary>
    /// 计算 HMACMD5 哈希值，并返回 BASE64 字符串形式。
    /// </summary>
    /// <param name="plaintext">给定的明文。</param>
    /// <param name="encoding">给定的字符编码（可选；默认为 <see cref="Dependencies.IExtensionsDependency.Encoding"/>）。</param>
    /// <returns>返回经过 BASE64 编码的加密字符串。</returns>
    public static string AsHmacMd5Base64String(this string plaintext, Encoding? encoding = null)
        => plaintext.FromEncodingString(encoding).AsHmacMd5().AsBase64String();

    /// <summary>
    /// 计算 HMACSHA1 哈希值，并返回 BASE64 字符串形式。
    /// </summary>
    /// <param name="plaintext">给定的明文。</param>
    /// <param name="encoding">给定的字符编码（可选；默认为 <see cref="Dependencies.IExtensionsDependency.Encoding"/>）。</param>
    /// <returns>返回经过 BASE64 编码的加密字符串。</returns>
    public static string AsHmacSha1Base64String(this string plaintext, Encoding? encoding = null)
        => plaintext.FromEncodingString(encoding).AsHmacSha1().AsBase64String();

    /// <summary>
    /// 计算 HMACSHA256 哈希值，并返回 BASE64 字符串形式。
    /// </summary>
    /// <param name="plaintext">给定的明文。</param>
    /// <param name="encoding">给定的字符编码（可选；默认为 <see cref="Dependencies.IExtensionsDependency.Encoding"/>）。</param>
    /// <returns>返回经过 BASE64 编码的加密字符串。</returns>
    public static string AsHmacSha256Base64String(this string plaintext, Encoding? encoding = null)
        => plaintext.FromEncodingString(encoding).AsHmacSha256().AsBase64String();

    /// <summary>
    /// 计算 HMACSHA384 哈希值，并返回 BASE64 字符串形式。
    /// </summary>
    /// <param name="plaintext">给定的明文。</param>
    /// <param name="encoding">给定的字符编码（可选；默认为 <see cref="Dependencies.IExtensionsDependency.Encoding"/>）。</param>
    /// <returns>返回经过 BASE64 编码的加密字符串。</returns>
    public static string AsHmacSha384Base64String(this string plaintext, Encoding? encoding = null)
        => plaintext.FromEncodingString(encoding).AsHmacSha384().AsBase64String();

    /// <summary>
    /// 计算 HMACSHA512 哈希值，并返回 BASE64 字符串形式。
    /// </summary>
    /// <param name="plaintext">给定的明文。</param>
    /// <param name="encoding">给定的字符编码（可选；默认为 <see cref="Dependencies.IExtensionsDependency.Encoding"/>）。</param>
    /// <returns>返回经过 BASE64 编码的加密字符串。</returns>
    public static string AsHmacSha512Base64String(this string plaintext, Encoding? encoding = null)
        => plaintext.FromEncodingString(encoding).AsHmacSha512().AsBase64String();


    /// <summary>
    /// 计算 HMACMD5 哈希值。
    /// </summary>
    /// <param name="buffer">给定要计算的字节数组。</param>
    /// <returns>返回经过计算的字节数组。</returns>
    public static byte[] AsHmacMd5(this byte[] buffer)
        => Dependency.LazyAlgorithmManager.Value.LazyHmacMd5.Value.ComputeHash(buffer);

    /// <summary>
    /// 计算 HMACSHA1 哈希值。
    /// </summary>
    /// <param name="buffer">给定要计算的字节数组。</param>
    /// <returns>返回经过计算的字节数组。</returns>
    public static byte[] AsHmacSha1(this byte[] buffer)
        => Dependency.LazyAlgorithmManager.Value.LazyHmacSha1.Value.ComputeHash(buffer);

    /// <summary>
    /// 计算 HMACSHA256 哈希值。
    /// </summary>
    /// <param name="buffer">给定要计算的字节数组。</param>
    /// <returns>返回经过计算的字节数组。</returns>
    public static byte[] AsHmacSha256(this byte[] buffer)
        => Dependency.LazyAlgorithmManager.Value.LazyHmacSha256.Value.ComputeHash(buffer);

    /// <summary>
    /// 计算 HMACSHA384 哈希值。
    /// </summary>
    /// <param name="buffer">给定要计算的字节数组。</param>
    /// <returns>返回经过计算的字节数组。</returns>
    public static byte[] AsHmacSha384(this byte[] buffer)
        => Dependency.LazyAlgorithmManager.Value.LazyHmacSha384.Value.ComputeHash(buffer);

    /// <summary>
    /// 计算 HMACSHA512 哈希值。
    /// </summary>
    /// <param name="buffer">给定要计算的字节数组。</param>
    /// <returns>返回经过计算的字节数组。</returns>
    public static byte[] AsHmacSha512(this byte[] buffer)
        => Dependency.LazyAlgorithmManager.Value.LazyHmacSha512.Value.ComputeHash(buffer);

    #endregion


    #region DES

    /// <summary>
    /// TripleDES 加密 BASE64 字符串形式。
    /// </summary>
    /// <param name="plaintext">给定的明文。</param>
    /// <param name="encoding">给定的字符编码（可选；默认为 <see cref="Dependencies.IExtensionsDependency.Encoding"/>）。</param>
    /// <returns>返回经过 BASE64 编码的加密字符串。</returns>
    public static string As3DesWithBase64String(this string plaintext, Encoding? encoding = null)
        => plaintext.FromEncodingString(encoding).As3Des().AsBase64String();

    /// <summary>
    /// TripleDES 解密 BASE64 字符串形式。
    /// </summary>
    /// <param name="ciphertext">给定的密文。</param>
    /// <param name="encoding">给定的字符编码（可选；默认为 <see cref="Dependencies.IExtensionsDependency.Encoding"/>）。</param>
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
        var transform = Dependency.LazyAlgorithmManager.Value.Lazy3Des.Value.CreateEncryptor();
        return transform.TransformFinalBlock(plaintext, 0, plaintext.Length);
    }

    /// <summary>
    /// TripleDES 解密。
    /// </summary>
    /// <param name="ciphertext">给定的密文。</param>
    /// <returns>返回经过解密的字节数组。</returns>
    public static byte[] From3Des(this byte[] ciphertext)
    {
        var transform = Dependency.LazyAlgorithmManager.Value.Lazy3Des.Value.CreateDecryptor();
        return transform.TransformFinalBlock(ciphertext, 0, ciphertext.Length);
    }

    #endregion


    #region AES

    /// <summary>
    /// AES 加密 BASE64 字符串形式。
    /// </summary>
    /// <param name="plaintext">给定的明文。</param>
    /// <param name="encoding">给定的字符编码（可选；默认为 <see cref="Dependencies.IExtensionsDependency.Encoding"/>）。</param>
    /// <returns>返回经过 BASE64 编码的加密字符串。</returns>
    public static string AsAesWithBase64String(this string plaintext, Encoding? encoding = null)
        => plaintext.FromEncodingString(encoding).AsAes().AsBase64String();

    /// <summary>
    /// AES 解密 BASE64 字符串形式。
    /// </summary>
    /// <param name="ciphertext">给定的密文。</param>
    /// <param name="encoding">给定的字符编码（可选；默认为 <see cref="Dependencies.IExtensionsDependency.Encoding"/>）。</param>
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
        var transform = Dependency.LazyAlgorithmManager.Value.LazyAes.Value.CreateEncryptor();
        return transform.TransformFinalBlock(plaintext, 0, plaintext.Length);
    }

    /// <summary>
    /// AES 解密。
    /// </summary>
    /// <param name="ciphertext">给定的密文。</param>
    /// <returns>返回经过解密的字节数组。</returns>
    public static byte[] FromAes(this byte[] ciphertext)
    {
        var transform = Dependency.LazyAlgorithmManager.Value.LazyAes.Value.CreateDecryptor();
        return transform.TransformFinalBlock(ciphertext, 0, ciphertext.Length);
    }

    #endregion


    #region AES-CCM

    /// <summary>
    /// AES-CCM 加密 BASE64 字符串形式。
    /// </summary>
    /// <param name="plaintext">给定的明文。</param>
    /// <param name="encoding">给定的字符编码（可选；默认为 <see cref="Dependencies.IExtensionsDependency.Encoding"/>）。</param>
    /// <returns>返回经过 BASE64 编码的加密字符串。</returns>
    public static string AsAesCcmWithBase64String(this string plaintext, Encoding? encoding = null)
        => plaintext.FromEncodingString(encoding).AsAesCcm().AsBase64String();

    /// <summary>
    /// AES-CCM 解密 BASE64 字符串形式。
    /// </summary>
    /// <param name="ciphertext">给定的密文。</param>
    /// <param name="encoding">给定的字符编码（可选；默认为 <see cref="Dependencies.IExtensionsDependency.Encoding"/>）。</param>
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
        var aesCcm = Dependency.LazyAlgorithmManager.Value.Keyring.AesCcm;

        Dependency.LazyAlgorithmManager.Value.LazyAesCcm.Value.Encrypt(aesCcm.Nonce, plaintext, ciphertext, aesCcm.Tag);

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
        var aesCcm = Dependency.LazyAlgorithmManager.Value.Keyring.AesCcm;

        Dependency.LazyAlgorithmManager.Value.LazyAesCcm.Value.Decrypt(aesCcm.Nonce, ciphertext, aesCcm.Tag, plaintext);

        return plaintext;
    }

    #endregion


    #region AES-GCM

    /// <summary>
    /// AES-GCM 加密 BASE64 字符串形式。
    /// </summary>
    /// <param name="plaintext">给定的明文。</param>
    /// <param name="encoding">给定的字符编码（可选；默认为 <see cref="Dependencies.IExtensionsDependency.Encoding"/>）。</param>
    /// <returns>返回经过 BASE64 编码的加密字符串。</returns>
    public static string AsAesGcmWithBase64String(this string plaintext, Encoding? encoding = null)
        => plaintext.FromEncodingString(encoding).AsAesGcm().AsBase64String();

    /// <summary>
    /// AES-GCM 解密 BASE64 字符串形式。
    /// </summary>
    /// <param name="ciphertext">给定的密文。</param>
    /// <param name="encoding">给定的字符编码（可选；默认为 <see cref="Dependencies.IExtensionsDependency.Encoding"/>）。</param>
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
        var aesGcm = Dependency.LazyAlgorithmManager.Value.Keyring.AesGcm;

        Dependency.LazyAlgorithmManager.Value.LazyAesGcm.Value.Encrypt(aesGcm.Nonce, plaintext, ciphertext, aesGcm.Tag);

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
        var aesGcm = Dependency.LazyAlgorithmManager.Value.Keyring.AesGcm;

        Dependency.LazyAlgorithmManager.Value.LazyAesGcm.Value.Decrypt(aesGcm.Nonce, ciphertext, aesGcm.Tag, plaintext);

        return plaintext;
    }

    #endregion


    #region ECDSA

    /// <summary>
    /// 数字证书 ECDSA 私钥签名 BASE64 字符串形式。
    /// </summary>
    /// <param name="plaintext">给定的明文。</param>
    /// <param name="encoding">给定的字符编码（可选；默认为 <see cref="Dependencies.IExtensionsDependency.Encoding"/>）。</param>
    /// <returns>返回经过 BASE64 编码的加密字符串。</returns>
    public static string SignEcdsaWithBase64String(this string plaintext, Encoding? encoding = null)
        => plaintext.FromEncodingString(encoding).SignEcdsa().AsBase64String();

    /// <summary>
    /// 数字证书 ECDSA 私钥验证 BASE64 字符串形式。
    /// </summary>
    /// <param name="ciphertext">给定的密文。</param>
    /// <param name="plaintext">给定的明文。</param>
    /// <param name="encoding">给定的字符编码（可选；默认为 <see cref="Dependencies.IExtensionsDependency.Encoding"/>）。</param>
    /// <returns>返回经过 BASE64 解码的解密字符串。</returns>
    public static bool VerifyEcdsaWithBase64String(this string ciphertext, string plaintext, Encoding? encoding = null)
        => ciphertext.FromBase64String().VerifyEcdsa(plaintext.FromEncodingString(encoding));


    /// <summary>
    /// 数字证书 ECDSA 私钥签名。
    /// </summary>
    /// <param name="plaintext">给定的明文。</param>
    /// <returns>返回经过加密的字节数组。</returns>
    public static byte[] SignEcdsa(this byte[] plaintext)
        => Dependency.LazyAlgorithmManager.Value.LazyPrivateEcdsa.Value.SignData(plaintext);

    /// <summary>
    /// 数字证书 ECDSA 私钥验证。
    /// </summary>
    /// <param name="ciphertext">给定待验证的签名密文。</param>
    /// <param name="signed">给定明确的签名数据。</param>
    /// <returns>返回经过解密的字节数组。</returns>
    public static bool VerifyEcdsa(this byte[] ciphertext, byte[] signed)
        => Dependency.LazyAlgorithmManager.Value.LazyPrivateEcdsa.Value.VerifyData(signed, ciphertext);

    #endregion


    #region Password

    /// <summary>
    /// 计算密码哈希 BASE64 排序字符串。
    /// </summary>
    /// <param name="plaintext">给定的密码明文。</param>
    /// <param name="encoding">给定的字符编码（可选；默认为 <see cref="Dependencies.IExtensionsDependency.Encoding"/>）。</param>
    /// <param name="hashFunc">给定计算哈希值的方法（可选；默认使用 <see cref="AsSha512(byte[])"/>）。</param>
    /// <param name="encryptFunc">给定的哈希值加密方法（可选；默认使用 <see cref="AsAes(byte[])"/>）。</param>
    /// <returns>返回经过编码的字符串。</returns>
    public static string AsPasswordHash(this string plaintext, Encoding? encoding = null,
        Func<byte[], byte[]>? hashFunc = null, Func<byte[], byte[]>? encryptFunc = null)
    {
        hashFunc ??= AsSha512;
        encryptFunc ??= AsAes;

        return Dependency.LazyAlgorithmManager.Value.FluentProcess(algo =>
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
    /// <param name="encoding">给定的字符编码（可选；默认为 <see cref="Dependencies.IExtensionsDependency.Encoding"/>）。</param>
    /// <param name="hashFunc">给定计算哈希值的方法（可选；默认使用 <see cref="AsSha512(byte[])"/>）。</param>
    /// <param name="decryptFunc">给定的哈希值解密方法（可选；默认使用 <see cref="FromAes(byte[])"/>）。</param>
    /// <returns>返回密码是否相等的布尔值。</returns>
    public static bool VerifyPasswordHash(this string passwordHash, string plaintext, Encoding? encoding = null,
        Func<byte[], byte[]>? hashFunc = null, Func<byte[], byte[]>? decryptFunc = null)
    {
        hashFunc ??= AsSha512;
        decryptFunc ??= FromAes;

        return Dependency.LazyAlgorithmManager.Value.FluentProcess(algo =>
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
