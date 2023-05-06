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
using Librame.Extensions.Microparts;

namespace Microsoft.Extensions.Configuration;

/// <summary>
/// 定义 <see cref="IConfigurationBuilder"/> 静态扩展。
/// </summary>
public static class ConfigurationBuilderExtensions
{

    /// <summary>
    /// 添加当前应用根目录的 appsettings.json 配置文件。
    /// </summary>
    /// <param name="configurationBuilder">给定的 <see cref="IConfigurationBuilder"/>。</param>
    /// <returns>返回 <see cref="IConfigurationBuilder"/>。</returns>
    public static IConfigurationBuilder AddAppSettingsJsonFile(this IConfigurationBuilder configurationBuilder)
    {
        return configurationBuilder
            .AddJsonFile("appsettings.json")
            .SetBasePath(PathExtensions.CurrentDirectoryWithoutDevelopmentRelativeSubpath);
    }

    /// <summary>
    /// 添加 URI JSON 配置流。
    /// </summary>
    /// <param name="configurationBuilder">给定的 <see cref="IConfigurationBuilder"/>。</param>
    /// <param name="requestUri">给定的 URI 请求。</param>
    /// <param name="options">给定的 <see cref="HttpClientOptions"/>（可选）。</param>
    /// <returns>返回 <see cref="IConfigurationBuilder"/>。</returns>
    public static IConfigurationBuilder AddUrlJsonStream(this IConfigurationBuilder configurationBuilder,
        [StringSyntax("Uri")] string? requestUri, HttpClientOptions? options = null)
    {
        var httpClient = MicropartActivator.CreateHttpClient(options ?? new()).Unwrap();

        var stream = httpClient.GetStreamAsync(requestUri).Result;

        return configurationBuilder.AddJsonStream(stream);
    }


    /// <summary>
    /// 获取配置对象（支持启用模板功能）。
    /// </summary>
    /// <param name="setupConfigurationBuilder">给定用于设置 <see cref="IConfigurationBuilder"/> 的动作（可选；为空则不配置）。</param>
    /// <param name="enableTemplate">针对配置对象启用模板功能，启用将支持配置文件中对键名的值的引用（可选；默认启用）。</param>
    /// <returns>返回 <see cref="IConfigurationRoot"/>。</returns>
    public static IConfigurationRoot GetConfiguration(Action<IConfigurationBuilder>? setupConfigurationBuilder,
        bool enableTemplate = true)
    {
        var configurationBuilder = new ConfigurationBuilder();
        setupConfigurationBuilder?.Invoke(configurationBuilder);

        var configuration = configurationBuilder.Build();
        if (enableTemplate)
            configuration.EnableTemplate();

        return configuration;
    }

}
