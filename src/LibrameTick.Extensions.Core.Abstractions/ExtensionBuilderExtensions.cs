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
/// <see cref="IExtensionBuilder"/> 静态扩展。
/// </summary>
public static class ExtensionBuilderExtensions
{

    /// <summary>
    /// 查找指定目标扩展构建器（支持链式查找父级扩展构建器）。
    /// </summary>
    /// <typeparam name="TTargetBuilder">指定的目标扩展构建器类型。</typeparam>
    /// <param name="lastBuilder">给定配置的最后一个 <see cref="IExtensionBuilder"/>。</param>
    /// <returns>返回 <typeparamref name="TTargetBuilder"/>。</returns>
    public static TTargetBuilder? FindBuilder<TTargetBuilder>(this IExtensionBuilder lastBuilder)
        where TTargetBuilder : IExtensionBuilder
    {
        if (!(lastBuilder is TTargetBuilder targetBuilder))
        {
            if (lastBuilder.ParentBuilder is not null)
                return FindBuilder<TTargetBuilder>(lastBuilder.ParentBuilder);

            return default;
        }

        return targetBuilder;
    }

    /// <summary>
    /// 获取必需的目标扩展构建器（通过 <see cref="FindBuilder{TTargetBuilder}(IExtensionBuilder)"/> 实现，如果未找到则抛出异常）。
    /// </summary>
    /// <typeparam name="TTargetBuilder">指定的目标扩展构建器类型。</typeparam>
    /// <param name="lastBuilder">给定配置的最后一个 <see cref="IExtensionBuilder"/>。</param>
    /// <returns>返回 <typeparamref name="TTargetBuilder"/>。</returns>
    public static TTargetBuilder GetRequiredBuilder<TTargetBuilder>(this IExtensionBuilder lastBuilder)
        where TTargetBuilder : IExtensionBuilder
    {
        var targetBuilder = lastBuilder.FindBuilder<TTargetBuilder>();
        if (targetBuilder is null)
            throw new ArgumentException($"Target builder instance '{typeof(TTargetBuilder)}' not found from current builder '{lastBuilder.GetType()}'.");

        return targetBuilder;
    }


    /// <summary>
    /// 将服务注册的所有扩展选项保存为 JSON 文件。
    /// </summary>
    /// <param name="lastBuilder">给定的 <see cref="IExtensionBuilder"/>。</param>
    /// <param name="services">给定的 <see cref="IServiceProvider"/>。</param>
    /// <returns>返回包含 JSON 文件路径和选项实例的字典集合。</returns>
    public static Dictionary<string, IExtensionOptions> SaveAllOptionsAsJson(this IExtensionBuilder lastBuilder,
        IServiceProvider services)
    {
        var allOptions = new Dictionary<string, IExtensionOptions>();

        SaveJson(allOptions, lastBuilder);

        return allOptions;

        void SaveJson(Dictionary<string, IExtensionOptions> dict, IExtensionBuilder currentBuilder)
        {
            dict.Add(currentBuilder.SaveOptionsAsJson(services, out var options), options);

            if (currentBuilder.ParentBuilder is not null)
                SaveJson(dict, currentBuilder.ParentBuilder); // 链式保存
        }
    }


    #region TryAddOrReplaceService

    /// <summary>
    /// 尝试通过服务特征实现添加或替换服务（支持扩展选项的替换服务字典集合）。
    /// </summary>
    /// <typeparam name="TService">指定的服务类型（如果扩展选项的替换服务字典集合中存在此服务实现类型，则优先使用扩展选项中的服务实现类型）。</typeparam>
    /// <param name="builder">给定的 <see cref="IExtensionBuilder"/>。</param>
    /// <returns>返回 <see cref="IExtensionBuilder"/>。</returns>
    public static IExtensionBuilder TryAddOrReplaceService<TService>(this IExtensionBuilder builder)
        where TService : class
        => builder.TryAddOrReplaceService<TService>(out _);

