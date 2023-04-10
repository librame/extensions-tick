﻿#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data;

/// <summary>
/// 定义更新时间周期数接口。
/// </summary>
/// <remarks>
/// 主要用于解决 <see cref="DateTimeOffset"/> 在不同数据库中 LINQ 查询的兼容性问题。
/// </remarks>
public interface IUpdationTimeTicks
{
    /// <summary>
    /// 更新时间周期数。
    /// </summary>
    long UpdatedTimeTicks { get; set; }
}
