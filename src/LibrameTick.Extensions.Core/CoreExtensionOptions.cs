#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Core.Cryptography;
using Librame.Extensions.Core.Network;
using Librame.Extensions.Core.Storage;
using Librame.Extensions.Bootstraps;

namespace Librame.Extensions.Core;

/// <summary>
/// 定义实现 <see cref="IExtensionOptions"/> 的核心扩展选项。
/// </summary>
public class CoreExtensionOptions : AbstractExtensionOptions<CoreExtensionOptions>
{
    /// <summary>
    /// 构造一个 <see cref="CoreExtensionOptions"/>。
    /// </summary>
    public CoreExtensionOptions()
    {
        Algorithm = new(Notifier);
        Request = new(Notifier);
        WebFile = new(Notifier);
    }


    /// <summary>
    /// 算法选项。
    /// </summary>
    public AlgorithmOptions Algorithm { get; init; }

    /// <summary>
    /// HTTP 客户端请求选项。
    /// </summary>
    public HttpClientRequestOptions Request { get; init; }

    /// <summary>
    /// Web 文件选项。
    /// </summary>
    public WebFileOptions WebFile { get; init; }


    /// <summary>
    /// 时钟（默认使用本地时钟）。
    /// </summary>
    [JsonIgnore]
    public IClockBootstrap Clock
    {
        get => Notifier.GetOrAdd(nameof(Clock), Bootstrapper.GetClock());
        set => Notifier.AddOrUpdate(nameof(Clock), value);
    }

    /// <summary>
    /// 时钟（默认使用本地锁定器）。
    /// </summary>
    [JsonIgnore]
    public ILockerBootstrap Locker
    {
        get => Notifier.GetOrAdd(nameof(Locker), Bootstrapper.GetLocker());
        set => Notifier.AddOrUpdate(nameof(Locker), value);
    }

}
