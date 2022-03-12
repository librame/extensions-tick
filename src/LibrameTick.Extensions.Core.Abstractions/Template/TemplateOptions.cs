﻿#region License

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
/// 定义实现 <see cref="IOptions"/> 的模板选项。
/// </summary>
public class TemplateOptions : IOptions
{
    /// <summary>
    /// 引用键选项。
    /// </summary>
    public RefKeyOptions RefKeys { get; set; } = new();


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