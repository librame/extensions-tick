#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core.Plugins;

/// <summary>
/// 定义实现 <see cref="IOptions"/> 的插件选项。
/// </summary>
public class PluginOptions : IOptions
{
    /// <summary>
    /// 程序集加载选项。
    /// </summary>
    public AssembliesOptions AssemblyLoading { get; set; } = new();
}
