#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Core;
using Librame.Extensions.Core.Cryptography;
using Librame.Extensions.Core.Network;
using Librame.Extensions.Core.Storage;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// <see cref="CoreExtensionBuilder"/> 与 <see cref="ServiceCollection"/> 静态扩展。
/// </summary>
public static class CoreExtensionBuilderServiceCollectionExtensions
{

    /// <summary>
    /// 注册 Librame 核心扩展构建器。
    /// </summary>
    /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
    /// <param name="setupOptions">给定用于设置选项的动作（可选；为空则不设置）。</param>
    /// <param name="configOptions">给定使用 <see cref="IConfiguration"/> 的选项配置（可选；为空则不配置）。</param>
    /// <param name="setupBuilder">给定用于设置构建器的动作（可选；为空则不设置）。</param>
    /// <returns>返回 <see cref="CoreExtensionBuilder"/>。</returns>
    public static CoreExtensionBuilder AddLibrameCore(this IServiceCollection services,
        Action<CoreExtensionOptions>? setupOptions = null, IConfiguration? configOptions = null,
        Action<CoreExtensionBuilder>? setupBuilder = null)
    {
        if (configOptions is null)
            configOptions = typeof(CoreExtensionOptions).GetConfigOptionsFromJson();

        var builder = new CoreExtensionBuilder(services, setupOptions, configOptions);
        setupBuilder?.Invoke(builder);

        builder.AddBase().AddCryptography().AddNetwork().AddStorage();

        return builder;
    }


    private static CoreExtensionBuilder AddBase(this CoreExtensionBuilder builder)
    {
        builder.TryAddOrReplaceService(typeof(ICloneable<>), typeof(BaseCloneable<>));
        builder.TryAddOrReplaceService(typeof(IDecoratable<>), typeof(BaseDecoratable<>));

        return builder;
    }

    private static CoreExtensionBuilder AddCryptography(this CoreExtensionBuilder builder)
    {
        builder.TryAddOrReplaceService<IAlgorithmParameterGenerator, InternalAlgorithmParameterGenerator>();
        builder.TryAddOrReplaceService<IAsymmetricAlgorithm, InternalAsymmetricAlgorithm>();
        builder.TryAddOrReplaceService<ISymmetricAlgorithm, InternalSymmetricAlgorithm>();

        return builder;
    }

    private static CoreExtensionBuilder AddNetwork(this CoreExtensionBuilder builder)
    {
        builder.TryAddOrReplaceService<IHttpClientInvokerFactory, InternalHttpClientInvokerFactory>();
        builder.TryAddOrReplaceService<IHttpEndpointsInvoker, InternalHttpEndpointsInvoker>();

        return builder;
    }

    private static CoreExtensionBuilder AddStorage(this CoreExtensionBuilder builder)
    {
        builder.TryAddOrReplaceService<IStorableFileManager, InternalStorableFileManager>();
        builder.TryAddOrReplaceService<IWebFilePermission, InternalWebFilePermission>();
        builder.TryAddOrReplaceService<IWebStorableFileTransfer, InternalWebStorableFileTransfer>();

        return builder;
    }

}
