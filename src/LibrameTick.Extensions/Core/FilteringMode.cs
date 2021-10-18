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
/// 定义筛选模式。
/// </summary>
public enum FilteringMode
{
    /// <summary>
    /// 不筛选。
    /// </summary>
    None,

    /// <summary>
    /// 排除筛选。
    /// </summary>
    Exclusive,

    /// <summary>
    /// 包含筛选。
    /// </summary>
    Inclusive
}
