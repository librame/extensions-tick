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
/// <see cref="IServiceCollection"/> 静态扩展。
/// </summary>
public static class ServiceCollectionExtensions
{
    private static Func<ServiceDescriptor, bool> GetPredicateDescriptor(Type serviceType,
        Type? implementationType = null)
    {
        if (implementationType is null)
            return p => p.ServiceType == serviceType;
        else
            return p => p.ServiceType == serviceType && p.ImplementationType == implementationType;
    }


    /// <summary>
    /// 包含服务类型。
    /// </summary>
    /// <typeparam name="TService">指定的服务类型。</typeparam>
    /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
    /// <returns>返回布尔值。</returns>
    public static bool ContainsService<TService>(this IServiceCollection services)
        => services.ContainsService(typeof(TService));

    /// <summary>
    /// 包含服务类型。
    /// </summary>
    /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
    /// <param name="serviceType">给定的服务类型。</param>
    /// <returns>返回布尔值。</returns>
    public static bool ContainsService(this IServiceCollection services, Type serviceType)
        => services.FirstOrDefault(p => p.ServiceType == serviceType) is not null;


    #region TryAddOrReplaceByCharacteristic

    /// <summary>
    /// 通过特征尝试添加或替换服务。
    /// </summary>
    /// <typeparam name="TImplementation">指定的实现类型。</typeparam>
    /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
    /// <param name="characteristic">给定的 <see cref="ServiceCharacteristic"/>。</param>
    /// <param name="factory">给定的服务对象工厂方法。</param>
    /// <returns>返回 <see cref="IServiceCollection"/>。</returns>
    public static IServiceCollection TryAddOrReplaceByCharacteristic<TImplementation>(this IServiceCollection services,
        ServiceCharacteristic characteristic, Func<IServiceProvider, TImplementation> factory)
        where TImplementation : class
    {
        var descriptor = new ServiceDescriptor(characteristic.ServiceType, factory, characteristic.Lifetime);
        return services.TryAddOrReplaceByCharacteristic(characteristic, descriptor);
    }

    /// <summary>
    /// 通过特征尝试添加或替换服务。
    /// </summary>
    /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
    /// <param name="characteristic">给定的 <see cref="ServiceCharacteristic"/>。</param>
    /// <param name="factory">给定的服务对象工厂方法。</param>
    /// <returns>返回 <see cref="IServiceCollection"/>。</returns>
    public static IServiceCollection TryAddOrReplaceByCharacteristic(this IServiceCollection services,
        ServiceCharacteristic characteristic, Func<IServiceProvider, object> factory)
    {
        var descriptor = new ServiceDescriptor(characteristic.ServiceType, factory, characteristic.Lifetime);
        return services.TryAddOrReplaceByCharacteristic(characteristic, descriptor);
    }

    /// <summary>
    /// 通过特征尝试添加或替换服务。
    /// </summary>
    /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
    /// <param name="characteristic">给定的 <see cref="ServiceCharacteristic"/>。</param>
    /// <param name="implementationType">给定的实现类型。</param>
    /// <returns>返回 <see cref="IServiceCollection"/>。</returns>
    public static IServiceCollection TryAddOrReplaceByCharacteristic(this IServiceCollection services,
        ServiceCharacteristic characteristic, Type implementationType)
    {
        var descriptor = new ServiceDescriptor(characteristic.ServiceType, implementationType, characteristic.Lifetime);
        return services.TryAddOrReplaceByCharacteristic(characteristic, descriptor);
    }

    /// <summary>
    /// 通过特征尝试添加或替换服务。
    /// </summary>
    /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
    /// <param name="characteristic">给定的 <see cref="ServiceCharacteristic"/>。</param>
    /// <param name="descriptor">给定的 <see cref="ServiceDescriptor"/>。</param>
    /// <returns>返回 <see cref="IServiceCollection"/>。</returns>
    public static IServiceCollection TryAddOrReplaceByCharacteristic(this IServiceCollection services,
        ServiceCharacteristic characteristic, ServiceDescriptor descriptor)
    {
        // 如果已存在相同服务类型
        if (services.Any(p => p.ServiceType == descriptor.ServiceType))
        {
            // 如果不替换则跳过
            if (!characteristic.ReplaceIfExists)
                return services;

            // 替换服务（只替换单个与服务类型匹配的服务描述符）
            services.Replace(descriptor);
            return services;
        }

        // 添加服务
        services.Add(descriptor);
        return services;
    }

    #endregion


    #region TryAddEnumerable

    /// <summary>
    /// 尝试添加可枚举服务集合（默认会忽略已注册的服务类型与实现类型）。
    /// </summary>
    /// <typeparam name="TService">指定的服务类型。</typeparam>
    /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
    /// <param name="implementationTypes">给定的实现类型集合。</param>
    /// <param name="lifetime">给定的 <see cref="ServiceLifetime"/>（可选；默认为单例）。</param>
    public static void TryAddEnumerable<TService>(this IServiceCollection services,
        IEnumerable<Type> implementationTypes,
        ServiceLifetime lifetime = ServiceLifetime.Singleton)
        => services.TryAddEnumerable(typeof(TService), implementationTypes, lifetime);

    /// <summary>
    /// 尝试添加可枚举服务集合（默认会忽略已注册的服务类型与实现类型）。
    /// </summary>
    /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
    /// <param name="serviceType">给定的服务类型。</param>
    /// <param name="implementationTypes">给定的实现类型集合。</param>
    /// <param name="lifetime">给定的 <see cref="ServiceLifetime"/>（可选；默认为单例）。</param>
    public static void TryAddEnumerable(this IServiceCollection services,
        Type serviceType, IEnumerable<Type> implementationTypes,
        ServiceLifetime lifetime = ServiceLifetime.Singleton)
    {
        var descriptors = implementationTypes
            .Select(implType => new ServiceDescriptor(serviceType, implType, lifetime));

        services.TryAddEnumerable(descriptors);
    }


