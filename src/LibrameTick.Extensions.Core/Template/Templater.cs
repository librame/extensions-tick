#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core.Template;

/// <summary>
/// 定义模板程序。
/// </summary>
public static class Templater
{
    private static readonly ITemplateProvider _provider
        = SingletonFactory<InternalTemplateProvider>.Instance;


    /// <summary>
    /// 模板提供程序。
    /// </summary>
    public static ITemplateProvider Provider
        => _provider;

    /// <summary>
    /// 默认模板选项。
    /// </summary>
    public static TemplateOptions DefaultOptions
        => _provider.GetOrAddOptions();


    /// <summary>
    /// 获取针对配置对象的 <see cref="RefKey"/> 查找器。
    /// </summary>
    /// <param name="options">给定的 <see cref="TemplateOptions"/>（可选；默认使用 <see cref="DefaultOptions"/>）。</param>
    /// <returns>返回 <see cref="IRefKeyFinder"/>。</returns>
    public static IRefKeyFinder GetConfigurationRefKeyFinder(TemplateOptions? options = null)
    {
        var finder = new ConfigurationRefKeyFinder(options ?? DefaultOptions);

        // 初始填充引用键集合
        finder.Populate();

        return finder;
    }

}