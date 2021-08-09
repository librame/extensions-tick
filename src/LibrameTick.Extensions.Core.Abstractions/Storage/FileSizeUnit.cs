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

namespace Librame.Extensions.Core.Storage
{
    /// <summary>
    /// 定义表示文件大小单位的枚举。
    /// </summary>
    public enum FileSizeUnit
    {
        /// <summary>
        /// 字节。
        /// </summary>
        [Description("字节")]
        Byte = 0,

        /// <summary>
        /// 千字节。
        /// </summary>
        [Description("千字节")]
        KiloByte,

        /// <summary>
        /// 兆字节。
        /// </summary>
        [Description("兆字节")]
        MegaByte,

        /// <summary>
        /// 吉字节。
        /// </summary>
        [Description("吉字节")]
        GigaByte,

        /// <summary>
        /// 太字节。
        /// </summary>
        [Description("太字节")]
        TeraByte,

        /// <summary>
        /// 拍字节。
        /// </summary>
        [Description("拍字节")]
        PetaByte,

        /// <summary>
        /// 艾字节。
        /// </summary>
        [Description("艾字节")]
        ExaByte,

        /// <summary>
        /// 泽字节。
        /// </summary>
        [Description("泽字节")]
        ZettaByte,

        /// <summary>
        /// 尧字节。
        /// </summary>
        [Description("尧字节")]
        YottaByte,

        /// <summary>
        /// 波字节。
        /// </summary>
        [Description("波字节")]
        BrontoByte
    }
}