    /// <summary>
    /// 尝试通过服务特征实现添加或替换服务（支持扩展选项的替换服务字典集合）。
    /// </summary>
    /// <typeparam name="TService">指定的服务类型（如果扩展选项的替换服务字典集合中存在此服务实现类型，则优先使用扩展选项中的服务实现类型）。</typeparam>
    /// <param name="builder">给定的 <see cref="IExtensionBuilder"/>。</param>
    /// <param name="descriptor">输出 <see cref="ServiceDescriptor"/>。</param>
    /// <returns>返回 <see cref="IExtensionBuilder"/>。</returns>
    public static IExtensionBuilder TryAddOrReplaceService<TService>(this IExtensionBuilder builder,
        out ServiceDescriptor descriptor)
        where TService : class
    {
        var serviceType = typeof(TService);
        return builder.TryAddOrReplaceService(serviceType, serviceType, out descriptor);
    }

    /// <summary>
    /// 尝试通过服务特征实现添加或替换服务（支持扩展选项的替换服务字典集合）。
    /// </summary>
    /// <typeparam name="TService">指定的服务类型。</typeparam>
    /// <typeparam name="TImplementation">指定的实现类型（如果扩展选项的替换服务字典集合中存在此服务实现类型，则优先使用扩展选项中的服务实现类型）。</typeparam>
    /// <param name="builder">给定的 <see cref="IExtensionBuilder"/>。</param>
    /// <returns>返回 <see cref="IExtensionBuilder"/>。</returns>
    public static IExtensionBuilder TryAddOrReplaceService<TService, TImplementation>(this IExtensionBuilder builder)
        where TService : class
        where TImplementation : class, TService
        => builder.TryAddOrReplaceService<TService, TImplementation>(out _);

    /// <summary>
    /// 尝试通过服务特征实现添加或替换服务（支持扩展选项的替换服务字典集合）。
    /// </summary>
    /// <typeparam name="TService">指定的服务类型。</typeparam>
    /// <typeparam name="TImplementation">指定的实现类型（如果扩展选项的替换服务字典集合中存在此服务实现类型，则优先使用扩展选项中的服务实现类型）。</typeparam>
    /// <param name="builder">给定的 <see cref="IExtensionBuilder"/>。</param>
    /// <param name="descriptor">输出 <see cref="ServiceDescriptor"/>。</param>
    /// <returns>返回 <see cref="IExtensionBuilder"/>。</returns>
    public static IExtensionBuilder TryAddOrReplaceService<TService, TImplementation>(this IExtensionBuilder builder,
        out ServiceDescriptor descriptor)
        where TService : class
        where TImplementation : class, TService
        => builder.TryAddOrReplaceService(typeof(TService), typeof(TImplementation), out descriptor);

    /// <summary>
    /// 尝试通过服务特征实现添加或替换服务（支持扩展选项的替换服务字典集合）。
    /// </summary>
    /// <typeparam name="TService">指定的服务类型。</typeparam>
    /// <param name="builder">给定的 <see cref="IExtensionBuilder"/>。</param>
    /// <param name="implementationType">给定的实现类型（如果扩展选项的替换服务字典集合中存在此服务实现类型，则优先使用扩展选项中的服务实现类型）。</param>
    /// <returns>返回 <see cref="IExtensionBuilder"/>。</returns>
    public static IExtensionBuilder TryAddOrReplaceService<TService>(this IExtensionBuilder builder,
        Type implementationType)
        where TService : class
        => builder.TryAddOrReplaceService<TService>(implementationType, out _);

    /// <summary>
    /// 尝试通过服务特征实现添加或替换服务（支持扩展选项的替换服务字典集合）。
    /// </summary>
    /// <typeparam name="TService">指定的服务类型。</typeparam>
    /// <param name="builder">给定的 <see cref="IExtensionBuilder"/>。</param>
    /// <param name="implementationType">给定的实现类型（如果扩展选项的替换服务字典集合中存在此服务实现类型，则优先使用扩展选项中的服务实现类型）。</param>
    /// <param name="descriptor">输出 <see cref="ServiceDescriptor"/>。</param>
    /// <returns>返回 <see cref="IExtensionBuilder"/>。</returns>
    public static IExtensionBuilder TryAddOrReplaceService<TService>(this IExtensionBuilder builder,
        Type implementationType, out ServiceDescriptor descriptor)
        where TService : class
        => builder.TryAddOrReplaceService(typeof(TService), implementationType, out descriptor);

