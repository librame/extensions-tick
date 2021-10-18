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
/// 定义实现 <see cref="IPluginInfo"/> 的 Librame 插件信息接口。
/// </summary>
/// <typeparam name="TInfo">指定实现 <see cref="IPluginInfo"/> 的插件信息类型。</typeparam>
public abstract class AbstractLibramePluginInfo<TInfo> : AbstractPluginInfo<TInfo>
    where TInfo : IPluginInfo
{
    /// <summary>
    /// 作者集合。
    /// </summary>
    public override string? Authors
        => "Librame Pong";

    /// <summary>
    /// 联系。
    /// </summary>
    public override string? Contact
        => InfoAssembly.GetCustomAttribute<AssemblyMetadataAttribute>()?.Value; // RepositoryUrl
}