    /// <summary>
    /// 通过特征尝试添加可枚举服务集合（默认会忽略已注册的服务类型与实现类型）。
    /// </summary>
    /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
    /// <param name="characteristic">给定的 <see cref="ServiceCharacteristic"/>。</param>
    /// <param name="implementationType">给定的实现类型。</param>
    /// <returns>返回 <see cref="IServiceCollection"/>。</returns>
    public static IServiceCollection TryAddEnumerableByCharacteristic(this IServiceCollection services,
        ServiceCharacteristic characteristic, Type implementationType)
        => services.TryAddEnumerableByCharacteristic(characteristic, implementationType, out _);

    /// <summary>
    /// 通过特征尝试添加可枚举服务集合（默认会忽略已注册的服务类型与实现类型）。
    /// </summary>
    /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
    /// <param name="characteristic">给定的 <see cref="ServiceCharacteristic"/>。</param>
    /// <param name="implementationType">给定的实现类型。</param>
    /// <param name="descriptor">输出 <see cref="ServiceDescriptor"/>。</param>
    /// <returns>返回 <see cref="IServiceCollection"/>。</returns>
    public static IServiceCollection TryAddEnumerableByCharacteristic(this IServiceCollection services,
        ServiceCharacteristic characteristic, Type implementationType, out ServiceDescriptor descriptor)
    {
        descriptor = new ServiceDescriptor(characteristic.ServiceType, implementationType, characteristic.Lifetime);

        services.TryAddEnumerable(descriptor);

        return services;
    }

    /// <summary>
    /// 通过特征尝试添加可枚举服务集合（默认会忽略已注册的服务类型与实现类型）。
    /// </summary>
    /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
    /// <param name="implementationTypes">给定的实现类型集合。</param>
    /// <param name="characteristic">给定的 <see cref="ServiceCharacteristic"/>。</param>
    /// <returns>返回 <see cref="IServiceCollection"/>。</returns>
    public static IServiceCollection TryAddEnumerableByCharacteristic(this IServiceCollection services,
        ServiceCharacteristic characteristic, IEnumerable<Type> implementationTypes)
        => services.TryAddEnumerableByCharacteristic(characteristic, implementationTypes, out _);

    /// <summary>
    /// 通过特征尝试添加可枚举服务集合（默认会忽略已注册的服务类型与实现类型）。
    /// </summary>
    /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
    /// <param name="characteristic">给定的 <see cref="ServiceCharacteristic"/>。</param>
    /// <param name="implementationTypes">给定的实现类型集合。</param>
    /// <param name="descriptors">输出 <see cref="IEnumerable{ServiceDescriptor}"/>。</param>
    /// <returns>返回 <see cref="IServiceCollection"/>。</returns>
    public static IServiceCollection TryAddEnumerableByCharacteristic(this IServiceCollection services,
        ServiceCharacteristic characteristic, IEnumerable<Type> implementationTypes,
        out IEnumerable<ServiceDescriptor> descriptors)
    {
        descriptors = implementationTypes.Select(implType =>
        {
            return new ServiceDescriptor(characteristic.ServiceType, implType, characteristic.Lifetime);
        });

        services.TryAddEnumerable(descriptors);

        return services;
    }

    #endregion


    #region TryGet

    /// <summary>
    /// 尝试获取指定类型的所有服务描述符。
    /// </summary>
    /// <typeparam name="TService">指定的服务类型。</typeparam>
    /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
    /// <param name="descriptors">输出 <see cref="IReadOnlyList{ServiceDescriptor}"/>。</param>
    /// <returns>返回是否成功获取的布尔值。</returns>
    public static bool TryGetAll<TService>(this IServiceCollection services,
        [MaybeNullWhen(false)] out IReadOnlyList<ServiceDescriptor> descriptors)
        where TService : class
        => services.TryGetAll(typeof(TService), out descriptors);

    /// <summary>
    /// 尝试获取指定类型的所有服务描述符。
    /// </summary>
    /// <typeparam name="TService">指定的服务类型。</typeparam>
    /// <typeparam name="TImplementation">指定的实现类型。</typeparam>
    /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
    /// <param name="descriptors">输出 <see cref="IReadOnlyList{ServiceDescriptor}"/>。</param>
    /// <returns>返回是否成功获取的布尔值。</returns>
    public static bool TryGetAll<TService, TImplementation>(this IServiceCollection services,
        [MaybeNullWhen(false)] out IReadOnlyList<ServiceDescriptor> descriptors)
        where TService : class
        where TImplementation : class, TService
        => services.TryGetAll(typeof(TService), out descriptors, typeof(TImplementation));

    /// <summary>
    /// 尝试获取指定类型的所有服务描述符。
    /// </summary>
    /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
    /// <param name="serviceType">给定的服务类型。</param>
    /// <param name="descriptors">输出 <see cref="IReadOnlyList{ServiceDescriptor}"/>。</param>
    /// <param name="implementationType">给定的实现类型（可选）。</param>
    /// <returns>返回是否成功获取的布尔值。</returns>
    public static bool TryGetAll(this IServiceCollection services, Type serviceType,
        [MaybeNullWhen(false)] out IReadOnlyList<ServiceDescriptor> descriptors, Type? implementationType = null)
    {
        // 存在多个相同服务与实现类型的服务集合
        descriptors = services.Where(GetPredicateDescriptor(serviceType, implementationType)).AsReadOnlyCollection();
        return descriptors.Count > 0;
    }