    /// <summary>
    /// 尝试通过服务特征实现添加或替换服务（支持扩展选项的替换服务字典集合）。
    /// </summary>
    /// <param name="builder">给定的 <see cref="IExtensionBuilder"/>。</param>
    /// <param name="serviceType">给定的服务类型。</param>
    /// <param name="implementationType">给定的实现类型（如果扩展选项的替换服务字典集合中存在此服务实现类型，则优先使用扩展选项中的服务实现类型）。</param>
    /// <returns>返回 <see cref="IExtensionBuilder"/>。</returns>
    public static IExtensionBuilder TryAddOrReplaceService(this IExtensionBuilder builder,
        Type serviceType, Type implementationType)
        => builder.TryAddOrReplaceService(serviceType, implementationType, out _);

    /// <summary>
    /// 尝试通过服务特征实现添加或替换服务（支持扩展选项的替换服务字典集合）。
    /// </summary>
    /// <param name="builder">给定的 <see cref="IExtensionBuilder"/>。</param>
    /// <param name="serviceType">给定的服务类型。</param>
    /// <param name="implementationType">给定的实现类型（如果扩展选项的替换服务字典集合中存在此服务实现类型，则优先使用扩展选项中的服务实现类型）。</param>
    /// <param name="descriptor">输出 <see cref="ServiceDescriptor"/>。</param>
    /// <returns>返回 <see cref="IExtensionBuilder"/>。</returns>
    public static IExtensionBuilder TryAddOrReplaceService(this IExtensionBuilder builder,
        Type serviceType, Type implementationType,
        out ServiceDescriptor descriptor)
    {
        if (!builder.ServiceCharacteristics.TryGetValue(serviceType, out var characteristic))
            characteristic = ServiceCharacteristic.Singleton(serviceType);

        if (builder.ReplacedServices.TryGetValue(serviceType, out var replacedType))
            implementationType = replacedType;

        descriptor = ServiceDescriptor.Describe(serviceType, implementationType, characteristic.Lifetime);
        builder.Services.TryAddOrReplaceByCharacteristic(characteristic, descriptor);

        if (characteristic.AddImplementationType && descriptor.ImplementationType is not null)
        {
            builder.Services.Add(new ServiceDescriptor(descriptor.ImplementationType,
                sp => sp.GetRequiredService(serviceType), descriptor.Lifetime));
        }

        return builder;
    }


    /// <summary>
    /// 尝试通过服务特征实现添加或替换服务。
    /// </summary>
    /// <typeparam name="TService">指定的服务类型。</typeparam>
    /// <param name="builder">给定的 <see cref="IExtensionBuilder"/>。</param>
    /// <param name="factory">给定的服务方法。</param>
    /// <returns>返回 <see cref="IExtensionBuilder"/>。</returns>
    public static IExtensionBuilder TryAddOrReplaceService<TService>(this IExtensionBuilder builder,
        Func<IServiceProvider, TService> factory)
        where TService : class
        => builder.TryAddOrReplaceService(factory, out _);

    /// <summary>
    /// 尝试通过服务特征实现添加或替换服务。
    /// </summary>
    /// <typeparam name="TService">指定的服务类型。</typeparam>
    /// <param name="builder">给定的 <see cref="IExtensionBuilder"/>。</param>
    /// <param name="factory">给定的服务方法。</param>
    /// <param name="descriptor">输出 <see cref="ServiceDescriptor"/>。</param>
    /// <returns>返回 <see cref="IExtensionBuilder"/>。</returns>
    public static IExtensionBuilder TryAddOrReplaceService<TService>(this IExtensionBuilder builder,
        Func<IServiceProvider, TService> factory, out ServiceDescriptor descriptor)
        where TService : class
        => builder.TryAddOrReplaceService(typeof(TService), factory, out descriptor);

    /// <summary>
    /// 尝试通过服务特征实现添加或替换服务。
    /// </summary>
    /// <param name="builder">给定的 <see cref="IExtensionBuilder"/>。</param>
    /// <param name="serviceType">给定的服务类型。</param>
    /// <param name="factory">给定的服务方法。</param>
    /// <returns>返回 <see cref="IExtensionBuilder"/>。</returns>
    public static IExtensionBuilder TryAddOrReplaceService(this IExtensionBuilder builder,
        Type serviceType, Func<IServiceProvider, object> factory)
        => builder.TryAddOrReplaceService(serviceType, factory, out _);

