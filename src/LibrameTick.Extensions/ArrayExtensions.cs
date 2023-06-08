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
/// 数组静态扩展。
/// </summary>
public static class ArrayExtensions
{

    /// <summary>
    /// 租用字节数组。
    /// </summary>
    /// <param name="length">给定的数组长度。</param>
    /// <returns></returns>
    public static byte[] RentByteArray(this int length)
        => length.RentArray<byte>();

    ///// <summary>
    ///// 归还字节数组。
    ///// </summary>
    ///// <param name="buffer">给定的字节数组。</param>
    ///// <param name="clearArray">是否清除数组内容（可选；默认清除）。</param>
    //public static void ReturnByteArray(this byte[] buffer, bool clearArray = true)
    //    => buffer.ReturnArray(clearArray);


    /// <summary>
    /// 租用数组。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    /// <param name="length">给定的数组长度。</param>
    /// <returns>返回 <typeparamref name="T"/> 数组。</returns>
    public static T[] RentArray<T>(this int length)
        => ArrayPool<T>.Shared.Rent(length);

    /// <summary>
    /// 归还数组。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    /// <param name="buffer">给定的 <typeparamref name="T"/> 数组。</param>
    /// <param name="clearArray">是否清除数组内容（可选；默认清除）。</param>
    public static void ReturnArray<T>(this T[] buffer, bool clearArray = true)
        => ArrayPool<T>.Shared.Return(buffer, clearArray);

}
