#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data.Accessing;

/// <summary>
/// 定义支持对存取器集合进行聚合或切片的冗余模式。
/// </summary>
public enum RedundancyMode
{
    /// <summary>
    /// 聚合。
    /// </summary>
    Aggregation,

    /// <summary>
    /// 切片。
    /// </summary>
    Slicing
}
