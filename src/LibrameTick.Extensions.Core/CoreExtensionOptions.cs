#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Bootstraps;
using Librame.Extensions.Core.Network;
using Librame.Extensions.Core.Storage;
using Librame.Extensions.Cryptography;
using Librame.Extensions.Dispatchers;

namespace Librame.Extensions.Core;

/// <summary>
/// 定义实现 <see cref="IExtensionOptions"/> 的核心扩展选项。
/// </summary>
public class CoreExtensionOptions : AbstractExtensionOptions<CoreExtensionOptions>
{
    /// <summary>
    /// 调度选项。
    /// </summary>
    public DispatchingOptions Dispatching { get; set; } = new();

    /// <summary>
    /// 算法选项。
    /// </summary>
    public AlgorithmOptions Algorithm { get; set; } = new();

    /// <summary>
    /// HTTP 客户端请求选项。
    /// </summary>
    public HttpClientRequestOptions Request { get; set; } = new();

    /// <summary>
    /// Web 文件选项。
    /// </summary>
    public WebFileOptions WebFile { get; set; } = new();


    /// <summary>
    /// 时钟（默认使用 <see cref="Bootstrapper.GetClock()"/>）。
    /// </summary>
    [JsonIgnore]
    public IClockBootstrap Clock { get; set; } = Bootstrapper.GetClock();

    /// <summary>
    /// 时钟（默认使用 <see cref="Bootstrapper.GetLocker()"/>）。
    /// </summary>
    [JsonIgnore]
    public ILockerBootstrap Locker { get; set; } = Bootstrapper.GetLocker();
}
