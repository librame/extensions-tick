#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Infrastructure;

/// <summary>
/// 定义继承 <see cref="IPlugin"/> 的泛型插件接口。
/// </summary>
/// <typeparam name="TSource">指定的插件源类型。</typeparam>
public interface IPlugin<TSource> : IPlugin
{
    /// <summary>
    /// 获取插件源。
    /// </summary>
    /// <value>
    /// 返回 <typeparamref name="TSource"/> 类型的插件源。
    /// </value>
    TSource Source { get; }
}


/// <summary>
/// 定义插件接口。
/// </summary>
public interface IPlugin
{
    /// <summary>
    /// 获取插件定型。
    /// </summary>
    /// <value>
    /// 返回 <see cref="Type"/>。
    /// </value>
    Type Typed { get; }

    /// <summary>
    /// 获取插件标识。
    /// </summary>
    /// <value>
    /// 返回标识字符串。
    /// </value>
    string Id { get; }

    /// <summary>
    /// 获取插件名称。
    /// </summary>
    /// <value>
    /// 返回名称字符串。
    /// </value>
    string Name { get; }

    /// <summary>
    /// 获取插件描述。
    /// </summary>
    /// <value>
    /// 返回描述字符串。
    /// </value>
    string Description { get; }

    /// <summary>
    /// 获取插件版本信息。
    /// </summary>
    /// <value>
    /// 返回 <see cref="Version"/>。
    /// </value>
    Version VersionInfo { get; }

    /// <summary>
    /// 获取插件是否为内部插件。
    /// </summary>
    /// <value>
    /// 返回是否为内部插件的布尔值。
    /// </value>
    bool IsInternal { get; }

    /// <summary>
    /// 获取或设置插件是否已启用。
    /// </summary>
    /// <value>
    /// 返回是否启用的布尔值。
    /// </value>
    bool IsEnabled { get; set; }


    /// <summary>
    /// 使用插件。
    /// </summary>
    /// <returns>
    /// 返回当前 <see cref="IPlugin"/> 实例。
    /// </returns>
    IPlugin Use();
}