    /// <summary>
    /// 尝试获取指定类型的服务描述符。
    /// </summary>
    /// <typeparam name="TService">指定的服务类型。</typeparam>
    /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
    /// <param name="descriptor">输出 <see cref="ServiceDescriptor"/>。</param>
    /// <param name="implementationType">给定的服务实现类型（可选）。</param>
    /// <returns>返回是否成功获取的布尔值。</returns>
    public static bool TryGetSingle<TService>(this IServiceCollection services,
        [MaybeNullWhen(false)] out ServiceDescriptor descriptor, Type? implementationType = null)
        => services.TryGetSingle(typeof(TService), out descriptor, implementationType);

    /// <summary>
    /// 尝试获取指定类型的服务描述符。
    /// </summary>
    /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
    /// <param name="serviceType">给定的服务类型。</param>
    /// <param name="descriptor">输出 <see cref="ServiceDescriptor"/>。</param>
    /// <param name="implementationType">给定的服务实现类型（可选）。</param>
    /// <returns>返回是否成功获取的布尔值。</returns>
    public static bool TryGetSingle(this IServiceCollection services, Type serviceType,
        [MaybeNullWhen(false)] out ServiceDescriptor descriptor, Type? implementationType = null)
    {
        descriptor = services.SingleOrDefault(GetPredicateDescriptor(serviceType, implementationType));
        return descriptor is not null;
    }


    /// <summary>
    /// 获取必需的指定类型服务描述符。
    /// </summary>
    /// <typeparam name="TService">指定的服务类型。</typeparam>
    /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
    /// <param name="implementationType">给定的服务实现类型（可选）。</param>
    /// <returns>返回 <see cref="ServiceDescriptor"/>。</returns>
    public static ServiceDescriptor GetRequiredSingle<TService>(this IServiceCollection services,
        Type? implementationType = null)
        => services.GetRequiredSingle(typeof(TService), implementationType);

    /// <summary>
    /// 获取必需的指定类型服务描述符。
    /// </summary>
    /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
    /// <param name="serviceType">给定的服务类型。</param>
    /// <param name="implementationType">给定的服务实现类型（可选）。</param>
    /// <returns>返回 <see cref="ServiceDescriptor"/>。</returns>
    public static ServiceDescriptor GetRequiredSingle(this IServiceCollection services, Type serviceType,
        Type? implementationType = null)
    {
        if (!services.TryGetSingle(serviceType, out var descriptor, implementationType))
            throw new ArgumentException($"The service type '{serviceType}' were not found.");

        return descriptor;
    }

    #endregion


    #region TryRemove

    /// <summary>
    /// 尝试移除指定类型的所有服务描述符。
    /// </summary>
    /// <typeparam name="TService">指定的服务类型。</typeparam>
    /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
    /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
    /// <returns>返回是否成功移除的布尔值。</returns>
    public static bool TryRemoveAll<TService>(this IServiceCollection services,
        bool throwIfNotFound = true)
        where TService : class
        => services.TryRemoveAll<TService>(out _, throwIfNotFound);

    /// <summary>
    /// 尝试移除指定类型的所有服务描述符。
    /// </summary>
    /// <typeparam name="TService">指定的服务类型。</typeparam>
    /// <typeparam name="TImplementation">指定的实现类型。</typeparam>
    /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
    /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
    /// <returns>返回是否成功移除的布尔值。</returns>
    public static bool TryRemoveAll<TService, TImplementation>(this IServiceCollection services,
        bool throwIfNotFound = true)
        where TService : class
        where TImplementation : class, TService
        => services.TryRemoveAll<TService, TImplementation>(out _, throwIfNotFound);

    /// <summary>
    /// 尝试移除指定类型的所有服务描述符。
    /// </summary>
    /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
    /// <param name="serviceType">给定的服务类型。</param>
    /// <param name="implementationType">给定的实现类型（可选）。</param>
    /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
    /// <returns>返回是否成功移除的布尔值。</returns>
    public static bool TryRemoveAll(this IServiceCollection services, Type serviceType,
        Type? implementationType = null, bool throwIfNotFound = true)
        => services.TryRemoveAll(serviceType, out _, implementationType, throwIfNotFound);


    /// <summary>
    /// 尝试移除指定类型的所有服务描述符。
    /// </summary>
    /// <typeparam name="TService">指定的服务类型。</typeparam>
    /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
    /// <param name="descriptors">输出 <see cref="IReadOnlyList{ServiceDescriptor}"/>。</param>
    /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
    /// <returns>返回是否成功移除的布尔值。</returns>
    public static bool TryRemoveAll<TService>(this IServiceCollection services,
        [MaybeNullWhen(false)] out IReadOnlyList<ServiceDescriptor> descriptors, bool throwIfNotFound = true)
        where TService : class
        => services.TryRemoveAll(typeof(TService), out descriptors, implementationType: null, throwIfNotFound);

