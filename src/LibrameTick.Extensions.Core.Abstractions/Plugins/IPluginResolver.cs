#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Plugins;

/// <summary>
/// 定义一个插件解析器接口。
/// </summary>
public interface IPluginResolver
{
    /// <summary>
    /// 解析插件信息列表。
    /// </summary>
    /// <returns>返回 <see cref="IReadOnlyList{IPluginInfo}"/>。</returns>
    IReadOnlyList<IPluginInfo> ResolveInfos();
}
