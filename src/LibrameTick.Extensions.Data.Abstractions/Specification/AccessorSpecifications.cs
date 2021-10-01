#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data.Specification;

/// <summary>
/// 定义常用访问器规约。
/// </summary>
public static class AccessorSpecifications
{
    /// <summary>
    /// 基础访问器规约。
    /// </summary>
    public static readonly IAccessorSpecification Base
        = new BaseAccessorSpecification();

    /// <summary>
    /// 读取访问器规约。
    /// </summary>
    public static readonly IAccessorSpecification Read
        = new ReadAccessorSpecification();

    /// <summary>
    /// 写入访问器规约。
    /// </summary>
    public static readonly IAccessorSpecification Write
        = new WriteAccessorSpecification();
}