    /// <summary>
    /// 尝试移除指定类型的所有服务描述符。
    /// </summary>
    /// <typeparam name="TService">指定的服务类型。</typeparam>
    /// <typeparam name="TImplementation">指定的实现类型。</typeparam>
    /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
    /// <param name="descriptors">输出 <see cref="IReadOnlyList{ServiceDescriptor}"/>。</param>
    /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
    /// <returns>返回是否成功移除的布尔值。</returns>
    public static bool TryRemoveAll<TService, TImplementation>(this IServiceCollection services,
        [MaybeNullWhen(false)] out IReadOnlyList<ServiceDescriptor> descriptors, bool throwIfNotFound = true)
        where TService : class
        where TImplementation : class, TService
        => services.TryRemoveAll(typeof(TService), out descriptors, typeof(TImplementation), throwIfNotFound);

    /// <summary>
    /// 尝试移除指定类型的所有服务描述符。
    /// </summary>
    /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
    /// <param name="serviceType">给定的服务类型。</param>
    /// <param name="descriptors">输出 <see cref="IReadOnlyList{ServiceDescriptor}"/>。</param>
    /// <param name="implementationType">给定的实现类型（可选）。</param>
    /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
    /// <returns>返回是否成功移除的布尔值。</returns>
    public static bool TryRemoveAll(this IServiceCollection services, Type serviceType,
        [MaybeNullWhen(false)] out IReadOnlyList<ServiceDescriptor> descriptors,
        Type? implementationType = null, bool throwIfNotFound = true)
    {
        if (!services.TryGetAll(serviceType, out descriptors, implementationType))
        {
            if (throwIfNotFound)
                throw new ArgumentNullException($"The service type '{serviceType}' and implementation type (if {nameof(implementationType)} not null) services were not found.");

            return false;
        }

        foreach (var descriptor in descriptors)
        {
            if (!services.Remove(descriptor))
                return false;
        }

        return true;
    }


    /// <summary>
    /// 尝试移除指定类型的服务描述符。
    /// </summary>
    /// <typeparam name="TService">指定的服务类型。</typeparam>
    /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
    /// <param name="implementationType">给定的服务实现类型（可选）。</param>
    /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
    /// <returns>返回是否成功移除的布尔值。</returns>
    public static bool TryRemoveSingle<TService>(this IServiceCollection services,
        Type? implementationType = null, bool throwIfNotFound = true)
        => services.TryRemoveSingle<TService>(out _, implementationType, throwIfNotFound);

    /// <summary>
    /// 尝试移除指定类型的服务描述符。
    /// </summary>
    /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
    /// <param name="serviceType">给定的服务类型。</param>
    /// <param name="implementationType">给定的服务实现类型（可选）。</param>
    /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
    /// <returns>返回是否成功移除的布尔值。</returns>
    public static bool TryRemoveSingle(this IServiceCollection services, Type serviceType,
        Type? implementationType = null, bool throwIfNotFound = true)
        => services.TryRemoveSingle(serviceType, out _, implementationType, throwIfNotFound);


    /// <summary>
    /// 尝试移除指定类型的服务描述符。
    /// </summary>
    /// <typeparam name="TService">指定的服务类型。</typeparam>
    /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
    /// <param name="descriptor">输出 <see cref="ServiceDescriptor"/>。</param>
    /// <param name="implementationType">给定的服务实现类型（可选）。</param>
    /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
    /// <returns>返回是否成功移除的布尔值。</returns>
    public static bool TryRemoveSingle<TService>(this IServiceCollection services,
        [MaybeNullWhen(false)] out ServiceDescriptor descriptor, Type? implementationType = null,
        bool throwIfNotFound = true)
        => services.TryRemoveSingle(typeof(TService), out descriptor, implementationType, throwIfNotFound);

    /// <summary>
    /// 尝试移除指定类型的服务描述符。
    /// </summary>
    /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
    /// <param name="serviceType">给定的服务类型。</param>
    /// <param name="descriptor">输出 <see cref="ServiceDescriptor"/>。</param>
    /// <param name="implementationType">给定的服务实现类型（可选）。</param>
    /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
    /// <returns>返回是否成功移除的布尔值。</returns>
    public static bool TryRemoveSingle(this IServiceCollection services, Type serviceType,
        [MaybeNullWhen(false)] out ServiceDescriptor descriptor, Type? implementationType = null,
        bool throwIfNotFound = true)
    {
        if (!services.TryGetSingle(serviceType, out descriptor, implementationType))
        {
            if (throwIfNotFound)
                throw new ArgumentNullException($"The service type '{serviceType}' and predicate's service were not found.");

            return false;
        }

        return services.Remove(descriptor!);
    }

    #endregion


    #region TryReplace.ImplementationType

    /// <summary>
    /// 尝试替换所有服务描述符。
    /// </summary>
    /// <typeparam name="TService">指定的服务类型。</typeparam>
    /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
    /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
    /// <returns>返回是否成功替换的布尔值。</returns>
    public static bool TryReplaceAll<TService>(this IServiceCollection services,
        bool throwIfNotFound = true)
        where TService : class
        => services.TryReplaceAll<TService>(out _, throwIfNotFound);

    /// <summary>
    /// 尝试替换所有服务描述符。
    /// </summary>
    /// <typeparam name="TService">指定的服务类型。</typeparam>
    /// <typeparam name="TNewImplementation">指定的新实现类型。</typeparam>
    /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
    /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
    /// <returns>返回是否成功替换的布尔值。</returns>
    public static bool TryReplaceAll<TService, TNewImplementation>(this IServiceCollection services,
        bool throwIfNotFound = true)
        where TService : class
        where TNewImplementation : class, TService
        => services.TryReplaceAll<TService, TNewImplementation>(out _, throwIfNotFound);

