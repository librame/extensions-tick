#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core.Network;

/// <summary>
/// 定义 HTTP 端点集合调用器接口。
/// </summary>
public interface IHttpEndpointsInvoker
{
    /// <summary>
    /// 字符编码。
    /// </summary>
    Encoding Encoding { get; }


    /// <summary>
    /// 调用端点集合。
    /// </summary>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回包含结果集合的异步操作。</returns>
    Task<IEnumerable<string>?> InvokeAsync(CancellationToken cancellationToken = default);
}
