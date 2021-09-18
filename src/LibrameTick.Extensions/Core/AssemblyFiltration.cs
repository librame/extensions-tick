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
/// 定义程序集筛选方式。
/// </summary>
public enum AssemblyFiltration
{
    /// <summary>
    /// 表示无筛选。
    /// </summary>
    None,

    /// <summary>
    /// 表示排除此程序集。
    /// </summary>
    Exclusive,

    /// <summary>
    /// 表示包含此程序集。
    /// </summary>
    Inclusive
}
