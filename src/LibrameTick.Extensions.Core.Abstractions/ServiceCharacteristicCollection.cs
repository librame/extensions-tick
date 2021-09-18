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
/// 服务特征集合。
/// </summary>
public class ServiceCharacteristicCollection : Dictionary<Type, ServiceCharacteristic>
{
    /// <summary>
    /// 构造一个 <see cref="ServiceCharacteristicCollection"/>。
    /// </summary>
    public ServiceCharacteristicCollection()
    {
    }

    /// <summary>
    /// 构造一个 <see cref="ServiceCharacteristicCollection"/>。
    /// </summary>
    /// <param name="dictionary">给定的 <see cref="IDictionary{Type, ServiceCharacteristic}"/>。</param>
    public ServiceCharacteristicCollection(IDictionary<Type, ServiceCharacteristic> dictionary)
        : base(dictionary)
    {
    }

    /// <summary>
    /// 构造一个 <see cref="ServiceCharacteristicCollection"/>。
    /// </summary>
    /// <param name="collection">给定的 <see cref="KeyValuePair{Type, ServiceCharacteristic}"/> 可枚举集合。</param>
    public ServiceCharacteristicCollection(IEnumerable<KeyValuePair<Type, ServiceCharacteristic>> collection)
        : base(collection)
    {
    }


    /// <summary>
    /// 添加单例服务特征。
    /// </summary>
    /// <typeparam name="TService">指定的服务类型。</typeparam>
    /// <param name="replaceIfExists">是否替换已存在的服务（可选；默认不替换）。</param>
    /// <param name="addImplementationType">添加实现类型。如果启用，则在默认添加服务类型的基础上，再添加实现类型（可选；默认不添加；此项对添加服务集合无效）。</param>
    /// <returns>返回 <see cref="ServiceCharacteristicCollection"/>。</returns>
    public ServiceCharacteristicCollection AddSingleton<TService>(bool replaceIfExists = false,
        bool addImplementationType = false)
        where TService : class
        => AddSingleton<TService>(out _, replaceIfExists, addImplementationType);

    /// <summary>
    /// 添加单例服务特征。
    /// </summary>
    /// <typeparam name="TService">指定的服务类型。</typeparam>
    /// <param name="characteristic">输出 <see cref="ServiceCharacteristic"/>。</param>
    /// <param name="replaceIfExists">是否替换已存在的服务（可选；默认不替换）。</param>
    /// <param name="addImplementationType">添加实现类型。如果启用，则在默认添加服务类型的基础上，再添加实现类型（可选；默认不添加；此项对添加服务集合无效）。</param>
    /// <returns>返回 <see cref="ServiceCharacteristicCollection"/>。</returns>
    public ServiceCharacteristicCollection AddSingleton<TService>(out ServiceCharacteristic characteristic,
        bool replaceIfExists = false, bool addImplementationType = false)
        where TService : class
        => AddSingleton(typeof(TService), out characteristic, replaceIfExists, addImplementationType);

    /// <summary>
    /// 添加单例服务特征。
    /// </summary>
    /// <param name="serviceType">给定的服务类型。</param>
    /// <param name="replaceIfExists">是否替换已存在的服务（可选；默认不替换）。</param>
    /// <param name="addImplementationType">添加实现类型。如果启用，则在默认添加服务类型的基础上，再添加实现类型（可选；默认不添加；此项对添加服务集合无效）。</param>
    /// <returns>返回 <see cref="ServiceCharacteristicCollection"/>。</returns>
    public ServiceCharacteristicCollection AddSingleton(Type serviceType, bool replaceIfExists = false,
        bool addImplementationType = false)
        => AddSingleton(serviceType, out _, replaceIfExists, addImplementationType);