    /// <summary>
    /// 尝试通过服务特征实现添加或替换服务。
    /// </summary>
    /// <param name="builder">给定的 <see cref="IExtensionBuilder"/>。</param>
    /// <param name="serviceType">给定的服务类型。</param>
    /// <param name="factory">给定的服务方法。</param>
    /// <param name="descriptor">输出 <see cref="ServiceDescriptor"/>。</param>
    /// <returns>返回 <see cref="IExtensionBuilder"/>。</returns>
    public static IExtensionBuilder TryAddOrReplaceService(this IExtensionBuilder builder,
        Type serviceType, Func<IServiceProvider, object> factory, out ServiceDescriptor descriptor)
    {
        if (!builder.ServiceCharacteristics.TryGetValue(serviceType, out var characteristic))
            characteristic = ServiceCharacteristic.Singleton(serviceType);

        descriptor = ServiceDescriptor.Singleton(serviceType, factory);
        builder.Services.TryAddOrReplaceByCharacteristic(characteristic, descriptor);

        if (characteristic.AddImplementationType && descriptor.ImplementationType is not null)
        {
            builder.Services.Add(new ServiceDescriptor(descriptor.ImplementationType,
                sp => sp.GetRequiredService(serviceType), descriptor.Lifetime));
        }

        return builder;
    }


    /// <summary>
    /// 尝试通过服务特征实现添加或替换服务。
    /// </summary>
    /// <typeparam name="TService">指定的服务类型。</typeparam>
    /// <param name="builder">给定的 <see cref="IExtensionBuilder"/>。</param>
    /// <param name="instance">给定的服务实例。</param>
    /// <returns>返回 <see cref="IExtensionBuilder"/>。</returns>
    public static IExtensionBuilder TryAddOrReplaceService<TService>(this IExtensionBuilder builder,
        TService instance)
        where TService : class
        => builder.TryAddOrReplaceService(instance, out _);

    /// <summary>
    /// 尝试通过服务特征实现添加或替换服务。
    /// </summary>
    /// <typeparam name="TService">指定的服务类型。</typeparam>
    /// <param name="builder">给定的 <see cref="IExtensionBuilder"/>。</param>
    /// <param name="instance">给定的服务实例。</param>
    /// <param name="descriptor">输出 <see cref="ServiceDescriptor"/>。</param>
    /// <returns>返回 <see cref="IExtensionBuilder"/>。</returns>
    public static IExtensionBuilder TryAddOrReplaceService<TService>(this IExtensionBuilder builder,
        TService instance, out ServiceDescriptor descriptor)
        where TService : class
        => builder.TryAddOrReplaceService(typeof(TService), instance, out descriptor);

    /// <summary>
    /// 尝试通过服务特征实现添加或替换服务。
    /// </summary>
    /// <param name="builder">给定的 <see cref="IExtensionBuilder"/>。</param>
    /// <param name="serviceType">给定的服务类型。</param>
    /// <param name="instance">给定的服务实例。</param>
    /// <returns>返回 <see cref="IExtensionBuilder"/>。</returns>
    public static IExtensionBuilder TryAddOrReplaceService(this IExtensionBuilder builder,
        Type serviceType, object instance)
        => builder.TryAddOrReplaceService(serviceType, instance, out _);

    /// <summary>
    /// 尝试通过服务特征实现添加或替换服务。
    /// </summary>
    /// <param name="builder">给定的 <see cref="IExtensionBuilder"/>。</param>
    /// <param name="serviceType">给定的服务类型。</param>
    /// <param name="instance">给定的服务实例。</param>
    /// <param name="descriptor">输出 <see cref="ServiceDescriptor"/>。</param>
    /// <returns>返回 <see cref="IExtensionBuilder"/>。</returns>
    public static IExtensionBuilder TryAddOrReplaceService(this IExtensionBuilder builder,
        Type serviceType, object instance, out ServiceDescriptor descriptor)
    {
        // 虽然对象服务实例都是单例，但此处需要使用服务特征的是否替换功能
        if (!builder.ServiceCharacteristics.TryGetValue(serviceType, out var characteristic))
            characteristic = ServiceCharacteristic.Singleton(serviceType);

        descriptor = ServiceDescriptor.Singleton(serviceType, instance);
        builder.Services.TryAddOrReplaceByCharacteristic(characteristic, descriptor);

        if (characteristic.AddImplementationType && descriptor.ImplementationType is not null)
        {
            builder.Services.Add(new ServiceDescriptor(descriptor.ImplementationType,
                sp => sp.GetRequiredService(serviceType), descriptor.Lifetime));
        }

        return builder;
    }

