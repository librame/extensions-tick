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
using Librame.Extensions.Infrastructure;
using Librame.Extensions.Device;
using Librame.Extensions.Dispatching;
using Librame.Extensions.Network;
using Librame.Extensions.Proxy;
using Librame.Extensions.Setting;
using Librame.Extensions.Storage;

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
            .AddBase()
            .AddCrypto()
            .AddDevice()
            .AddNetwork()
            .AddPlugins()
            .AddStorage();
    }


    private static CoreExtensionBuilder AddBase(this CoreExtensionBuilder builder)
    {
        builder.TryAddOrReplaceService<IDispatcherFactory, InternalDispatcherFactory>();
        builder.TryAddOrReplaceService<IProxyGenerator, ProxyGenerator>();
        builder.TryAddOrReplaceService<IInterceptor, MethodInterceptor>();
        builder.TryAddOrReplaceService(typeof(IProxyDecorator<>), implementationType: typeof(ProxyDecoratorInjection<>));

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
    /// <typeparam name="TProvider">指定的设置提供程序类型（推荐从诸如 <see cref="BaseSettingProvider{TSetting}"/> 等类型继承实现）。</typeparam>
    /// <param name="builder">给定的 <see cref="IExtensionBuilder"/>。</param>
    /// <returns>返回 <see cref="IExtensionBuilder"/>。</returns>
    public static IExtensionBuilder AddSettingProvider<TProvider>(this IExtensionBuilder builder)
        => builder.AddSettingProvider(typeof(TProvider));

    private static readonly Type _settingProviderType = typeof(ISettingProvider<>);
    /// <summary>
    /// 添加设置提供程序。
    /// </summary>
    /// <typeparam name="TBuidler">指定的扩展构建器类型。</typeparam>
    /// <param name="builder">给定的 <typeparamref name="TBuidler"/>。</param>
    /// <param name="providerType">给定的设置提供程序类型（推荐从诸如 <see cref="BaseSettingProvider{TSetting}"/> 等类型继承实现）。</param>
    /// <returns>返回 <typeparamref name="TBuidler"/>。</returns>
    public static TBuidler AddSettingProvider<TBuidler>(this TBuidler builder, Type providerType)
        where TBuidler : IExtensionBuilder
    {
        if (!providerType.IsImplementedType(_settingProviderType, out var resultType))
            throw new ArgumentException($"Invalid setting provider type, the required interface '{_settingProviderType}' was not implemented.");

        var serviceType = _settingProviderType.MakeGenericType(resultType.GetGenericArguments()[0]);
        builder.TryAddOrReplaceService(serviceType, implementationType: providerType);

        return builder;
    }

}
