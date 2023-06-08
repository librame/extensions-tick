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
/// 定义实现 <see cref="IPluginInfo"/> 的泛型插件信息接口。
/// </summary>
/// <typeparam name="TInfo">指定实现 <see cref="IPluginInfo"/> 的插件信息类型。</typeparam>
public abstract class AbstractPluginInfo<TInfo> : AbstractPluginInfo
    where TInfo : IPluginInfo
{
    /// <summary>
    /// 信息类型。
    /// </summary>
    public override Type InfoType
        => typeof(TInfo);
}


/// <summary>
/// 定义实现 <see cref="IPluginInfo"/> 的插件信息接口。
/// </summary>
public abstract class AbstractPluginInfo : IPluginInfo
{
    /// <summary>
    /// 信息程序集。
    /// </summary>
    public virtual Assembly InfoAssembly
        => InfoType.Assembly;

    /// <summary>
    /// 信息类型。
    /// </summary>
    public virtual Type InfoType
        => GetType();


    /// <summary>
    /// 标识。
    /// </summary>
    public virtual Guid Id
        => Guid.NewGuid();

    /// <summary>
    /// 优先级。
    /// </summary>
    public float Priority { get; set; } = 9;

    /// <summary>
    /// 名称。
    /// </summary>
    public abstract string Name { get; }

    /// <summary>
    /// 显示名称（通常为本地化名称）。
    /// </summary>
    public virtual string DisplayName
        => Localizer?.GetString(nameof(DisplayName)) ?? Name;

    /// <summary>
    /// 作者集合。
    /// </summary>
    public abstract string? Authors { get; }

    /// <summary>
    /// 联系。
    /// </summary>
    public abstract string? Contact { get; }

    /// <summary>
    /// 公司。
    /// </summary>
    public virtual string? Company
        => InfoAssembly.GetCustomAttribute<AssemblyCompanyAttribute>()?.Company;

    /// <summary>
    /// 版权。
    /// </summary>
    public virtual string? Copyright
        => InfoAssembly.GetCustomAttribute<AssemblyCopyrightAttribute>()?.Copyright;

    /// <summary>
    /// 商标。
    /// </summary>
    public virtual string? Trademark
        => InfoAssembly.GetCustomAttribute<AssemblyTrademarkAttribute>()?.Trademark;

    /// <summary>
    /// 版本。
    /// </summary>
    public virtual string? Version
        => InfoAssembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;

    /// <summary>
    /// 文件版本。
    /// </summary>
    public virtual string? FileVersion
        => InfoAssembly.GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version;

    /// <summary>
    /// 目标框架。
    /// </summary>
    public virtual string? TargetFramework
        => InfoAssembly.GetCustomAttribute<TargetFrameworkAttribute>()?.FrameworkName;

    /// <summary>
    /// 本地化字符串定位器。
    /// </summary>
    public abstract IStringLocalizer? Localizer { get; }


    /// <summary>
    /// 是否相等。
    /// </summary>
    /// <param name="other">给定的 <see cref="IPluginInfo"/>。</param>
    /// <returns>返回布尔值。</returns>
    public virtual bool Equals(IPluginInfo? other)
        => Id == other?.Id;

    /// <summary>
    /// 重写是否相等。
    /// </summary>
    /// <param name="obj">给定要比较的对象。</param>
    /// <returns>返回布尔值。</returns>
    public override bool Equals(object? obj)
        => (obj is IPluginInfo other) && Equals(other);


    /// <summary>
    /// 获取哈希码。
    /// </summary>
    /// <returns>返回 32 位整数。</returns>
    public override int GetHashCode()
        => ToString().GetHashCode();


    /// <summary>
    /// 转换为字符串。
    /// </summary>
    /// <returns>返回字符串。</returns>
    public override string ToString()
        => $"{Id:N}:{Name}";

}