    /// <summary>
    /// 添加单例服务特征。
    /// </summary>
    /// <param name="serviceType">给定的服务类型。</param>
    /// <param name="characteristic">输出 <see cref="ServiceCharacteristic"/>。</param>
    /// <param name="replaceIfExists">是否替换已存在的服务（可选；默认不替换）。</param>
    /// <param name="addImplementationType">添加实现类型。如果启用，则在默认添加服务类型的基础上，再添加实现类型（可选；默认不添加；此项对添加服务集合无效）。</param>
    /// <returns>返回 <see cref="ServiceCharacteristicCollection"/>。</returns>
    public ServiceCharacteristicCollection AddSingleton(Type serviceType,
        out ServiceCharacteristic characteristic, bool replaceIfExists = false, bool addImplementationType = false)
    {
        Add(characteristic = ServiceCharacteristic.Singleton(serviceType, replaceIfExists, addImplementationType));
        return this;
    }


    /// <summary>
    /// 添加域例服务特征。
    /// </summary>
    /// <typeparam name="TService">指定的服务类型。</typeparam>
    /// <param name="replaceIfExists">是否替换已存在的服务（可选；默认不替换）。</param>
    /// <param name="addImplementationType">添加实现类型。如果启用，则在默认添加服务类型的基础上，再添加实现类型（可选；默认不添加；此项对添加服务集合无效）。</param>
    /// <returns>返回 <see cref="ServiceCharacteristicCollection"/>。</returns>
    public ServiceCharacteristicCollection AddScope<TService>(bool replaceIfExists = false,
        bool addImplementationType = false)
        where TService : class
        => AddScope<TService>(out _, replaceIfExists, addImplementationType);

    /// <summary>
    /// 添加域例服务特征。
    /// </summary>
    /// <typeparam name="TService">指定的服务类型。</typeparam>
    /// <param name="characteristic">输出 <see cref="ServiceCharacteristic"/>。</param>
    /// <param name="replaceIfExists">是否替换已存在的服务（可选；默认不替换）。</param>
    /// <param name="addImplementationType">添加实现类型。如果启用，则在默认添加服务类型的基础上，再添加实现类型（可选；默认不添加；此项对添加服务集合无效）。</param>
    /// <returns>返回 <see cref="ServiceCharacteristicCollection"/>。</returns>
    public ServiceCharacteristicCollection AddScope<TService>(out ServiceCharacteristic characteristic,
        bool replaceIfExists = false, bool addImplementationType = false)
        where TService : class
        => AddScope(typeof(TService), out characteristic, replaceIfExists, addImplementationType);

    /// <summary>
    /// 添加域例服务特征。
    /// </summary>
    /// <param name="serviceType">给定的服务类型。</param>
    /// <param name="replaceIfExists">是否替换已存在的服务（可选；默认不替换）。</param>
    /// <param name="addImplementationType">添加实现类型。如果启用，则在默认添加服务类型的基础上，再添加实现类型（可选；默认不添加；此项对添加服务集合无效）。</param>
    /// <returns>返回 <see cref="ServiceCharacteristicCollection"/>。</returns>
    public ServiceCharacteristicCollection AddScope(Type serviceType, bool replaceIfExists = false,
        bool addImplementationType = false)
        => AddScope(serviceType, out _, replaceIfExists, addImplementationType);

    /// <summary>
    /// 添加域例服务特征。
    /// </summary>
    /// <param name="serviceType">给定的服务类型。</param>
    /// <param name="characteristic">输出 <see cref="ServiceCharacteristic"/>。</param>
    /// <param name="replaceIfExists">是否替换已存在的服务（可选；默认不替换）。</param>
    /// <param name="addImplementationType">添加实现类型。如果启用，则在默认添加服务类型的基础上，再添加实现类型（可选；默认不添加；此项对添加服务集合无效）。</param>
    /// <returns>返回 <see cref="ServiceCharacteristicCollection"/>。</returns>
    public ServiceCharacteristicCollection AddScope(Type serviceType,
        out ServiceCharacteristic characteristic, bool replaceIfExists = false, bool addImplementationType = false)
    {
        Add(characteristic = ServiceCharacteristic.Scope(serviceType, replaceIfExists, addImplementationType));
        return this;
    }


