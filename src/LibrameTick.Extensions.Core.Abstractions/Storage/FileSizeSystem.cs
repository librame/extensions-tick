#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.ComponentModel;

namespace Librame.Extensions.Storage.Capacities
{
    /// <summary>
    /// 定义表示文件大小的进制。
    /// </summary>
    public enum FileSizeSystem
    {
        /// <summary>
        /// 二进制。
        /// </summary>
        [Description("二进制")]
        Binary = 2,

        /// <summary>
        /// 十进制。
        /// </summary>
        [Description("十进制")]
        Decimal = 10
    }
}
