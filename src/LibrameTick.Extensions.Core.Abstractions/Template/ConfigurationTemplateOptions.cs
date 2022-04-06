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
/// 定义继承 <see cref="TemplateOptions"/> 的配置模板选项。
/// </summary>
public class ConfigurationTemplateOptions : TemplateOptions
{
    /// <summary>
    /// 给定的配置源。
    /// </summary>
    [JsonIgnore]
    public IConfiguration? Source { get; set; }

    /// <summary>
    /// 当配置变化时刷新模板（默认启用）。
    /// </summary>
    public bool RefreshOnChange { get; set; } = true;
}
