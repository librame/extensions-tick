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
/// 定义抽象实现 <see cref="ITemplateProvider"/> 的模板提供程序。
/// </summary>
public abstract class AbstractTemplateProvider : ITemplateProvider
{
    private readonly ConcurrentDictionary<string, TemplateOptions> _options = new();


    /// <summary>
    /// 获取或新增模板选项。
    /// </summary>
    /// <param name="name">给定的实例名称。</param>
    /// <param name="addFunc">给定的新增模板选项方法（可选）。</param>
    /// <returns>返回 <see cref="TemplateOptions"/>。</returns>
    public virtual TOptions GetOrAddOptions<TOptions>(string name,
        Func<string, TOptions>? addFunc = null)
        where TOptions : TemplateOptions, new()
        => (TOptions)_options.GetOrAdd(name, addFunc ?? (key => new()));

    /// <summary>
    /// 获取或新增模板选项。
    /// </summary>
    /// <param name="name">给定的实例名称。</param>
    /// <param name="addFunc">给定的新增模板选项方法。</param>
    /// <returns>返回 <see cref="TemplateOptions"/>。</returns>
    public virtual TemplateOptions GetOrAddOptions(string name,
        Func<string, TemplateOptions> addFunc)
        => _options.GetOrAdd(name, addFunc);

}