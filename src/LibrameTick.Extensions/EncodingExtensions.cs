#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Text;

namespace Librame.Extensions
{
    /// <summary>
    /// <see cref="Encoding"/> 静态扩展。
    /// </summary>
    public static class EncodingExtensions
    {
        /// <summary>
        /// <see cref="Encoding.UTF8"/> 字符编码。
        /// </summary>
        public static readonly Encoding UTF8Encoding = Encoding.UTF8;


        /// <summary>
        /// 将字符编码转换为名称的字符串形式。
        /// </summary>
        /// <param name="encoding">给定的字符编码。</param>
        /// <returns>返回代码页名称。</returns>
        public static string AsEncodingName(this Encoding encoding)
            => encoding.WebName;

        /// <summary>
        /// 从名称的字符串形式还原字符编码。
        /// </summary>
        /// <param name="name">首选编码的代码页名称。</param>
        /// <returns>返回与指定代码页关联的编码。</returns>
        public static Encoding FromEncodingName(this string name)
            => Encoding.GetEncoding(name);


        /// <summary>
        /// 转换为字符编码的字符串。
        /// </summary>
        /// <param name="bytes">给定的字节数组。</param>
        /// <param name="encoding">给定的 <see cref="Encoding"/>（可选；默认为 <see cref="UTF8Encoding"/>）。</param>
        /// <returns>返回字符串。</returns>
        public static string AsEncodingString(this byte[] bytes, Encoding? encoding = null)
            => (encoding ?? UTF8Encoding).GetString(bytes);

        /// <summary>
        /// 还原为字符编码的字节数组。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <param name="encoding">给定的 <see cref="Encoding"/>（可选；默认为 <see cref="UTF8Encoding"/>）。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] FromEncodingString(this string str, Encoding? encoding = null)
            => (encoding ?? UTF8Encoding).GetBytes(str);

    }
}
