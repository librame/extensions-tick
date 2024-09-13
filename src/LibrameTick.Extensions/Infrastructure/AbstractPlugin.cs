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
/// 定义抽象继承 <see cref="AbstractPlugin"/> 并实现 <see cref="IPlugin{TSource}"/> 的泛型插件。
/// </summary>
/// <typeparam name="TSource">指定的插件源类型。</typeparam>
public abstract class AbstractPlugin<TSource>(TSource source) : AbstractPlugin, IPlugin<TSource>
{
    /// <summary>
    /// 获取插件源。
    /// </summary>
    /// <value>
    /// 返回 <typeparamref name="TSource"/> 类型的插件源。
    /// </value>
    public TSource Source { get; init; } = source;
}


/// <summary>
/// 定义抽象实现 <see cref="IPlugin"/> 的插件。
/// </summary>
public abstract class AbstractPlugin : IPlugin
{
    /// <summary>
    /// 默认版本信息。
    /// </summary>
    /// <remarks>
    /// 默认版本为 1.0.0.0。
    /// </remarks>
    public static readonly Version DefaultVersionInfo = new(1, 0, 0, 0);


    /// <summary>
    /// 获取插件定型。
    /// </summary>
    /// <value>
    /// 返回当前插件类型实例。
    /// </value>
    public virtual Type Typed => GetType();

    /// <summary>
    /// 获取插件标识。
    /// </summary>
    /// <remarks>
    /// 默认使用当前类型 GUID。
    /// </remarks>
    /// <value>
    /// 返回标识字符串。
    /// </value>
    public virtual string Id => Typed.GUID.ToString("N");

    /// <summary>
    /// 获取插件名称。
    /// </summary>
    /// <remarks>
    /// 默认使用当前类型名称。
    /// </remarks>
    /// <value>
    /// 返回名称字符串。
    /// </value>
    public virtual string Name => Typed.Name;

    /// <summary>
    /// 获取插件描述。
    /// </summary>
    /// <remarks>
    /// 默认使用当前类型名称。
    /// </remarks>
    /// <value>
    /// 返回描述字符串。
    /// </value>
    public virtual string Description => Typed.Name;

    /// <summary>
    /// 获取插件版本信息。
    /// </summary>
    /// <remarks>
    /// 默认尝试从当前插件程序集获取版本信息，若获取失败则返回 <see cref="DefaultVersionInfo"/>。
    /// </remarks>
    /// <value>
    /// 返回 <see cref="Version"/>。
    /// </value>
    public virtual Version VersionInfo
        => Typed.Assembly.GetName().Version ?? DefaultVersionInfo;

    /// <summary>
    /// 获取插件是否为内部插件。
    /// </summary>
    /// <remarks>
    /// 默认不是内部插件。
    /// </remarks>
    /// <value>
    /// 返回是否为内部插件的布尔值。
    /// </value>
    public bool IsInternal { get; protected set; }

    /// <summary>
    /// 获取或设置插件是否已启用。
    /// </summary>
    /// <remarks>
    /// 默认启用插件。
    /// </remarks>
    /// <value>
    /// 返回是否启用的布尔值。
    /// </value>
    public bool IsEnabled { get; set; } = true;


    /// <summary>
    /// 使用插件。
    /// </summary>
    /// <returns>
    /// 返回当前 <see cref="IPlugin"/> 实例。
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// The plugin is not enabled.
    /// </exception>
    public virtual IPlugin Use()
    {
        if (!IsEnabled)
        {
            throw new InvalidOperationException($"The plugin '{Name}' is not enabled.");
        }

        return this;
    }

}
