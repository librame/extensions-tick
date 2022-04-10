#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Core.Plugins;
using Librame.Extensions.Core.Storage;
using Librame.Extensions.Cryptography;

namespace Librame.Extensions.Core;

/// <summary>
/// 定义实现 <see cref="IExtensionBuilder"/> 的核心扩展构建器。
/// </summary>
public class CoreExtensionBuilder : AbstractExtensionBuilder<CoreExtensionBuilder>
{
    /// <summary>
    /// 构造一个 <see cref="CoreExtensionBuilder"/>。
    /// </summary>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="services"/> 为空。
    /// </exception>
    /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
    public CoreExtensionBuilder(IServiceCollection services)
        : base(services)
    {
        // Cryptography
        ServiceCharacteristics.AddSingleton<IAlgorithmParameterGenerator>();
        ServiceCharacteristics.AddSingleton<IAsymmetricAlgorithm>();
        ServiceCharacteristics.AddSingleton<ISymmetricAlgorithm>();

        // Plugins
        ServiceCharacteristics.AddSingleton<IPluginResolver>();

        // Storage
        ServiceCharacteristics.AddSingleton<IStorableFileManager>();
        ServiceCharacteristics.AddSingleton<IWebFilePermission>();
        ServiceCharacteristics.AddSingleton<IWebStorableFileTransfer>();
    }

}
