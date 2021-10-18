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

namespace Librame.Extensions.Core.Plugins;

/// <summary>
/// 定义抽象实现 <see cref="IPluginResource"/> 的插件资源。
/// </summary>
public abstract class AbstractPluginResource : AbstractResourceDictionary, IPluginResource
{
    /// <summary>
    /// 构造一个 <see cref="AbstractPluginResource"/>。
    /// </summary>
    /// <param name="resourceName">给定的资源名称。</param>
    protected AbstractPluginResource(string resourceName)
        : base(resourceName)
    {
        DisplayName = resourceName;
    }


    /// <summary>
    /// 显示名称。
    /// </summary>
    public string DisplayName
    {
        get => GetString(nameof(ResourceName));
        set => AddOrSet(nameof(ResourceName), value);
    }

}
