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
/// <see cref="byte"/> 静态扩展。
/// </summary>
public static class ByteExtensions
{

    #region Base32String

    private readonly static char[] _base32Chars
        = (StringExtensions.UppercaseLetters + "234567").ToCharArray();


    /// <summary>
    /// 转换 BASE32 字符串。
    /// </summary>
    /// <param name="bytes">给定的字节数组。</param>
    /// <returns>返回字符串。</returns>
    public static string AsBase32String(this byte[] bytes)
    {
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
        static int GetNextGroup(byte[] buffer, ref int offset,
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
    /// <param name="base32String">给定的 BASE32 字符串。</param>
    /// <returns>返回字节数组。</returns>
    public static byte[] FromBase32String(this string base32String)
    {
        base32String = base32String.TrimEnd('=');
        if (base32String.Length is 0)
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

    #endregion


    #region Base64String

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

    #endregion


    #region HexString

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


    #region ImageBase64String

    /// <summary>
    /// 将图像字节数组转换为 BASE64 字符串形式。
    /// </summary>
    /// <param name="buffer">给定的图像字节数组。</param>
    /// <param name="imageType">给定的图像类型（如：image/jpeg）。</param>
    /// <returns>返回字符串。</returns>
    public static string AsImageBase64String(this byte[] buffer, string imageType)
        => $"data:{imageType};base64,{buffer.AsBase64String()}";

    /// <summary>
    /// 从 BASE64 字符串还原图像类型与字节数组。
    /// </summary>
    /// <param name="base64String">给定的图像 BASE64 字符串。</param>
    /// <returns>返回包含图像类型与字节数组的元组。</returns>
    /// <exception cref="ArgumentException">
    /// Invalid image base64 format string <paramref name="base64String"/>.
    /// </exception>
    public static (string imageType, byte[] buffer) FromImageBase64String(string base64String)
    {
        if (!base64String.TrySplitPair(';', out var pair))
            throw new ArgumentException($"Invalid image base64 format string '{base64String}'.");

        return (pair.Key.TrimStart("data:"), pair.Value.TrimStart("base64,").FromBase64String());
    }

    #endregion

}