    #endregion


    #region TryAddEnumerableServices

    /// <summary>
    /// 尝试通过特征尝试将单个服务添加为可枚举服务集合（默认会忽略已注册的服务类型与实现类型）。
    /// </summary>
    /// <typeparam name="TService">指定的服务类型。</typeparam>
    /// <typeparam name="TImplementation">指定的实现类型。</typeparam>
    /// <param name="builder">给定的 <see cref="IExtensionBuilder"/>。</param>
    /// <returns>返回 <see cref="IExtensionBuilder"/>。</returns>
    public static IExtensionBuilder TryAddEnumerableServices<TService, TImplementation>(this IExtensionBuilder builder)
        where TService : class
        where TImplementation : TService
        => builder.TryAddEnumerableServices<TService, TImplementation>(out _);

    /// <summary>
    /// 尝试通过特征尝试将单个服务添加为可枚举服务集合（默认会忽略已注册的服务类型与实现类型）。
    /// </summary>
    /// <typeparam name="TService">指定的服务类型。</typeparam>
    /// <typeparam name="TImplementation">指定的实现类型。</typeparam>
    /// <param name="builder">给定的 <see cref="IExtensionBuilder"/>。</param>
    /// <param name="descriptor">输出 <see cref="ServiceDescriptor"/>。</param>
    /// <returns>返回 <see cref="IExtensionBuilder"/>。</returns>
    public static IExtensionBuilder TryAddEnumerableServices<TService, TImplementation>(this IExtensionBuilder builder,
        out ServiceDescriptor descriptor)
        where TService : class
        where TImplementation : TService
        => builder.TryAddEnumerableServices(typeof(TService), typeof(TImplementation), out descriptor);

    /// <summary>
    /// 尝试通过特征尝试将单个服务添加为可枚举服务集合（默认会忽略已注册的服务类型与实现类型）。
    /// </summary>
    /// <typeparam name="TService">指定的服务类型。</typeparam>
    /// <param name="builder">给定的 <see cref="IExtensionBuilder"/>。</param>
    /// <param name="implementationType">给定的实现类型。</param>
    /// <returns>返回 <see cref="IExtensionBuilder"/>。</returns>
    public static IExtensionBuilder TryAddEnumerableServices<TService>(this IExtensionBuilder builder,
        Type implementationType)
        where TService : class
        => builder.TryAddEnumerableServices<TService>(implementationType, out _);

    /// <summary>
    /// 尝试通过特征尝试将单个服务添加为可枚举服务集合（默认会忽略已注册的服务类型与实现类型）。
    /// </summary>
    /// <typeparam name="TService">指定的服务类型。</typeparam>
    /// <param name="builder">给定的 <see cref="IExtensionBuilder"/>。</param>
    /// <param name="implementationType">给定的实现类型。</param>
    /// <param name="descriptor">输出 <see cref="ServiceDescriptor"/>。</param>
    /// <returns>返回 <see cref="IExtensionBuilder"/>。</returns>
    public static IExtensionBuilder TryAddEnumerableServices<TService>(this IExtensionBuilder builder,
        Type implementationType, out ServiceDescriptor descriptor)
        where TService : class
        => builder.TryAddEnumerableServices(typeof(TService), implementationType, out descriptor);

    /// <summary>
    /// 尝试通过特征尝试将单个服务添加为可枚举服务集合（默认会忽略已注册的服务类型与实现类型）。
    /// </summary>
    /// <param name="builder">给定的 <see cref="IExtensionBuilder"/>。</param>
    /// <param name="serviceType">给定的服务类型。</param>
    /// <param name="implementationType">给定的实现类型。</param>
    /// <returns>返回 <see cref="IExtensionBuilder"/>。</returns>
    public static IExtensionBuilder TryAddEnumerableServices(this IExtensionBuilder builder,
        Type serviceType, Type implementationType)
        => builder.TryAddEnumerableServices(serviceType, implementationType, out _);

