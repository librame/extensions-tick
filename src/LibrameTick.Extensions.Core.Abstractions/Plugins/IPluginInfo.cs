#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Data;

namespace Librame.Extensions.Plugins;

/// <summary>
/// 定义一个继承 <see cref="IEquatable{IPluginInfo}"/>、<see cref="IPrioritization{Single}"/> 的插件信息接口。
/// </summary>
public interface IPluginInfo : IEquatable<IPluginInfo>, IPrioritization<float>
{
    /// <summary>
    /// 信息程序集。
    /// </summary>
    Assembly InfoAssembly { get; }

    /// <summary>
    /// 信息类型。
    /// </summary>
    Type InfoType { get; }


    /// <summary>
    /// 标识。
    /// </summary>
    Guid Id { get; }

    /// <summary>
    /// 名称。
    /// </summary>
    string Name { get; }

    /// <summary>
    /// 显示名称（通常为本地化名称）。
    /// </summary>
    string DisplayName { get; }

    /// <summary>
    /// 作者集合。
    /// </summary>
    string? Authors { get; }

    /// <summary>
    /// 联系。
    /// </summary>
    string? Contact { get; }

    /// <summary>
    /// 公司。
    /// </summary>
    string? Company { get; }

    /// <summary>
    /// 版权。
    /// </summary>
    string? Copyright { get; }

    /// <summary>
    /// 商标。
    /// </summary>
    string? Trademark { get; }

    /// <summary>
    /// 版本。
    /// </summary>
    string? Version { get; }

    /// <summary>
    /// 文件版本。
    /// </summary>
    string? FileVersion { get; }

    /// <summary>
    /// 目标框架。
    /// </summary>
    string? TargetFramework { get; }

    /// <summary>
    /// 本地化字符串定位器。
    /// </summary>
    IStringLocalizer? Localizer { get; }
}
