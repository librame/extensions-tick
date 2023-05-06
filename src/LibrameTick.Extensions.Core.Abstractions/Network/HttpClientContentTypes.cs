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
/// 定义 HTTP 客户端内容类型支持的枚举列表。
/// </summary>
public enum HttpClientContentTypes
{
    /// <summary>
    /// 默认内容类型。
    /// </summary>
    [Description("application/x-www-form-urlencoded")]
    FormUrlEncoded,

    /// <summary>
    /// 文件内容类型。
    /// </summary>
    [Description("multipart/form-data")]
    MultipartFormData,

    /// <summary>
    /// JSON 内容类型。
    /// </summary>
    [Description("application/json")]
    Json,

    /// <summary>
    /// 二进制内容类型。
    /// </summary>
    [Description("binary")]
    Stream
}