    /// <summary>
    /// 尝试替换所有服务描述符。
    /// </summary>
    /// <typeparam name="TService">指定的服务类型。</typeparam>
    /// <typeparam name="TOldImplementation">指定的旧实现类型。</typeparam>
    /// <typeparam name="TNewImplementation">指定的新实现类型。</typeparam>
    /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
    /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
    /// <returns>返回是否成功替换的布尔值。</returns>
    public static bool TryReplaceAll<TService, TOldImplementation, TNewImplementation>(this IServiceCollection services,
        bool throwIfNotFound = true)
        where TService : class
        where TOldImplementation : class, TService
        where TNewImplementation : class, TService
        => services.TryReplaceAll<TService, TOldImplementation, TNewImplementation>(out _, throwIfNotFound);

    /// <summary>
    /// 尝试替换所有服务描述符。
    /// </summary>
    /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
    /// <param name="serviceType">给定的服务类型。</param>
    /// <param name="newImplementationType">给定用于替换的新实现类型。</param>
    /// <param name="oldImplementationType">给定的旧实现类型（可选）。</param>
    /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
    /// <returns>返回是否成功替换的布尔值。</returns>
    public static bool TryReplaceAll(this IServiceCollection services, Type serviceType,
        Type newImplementationType, Type? oldImplementationType = null, bool throwIfNotFound = true)
        => services.TryReplaceAll(serviceType, newImplementationType, out _, oldImplementationType, throwIfNotFound);


    /// <summary>
    /// 尝试替换所有服务描述符。
    /// </summary>
    /// <typeparam name="TService">指定的服务类型。</typeparam>
    /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
    /// <param name="oldDescriptors">输出旧 <see cref="IReadOnlyList{ServiceDescriptor}"/>。</param>
    /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
    /// <returns>返回是否成功替换的布尔值。</returns>
    public static bool TryReplaceAll<TService>(this IServiceCollection services,
        [MaybeNullWhen(false)] out IReadOnlyList<ServiceDescriptor> oldDescriptors, bool throwIfNotFound = true)
        where TService : class
        => services.TryReplaceAll<TService, TService>(out oldDescriptors, throwIfNotFound);

    /// <summary>
    /// 尝试替换所有服务描述符。
    /// </summary>
    /// <typeparam name="TService">指定的服务类型。</typeparam>
    /// <typeparam name="TNewImplementation">指定的新实现类型。</typeparam>
    /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
    /// <param name="oldDescriptors">输出旧 <see cref="IReadOnlyList{ServiceDescriptor}"/>。</param>
    /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
    /// <returns>返回是否成功替换的布尔值。</returns>
    public static bool TryReplaceAll<TService, TNewImplementation>(this IServiceCollection services,
        [MaybeNullWhen(false)] out IReadOnlyList<ServiceDescriptor> oldDescriptors, bool throwIfNotFound = true)
        where TService : class
        where TNewImplementation : class, TService
        => services.TryReplaceAll(typeof(TService), typeof(TNewImplementation), out oldDescriptors,
            oldImplementationType: null, throwIfNotFound);

    /// <summary>
    /// 尝试替换所有服务描述符。
    /// </summary>
    /// <typeparam name="TService">指定的服务类型。</typeparam>
    /// <typeparam name="TOldImplementation">指定的旧实现类型。</typeparam>
    /// <typeparam name="TNewImplementation">指定的新实现类型。</typeparam>
    /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
    /// <param name="oldDescriptors">输出旧 <see cref="IReadOnlyList{ServiceDescriptor}"/>。</param>
    /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
    /// <returns>返回是否成功替换的布尔值。</returns>
    public static bool TryReplaceAll<TService, TOldImplementation, TNewImplementation>(this IServiceCollection services,
        [MaybeNullWhen(false)] out IReadOnlyList<ServiceDescriptor> oldDescriptors, bool throwIfNotFound = true)
        where TService : class
        where TOldImplementation : class, TService
        where TNewImplementation : class, TService
        => services.TryReplaceAll(typeof(TService), typeof(TNewImplementation), out oldDescriptors,
            typeof(TOldImplementation), throwIfNotFound);

    /// <summary>
    /// 尝试替换所有服务描述符。
    /// </summary>
    /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
    /// <param name="serviceType">给定的服务类型。</param>
    /// <param name="newImplementationType">给定用于替换的新实现类型。</param>
    /// <param name="oldDescriptors">输出旧 <see cref="IReadOnlyList{ServiceDescriptor}"/>。</param>
    /// <param name="oldImplementationType">给定的旧实现类型（可选）。</param>
    /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
    /// <returns>返回是否成功替换的布尔值。</returns>
    public static bool TryReplaceAll(this IServiceCollection services, Type serviceType,
        Type newImplementationType, [MaybeNullWhen(false)] out IReadOnlyList<ServiceDescriptor> oldDescriptors,
        Type? oldImplementationType = null, bool throwIfNotFound = true)
    {
        if (services.TryGetAll(serviceType, out oldDescriptors, oldImplementationType))
        {
            var lifetime = oldDescriptors[0].Lifetime;

            // 移除找到的所有描述符
            oldDescriptors.ForEach(descriptor => services.Remove(descriptor));
            
            // 添加新描述符
            services.Add(ServiceDescriptor.Describe(serviceType, newImplementationType, lifetime));
            return true;
        }

        if (throwIfNotFound)
        {
            if (oldImplementationType is not null)
                throw new ArgumentException($"No service type '{serviceType.FullName}' and old implementation type '{oldImplementationType.FullName}' were found.");
            else
                throw new ArgumentException($"No service type '{serviceType.FullName}' were found.");
        }

        return false;
    }


