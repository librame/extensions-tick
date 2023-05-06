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
/// 定义实现 <see cref="IStringLocalizer{TResourceBase}"/> 的插件资源字符串定位器。
/// </summary>
/// <typeparam name="TResourceBase">指定的资源基础类型。</typeparam>
public class PluginResourceStringLocalizer<TResourceBase> : PluginResourceStringLocalizer, IStringLocalizer<TResourceBase>
{
    /// <summary>
    /// 构造一个 <see cref="PluginResourceStringLocalizer{TResourceBase}"/>。
    /// </summary>
    /// <param name="resourceAssembly">给定的资源程序集（可选；默认使用与资源基础类型相同的程序集）。</param>
    public PluginResourceStringLocalizer(Assembly? resourceAssembly = null)
        : base(typeof(TResourceBase), resourceAssembly)
    {
    }

}


/// <summary>
/// 定义实现 <see cref="IStringLocalizer"/> 的插件资源字符串定位器。
/// </summary>
public class PluginResourceStringLocalizer : ResourceDictionaryStringLocalizer
{
    /// <summary>
    /// 构造一个 <see cref="PluginResourceStringLocalizer"/>。
    /// </summary>
    /// <param name="resourceBaseType">给定的资源基础类型。</param>
    /// <param name="resourceAssembly">给定的资源程序集（可选；默认使用与资源基础类型相同的程序集）。</param>
    public PluginResourceStringLocalizer(Type resourceBaseType, Assembly? resourceAssembly = null)
        : this(new PluginResourceFactory(resourceBaseType, resourceAssembly))
    {
    }

    /// <summary>
    /// 构造一个 <see cref="PluginResourceStringLocalizer"/>。
    /// </summary>
    /// <param name="resourceFactory">给定的 <see cref="IPluginResourceFactory"/>。</param>
    public PluginResourceStringLocalizer(IPluginResourceFactory resourceFactory)
        : base(resourceFactory)
    {
    }

}
