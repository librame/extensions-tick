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
using Librame.Extensions.Core.Plugins;
using Librame.Extensions.Core.Storage;

namespace Librame.Extensions.Core;

/// <summary>
/// 定义实现 <see cref="IExtensionBuilder"/> 的核心扩展构建器。
/// </summary>
public class CoreExtensionBuilder : BaseExtensionBuilder<CoreExtensionBuilder, CoreExtensionOptions>
{
    /// <summary>
    /// 构造一个 <see cref="CoreExtensionBuilder"/>。
    /// </summary>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="services"/> 为空。
    /// </exception>
    /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
    /// <param name="setupOptions">给定用于设置选项的动作（可选；为空则不设置）。</param>
    /// <param name="configOptions">给定使用 <see cref="IConfiguration"/> 的选项配置（可选；为空则不配置）。</param>
    public CoreExtensionBuilder(IServiceCollection services,
        Action<CoreExtensionOptions>? setupOptions = null, IConfiguration? configOptions = null)
        : base(services, setupOptions, configOptions)
    {
        if (!Services.ContainsService<IMemoryCache>())
            Services.AddMemoryCache();

        if (!Services.ContainsService<IHttpClientFactory>())
            Services.AddHttpClient();

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
