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
        : base(parentOptions: null)
    {
        Algorithm = new AlgorithmOptions(Notifier);
        AssemblyLoading = new AssemblyLoadingOptions(Notifier);
        WebRequest = new WebRequestOptions(Notifier);

        // Cryptography
        ServiceCharacteristics.AddSingleton<IAlgorithmParameterGenerator>();
        ServiceCharacteristics.AddSingleton<IAsymmetricAlgorithm>();
        ServiceCharacteristics.AddSingleton<ISymmetricAlgorithm>();

        // Storage
        ServiceCharacteristics.AddSingleton<IFileManager>();
        ServiceCharacteristics.AddSingleton<IFilePermission>();
        ServiceCharacteristics.AddSingleton<IFileTransmission>();
    }


    /// <summary>
    /// 算法选项。
    /// </summary>
    public AlgorithmOptions Algorithm { get; init; }

    /// <summary>
    /// 程序集加载选项。
    /// </summary>
    public AssemblyLoadingOptions AssemblyLoading { get; init; }

    /// <summary>
    /// Web 请求选项。
    /// </summary>
    public WebRequestOptions WebRequest { get; init; }


    /// <summary>
    /// 时钟（默认使用本地时钟）。
    /// </summary>
    [JsonIgnore]
    public IClock Clock
    {
        get => Notifier.GetOrAdd(nameof(Clock), Instantiator.GetClock());
        set => Notifier.AddOrUpdate(nameof(Clock), value);
    }

    /// <summary>
    /// 机器中心标识（默认 1）。
    /// </summary>
    public long DataCenterId
    {
        get => Notifier.GetOrAdd(nameof(DataCenterId), 1L);
        set => Notifier.AddOrUpdate(nameof(DataCenterId), value);
    }

    /// <summary>
    /// 机器标识（默认 1）。
    /// </summary>
    public long MachineId
    {
        get => Notifier.GetOrAdd(nameof(MachineId), 1L);
        set => Notifier.AddOrUpdate(nameof(MachineId), value);
    }

    /// <summary>
    /// 启用 <see cref="IAutoloader"/> 激活器（默认不启用）。
    /// </summary>
    public bool EnableAutoloaderActivator
    {
        get => Notifier.GetOrAdd(nameof(EnableAutoloaderActivator), false);
        set => Notifier.AddOrUpdate(nameof(EnableAutoloaderActivator), value);
    }

}