    /// <summary>
    /// 尝试通过特征尝试将单个服务添加为可枚举服务集合（默认会忽略已注册的服务类型与实现类型）。
    /// </summary>
    /// <param name="builder">给定的 <see cref="IExtensionBuilder"/>。</param>
    /// <param name="serviceType">给定的服务类型。</param>
    /// <param name="implementationType">给定的实现类型。</param>
    /// <param name="descriptor">输出 <see cref="ServiceDescriptor"/>。</param>
    /// <returns>返回 <see cref="IExtensionBuilder"/>。</returns>
    public static IExtensionBuilder TryAddEnumerableServices(this IExtensionBuilder builder,
        Type serviceType, Type implementationType, out ServiceDescriptor descriptor)
    {
        if (!builder.ServiceCharacteristics.TryGetValue(serviceType, out var characteristic))
            characteristic = ServiceCharacteristic.Singleton(serviceType);

        builder.Services.TryAddEnumerableByCharacteristic(characteristic, implementationType, out descriptor);

        return builder;
    }


    /// <summary>
    /// 尝试通过特征尝试添加可枚举服务集合（默认会忽略已注册的服务类型与实现类型）。如果特征不存在此服务类型，则默认使用单例注册服务。
    /// </summary>
    /// <typeparam name="TService">指定的服务类型。</typeparam>
    /// <param name="builder">给定的 <see cref="IExtensionBuilder"/>。</param>
    /// <param name="implementationTypes">给定的实现类型集合。</param>
    /// <returns>返回 <see cref="IExtensionBuilder"/>。</returns>
    public static IExtensionBuilder TryAddEnumerableServices<TService>(this IExtensionBuilder builder,
        IEnumerable<Type> implementationTypes)
        where TService : class
        => builder.TryAddEnumerableServices<TService>(implementationTypes, out _);

    /// <summary>
    /// 尝试通过特征尝试添加可枚举服务集合（默认会忽略已注册的服务类型与实现类型）。如果特征不存在此服务类型，则默认使用单例注册服务。
    /// </summary>
    /// <typeparam name="TService">指定的服务类型。</typeparam>
    /// <param name="builder">给定的 <see cref="IExtensionBuilder"/>。</param>
    /// <param name="implementationTypes">给定的实现类型集合。</param>
    /// <param name="descriptors">输出 <see cref="IEnumerable{ServiceDescriptor}"/>。</param>
    /// <returns>返回 <see cref="IExtensionBuilder"/>。</returns>
    public static IExtensionBuilder TryAddEnumerableServices<TService>(this IExtensionBuilder builder,
        IEnumerable<Type> implementationTypes, out IEnumerable<ServiceDescriptor> descriptors)
        where TService : class
        => builder.TryAddEnumerableServices(typeof(TService), implementationTypes, out descriptors);

    /// <summary>
    /// 尝试通过特征尝试添加可枚举服务集合（默认会忽略已注册的服务类型与实现类型）。如果特征不存在此服务类型，则默认使用单例注册服务。
    /// </summary>
    /// <param name="builder">给定的 <see cref="IExtensionBuilder"/>。</param>
    /// <param name="serviceType">给定的服务类型。</param>
    /// <param name="implementationTypes">给定的实现类型集合。</param>
    /// <returns>返回 <see cref="IExtensionBuilder"/>。</returns>
    public static IExtensionBuilder TryAddEnumerableServices(this IExtensionBuilder builder,
        Type serviceType, IEnumerable<Type> implementationTypes)
        => builder.TryAddEnumerableServices(serviceType, implementationTypes, out _);

    /// <summary>
    /// 尝试通过特征尝试添加可枚举服务集合（默认会忽略已注册的服务类型与实现类型）。如果特征不存在此服务类型，则默认使用单例注册服务。
    /// </summary>
    /// <param name="builder">给定的 <see cref="IExtensionBuilder"/>。</param>
    /// <param name="serviceType">给定的服务类型。</param>
    /// <param name="implementationTypes">给定的实现类型集合。</param>
    /// <param name="descriptors">输出 <see cref="IEnumerable{ServiceDescriptor}"/>。</param>
    /// <returns>返回 <see cref="IExtensionBuilder"/>。</returns>
    public static IExtensionBuilder TryAddEnumerableServices(this IExtensionBuilder builder,
        Type serviceType, IEnumerable<Type> implementationTypes, out IEnumerable<ServiceDescriptor> descriptors)
    {
        if (!builder.ServiceCharacteristics.TryGetValue(serviceType, out var characteristic))
            characteristic = ServiceCharacteristic.Singleton(serviceType);

        builder.Services.TryAddEnumerableByCharacteristic(characteristic, implementationTypes, out descriptors);

        return builder;
    }

    #endregion

}