    /// <summary>
    /// 尝试替换单个服务描述符。
    /// </summary>
    /// <typeparam name="TService">指定的服务类型。</typeparam>
    /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
    /// <param name="newDescriptorFunc">给定用于替换的新 <see cref="ServiceDescriptor"/> 方法。</param>
    /// <param name="implementationType">给定的服务实现类型（可选）。</param>
    /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
    /// <returns>返回是否成功替换的布尔值。</returns>
    public static bool TryReplaceSingle<TService>(this IServiceCollection services,
        Func<ServiceDescriptor, ServiceDescriptor> newDescriptorFunc,
        Type? implementationType = null, bool throwIfNotFound = true)
        => services.TryReplaceSingle(typeof(TService), newDescriptorFunc, implementationType, throwIfNotFound);

    /// <summary>
    /// 尝试替换单个服务描述符。
    /// </summary>
    /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
    /// <param name="serviceType">给定的服务类型。</param>
    /// <param name="newDescriptorFunc">给定用于替换的新 <see cref="ServiceDescriptor"/> 方法。</param>
    /// <param name="implementationType">给定的服务实现类型（可选）。</param>
    /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
    /// <returns>返回是否成功替换的布尔值。</returns>
    public static bool TryReplaceSingle(this IServiceCollection services, Type serviceType,
        Func<ServiceDescriptor, ServiceDescriptor> newDescriptorFunc,
        Type? implementationType = null, bool throwIfNotFound = true)
    {
        if (services.TryGetSingle(serviceType, out var oldDescriptor, implementationType))
        {
            var newDescriptor = newDescriptorFunc(oldDescriptor!);

            services.Remove(oldDescriptor!);
            services.Add(newDescriptor);
            return true;
        }

        if (throwIfNotFound)
            throw new ArgumentException($"No service type '{serviceType.FullName}' were found.");

        return false;
    }

    #endregion


    #region TryReplace.ImplementationFunc

    /// <summary>
    /// 尝试替换服务描述符。
    /// </summary>
    /// <typeparam name="TService">指定的服务类型。</typeparam>
    /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
    /// <param name="newImplementationFunc">给定用于替换的新实现类型方法。</param>
    /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
    /// <returns>返回是否成功替换的布尔值。</returns>
    public static bool TryReplaceAll<TService>(this IServiceCollection services,
        Func<IServiceProvider, TService> newImplementationFunc, bool throwIfNotFound = true)
        where TService : class
        => services.TryReplaceAll(newImplementationFunc, out _, throwIfNotFound);

    /// <summary>
    /// 尝试替换服务描述符。
    /// </summary>
    /// <typeparam name="TService">指定的服务类型。</typeparam>
    /// <typeparam name="TNewImplementation">指定的新实现类型。</typeparam>
    /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
    /// <param name="newImplementationFunc">给定用于替换的新实现类型方法。</param>
    /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
    /// <returns>返回是否成功替换的布尔值。</returns>
    public static bool TryReplaceAll<TService, TNewImplementation>(this IServiceCollection services,
        Func<IServiceProvider, TNewImplementation> newImplementationFunc, bool throwIfNotFound = true)
        where TService : class
        where TNewImplementation : class, TService
        => services.TryReplaceAll<TService, TNewImplementation>(newImplementationFunc,
            out _, throwIfNotFound);

    /// <summary>
    /// 尝试替换服务描述符。
    /// </summary>
    /// <typeparam name="TService">指定的服务类型。</typeparam>
    /// <typeparam name="TOldImplementation">指定的旧实现类型。</typeparam>
    /// <typeparam name="TNewImplementation">指定的新实现类型。</typeparam>
    /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
    /// <param name="newImplementationFunc">给定用于替换的新实现类型方法。</param>
    /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
    /// <returns>返回是否成功替换的布尔值。</returns>
    public static bool TryReplaceAll<TService, TOldImplementation, TNewImplementation>(this IServiceCollection services,
        Func<IServiceProvider, TNewImplementation> newImplementationFunc, bool throwIfNotFound = true)
        where TService : class
        where TOldImplementation : class, TService
        where TNewImplementation : class, TService
        => services.TryReplaceAll<TService, TOldImplementation, TNewImplementation>(newImplementationFunc,
            out _, throwIfNotFound);

    /// <summary>
    /// 尝试替换服务描述符。
    /// </summary>
    /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
    /// <param name="serviceType">给定的服务类型。</param>
    /// <param name="oldImplementationType">给定的旧实现类型。</param>
    /// <param name="newImplementationFunc">给定用于替换的新实现类型方法。</param>
    /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
    /// <returns>返回是否成功替换的布尔值。</returns>
    public static bool TryReplaceAll(this IServiceCollection services, Type serviceType,
        Func<IServiceProvider, object> newImplementationFunc,
        Type? oldImplementationType = null, bool throwIfNotFound = true)
        => services.TryReplaceAll(serviceType, newImplementationFunc, out _,
            oldImplementationType, throwIfNotFound);


    /// <summary>
    /// 尝试替换服务描述符。
    /// </summary>
    /// <typeparam name="TService">指定的服务类型。</typeparam>
    /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
    /// <param name="newImplementationFunc">给定用于替换的新实现类型方法。</param>
    /// <param name="oldDescriptors">输出旧 <see cref="IReadOnlyList{ServiceDescriptor}"/>。</param>
    /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
    /// <returns>返回是否成功替换的布尔值。</returns>
    public static bool TryReplaceAll<TService>(this IServiceCollection services,
        Func<IServiceProvider, TService> newImplementationFunc,
        [MaybeNullWhen(false)] out IReadOnlyList<ServiceDescriptor> oldDescriptors, bool throwIfNotFound = true)
        where TService : class
        => services.TryReplaceAll<TService, TService>(newImplementationFunc, out oldDescriptors,
            throwIfNotFound);

