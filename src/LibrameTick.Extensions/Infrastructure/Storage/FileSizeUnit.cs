#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Infrastructure.Storage;

/// <summary>
/// 定义表示文件大小单位的枚举。
/// </summary>
public enum FileSizeUnit
{
    /// <summary>
    /// 字节。
    /// </summary>
    [FileSizeDescription("字节", "Byte", "Byte", FileSizeSystem.None, exponent: 0)]
    Byte = 0,

    /// <summary>
    /// 千字节。
    /// </summary>
    [FileSizeDescription("千字节", "KibiByte", "KiB", FileSizeSystem.Binary, exponent: 10)]
    [FileSizeDescription("千字节", "KiloByte", "KB", FileSizeSystem.Decimal, exponent: 3)]
    KiByte,

    /// <summary>
    /// 兆字节。
    /// </summary>
    [FileSizeDescription("兆字节", "MebiByte", "MiB", FileSizeSystem.Binary, exponent: 20)]
    [FileSizeDescription("兆字节", "MegaByte", "MB", FileSizeSystem.Decimal, exponent: 6)]
    MeByte,

    /// <summary>
    /// 吉字节。
    /// </summary>
    [FileSizeDescription("吉字节", "GibiByte", "GiB", FileSizeSystem.Binary, exponent: 30)]
    [FileSizeDescription("吉字节", "GigaByte", "GB", FileSizeSystem.Decimal, exponent: 9)]
    GiByte,

    /// <summary>
    /// 太字节。
    /// </summary>
    [FileSizeDescription("太字节", "TebiByte", "TiB", FileSizeSystem.Binary, exponent: 40)]
    [FileSizeDescription("太字节", "TeraByte", "TB", FileSizeSystem.Decimal, exponent: 12)]
    TeByte,

    /// <summary>
    /// 拍字节。
    /// </summary>
    [FileSizeDescription("拍字节", "PebiByte", "PiB", FileSizeSystem.Binary, exponent: 50)]
    [FileSizeDescription("拍字节", "PetaByte", "PB", FileSizeSystem.Decimal, exponent: 15)]
    PeByte,

    /// <summary>
    /// 艾字节。
    /// </summary>
    [FileSizeDescription("艾字节", "ExbiByte", "EiB", FileSizeSystem.Binary, exponent: 60)]
    [FileSizeDescription("艾字节", "ExaByte", "EB", FileSizeSystem.Decimal, exponent: 18)]
    ExByte,

    /// <summary>
    /// 泽字节。
    /// </summary>
    [FileSizeDescription("泽字节", "ZebiByte", "ZiB", FileSizeSystem.Binary, exponent: 70)]
    [FileSizeDescription("泽字节", "ZettaByte", "ZB", FileSizeSystem.Decimal, exponent: 21)]
    ZeByte,

    /// <summary>
    /// 尧字节。
    /// </summary>
    [FileSizeDescription("尧字节", "YobiByte", "YiB", FileSizeSystem.Binary, exponent: 80)]
    [FileSizeDescription("尧字节", "YottaByte", "YB", FileSizeSystem.Decimal, exponent: 24)]
    YoByte
}
