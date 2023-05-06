#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Resources;

namespace Librame.Extensions.Plugins;

/// <summary>
/// 定义一个继承 <see cref="IResourceDictionaryFactory"/> 的插件资源工厂接口。
/// </summary>
public interface IPluginResourceFactory : IResourceDictionaryFactory
{
    /// <summary>
    /// 创建指定文化信息的插件资源。
    /// </summary>
    /// <param name="culture">给定的 <see cref="CultureInfo"/>（可选）。</param>
    /// <returns>返回 <see cref="IPluginResource"/>。</returns>
    new IPluginResource Create(CultureInfo? culture = null);
}
