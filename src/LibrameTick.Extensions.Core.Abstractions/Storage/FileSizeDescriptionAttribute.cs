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

namespace Librame.Extensions.Core.Storage
{
    /// <summary>
    /// 定义一个表示文件大小的描述符特性。
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class FileSizeDescriptionAttribute : Attribute
    {
        /// <summary>
        /// 描述。
        /// </summary>
        public string Description { get; init; }

        /// <summary>
        /// 名称（如：KibiByte/KiloByte）。
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// 简称（如：KiB/KB）。
        /// </summary>
        public string AbbrName { get; init; }

        /// <summary>
        /// 大小。
        /// </summary>
        public long Size { get; init; }
    }
}
