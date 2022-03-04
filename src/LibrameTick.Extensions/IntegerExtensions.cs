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
/// 整数静态扩展。
/// </summary>
public static class IntegerExtensions
{

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
