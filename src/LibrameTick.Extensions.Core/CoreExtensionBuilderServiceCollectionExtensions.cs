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
using Librame.Extensions.Core.Network;
using Librame.Extensions.Core.Storage;
using Librame.Extensions.Cryptography;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// <see cref="CoreExtensionBuilder"/> 与 <see cref="ServiceCollection"/> 静态扩展。
/// </summary>
public static class CoreExtensionBuilderServiceCollectionExtensions
{

    /// <summary>
    /// 使用配置对象与（或）选项动作配置实现 <see cref="IExtensionOptions"/> 的扩展选项。
    /// </summary>
    /// <typeparam name="TOptions">指定实现的 <see cref="IExtensionOptions"/> 的扩展选项。</typeparam>
    /// <param name="parentBuilder">给定的 <see cref="IExtensionBuilder"/>。</param>
    /// <param name="setupOptions">给定可用于设置 <typeparamref name="TOptions"/> 选项的动作（可空；为空则不设置）。</param>
    /// <param name="configuration">给定的 <see cref="IConfiguration"/>（可空；为空则不设置）。</param>
    /// <returns>返回 <see cref="IExtensionBuilder"/>。</returns>
    public static IExtensionBuilder ConfigureExtensionOptions<TOptions>(
        this IExtensionBuilder parentBuilder,
        Action<TOptions>? setupOptions, IConfiguration? configuration)
        where TOptions : class, IExtensionOptions
    {
        parentBuilder.Services.ConfigureExtensionOptions(setupOptions, configuration);
        return parentBuilder;
    }

    /// <summary>
    /// 使用配置对象与（或）选项动作配置实现 <see cref="IExtensionOptions"/> 的扩展选项。
    /// </summary>
    /// <typeparam name="TOptions">指定实现的 <see cref="IExtensionOptions"/> 的扩展选项。</typeparam>
    /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
    /// <param name="setupOptions">给定可用于设置 <typeparamref name="TOptions"/> 选项的动作（可空；为空则不设置）。</param>
    /// <param name="configuration">给定的 <see cref="IConfiguration"/>（可空；为空则不设置）。</param>
    /// <returns>返回 <see cref="IServiceCollection"/>。</returns>
    public static IServiceCollection ConfigureExtensionOptions<TOptions>(
        this IServiceCollection services,
        Action<TOptions>? setupOptions, IConfiguration? configuration)
        where TOptions : class, IExtensionOptions
    {
        if (configuration is not null)
            services.Configure<TOptions>(configuration);

        if (setupOptions is not null)
            services.Configure(setupOptions);

        return services;
    }


    /// <summary>
    /// 注册 Librame 核心扩展构建器。
    /// </summary>
    /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
    /// <param name="setupOptions">给定可用于设置 <see cref="CoreExtensionOptions"/> 选项的动作（可选；为空则不设置）。</param>
    /// <param name="configuration">给定可用于 <see cref="CoreExtensionOptions"/> 选项的配置对象（可选；为空则不配置）。</param>
    /// <returns>返回 <see cref="CoreExtensionBuilder"/>。</returns>
    public static CoreExtensionBuilder AddLibrameCore(this IServiceCollection services,
        Action<CoreExtensionOptions>? setupOptions = null, IConfiguration? configuration = null)
    {
        // 添加依赖服务
        if (!services.ContainsService<IMemoryCache>())
            services.AddMemoryCache();

        if (!services.ContainsService<IHttpClientFactory>())
            services.AddHttpClient();

        // 配置扩展选项
        services.ConfigureExtensionOptions(setupOptions, configuration);

        // 创建扩展构建器
        var builder = new CoreExtensionBuilder(services);

        // 注册扩展服务集合
        return builder
            .AddCommon()
            .AddCryptography()
            .AddNetwork()
            .AddStorage();
    }


    private static CoreExtensionBuilder AddCommon(this CoreExtensionBuilder builder)
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
