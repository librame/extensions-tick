#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core;

/// <summary>
/// 表示时间精度的枚举（常量值为单位换算）。
/// </summary>
public enum TemporalAccuracy : long
{
    /// <summary>
    /// 小时（h）。
    /// </summary>
    Hour = 1,

    /// <summary>
    /// 分（m）。
    /// </summary>
    Minute = 60,

    /// <summary>
    /// 秒（s）。
    /// </summary>
    Second = 3600,

    /// <summary>
    /// 毫秒（ms）。
    /// </summary>
    Millisecond = 3600_000,

    /// <summary>
    /// 微秒（us）。
    /// </summary>
    Microsecond = 3600_000_000,

    /// <summary>
    /// 纳秒（ns）。
    /// </summary>
    Nanosecond = 3600_000_000_000,

    /// <summary>
    /// 皮秒（ps）。
    /// </summary>
    Picosecond = 3600_000_000_000_000
}