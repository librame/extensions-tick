#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Librame.Extensions
{
    /// <summary>
    /// 算法静态扩展。
    /// </summary>
    public static class AlgorithmExtensions
    {

        #region Base and Hex

        private readonly static char[] _base32Chars
            = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567".ToCharArray();


        /// <summary>
        /// 转换 BASE32 字符串。
        /// </summary>
        /// <exception cref="ArgumentException">
        /// <paramref name="bytes"/> is null or empty.
        /// </exception>
        /// <param name="bytes">给定的字节数组。</param>
        /// <returns>返回字符串。</returns>
        public static string AsBase32String(this byte[] bytes)
        {
            bytes.NotEmpty(nameof(bytes));

            var chars = _base32Chars;

            var sb = new StringBuilder();
            for (var offset = 0; offset < bytes.Length;)
            {
                var numCharsToOutput = GetNextGroup(bytes, ref offset,
                    out byte a, out byte b, out byte c, out byte d, out byte e, out byte f, out byte g, out byte h);

                sb.Append((numCharsToOutput >= 1) ? chars[a] : '=');
                sb.Append((numCharsToOutput >= 2) ? chars[b] : '=');
                sb.Append((numCharsToOutput >= 3) ? chars[c] : '=');
                sb.Append((numCharsToOutput >= 4) ? chars[d] : '=');
                sb.Append((numCharsToOutput >= 5) ? chars[e] : '=');
                sb.Append((numCharsToOutput >= 6) ? chars[f] : '=');
                sb.Append((numCharsToOutput >= 7) ? chars[g] : '=');
                sb.Append((numCharsToOutput >= 8) ? chars[h] : '=');
            }

            return sb.ToString();

            // 获取下一组字节
            int GetNextGroup(byte[] buffer, ref int offset,
                out byte a, out byte b, out byte c, out byte d, out byte e, out byte f, out byte g, out byte h)
            {
                uint b1, b2, b3, b4, b5;

                int retVal;
                switch (offset - buffer.Length)
                {
                    case 1: retVal = 2; break;
                    case 2: retVal = 4; break;
                    case 3: retVal = 5; break;
                    case 4: retVal = 7; break;
                    default: retVal = 8; break;
                }

                b1 = (offset < buffer.Length) ? buffer[offset++] : 0U;
                b2 = (offset < buffer.Length) ? buffer[offset++] : 0U;
                b3 = (offset < buffer.Length) ? buffer[offset++] : 0U;
                b4 = (offset < buffer.Length) ? buffer[offset++] : 0U;
                b5 = (offset < buffer.Length) ? buffer[offset++] : 0U;

                a = (byte)(b1 >> 3);
                b = (byte)(((b1 & 0x07) << 2) | (b2 >> 6));
                c = (byte)((b2 >> 1) & 0x1f);
                d = (byte)(((b2 & 0x01) << 4) | (b3 >> 4));
                e = (byte)(((b3 & 0x0f) << 1) | (b4 >> 7));
                f = (byte)((b4 >> 2) & 0x1f);
                g = (byte)(((b4 & 0x3) << 3) | (b5 >> 5));
                h = (byte)(b5 & 0x1f);

                return retVal;
            }
        }

        /// <summary>
        /// 还原 BASE32 字符串。
        /// </summary>
        /// <exception cref="ArgumentException">
        /// <paramref name="base32String"/> is null or empty.
        /// </exception>
        /// <param name="base32String">给定的 BASE32 字符串。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] FromBase32String(this string base32String)
        {
            base32String.NotEmpty(nameof(base32String));

            base32String = base32String.TrimEnd('=');
            if (base32String.Length == 0)
                return Array.Empty<byte>();

            if (base32String.HasLower())
                base32String = base32String.ToUpperInvariant();

            var chars = _base32Chars;

            var bytes = new byte[base32String.Length * 5 / 8];
            var bitIndex = 0;
            var inputIndex = 0;
            var outputBits = 0;
            var outputIndex = 0;

            while (outputIndex < bytes.Length)
            {
                var byteIndex = Array.IndexOf(chars, base32String[inputIndex]);
                if (byteIndex < 0)
                    throw new FormatException($"Invalid BASE32 string format '{base32String}'.");

                var bits = Math.Min(5 - bitIndex, 8 - outputBits);
                bytes[outputIndex] <<= bits;
                bytes[outputIndex] |= (byte)(byteIndex >> (5 - (bitIndex + bits)));

                bitIndex += bits;
                if (bitIndex >= 5)
                {
                    inputIndex++;
                    bitIndex = 0;
                }

                outputBits += bits;
                if (outputBits >= 8)
                {
                    outputIndex++;
                    outputBits = 0;
                }
            }

            // 因字符串强制以“\0”结尾，故需手动移除数组末尾的“0”字节，才能正确还原源数组
            bytes = bytes.TrimLast(byte.MinValue).ToArray();

            return bytes;
        }


        /// <summary>
        /// 转换 BASE64 字符串。
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
            hexString.NotEmpty(nameof(hexString));

            if (!hexString.Length.IsMultiples(2))
                throw new ArgumentException($"Invalid hex string '{hexString}'.");

            //var memory = hexString.AsMemory();
            var length = hexString.Length / 2;
            var buffer = new byte[length];

            for (int i = 0; i < length; i++)
                buffer[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16); // memory.Slice(i * 2, 2)

            return buffer;
        }

        #endregion


        #region Hash

        private static readonly Lazy<SHA256> _sha256 =
            new Lazy<SHA256>(SHA256.Create());

        private static readonly Lazy<SHA384> _sha384 =
            new Lazy<SHA384>(SHA384.Create());

        private static readonly Lazy<SHA512> _sha512 =
            new Lazy<SHA512>(SHA512.Create());


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

        private static readonly Lazy<HMAC?> _hmacSha256 =
            new Lazy<HMAC?>(HMAC.Create(nameof(HMACSHA256)));

        private static readonly Lazy<HMAC?> _hmacSha384 =
            new Lazy<HMAC?>(HMAC.Create(nameof(HMACSHA384)));

        private static readonly Lazy<HMAC?> _hmacSha512 =
            new Lazy<HMAC?>(HMAC.Create(nameof(HMACSHA512)));


        /// <summary>
        /// 计算 HMACSHA256 哈希值，并返回 BASE64 字符串形式。
        /// </summary>
        /// <param name="plaintext">给定的明文。</param>
        /// <param name="key">给定的密钥（可选）。</param>
        /// <param name="encoding">给定的 <see cref="Encoding"/>（可选；默认使用 <see cref="EncodingExtensions.UTF8Encoding"/>）。</param>
        /// <returns>返回经过 BASE64 编码的加密字符串。</returns>
        public static string AsHmacSha256Base64String(this string plaintext,
            byte[]? key = null, Encoding? encoding = null)
            => plaintext.FromEncodingString(encoding).AsHmacSha256(key).AsBase64String();

        /// <summary>
        /// 计算 HMACSHA384 哈希值，并返回 BASE64 字符串形式。
        /// </summary>
        /// <param name="plaintext">给定的明文。</param>
        /// <param name="key">给定的密钥（可选）。</param>
        /// <param name="encoding">给定的 <see cref="Encoding"/>（可选；默认使用 <see cref="EncodingExtensions.UTF8Encoding"/>）。</param>
        /// <returns>返回经过 BASE64 编码的加密字符串。</returns>
        public static string AsHmacSha384Base64String(this string plaintext,
            byte[]? key = null, Encoding? encoding = null)
            => plaintext.FromEncodingString(encoding).AsHmacSha384(key).AsBase64String();

        /// <summary>
        /// 计算 HMACSHA512 哈希值，并返回 BASE64 字符串形式。
        /// </summary>
        /// <param name="plaintext">给定的明文。</param>
        /// <param name="key">给定的密钥（可选）。</param>
        /// <param name="encoding">给定的 <see cref="Encoding"/>（可选；默认使用 <see cref="EncodingExtensions.UTF8Encoding"/>）。</param>
        /// <returns>返回经过 BASE64 编码的加密字符串。</returns>
        public static string AsHmacSha512Base64String(this string plaintext,
            byte[]? key = null, Encoding? encoding = null)
            => plaintext.FromEncodingString(encoding).AsHmacSha512(key).AsBase64String();


        /// <summary>
        /// 计算 HMACSHA256 哈希值。
        /// </summary>
        /// <param name="buffer">给定要计算的字节数组。</param>
        /// <param name="key">给定的密钥（可选）。</param>
        /// <returns>返回经过计算的字节数组。</returns>
        public static byte[] AsHmacSha256(this byte[] buffer, byte[]? key = null)
        {
            return RunHmacSha256(hmac =>
            {
                if (key.IsNotEmpty())
                    hmac.Key = key;

                return hmac.ComputeHash(buffer);
            });
        }

        /// <summary>
        /// 计算 HMACSHA384 哈希值。
        /// </summary>
        /// <param name="buffer">给定要计算的字节数组。</param>
        /// <param name="key">给定的密钥（可选）。</param>
        /// <returns>返回经过计算的字节数组。</returns>
        public static byte[] AsHmacSha384(this byte[] buffer, byte[]? key = null)
        {
            return RunHmacSha384(hmac =>
            {
                if (key.IsNotEmpty())
                    hmac.Key = key;

                return hmac.ComputeHash(buffer);
            });
        }

        /// <summary>
        /// 计算 HMACSHA512 哈希值。
        /// </summary>
        /// <param name="buffer">给定要计算的字节数组。</param>
        /// <param name="key">给定的密钥（可选）。</param>
        /// <returns>返回经过计算的字节数组。</returns>
        public static byte[] AsHmacSha512(this byte[] buffer, byte[]? key = null)
        {
            return RunHmacSha512(hmac =>
            {
                if (key.IsNotEmpty())
                    hmac.Key = key;

                return hmac.ComputeHash(buffer);
            });
        }


        /// <summary>
        /// 获取 HMACSHA256 哈希密钥。
        /// </summary>
        /// <returns>返回包含密钥和向量的元组。</returns>
        public static byte[] GetHmacSha256Key()
            => RunHmacSha256(hmac => hmac.Key);

        /// <summary>
        /// 获取 HMACSHA384 哈希密钥。
        /// </summary>
        /// <returns>返回包含密钥和向量的元组。</returns>
        public static byte[] GetHmacSha384Key()
            => RunHmacSha384(hmac => hmac.Key);

        /// <summary>
        /// 获取 HMACSHA512 哈希密钥。
        /// </summary>
        /// <returns>返回包含密钥和向量的元组。</returns>
        public static byte[] GetHmacSha512Key()
            => RunHmacSha512(hmac => hmac.Key);


        /// <summary>
        /// 运行 HMACSHA256 哈希。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="action"/> is null.
        /// </exception>
        /// <param name="action">给定的动作。</param>
        public static void RunHmacSha256(this Action<HMAC> action)
            => action.NotNull(nameof(action)).Invoke(GetHmacHash(nameof(HashAlgorithmName.SHA256)));

        /// <summary>
        /// 运行 HMACSHA256 哈希，并返回值。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="valueFunc"/> is null.
        /// </exception>
        /// <typeparam name="TValue">指定的值类型。</typeparam>
        /// <param name="valueFunc">给定的值方法。</param>
        /// <returns>返回 <typeparamref name="TValue"/>。</returns>
        public static TValue RunHmacSha256<TValue>(this Func<HMAC, TValue> valueFunc)
            => valueFunc.NotNull(nameof(valueFunc)).Invoke(GetHmacHash(nameof(HashAlgorithmName.SHA256)));


        /// <summary>
        /// 运行 HMACSHA384 哈希。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="action"/> is null.
        /// </exception>
        /// <param name="action">给定的动作。</param>
        public static void RunHmacSha384(this Action<HMAC> action)
            => action.NotNull(nameof(action)).Invoke(GetHmacHash(nameof(HashAlgorithmName.SHA384)));

        /// <summary>
        /// 运行 HMACSHA384 哈希，并返回值。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="valueFunc"/> is null.
        /// </exception>
        /// <typeparam name="TValue">指定的值类型。</typeparam>
        /// <param name="valueFunc">给定的值方法。</param>
        /// <returns>返回 <typeparamref name="TValue"/>。</returns>
        public static TValue RunHmacSha384<TValue>(this Func<HMAC, TValue> valueFunc)
            => valueFunc.NotNull(nameof(valueFunc)).Invoke(GetHmacHash(nameof(HashAlgorithmName.SHA384)));


        /// <summary>
        /// 运行 HMACSHA512 哈希。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="action"/> is null.
        /// </exception>
        /// <param name="action">给定的动作。</param>
        public static void RunHmacSha512(this Action<HMAC> action)
            => action.NotNull(nameof(action)).Invoke(GetHmacHash(nameof(HashAlgorithmName.SHA512)));

        /// <summary>
        /// 运行 HMACSHA512 哈希，并返回值。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="valueFunc"/> is null.
        /// </exception>
        /// <typeparam name="TValue">指定的值类型。</typeparam>
        /// <param name="valueFunc">给定的值方法。</param>
        /// <returns>返回 <typeparamref name="TValue"/>。</returns>
        public static TValue RunHmacSha512<TValue>(this Func<HMAC, TValue> valueFunc)
            => valueFunc.NotNull(nameof(valueFunc)).Invoke(GetHmacHash(nameof(HashAlgorithmName.SHA512)));


        private static HMAC GetHmacHash(string hashName)
        {
            return hashName switch
            {
                nameof(HashAlgorithmName.SHA256) => _hmacSha256.Value!,
                nameof(HashAlgorithmName.SHA384) => _hmacSha384.Value!,
                nameof(HashAlgorithmName.SHA512) => _hmacSha512.Value!,
                _ => throw new NotSupportedException($"Not supported algorithm name '{hashName}'")
            };
        }

        #endregion


        #region AES

        private static readonly Lazy<Aes> _aes =
            new Lazy<Aes>(() => Aes.Create());


        /// <summary>
        /// AES 加密 BASE64 字符串形式。
        /// </summary>
        /// <param name="plaintext">给定的明文。</param>
        /// <param name="key">给定的密钥（可选）。</param>
        /// <param name="iv">给定的向量（可选）。</param>
        /// <param name="encoding">给定的 <see cref="Encoding"/>（可选；默认使用 <see cref="EncodingExtensions.UTF8Encoding"/>）。</param>
        /// <returns>返回经过 BASE64 编码的加密字符串。</returns>
        public static string AsAesWithBase64String(this string plaintext, byte[]? key = null,
            byte[]? iv = null, Encoding? encoding = null)
            => plaintext.FromEncodingString(encoding).AsAes(key, iv).AsBase64String();

        /// <summary>
        /// AES 解密 BASE64 字符串形式。
        /// </summary>
        /// <param name="ciphertext">给定的密文。</param>
        /// <param name="key">给定的密钥（可选）。</param>
        /// <param name="iv">给定的向量（可选）。</param>
        /// <param name="encoding">给定的 <see cref="Encoding"/>（可选；默认使用 <see cref="EncodingExtensions.UTF8Encoding"/>）。</param>
        /// <returns>返回经过 BASE64 解码的解密字符串。</returns>
        public static string FromAesWithBase64String(this string ciphertext, byte[]? key = null,
            byte[]? iv = null, Encoding? encoding = null)
            => ciphertext.FromBase64String().FromAes(key, iv).AsEncodingString(encoding);


        /// <summary>
        /// AES 加密。
        /// </summary>
        /// <param name="plaintext">给定的明文。</param>
        /// <param name="key">给定的密钥（可选）。</param>
        /// <param name="iv">给定的向量（可选）。</param>
        /// <returns>返回经过加密的字节数组。</returns>
        public static byte[] AsAes(this byte[] plaintext, byte[]? key = null, byte[]? iv = null)
        {
            return RunAes(aes =>
            {
                if (key.IsNotEmpty())
                    aes.Key = key;

                if (iv.IsNotEmpty())
                    aes.IV = iv;

                var transform = aes.CreateEncryptor();
                return transform.TransformFinalBlock(plaintext, 0, plaintext.Length);
            });
        }

        /// <summary>
        /// AES 解密。
        /// </summary>
        /// <param name="ciphertext">给定的密文。</param>
        /// <param name="key">给定的密钥（可选）。</param>
        /// <param name="iv">给定的向量（可选）。</param>
        /// <returns>返回经过解密的字节数组。</returns>
        public static byte[] FromAes(this byte[] ciphertext, byte[]? key = null, byte[]? iv = null)
        {
            return RunAes(aes =>
            {
                if (key.IsNotEmpty())
                    aes.Key = key;

                if (iv.IsNotEmpty())
                    aes.IV = iv;

                var transform = aes.CreateDecryptor();
                return transform.TransformFinalBlock(ciphertext, 0, ciphertext.Length);
            });
        }


        /// <summary>
        /// 获取 AES 密钥和向量。
        /// </summary>
        /// <returns>返回包含密钥和向量的元组。</returns>
        public static (byte[] Key, byte[] IV) GetAesKeyAndIV()
            => RunAes(aes => (aes.Key, aes.IV));


        /// <summary>
        /// 运行 AES。
        /// </summary>
        /// <param name="action">给定的动作。</param>
        public static void RunAes(this Action<Aes> action)
            => action.NotNull(nameof(action)).Invoke(_aes.Value);

        /// <summary>
        /// 运行 AES，并返回值。
        /// </summary>
        /// <typeparam name="TValue">指定的值类型。</typeparam>
        /// <param name="valueFunc">给定的值方法。</param>
        /// <returns>返回 <typeparamref name="TValue"/>。</returns>
        public static TValue RunAes<TValue>(this Func<Aes, TValue> valueFunc)
            => valueFunc.NotNull(nameof(valueFunc)).Invoke(_aes.Value);

        #endregion

    }
}
