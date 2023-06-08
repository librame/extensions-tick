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
/// 数字静态扩展。
/// </summary>
public static class NumberExtensions
{

    #region SubWithoutRound

    /// <summary>
    /// 截取没有四舍五入的整数与小数部分。
    /// </summary>
    /// <remarks>
    /// <para>注：虽然 float 最大支持包含整数部分在内的 9 位长度，但超出 8 位长度会出现精度丢失的情况。</para>
    /// </remarks>
    /// <param name="f">给定的单精度浮点。</param>
    /// <param name="fractDigits">要截取小数位数（可选；默认保留 2 位小数）。</param>
    /// <returns>返回单精度浮点。</returns>
    public static float SubWithoutRound(this float f, int fractDigits = 2)
    {
        // 精度默认仅支持 7 位，可使用 G9 格式符保留完整最大 9 位长度。
        var format = f.ToString("G9");

        // 使用字符串分段截取整数与小数部分再结合可最大限度保留较长精度
        var integerPart = float.Parse(format[..format.IndexOf('.')]);
        if (fractDigits < 1)
            return integerPart;

        var decimalPart = float.Parse(string.Concat("0",
            format.AsSpan(format.IndexOf('.'), fractDigits + 1)));

        // 如果精度太长，结合时可能出现小数位数不理想
        return integerPart + decimalPart;
    }

    /// <summary>
    /// 截取没有四舍五入的整数与小数部分。
    /// </summary>
    /// <remarks>
    /// <para>注：虽然 double 最大支持包含整数部分在内的 17 位长度，但超出 16 位长度会出现精度丢失的情况。</para>
    /// </remarks>
    /// <param name="d">给定的双精度浮点。</param>
    /// <param name="fractDigits">要截取小数位数（可选；默认保留 2 位小数）。</param>
    /// <returns>返回双精度浮点。</returns>
    public static double SubWithoutRound(this double d, int fractDigits = 2)
    {
        // double 最大仅支持 16 位精度
        var format = d.ToString();

        // 使用字符串分段截取整数与小数部分再结合可最大限度保留较长精度
        var integerPart = double.Parse(format[..format.IndexOf('.')]);
        if (fractDigits < 1)
            return integerPart;

        var decimalPart = double.Parse(string.Concat("0",
            format.AsSpan(format.IndexOf('.'), fractDigits + 1)));

        // 如果精度太长，结合时可能出现小数位数不理想
        return integerPart + decimalPart;
    }

    #endregion


    #region IPAddress

    /// <summary>
    /// 将 IPv4 格式字符串转为 64 位整数。
    /// </summary>
    /// <param name="iPv4String">给定的 IPv4 字符串。</param>
    /// <returns>返回 64 位整数。</returns>
    public static long AsIPv4Long(this string iPv4String)
        => IPAddress.Parse(iPv4String).AsIPv4Long();

    /// <summary>
    /// 将 IPv4 地址转为 64 位整数。
    /// </summary>
    /// <param name="iPv4">给定的 IPv4。</param>
    /// <returns>返回长整数。</returns>
    public static long AsIPv4Long(this IPAddress iPv4)
    {
        var x = 3;
        var result = 0L;

        foreach (var b in iPv4.GetAddressBytes())
        {
            result += (long)b << 8 * x--;
        }

        return result;
    }

    /// <summary>
    /// 从 64 位整数还原 IPv4 地址。
    /// </summary>
    /// <param name="iPv4Long">给定的 IPv4 64 位整数。</param>
    /// <returns>返回 <see cref="IPAddress"/>。</returns>
    public static IPAddress FromIPv4Long(this long iPv4Long)
    {
        var buffer = new byte[4];

        for (var i = 0; i < 4; i++)
        {
            buffer[i] = (byte)(iPv4Long >> 8 * (3 - i) & 255);
        }

        return new IPAddress(buffer);
    }

    /// <summary>
    /// 从 64 位整数还原 IPv4 地址。
    /// </summary>
    /// <param name="iPv4Long">给定的 IPv4 64 位整数。</param>
    /// <returns>返回 <see cref="IPAddress"/>。</returns>
    public static string FromIPv4LongAsString(this long iPv4Long)
        => iPv4Long.FromIPv4Long().ToString();


    /// <summary>
    /// 将 IPv6 格式字符串转为大整数。
    /// </summary>
    /// <param name="iPv6String">给定的 IPv6 字符串。</param>
    /// <returns>返回 <see cref="BigInteger"/>。</returns>
    public static BigInteger AsIPv6BigInteger(this string iPv6String)
        => IPAddress.Parse(iPv6String).AsIPv6BigInteger();

    /// <summary>
    /// 将 IPv6 地址转为大整数。
    /// </summary>
    /// <param name="iPv6">给定的 IPv6。</param>
    /// <returns>返回 <see cref="BigInteger"/>。</returns>
    public static BigInteger AsIPv6BigInteger(this IPAddress iPv6)
    {
        var list = iPv6.GetAddressBytes().ToList();
        list.Reverse();
        list.Add(byte.MinValue);

        return new BigInteger(list.ToArray());
    }

    /// <summary>
    /// 从大整数还原 IPv6 地址。
    /// </summary>
    /// <param name="iPv6Long">给定的 IPv6 大整数。</param>
    /// <returns>返回 <see cref="IPAddress"/>。</returns>
    public static IPAddress FromIPv6BigInteger(this BigInteger iPv6Long)
    {
        var list = iPv6Long.ToByteArray().ToList();
        list.Reverse();

        return new IPAddress(list.ToArray());
    }

    /// <summary>
    /// 从大整数还原 IPv6 格式字符串。
    /// </summary>
    /// <param name="iPv6Long">给定的 IPv6 大整数。</param>
    /// <returns>返回字符串。</returns>
    public static string FromIPv6BigIntegerAsString(this BigInteger iPv6Long)
        => iPv6Long.FromIPv6BigInteger().ToString();

    #endregion

}
