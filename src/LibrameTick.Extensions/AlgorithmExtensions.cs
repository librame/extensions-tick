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
    private static readonly AlgorithmKeys _keys;


    static AlgorithmExtensions()
    {
        if (_keys is null)
            _keys = AlgorithmKeys.LoadOrCreateFile();
    }


    #region AlgorithmKeys

    class AlgorithmKeys
    {
        public byte[]? Id { get; set; }

        public byte[]? HmacMd5Key { get; set; }

        public byte[]? HmacSha256Key { get; set; }

        public byte[]? HmacSha384Key { get; set; }

        public byte[]? HmacSha512Key { get; set; }

        public byte[]? AesKey { get; set; }

        public byte[]? AesIV { get; set; }


        public static AlgorithmKeys LoadOrCreateFile()
        {
            // bin 目录禁止访问
            var keyFile = $"{typeof(AlgorithmExtensions).GetAssemblyName()}.keys"
                .SetBasePath(PathExtensions.CurrentDirectory);
            if (!keyFile.FileExists())
            {
                var keys = new AlgorithmKeys
                {
                    Id = RandomExtensions.GenerateByteArray(16),
                    HmacMd5Key = RandomExtensions.GenerateByteArray(8),
                    HmacSha256Key = RandomExtensions.GenerateByteArray(8),
                    HmacSha384Key = RandomExtensions.GenerateByteArray(16),
                    HmacSha512Key = RandomExtensions.GenerateByteArray(16),
                    AesKey = RandomExtensions.GenerateByteArray(32),
                    AesIV = RandomExtensions.GenerateByteArray(16)
                };

                keyFile.SerializeJsonFile(keys);

                return keys;
            }
            else
            {
                var keys = keyFile.DeserializeJsonFile<AlgorithmKeys>();
                if (keys is null)
                    throw new InvalidOperationException($"The key file '{keyFile}' format is error.");

                return keys;
            }
        }

    }

    #endregion


    #region Hash

    private static readonly Lazy<MD5> _md5 =
        new Lazy<MD5>(MD5.Create());

    private static readonly Lazy<SHA256> _sha256 =
        new Lazy<SHA256>(SHA256.Create());

    private static readonly Lazy<SHA384> _sha384 =
        new Lazy<SHA384>(SHA384.Create());

    private static readonly Lazy<SHA512> _sha512 =
        new Lazy<SHA512>(SHA512.Create());


    /// <summary>
    /// 计算 MD5 哈希值，并返回 BASE64 字符串形式。
    /// </summary>
    /// <param name="plaintext">给定的明文。</param>
    /// <param name="encoding">给定的 <see cref="Encoding"/>（可选；默认使用 <see cref="EncodingExtensions.UTF8Encoding"/>）。</param>
    /// <returns>返回经过 BASE64 编码的加密字符串。</returns>
    public static string AsMd5Base64String(this string plaintext, Encoding? encoding = null)
        => plaintext.FromEncodingString(encoding).AsMd5().AsBase64String();

    /// <summary>
    /// 计算 SHA256 哈希值，并返回 BASE64 字符串形式。
    /// </summary>
    /// <param name="plaintext">给定的明文。</param>
    /// <param name="encoding">给定的 <see cref="Encoding"/>（可选；默认使用 <see cref="EncodingExtensions.UTF8Encoding"/>）。</param>
    /// <returns>返回经过 BASE64 编码的加密字符串。</returns>
    public static string AsSha256Base64String(this string plaintext, Encoding? encoding = null)
        => plaintext.FromEncodingString(encoding).AsSha256().AsBase64String();

    /// <summary>
    /// 计算 SHA384 哈希值，并返回 BASE64 字符串形式。
    /// </summary>
    /// <param name="plaintext">给定的明文。</param>
    /// <param name="encoding">给定的 <see cref="Encoding"/>（可选；默认使用 <see cref="EncodingExtensions.UTF8Encoding"/>）。</param>
    /// <returns>返回经过 BASE64 编码的加密字符串。</returns>
    public static string AsSha384Base64String(this string plaintext, Encoding? encoding = null)
        => plaintext.FromEncodingString(encoding).AsSha384().AsBase64String();

    /// <summary>
    /// 计算 SHA512 哈希值，并返回 BASE64 字符串形式。
    /// </summary>
    /// <param name="plaintext">给定的明文。</param>
    /// <param name="encoding">给定的 <see cref="Encoding"/>（可选；默认使用 <see cref="EncodingExtensions.UTF8Encoding"/>）。</param>
    /// <returns>返回经过 BASE64 编码的加密字符串。</returns>
    public static string AsSha512Base64String(this string plaintext, Encoding? encoding = null)
        => plaintext.FromEncodingString(encoding).AsSha512().AsBase64String();


    /// <summary>
    /// 计算 MD5 哈希值。
    /// </summary>
    /// <param name="buffer">给定要计算的字节数组。</param>
    /// <returns>返回经过计算的字节数组。</returns>
    public static byte[] AsMd5(this byte[] buffer)
        => _md5.Value.ComputeHash(buffer);

    /// <summary>
    /// 计算 SHA256 哈希值。
    /// </summary>
    /// <param name="buffer">给定要计算的字节数组。</param>
    /// <returns>返回经过计算的字节数组。</returns>
    public static byte[] AsSha256(this byte[] buffer)
        => _sha256.Value.ComputeHash(buffer);

    /// <summary>
    /// 计算 SHA384 哈希值。
    /// </summary>
    /// <param name="buffer">给定要计算的字节数组。</param>
    /// <returns>返回经过计算的字节数组。</returns>
    public static byte[] AsSha384(this byte[] buffer)
        => _sha384.Value.ComputeHash(buffer);

    /// <summary>
    /// 计算 SHA512 哈希值。
    /// </summary>
    /// <param name="buffer">给定要计算的字节数组。</param>
    /// <returns>返回经过计算的字节数组。</returns>
    public static byte[] AsSha512(this byte[] buffer)
        => _sha512.Value.ComputeHash(buffer);

    #endregion


    #region HMAC Hash

    private static readonly Lazy<HMACMD5> _hmacMd5 =
        new Lazy<HMACMD5>(InitialHmacMd5);

    private static readonly Lazy<HMACSHA256> _hmacSha256 =
        new Lazy<HMACSHA256>(InitialHmacSha256);

    private static readonly Lazy<HMACSHA384> _hmacSha384 =
        new Lazy<HMACSHA384>(InitialHmacSha384);

    private static readonly Lazy<HMACSHA512> _hmacSha512 =
        new Lazy<HMACSHA512>(InitialHmacSha512);


    private static HMACMD5 InitialHmacMd5()
        => new HMACMD5(_keys.HmacMd5Key!);

    private static HMACSHA256 InitialHmacSha256()
        => new HMACSHA256(_keys.HmacSha256Key!);

    private static HMACSHA384 InitialHmacSha384()
        => new HMACSHA384(_keys.HmacSha384Key!);

    private static HMACSHA512 InitialHmacSha512()
        => new HMACSHA512(_keys.HmacSha512Key!);


    /// <summary>
    /// 计算 HMACMD5 哈希值，并返回 BASE64 字符串形式。
    /// </summary>
    /// <param name="plaintext">给定的明文。</param>
    /// <param name="encoding">给定的 <see cref="Encoding"/>（可选；默认使用 <see cref="EncodingExtensions.UTF8Encoding"/>）。</param>
    /// <returns>返回经过 BASE64 编码的加密字符串。</returns>
    public static string AsHmacMd5Base64String(this string plaintext,
        Encoding? encoding = null)
        => plaintext.FromEncodingString(encoding).AsHmacMd5().AsBase64String();

    /// <summary>
    /// 计算 HMACSHA256 哈希值，并返回 BASE64 字符串形式。
    /// </summary>
    /// <param name="plaintext">给定的明文。</param>
    /// <param name="encoding">给定的 <see cref="Encoding"/>（可选；默认使用 <see cref="EncodingExtensions.UTF8Encoding"/>）。</param>
    /// <returns>返回经过 BASE64 编码的加密字符串。</returns>
    public static string AsHmacSha256Base64String(this string plaintext,
        Encoding? encoding = null)
        => plaintext.FromEncodingString(encoding).AsHmacSha256().AsBase64String();

    /// <summary>
    /// 计算 HMACSHA384 哈希值，并返回 BASE64 字符串形式。
    /// </summary>
    /// <param name="plaintext">给定的明文。</param>
    /// <param name="encoding">给定的 <see cref="Encoding"/>（可选；默认使用 <see cref="EncodingExtensions.UTF8Encoding"/>）。</param>
    /// <returns>返回经过 BASE64 编码的加密字符串。</returns>
    public static string AsHmacSha384Base64String(this string plaintext,
        Encoding? encoding = null)
        => plaintext.FromEncodingString(encoding).AsHmacSha384().AsBase64String();

    /// <summary>
    /// 计算 HMACSHA512 哈希值，并返回 BASE64 字符串形式。
    /// </summary>
    /// <param name="plaintext">给定的明文。</param>
    /// <param name="encoding">给定的 <see cref="Encoding"/>（可选；默认使用 <see cref="EncodingExtensions.UTF8Encoding"/>）。</param>
    /// <returns>返回经过 BASE64 编码的加密字符串。</returns>
    public static string AsHmacSha512Base64String(this string plaintext,
        Encoding? encoding = null)
        => plaintext.FromEncodingString(encoding).AsHmacSha512().AsBase64String();


    /// <summary>
    /// 计算 HMACMD5 哈希值。
    /// </summary>
    /// <param name="buffer">给定要计算的字节数组。</param>
    /// <returns>返回经过计算的字节数组。</returns>
    public static byte[] AsHmacMd5(this byte[] buffer)
        => _hmacMd5.Value.ComputeHash(buffer);

    /// <summary>
    /// 计算 HMACSHA256 哈希值。
    /// </summary>
    /// <param name="buffer">给定要计算的字节数组。</param>
    /// <returns>返回经过计算的字节数组。</returns>
    public static byte[] AsHmacSha256(this byte[] buffer)
        => _hmacSha256.Value.ComputeHash(buffer);

    /// <summary>
    /// 计算 HMACSHA384 哈希值。
    /// </summary>
    /// <param name="buffer">给定要计算的字节数组。</param>
    /// <returns>返回经过计算的字节数组。</returns>
    public static byte[] AsHmacSha384(this byte[] buffer)
        => _hmacSha384.Value.ComputeHash(buffer);

    /// <summary>
    /// 计算 HMACSHA512 哈希值。
    /// </summary>
    /// <param name="buffer">给定要计算的字节数组。</param>
    /// <returns>返回经过计算的字节数组。</returns>
    public static byte[] AsHmacSha512(this byte[] buffer)
        => _hmacSha512.Value.ComputeHash(buffer);


    /// <summary>
    /// 获取 HMACMD5 哈希密钥。
    /// </summary>
    /// <returns>返回包含密钥和向量的元组。</returns>
    public static byte[] GetHmacMd5Key()
        => _hmacMd5.Value.Key;

    /// <summary>
    /// 获取 HMACSHA256 哈希密钥。
    /// </summary>
    /// <returns>返回包含密钥和向量的元组。</returns>
    public static byte[] GetHmacSha256Key()
        => _hmacSha256.Value.Key;

    /// <summary>
    /// 获取 HMACSHA384 哈希密钥。
    /// </summary>
    /// <returns>返回包含密钥和向量的元组。</returns>
    public static byte[] GetHmacSha384Key()
        => _hmacSha384.Value.Key;

    /// <summary>
    /// 获取 HMACSHA512 哈希密钥。
    /// </summary>
    /// <returns>返回包含密钥和向量的元组。</returns>
    public static byte[] GetHmacSha512Key()
        => _hmacSha512.Value.Key;

    #endregion


    #region AES

    private static readonly Lazy<Aes> _aes =
        new Lazy<Aes>(InitialAes);

    private static Aes InitialAes()
    {
        var aes = Aes.Create();

        aes.Key = _keys.AesKey!;
        aes.IV = _keys.AesIV!;

        return aes;
    }


    /// <summary>
    /// AES 加密 BASE64 字符串形式。
    /// </summary>
    /// <param name="plaintext">给定的明文。</param>
    /// <param name="encoding">给定的 <see cref="Encoding"/>（可选；默认使用 <see cref="EncodingExtensions.UTF8Encoding"/>）。</param>
    /// <returns>返回经过 BASE64 编码的加密字符串。</returns>
    public static string AsAesWithBase64String(this string plaintext, Encoding? encoding = null)
        => plaintext.FromEncodingString(encoding).AsAes().AsBase64String();

    /// <summary>
    /// AES 解密 BASE64 字符串形式。
    /// </summary>
    /// <param name="ciphertext">给定的密文。</param>
    /// <param name="encoding">给定的 <see cref="Encoding"/>（可选；默认使用 <see cref="EncodingExtensions.UTF8Encoding"/>）。</param>
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
        var transform = _aes.Value.CreateEncryptor();
        return transform.TransformFinalBlock(plaintext, 0, plaintext.Length);
    }

    /// <summary>
    /// AES 解密。
    /// </summary>
    /// <param name="ciphertext">给定的密文。</param>
    /// <returns>返回经过解密的字节数组。</returns>
    public static byte[] FromAes(this byte[] ciphertext)
    {
        var transform = _aes.Value.CreateDecryptor();
        return transform.TransformFinalBlock(ciphertext, 0, ciphertext.Length);
    }


    /// <summary>
    /// 获取 AES 密钥和向量。
    /// </summary>
    /// <returns>返回包含密钥和向量的元组。</returns>
    public static (byte[] key, byte[] iv) GetAesKeyAndIV()
        => (_aes.Value.Key, _aes.Value.IV);

    #endregion

}
