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
/// 定义一个继承 <see cref="IResourceDictionary"/> 的插件资源接口。
/// </summary>
public interface IPluginResource : IResourceDictionary
{
    /// <summary>
    /// 显示名称。
    /// </summary>
    string DisplayName { get; set; }
}