    /// <summary>
    /// 尝试替换服务描述符。
    /// </summary>
    /// <typeparam name="TService">指定的服务类型。</typeparam>
    /// <typeparam name="TNewImplementation">指定的新实现类型。</typeparam>
    /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
    /// <param name="newImplementationFunc">给定用于替换的新实现类型方法。</param>
    /// <param name="oldDescriptors">输出旧 <see cref="IReadOnlyList{ServiceDescriptor}"/>。</param>
    /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
    /// <returns>返回是否成功替换的布尔值。</returns>
    public static bool TryReplaceAll<TService, TNewImplementation>(this IServiceCollection services,
        Func<IServiceProvider, TNewImplementation> newImplementationFunc,
        [MaybeNullWhen(false)] out IReadOnlyList<ServiceDescriptor> oldDescriptors, bool throwIfNotFound = true)
        where TService : class
        where TNewImplementation : class, TService
        => services.TryReplaceAll(typeof(TService), newImplementationFunc, out oldDescriptors,
            oldImplementationType: null, throwIfNotFound);

    /// <summary>
    /// 尝试替换服务描述符。
    /// </summary>
    /// <typeparam name="TService">指定的服务类型。</typeparam>
    /// <typeparam name="TOldImplementation">指定的旧实现类型。</typeparam>
    /// <typeparam name="TNewImplementation">指定的新实现类型。</typeparam>
    /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
    /// <param name="newImplementationFunc">给定用于替换的新实现类型方法。</param>
    /// <param name="oldDescriptors">输出旧 <see cref="IReadOnlyList{ServiceDescriptor}"/>。</param>
    /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
    /// <returns>返回是否成功替换的布尔值。</returns>
    public static bool TryReplaceAll<TService, TOldImplementation, TNewImplementation>(this IServiceCollection services,
        Func<IServiceProvider, TNewImplementation> newImplementationFunc,
        [MaybeNullWhen(false)] out IReadOnlyList<ServiceDescriptor> oldDescriptors, bool throwIfNotFound = true)
        where TService : class
        where TOldImplementation : class, TService
        where TNewImplementation : class, TService
        => services.TryReplaceAll(typeof(TService), newImplementationFunc, out oldDescriptors,
            typeof(TOldImplementation), throwIfNotFound);

    /// <summary>
    /// 尝试替换服务描述符。
    /// </summary>
    /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
    /// <param name="serviceType">给定的服务类型。</param>
    /// <param name="oldImplementationType">给定的旧实现类型。</param>
    /// <param name="newImplementationFunc">给定用于替换的新实现类型方法。</param>
    /// <param name="oldDescriptors">输出旧 <see cref="IReadOnlyList{ServiceDescriptor}"/>。</param>
    /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
    /// <returns>返回是否成功替换的布尔值。</returns>
    public static bool TryReplaceAll(this IServiceCollection services, Type serviceType,
        Func<IServiceProvider, object> newImplementationFunc,
        [MaybeNullWhen(false)] out IReadOnlyList<ServiceDescriptor> oldDescriptors,
        Type? oldImplementationType = null, bool throwIfNotFound = true)
    {
        if (services.TryGetAll(serviceType, out oldDescriptors, oldImplementationType))
        {
            var lifetime = oldDescriptors[0].Lifetime;

            // 移除找到的所有描述符
            oldDescriptors.ForEach(descriptor => services.Remove(descriptor));

            services.Add(ServiceDescriptor.Describe(serviceType, newImplementationFunc, lifetime));
            return true;
        }

        if (throwIfNotFound)
        {
            if (oldImplementationType is not null)
                throw new ArgumentException($"No service type '{serviceType.FullName}' and old implementation type '{oldImplementationType.FullName}' were found.");
            else
                throw new ArgumentException($"No service type '{serviceType.FullName}' were found.");
        }

        return false;
    }

    #endregion


    #region TryReplace.Instance

    /// <summary>
    /// 尝试替换所有服务描述符。
    /// </summary>
    /// <typeparam name="TService">指定的服务类型。</typeparam>
    /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
    /// <param name="newInstance">给定的新实例。</param>
    /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
    /// <returns>返回是否成功替换的布尔值。</returns>
    public static bool TryReplaceAll<TService>(this IServiceCollection services,
        TService newInstance, bool throwIfNotFound = true)
        where TService : class
        => services.TryReplaceAll(newInstance, out _, throwIfNotFound);

    /// <summary>
    /// 尝试替换所有服务描述符。
    /// </summary>
    /// <typeparam name="TService">指定的服务类型。</typeparam>
    /// <typeparam name="TNewImplementation">指定的新实现类型。</typeparam>
    /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
    /// <param name="newInstance">给定的新实例。</param>
    /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
    /// <returns>返回是否成功替换的布尔值。</returns>
    public static bool TryReplaceAll<TService, TNewImplementation>(this IServiceCollection services,
        TNewImplementation newInstance, bool throwIfNotFound = true)
        where TService : class
        where TNewImplementation : class, TService
        => services.TryReplaceAll<TService, TNewImplementation>(newInstance, out _, throwIfNotFound);

