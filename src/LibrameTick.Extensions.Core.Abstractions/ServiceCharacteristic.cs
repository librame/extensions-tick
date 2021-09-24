#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core;

/// <summary>
/// 服务特征。
/// </summary>
public class ServiceCharacteristic : IEquatable<ServiceCharacteristic>
{
    /// <summary>
    /// 构造一个 <see cref="ServiceCharacteristic"/>。
    /// </summary>
    /// <param name="serviceType">给定的服务类型。</param>
    /// <param name="replaceIfExists">替换已存在的服务（可选；默认不替换）。</param>
    /// <param name="addImplementationType">添加实现类型。如果启用，则在默认添加服务类型的基础上，再添加实现类型（可选；默认不添加；此项对添加服务集合无效）。</param>
    /// <param name="lifetime">给定的 <see cref="ServiceLifetime"/>（可选；默认使用 <see cref="ServiceLifetime.Singleton"/>）。</param>
    public ServiceCharacteristic(Type serviceType, bool replaceIfExists = false,
        bool addImplementationType = false, ServiceLifetime lifetime = ServiceLifetime.Singleton)
    {
        ServiceType = serviceType;
        Lifetime = lifetime;
        ReplaceIfExists = replaceIfExists;
        AddImplementationType = addImplementationType;
    }


    /// <summary>
    /// 服务类型。
    /// </summary>
    public Type ServiceType { get; init; }

    /// <summary>
    /// 添加实现类型本身。如果启用，则在添加服务类型的基础上，再添加实现类型作为服务类型（此项对添加服务集合无效）。
    /// </summary>
    public bool AddImplementationType { get; set; }

    /// <summary>
    /// 是否替换已存在的服务。
    /// </summary>
    public bool ReplaceIfExists { get; set; }

    /// <summary>
    /// 服务的生命周期。
    /// </summary>
    public ServiceLifetime Lifetime { get; set; }


    /// <summary>
    /// 比较相等。
    /// </summary>
    /// <param name="other">给定的 <see cref="ServiceCharacteristic"/>。</param>
    /// <returns>返回布尔值。</returns>
    public bool Equals(ServiceCharacteristic? other)
        => other is not null && ServiceType == other.ServiceType;


    /// <summary>
    /// 单例服务。
    /// </summary>
    /// <param name="serviceType">给定的服务类型。</param>
    /// <param name="replaceIfExists">是否替换已存在的服务（可选；默认不替换）。</param>
    /// <param name="addImplementationType">添加实现类型。如果启用，则在默认添加服务类型的基础上，再添加实现类型（可选；默认不添加；此项对添加服务集合无效）。</param>
    /// <returns>返回 <see cref="ServiceCharacteristic"/>。</returns>
    public static ServiceCharacteristic Singleton(Type serviceType, bool replaceIfExists = false, bool addImplementationType = false)
        => new ServiceCharacteristic(serviceType, replaceIfExists, addImplementationType);

    /// <summary>
    /// 域例服务。
    /// </summary>
    /// <param name="serviceType">给定的服务类型。</param>
    /// <param name="replaceIfExists">是否替换已存在的服务（可选；默认不替换）。</param>
    /// <param name="addImplementationType">添加实现类型。如果启用，则在默认添加服务类型的基础上，再添加实现类型（可选；默认不添加；此项对添加服务集合无效）。</param>
    /// <returns>返回 <see cref="ServiceCharacteristic"/>。</returns>
    public static ServiceCharacteristic Scope(Type serviceType, bool replaceIfExists = false, bool addImplementationType = false)
        => new ServiceCharacteristic(serviceType, replaceIfExists, addImplementationType, ServiceLifetime.Scoped);

    /// <summary>
    /// 瞬例服务。
    /// </summary>
    /// <param name="serviceType">给定的服务类型。</param>
    /// <param name="replaceIfExists">是否替换已存在的服务（可选；默认不替换）。</param>
    /// <param name="addImplementationType">添加实现类型。如果启用，则在默认添加服务类型的基础上，再添加实现类型（可选；默认不添加；此项对添加服务集合无效）。</param>
    /// <returns>返回 <see cref="ServiceCharacteristic"/>。</returns>
    public static ServiceCharacteristic Transient(Type serviceType, bool replaceIfExists = false, bool addImplementationType = false)
        => new ServiceCharacteristic(serviceType, replaceIfExists, addImplementationType, ServiceLifetime.Transient);

}
