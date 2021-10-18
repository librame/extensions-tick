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
/// 定义实现 <see cref="IOptionsNotifier"/> 的插件选项。
/// </summary>
public class PluginOptions : AbstractOptionsNotifier
{
    /// <summary>
    /// 构造一个独立属性通知器的 <see cref="PluginOptions"/>（此构造函数适用于独立使用 <see cref="PluginOptions"/> 的情况）。
    /// </summary>
    /// <param name="sourceAliase">给定的源别名（独立属性通知器必须命名实例）。</param>
    public PluginOptions(string sourceAliase)
        : base(sourceAliase)
    {
        AssemblyLoading = new(Notifier);
    }

    /// <summary>
    /// 构造一个 <see cref="PluginOptions"/>。
    /// </summary>
    /// <param name="parentNotifier">给定的父级 <see cref="IPropertyNotifier"/>。</param>
    /// <param name="sourceAliase">给定的源别名（可选）。</param>
    public PluginOptions(IPropertyNotifier parentNotifier, string? sourceAliase = null)
        : base(parentNotifier, sourceAliase)
    {
        AssemblyLoading = new(Notifier);
    }


    /// <summary>
    /// 程序集加载选项。
    /// </summary>
    public AssemblyLoadingOptions AssemblyLoading { get; init; }
}