    /// <summary>
    /// 尝试替换所有服务描述符。
    /// </summary>
    /// <typeparam name="TService">指定的服务类型。</typeparam>
    /// <typeparam name="TOldImplementation">指定的旧实现类型。</typeparam>
    /// <typeparam name="TNewImplementation">指定的新实现类型。</typeparam>
    /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
    /// <param name="newInstance">给定的新实例。</param>
    /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
    /// <returns>返回是否成功替换的布尔值。</returns>
    public static bool TryReplaceAll<TService, TOldImplementation, TNewImplementation>(this IServiceCollection services,
        TNewImplementation newInstance, bool throwIfNotFound = true)
        where TService : class
        where TOldImplementation : class, TService
        where TNewImplementation : class, TService
        => services.TryReplaceAll<TService, TOldImplementation, TNewImplementation>(newInstance, out _, throwIfNotFound);

    /// <summary>
    /// 尝试替换所有服务描述符。
    /// </summary>
    /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
    /// <param name="serviceType">给定的服务类型。</param>
    /// <param name="newInstance">给定的新实例。</param>
    /// <param name="oldImplementationType">给定的旧实现类型（可选）。</param>
    /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
    /// <returns>返回是否成功替换的布尔值。</returns>
    public static bool TryReplaceAll(this IServiceCollection services, Type serviceType,
        object newInstance, Type? oldImplementationType = null, bool throwIfNotFound = true)
        => services.TryReplaceAll(serviceType, newInstance, out _, oldImplementationType, throwIfNotFound);


    /// <summary>
    /// 尝试替换所有服务描述符。
    /// </summary>
    /// <typeparam name="TService">指定的服务类型。</typeparam>
    /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
    /// <param name="newInstance">给定的新实例。</param>
    /// <param name="oldDescriptors">输出旧 <see cref="IReadOnlyList{ServiceDescriptor}"/>。</param>
    /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
    /// <returns>返回是否成功替换的布尔值。</returns>
    public static bool TryReplaceAll<TService>(this IServiceCollection services,
        TService newInstance, [MaybeNullWhen(false)] out IReadOnlyList<ServiceDescriptor> oldDescriptors, bool throwIfNotFound = true)
        where TService : class
        => services.TryReplaceAll<TService, TService>(newInstance, out oldDescriptors, throwIfNotFound);

    /// <summary>
    /// 尝试替换所有服务描述符。
    /// </summary>
    /// <typeparam name="TService">指定的服务类型。</typeparam>
    /// <typeparam name="TNewImplementation">指定的新实现类型。</typeparam>
    /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
    /// <param name="newInstance">给定的新实例。</param>
    /// <param name="oldDescriptors">输出旧 <see cref="IReadOnlyList{ServiceDescriptor}"/>。</param>
    /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
    /// <returns>返回是否成功替换的布尔值。</returns>
    public static bool TryReplaceAll<TService, TNewImplementation>(this IServiceCollection services,
        TNewImplementation newInstance, [MaybeNullWhen(false)] out IReadOnlyList<ServiceDescriptor> oldDescriptors, bool throwIfNotFound = true)
        where TService : class
        where TNewImplementation : class, TService
        => services.TryReplaceAll(typeof(TService), newInstance, out oldDescriptors,
            oldImplementationType: null, throwIfNotFound);

    /// <summary>
    /// 尝试替换所有服务描述符。
    /// </summary>
    /// <typeparam name="TService">指定的服务类型。</typeparam>
    /// <typeparam name="TOldImplementation">指定的旧实现类型。</typeparam>
    /// <typeparam name="TNewImplementation">指定的新实现类型。</typeparam>
    /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
    /// <param name="newInstance">给定的新实例。</param>
    /// <param name="oldDescriptors">输出旧 <see cref="IReadOnlyList{ServiceDescriptor}"/>。</param>
    /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
    /// <returns>返回是否成功替换的布尔值。</returns>
    public static bool TryReplaceAll<TService, TOldImplementation, TNewImplementation>(this IServiceCollection services,
        TNewImplementation newInstance, [MaybeNullWhen(false)] out IReadOnlyList<ServiceDescriptor> oldDescriptors, bool throwIfNotFound = true)
        where TService : class
        where TOldImplementation : class, TService
        where TNewImplementation : class, TService
        => services.TryReplaceAll(typeof(TService), newInstance, out oldDescriptors,
            typeof(TOldImplementation), throwIfNotFound);

    /// <summary>
    /// 尝试替换所有服务描述符。
    /// </summary>
    /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
    /// <param name="serviceType">给定的服务类型。</param>
    /// <param name="newInstance">给定的新实例。</param>
    /// <param name="oldDescriptors">输出旧 <see cref="IReadOnlyList{ServiceDescriptor}"/>。</param>
    /// <param name="oldImplementationType">给定的旧实现类型（可选）。</param>
    /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
    /// <returns>返回是否成功替换的布尔值。</returns>
    public static bool TryReplaceAll(this IServiceCollection services, Type serviceType,
        object newInstance, [MaybeNullWhen(false)] out IReadOnlyList<ServiceDescriptor> oldDescriptors,
        Type? oldImplementationType = null, bool throwIfNotFound = true)
    {
        if (services.TryGetAll(serviceType, out oldDescriptors, oldImplementationType))
        {
            // 移除找到的所有描述符
            oldDescriptors.ForEach(descriptor => services.Remove(descriptor));

            services.Add(new ServiceDescriptor(serviceType, newInstance));
            return true;
        }

        if (throwIfNotFound)
        {
            if (oldImplementationType is not null)
                throw new ArgumentException($"No service type '{serviceType.FullName}' and old implementation type '{oldImplementationType.FullName}' were found.");
            else
                throw new ArgumentException($"No service type '{serviceType.FullName}' were found.");
        }

        return false;
    }

    #endregion

}