    /// <summary>
    /// 添加瞬例服务特征。
    /// </summary>
    /// <typeparam name="TService">指定的服务类型。</typeparam>
    /// <param name="replaceIfExists">是否替换已存在的服务（可选；默认不替换）。</param>
    /// <param name="addImplementationType">添加实现类型。如果启用，则在默认添加服务类型的基础上，再添加实现类型（可选；默认不添加；此项对添加服务集合无效）。</param>
    /// <returns>返回 <see cref="ServiceCharacteristicCollection"/>。</returns>
    public ServiceCharacteristicCollection AddTransient<TService>(bool replaceIfExists = false,
        bool addImplementationType = false)
        where TService : class
        => AddTransient<TService>(out _, replaceIfExists, addImplementationType);

    /// <summary>
    /// 添加瞬例服务特征。
    /// </summary>
    /// <typeparam name="TService">指定的服务类型。</typeparam>
    /// <param name="characteristic">输出 <see cref="ServiceCharacteristic"/>。</param>
    /// <param name="replaceIfExists">是否替换已存在的服务（可选；默认不替换）。</param>
    /// <param name="addImplementationType">添加实现类型。如果启用，则在默认添加服务类型的基础上，再添加实现类型（可选；默认不添加；此项对添加服务集合无效）。</param>
    /// <returns>返回 <see cref="ServiceCharacteristicCollection"/>。</returns>
    public ServiceCharacteristicCollection AddTransient<TService>(out ServiceCharacteristic characteristic,
        bool replaceIfExists = false, bool addImplementationType = false)
        where TService : class
        => AddTransient(typeof(TService), out characteristic, replaceIfExists, addImplementationType);

    /// <summary>
    /// 添加瞬例服务特征。
    /// </summary>
    /// <param name="serviceType">给定的服务类型。</param>
    /// <param name="replaceIfExists">是否替换已存在的服务（可选；默认不替换）。</param>
    /// <param name="addImplementationType">添加实现类型。如果启用，则在默认添加服务类型的基础上，再添加实现类型（可选；默认不添加；此项对添加服务集合无效）。</param>
    /// <returns>返回 <see cref="ServiceCharacteristicCollection"/>。</returns>
    public ServiceCharacteristicCollection AddTransient(Type serviceType, bool replaceIfExists = false,
        bool addImplementationType = false)
        => AddTransient(serviceType, out _, replaceIfExists, addImplementationType);

    /// <summary>
    /// 添加瞬例服务特征。
    /// </summary>
    /// <param name="serviceType">给定的服务类型。</param>
    /// <param name="characteristic">输出 <see cref="ServiceCharacteristic"/>。</param>
    /// <param name="replaceIfExists">是否替换已存在的服务（可选；默认不替换）。</param>
    /// <param name="addImplementationType">添加实现类型。如果启用，则在默认添加服务类型的基础上，再添加实现类型（可选；默认不添加；此项对添加服务集合无效）。</param>
    /// <returns>返回 <see cref="ServiceCharacteristicCollection"/>。</returns>
    public ServiceCharacteristicCollection AddTransient(Type serviceType,
        out ServiceCharacteristic characteristic, bool replaceIfExists = false, bool addImplementationType = false)
    {
        Add(characteristic = ServiceCharacteristic.Transient(serviceType, replaceIfExists, addImplementationType));
        return this;
    }


    /// <summary>
    /// 添加服务特征。
    /// </summary>
    /// <param name="characteristic">给定的 <see cref="ServiceCharacteristic"/>。</param>
    /// <returns>返回 <see cref="ServiceCharacteristicCollection"/>。</returns>
    public ServiceCharacteristicCollection Add(ServiceCharacteristic characteristic)
    {
        Add(characteristic.ServiceType, characteristic);
        return this;
    }

}
