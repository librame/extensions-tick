#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Template;

/// <summary>
/// 定义一个模板提供程序接口。
/// </summary>
public interface ITemplateProvider
{
    /// <summary>
    /// 获取或新增模板选项。
    /// </summary>
    /// <param name="name">给定的实例名称。</param>
    /// <param name="addFunc">给定的新增模板选项方法（可选）。</param>
    /// <returns>返回 <see cref="TemplateOptions"/>。</returns>
    TOptions GetOrAddOptions<TOptions>(string name, Func<string, TOptions>? addFunc = null)
        where TOptions : TemplateOptions, new();

    /// <summary>
    /// 获取或新增模板选项。
    /// </summary>
    /// <param name="name">给定的实例名称。</param>
    /// <param name="addFunc">给定的新增模板选项方法。</param>
    /// <returns>返回 <see cref="TemplateOptions"/>。</returns>
    TemplateOptions GetOrAddOptions(string name, Func<string, TemplateOptions> addFunc);
}