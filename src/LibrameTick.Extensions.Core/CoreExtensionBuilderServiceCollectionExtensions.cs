#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions;
using Librame.Extensions.Core;
using Librame.Extensions.Network;
using Librame.Extensions.Setting;
using Librame.Extensions.Storage;
using Librame.Extensions.Crypto;
using Librame.Extensions.Device;
using Librame.Extensions.Dispatchers;

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
    /// <param name="setupConfigurationBuilder">给定可用于 <see cref="CoreExtensionOptions"/> 选项的配置对象（可选；为空则不配置）。</param>
    /// <param name="enableTemplate">针对配置对象启用模板功能，启用将支持配置文件中对键名的值的引用（可选；默认启用）。</param>
    /// <returns>返回 <see cref="CoreExtensionBuilder"/>。</returns>
    public static CoreExtensionBuilder AddLibrame(this IServiceCollection services,
        Action<CoreExtensionOptions>? setupOptions, Action<IConfigurationBuilder>? setupConfigurationBuilder,
        bool enableTemplate = true)
    {
        var configuration = ConfigurationBuilderExtensions.GetConfiguration(setupConfigurationBuilder, enableTemplate);

        return services.AddLibrame(setupOptions, configuration);
    }

    /// <summary>
    /// 注册 Librame 核心扩展构建器。
    /// </summary>
    /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
    /// <param name="setupOptions">给定可用于设置 <see cref="CoreExtensionOptions"/> 选项的动作（可选；为空则不设置）。</param>
    /// <param name="configuration">给定可用于 <see cref="CoreExtensionOptions"/> 选项的配置对象（可选；为空则不配置）。</param>
    /// <returns>返回 <see cref="CoreExtensionBuilder"/>。</returns>
    public static CoreExtensionBuilder AddLibrame(this IServiceCollection services,
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
            .AddCrypto()
            .AddDevice()
            .AddNetwork()
            .AddPlugins()
            .AddSetting()
            .AddStorage();
    }


    private static CoreExtensionBuilder AddCommon(this CoreExtensionBuilder builder)
    {
        builder.TryAddOrReplaceService(typeof(IOptionsValues<>), implementationType: typeof(InternalOptionsValues<>));

        builder.TryAddOrReplaceService<IDispatcherFactory, InternalDispatcherFactory>();

        return builder;
    }

    private static CoreExtensionBuilder AddCrypto(this CoreExtensionBuilder builder)
    {
        builder.TryAddOrReplaceService<IAlgorithmParameterGenerator, InternalAlgorithmParameterGenerator>();
        builder.TryAddOrReplaceService<IAsymmetricAlgorithm, InternalAsymmetricAlgorithm>();
        builder.TryAddOrReplaceService<ISymmetricAlgorithm, InternalSymmetricAlgorithm>();

        return builder;
    }

    private static CoreExtensionBuilder AddDevice(this CoreExtensionBuilder builder)
    {
        builder.TryAddOrReplaceService<IDeviceLoader, InternalDeviceLoader>();

        return builder;
    }

    private static CoreExtensionBuilder AddNetwork(this CoreExtensionBuilder builder)
    {
        builder.TryAddOrReplaceService<IHttpClientInvokerFactory, InternalHttpClientInvokerFactory>();
        builder.TryAddOrReplaceService<IHttpEndpointsInvoker, InternalHttpEndpointsInvoker>();

        return builder;
    }

    private static CoreExtensionBuilder AddPlugins(this CoreExtensionBuilder builder)
    {
        return builder;
    }

    private static CoreExtensionBuilder AddSetting(this CoreExtensionBuilder builder)
    {
        builder.TryAddOrReplaceService(typeof(ISettingValues<>), implementationType: typeof(InternalSettingValues<>));

        return builder;
    }

    private static CoreExtensionBuilder AddStorage(this CoreExtensionBuilder builder)
    {
        builder.TryAddOrReplaceService<IStorableFileManager, InternalStorableFileManager>();
        builder.TryAddOrReplaceService<IWebFilePermission, InternalWebFilePermission>();
        builder.TryAddOrReplaceService<IWebStorableFileTransfer, InternalWebStorableFileTransfer>();

        return builder;
    }


    /// <summary>
    /// 添加设置提供程序。
    /// </summary>
    /// <typeparam name="TProvider">指定的设置提供程序类型（推荐从诸如 <see cref="JsonFileSettingProvider{TSetting}"/> 等类型继承实现）。</typeparam>
    /// <param name="builder">给定的 ?<see cref="CoreExtensionBuilder"/>。</param>
    /// <returns>返回 <see cref="CoreExtensionBuilder"/>。</returns>
    public static CoreExtensionBuilder AddSettingProvider<TProvider>(this CoreExtensionBuilder builder)
        => builder.AddSettingProvider(typeof(TProvider));

    private static readonly Type _iSettingProviderType = typeof(ISettingProvider<>);
    /// <summary>
    /// 添加设置提供程序。
    /// </summary>
    /// <param name="builder">给定的 ?<see cref="CoreExtensionBuilder"/>。</param>
    /// <param name="providerType">给定的设置提供程序类型（推荐从诸如 <see cref="JsonFileSettingProvider{TSetting}"/> 等类型继承实现）。</param>
    /// <returns>返回 <see cref="CoreExtensionBuilder"/>。</returns>
    public static CoreExtensionBuilder AddSettingProvider(this CoreExtensionBuilder builder, Type providerType)
    {
        if (!providerType.IsImplementedType(_iSettingProviderType, out var resultType))
            throw new ArgumentException($"Invalid setting provider type, the required interface '{_iSettingProviderType}' was not implemented.");

        var serviceType = _iSettingProviderType.MakeGenericType(resultType.GetGenericArguments()[0]);
        builder.TryAddOrReplaceService(serviceType, implementationType: providerType);

        return builder;
    }

}
