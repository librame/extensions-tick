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
using System.IO;
using System.Text;

namespace Librame.Extensions
{
    /// <summary>
    /// 静态扩展默认值。
    /// </summary>
    public static class ExtensionDefaults
    {
        /// <summary>
        /// 当前字符编码（默认为 <see cref="Encoding.UTF8"/>）。
        /// </summary>
        public static readonly Encoding CurrentEncoding
            = Encoding.UTF8;

        /// <summary>
        /// 当前路径分隔符（默认为 <see cref="Path.DirectorySeparatorChar"/>）。
        /// </summary>
        public static readonly char CurrentPathSeparator
            = Path.DirectorySeparatorChar;

        /// <summary>
        /// 当前路径分隔符字符串形式。
        /// </summary>
        public static readonly string CurrentPathSeparatorString
            = CurrentPathSeparator.ToString();

        /// <summary>
        /// 当前目录（默认为 <see cref="Directory.GetCurrentDirectory()"/>）。
        /// </summary>
        public static readonly string CurrentDirectory
            = Directory.GetCurrentDirectory();


        /// <summary>
        /// 可空泛类型定义。
        /// </summary>
        public static readonly Type NullableGenericTypeDefinition
            = typeof(Nullable<>);

    }
}
