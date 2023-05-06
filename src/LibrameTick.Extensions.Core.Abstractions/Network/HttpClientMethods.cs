#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Network;

/// <summary>
/// 定义 HTTP 客户端方法支持的枚举列表。
/// </summary>
public enum HttpClientMethods
{
    /// <summary>
    /// 获取请求。
    /// </summary>
    Get,

    /// <summary>
    /// 创建请求。
    /// </summary>
    Post,

    /// <summary>
    /// 创建或更新请求。
    /// </summary>
    Put,

    /// <summary>
    /// 非空更新请求。
    /// </summary>
    Patch,

    /// <summary>
    /// 删除请求。
    /// </summary>
    Delete
}
