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
using Librame.Extensions.Core.Storage;

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
        WebRequest = new(Notifier);
    }


    /// <summary>
    /// 算法选项。
    /// </summary>
    public AlgorithmOptions Algorithm { get; init; }

    /// <summary>
    /// Web 请求选项。
    /// </summary>
    public WebRequestOptions WebRequest { get; init; }


    /// <summary>
    /// 时钟（默认使用本地时钟）。
    /// </summary>
    [JsonIgnore]
    public IRegisterableClock Clock
    {
        get => Notifier.GetOrAdd(nameof(Clock), Registration.GetRegisterableClock());
        set => Notifier.AddOrUpdate(nameof(Clock), value);
    }

    /// <summary>
    /// 时钟（默认使用本地锁定器）。
    /// </summary>
    [JsonIgnore]
    public IRegisterableLocker Locker
    {
        get => Notifier.GetOrAdd(nameof(Locker), Registration.GetRegisterableLocker());
        set => Notifier.AddOrUpdate(nameof(Locker), value);
    }

}
