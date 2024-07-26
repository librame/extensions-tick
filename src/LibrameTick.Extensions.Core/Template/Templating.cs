#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Infrastructure;

namespace Librame.Extensions.Template;

/// <summary>
/// 定义模板工具。
/// </summary>
public static class Templating
{
    private static readonly ITemplateProvider _provider
        = SingletonFactory<InternalTemplateProvider>.Instance;


    /// <summary>
    /// 模板提供程序。
    /// </summary>
    public static ITemplateProvider Provider
        => _provider;

    /// <summary>
    /// 默认配置模板选项。
    /// </summary>
    public static ConfigurationTemplateOptions DefaultConfigurationOptions
        => _provider.GetOrAddOptions<ConfigurationTemplateOptions>(Options.DefaultName);


    /// <summary>
    /// 获取用于配置对象的 <see cref="TemplateKeyDescriptor"/> 查找器。
    /// </summary>
    /// <param name="options">给定的 <see cref="ConfigurationTemplateOptions"/>（可选；默认使用 <see cref="DefaultConfigurationOptions"/>）。</param>
    /// <returns>返回 <see cref="ITemplateKeyFinder"/>。</returns>
    public static ITemplateKeyFinder GetConfigurationTemplateKeyFinder(ConfigurationTemplateOptions? options = null)
    {
        options ??= DefaultConfigurationOptions;
        
        var finder = new ConfigurationTemplateKeyFinder(options);

        // 初始填充模板键集合
        finder.Populate(options);

        return finder;
    }

}