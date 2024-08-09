#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Serialization;

/// <summary>
/// 定义支持位与运算的二进制版本比较枚举。
/// </summary>
[Flags]
public enum BinaryVersionComparison
{
    /// <summary>
    /// 未知版本。
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// 版本相等。
    /// </summary>
    Equals = 1,

    /// <summary>
    /// 版本大于。
    /// </summary>
    GreaterThan = 2,

    /// <summary>
    /// 版本大于或等于。
    /// </summary>
    GreaterThanOrEquals = Equals | GreaterThan,

    /// <summary>
    /// 版本小于。
    /// </summary>
    LessThan = 4,

    /// <summary>
    /// 版本小于或等于。
    /// </summary>
    LessThanOrEquals = Equals | LessThan
}
