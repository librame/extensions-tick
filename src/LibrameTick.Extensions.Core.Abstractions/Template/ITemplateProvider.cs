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
/// 定义一个模板提供程序接口。
/// </summary>
public interface ITemplateProvider
{
    /// <summary>
    /// 获取或新增模板选项。
    /// </summary>
    /// <param name="name">给定的实例名称。</param>
    /// <returns>返回 <see cref="TemplateOptions"/>。</returns>
    TemplateOptions GetOrAddOptions(string? name = null);
}