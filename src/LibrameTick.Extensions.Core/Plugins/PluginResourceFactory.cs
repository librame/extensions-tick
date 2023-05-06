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
/// 定义实现 <see cref="IPluginResourceFactory"/> 的泛型插件资源工厂。
/// </summary>
/// <typeparam name="TResourceBase">指定的资源基础类型。</typeparam>
public class PluginResourceFactory<TResourceBase> : PluginResourceFactory
{
    /// <summary>
    /// 构造一个 <see cref="PluginResourceFactory{TResourceBase}"/>。
    /// </summary>
    /// <param name="resourceAssembly">给定的资源程序集（可选；默认使用与资源基础类型相同的程序集）。</param>
    public PluginResourceFactory(Assembly? resourceAssembly = null)
        : base(typeof(TResourceBase), resourceAssembly)
    {
    }

}


/// <summary>
/// 定义实现 <see cref="IPluginResourceFactory"/> 的插件资源工厂。
/// </summary>
public class PluginResourceFactory : ResourceDictionaryFactory, IPluginResourceFactory
{
    /// <summary>
    /// 构造一个 <see cref="PluginResourceFactory"/>。
    /// </summary>
    /// <param name="resourceBaseType">给定的资源基础类型。</param>
    /// <param name="resourceAssembly">给定的资源程序集（可选；默认使用与资源基础类型相同的程序集）。</param>
    public PluginResourceFactory(Type resourceBaseType, Assembly? resourceAssembly = null)
        : base(resourceBaseType, resourceAssembly)
    {
    }


    /// <summary>
    /// 创建指定文化信息的插件资源。
    /// </summary>
    /// <param name="culture">给定的 <see cref="CultureInfo"/>（可选；默认使用 <see cref="CultureInfo.CurrentUICulture"/>）。</param>
    /// <returns>返回 <see cref="IPluginResource"/>。</returns>
    public new IPluginResource Create(CultureInfo? culture = null)
        => (IPluginResource)base.Create(culture);
}
