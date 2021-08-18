#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.DependencyInjection;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 定义抽象实现 <see cref="IExtensionBuilder"/>。
    /// </summary>
    public abstract class AbstractExtensionBuilder : AbstractExtensionInfo, IExtensionBuilder
    {
        /// <summary>
        /// 构造一个父级 <see cref="AbstractExtensionBuilder"/>。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="services"/> 或 <paramref name="options"/> 为空。
        /// </exception>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="options">给定的 <see cref="IExtensionOptions"/>。</param>
        protected AbstractExtensionBuilder(IServiceCollection services, IExtensionOptions options)
            : base(parentInfo: null)
        {
            Services = services;
            Options = options;
        }

        /// <summary>
        /// 构造一个子级 <see cref="AbstractExtensionBuilder"/>。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="parentBuilder"/> 或 <paramref name="options"/> 为空。
        /// </exception>
        /// <param name="parentBuilder">给定的父级 <see cref="IExtensionBuilder"/>。</param>
        /// <param name="options">给定的 <see cref="IExtensionOptions"/>。</param>
        protected AbstractExtensionBuilder(IExtensionBuilder parentBuilder, IExtensionOptions options)
            : base(parentBuilder)
        {
            ParentBuilder = parentBuilder;
            Services = parentBuilder.Services;
            Options = options;
        }


        /// <summary>
        /// 服务集合。
        /// </summary>
        public IServiceCollection Services { get; init; }

        /// <summary>
        /// 扩展选项。
        /// </summary>
        public IExtensionOptions Options { get; init; }

        /// <summary>
        /// 父级构建器。
        /// </summary>
        public IExtensionBuilder? ParentBuilder { get; init; }


        #region TryAddOrReplaceServiceByCharacteristic

        /// <summary>
        /// 通过服务特征实现添加或替换服务（支持扩展选项的替换服务字典集合）。与 <see cref="TryAddOrReplaceService(Type, Type)"/> 区别是此方法的特征服务仅用于匹配特征，不用于容器注册。
        /// </summary>
        /// <typeparam name="TCharacteristic">指定的特征类型。</typeparam>
        /// <typeparam name="TService">指定的服务类型（如果扩展选项的替换服务字典集合中存在此服务类型，则优先使用扩展选项中的服务类型）。</typeparam>
        /// <returns>返回 <see cref="IExtensionBuilder"/>。</returns>
        public virtual IExtensionBuilder TryAddOrReplaceServiceByCharacteristic<TCharacteristic, TService>()
            => TryAddOrReplaceServiceByCharacteristic(typeof(TCharacteristic), typeof(TService));

        /// <summary>
        /// 通过服务特征实现添加或替换服务（支持扩展选项的替换服务字典集合）。与 <see cref="TryAddOrReplaceService(Type, Type)"/> 区别是此方法的特征服务仅用于匹配特征，不用于容器注册。
        /// </summary>
        /// <typeparam name="TCharacteristic">指定的特征类型。</typeparam>
        /// <param name="serviceType">给定的服务类型（如果扩展选项的替换服务字典集合中存在此服务类型，则优先使用扩展选项中的服务类型）。</param>
        /// <returns>返回 <see cref="IExtensionBuilder"/>。</returns>
        public virtual IExtensionBuilder TryAddOrReplaceServiceByCharacteristic<TCharacteristic>(Type serviceType)
            => TryAddOrReplaceServiceByCharacteristic(typeof(TCharacteristic), serviceType);

        /// <summary>
        /// 通过服务特征实现添加或替换服务（支持扩展选项的替换服务字典集合）。与 <see cref="TryAddOrReplaceService(Type, Type)"/> 区别是此方法的特征服务仅用于匹配特征，不用于容器注册。
        /// </summary>
        /// <param name="characteristicType">给定的特征类型。</param>
        /// <param name="serviceType">给定的服务类型（如果扩展选项的替换服务字典集合中存在此服务类型，则优先使用扩展选项中的服务类型）。</param>
        /// <returns>返回 <see cref="IExtensionBuilder"/>。</returns>
        public virtual IExtensionBuilder TryAddOrReplaceServiceByCharacteristic(Type characteristicType, Type serviceType)
        {
            if (!Options.ServiceCharacteristics.TryGetValue(characteristicType, out var characteristic))
                characteristic = ServiceCharacteristic.Singleton(characteristicType);

            if (Options.ReplacedServices.TryGetValue(serviceType, out var replacedType))
                serviceType = replacedType;

            var descriptor = ServiceDescriptor.Describe(serviceType, serviceType, characteristic.Lifetime);
            Services.TryAddOrReplaceByCharacteristic(characteristic, descriptor);

            return this;
        }

        #endregion


        #region TryAddOrReplaceService

        /// <summary>
        /// 通过服务特征实现添加或替换服务（支持扩展选项的替换服务字典集合）。与 <see cref="TryAddOrReplaceServiceByCharacteristic(Type, Type)"/> 区别是此方法的服务类型即特征服务类型，服务类型既用于匹配特征，也用于容器注册。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型（如果扩展选项的替换服务字典集合中存在此服务实现类型，则优先使用扩展选项中的服务实现类型）。</typeparam>
        /// <returns>返回 <see cref="IExtensionBuilder"/>。</returns>
        public virtual IExtensionBuilder TryAddOrReplaceService<TService>()
            where TService : class
            => TryAddOrReplaceService<TService>(out _);

        /// <summary>
        /// 通过服务特征实现添加或替换服务（支持扩展选项的替换服务字典集合）。与 <see cref="TryAddOrReplaceServiceByCharacteristic(Type, Type)"/> 区别是此方法的服务类型即特征服务类型，服务类型既用于匹配特征，也用于容器注册。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型（如果扩展选项的替换服务字典集合中存在此服务实现类型，则优先使用扩展选项中的服务实现类型）。</typeparam>
        /// <param name="descriptor">输出 <see cref="ServiceDescriptor"/>。</param>
        /// <returns>返回 <see cref="IExtensionBuilder"/>。</returns>
        public virtual IExtensionBuilder TryAddOrReplaceService<TService>(out ServiceDescriptor descriptor)
            where TService : class
        {
            var serviceType = typeof(TService);
            return TryAddOrReplaceService(serviceType, serviceType, out descriptor);
        }

        /// <summary>
        /// 通过服务特征实现添加或替换服务（支持扩展选项的替换服务字典集合）。与 <see cref="TryAddOrReplaceServiceByCharacteristic(Type, Type)"/> 区别是此方法的服务类型即特征服务类型，服务类型既用于匹配特征，也用于容器注册。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <typeparam name="TImplementation">指定的实现类型（如果扩展选项的替换服务字典集合中存在此服务实现类型，则优先使用扩展选项中的服务实现类型）。</typeparam>
        /// <returns>返回 <see cref="IExtensionBuilder"/>。</returns>
        public virtual IExtensionBuilder TryAddOrReplaceService<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService
            => TryAddOrReplaceService<TService, TImplementation>(out _);

        /// <summary>
        /// 通过服务特征实现添加或替换服务（支持扩展选项的替换服务字典集合）。与 <see cref="TryAddOrReplaceServiceByCharacteristic(Type, Type)"/> 区别是此方法的服务类型即特征服务类型，服务类型既用于匹配特征，也用于容器注册。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <typeparam name="TImplementation">指定的实现类型（如果扩展选项的替换服务字典集合中存在此服务实现类型，则优先使用扩展选项中的服务实现类型）。</typeparam>
        /// <param name="descriptor">输出 <see cref="ServiceDescriptor"/>。</param>
        /// <returns>返回 <see cref="IExtensionBuilder"/>。</returns>
        public virtual IExtensionBuilder TryAddOrReplaceService<TService, TImplementation>(out ServiceDescriptor descriptor)
            where TService : class
            where TImplementation : class, TService
            => TryAddOrReplaceService(typeof(TService), typeof(TImplementation), out descriptor);

        /// <summary>
        /// 通过服务特征实现添加或替换服务（支持扩展选项的替换服务字典集合）。与 <see cref="TryAddOrReplaceServiceByCharacteristic(Type, Type)"/> 区别是此方法的服务类型即特征服务类型，服务类型既用于匹配特征，也用于容器注册。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <param name="implementationType">给定的实现类型（如果扩展选项的替换服务字典集合中存在此服务实现类型，则优先使用扩展选项中的服务实现类型）。</param>
        /// <returns>返回 <see cref="IExtensionBuilder"/>。</returns>
        public virtual IExtensionBuilder TryAddOrReplaceService<TService>(Type implementationType)
            where TService : class
            => TryAddOrReplaceService<TService>(implementationType, out _);

        /// <summary>
        /// 通过服务特征实现添加或替换服务（支持扩展选项的替换服务字典集合）。与 <see cref="TryAddOrReplaceServiceByCharacteristic(Type, Type)"/> 区别是此方法的服务类型即特征服务类型，服务类型既用于匹配特征，也用于容器注册。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <param name="implementationType">给定的实现类型（如果扩展选项的替换服务字典集合中存在此服务实现类型，则优先使用扩展选项中的服务实现类型）。</param>
        /// <param name="descriptor">输出 <see cref="ServiceDescriptor"/>。</param>
        /// <returns>返回 <see cref="IExtensionBuilder"/>。</returns>
        public virtual IExtensionBuilder TryAddOrReplaceService<TService>(Type implementationType,
            out ServiceDescriptor descriptor)
            where TService : class
            => TryAddOrReplaceService(typeof(TService), implementationType, out descriptor);

        /// <summary>
        /// 通过服务特征实现添加或替换服务（支持扩展选项的替换服务字典集合）。与 <see cref="TryAddOrReplaceServiceByCharacteristic(Type, Type)"/> 区别是此方法的服务类型即特征服务类型，服务类型既用于匹配特征，也用于容器注册。
        /// </summary>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <param name="implementationType">给定的实现类型（如果扩展选项的替换服务字典集合中存在此服务实现类型，则优先使用扩展选项中的服务实现类型）。</param>
        /// <returns>返回 <see cref="IExtensionBuilder"/>。</returns>
        public virtual IExtensionBuilder TryAddOrReplaceService(Type serviceType, Type implementationType)
            => TryAddOrReplaceService(serviceType, implementationType, out _);

        /// <summary>
        /// 通过服务特征实现添加或替换服务（支持扩展选项的替换服务字典集合）。与 <see cref="TryAddOrReplaceServiceByCharacteristic(Type, Type)"/> 区别是此方法的服务类型即特征服务类型，服务类型既用于匹配特征，也用于容器注册。
        /// </summary>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <param name="implementationType">给定的实现类型（如果扩展选项的替换服务字典集合中存在此服务实现类型，则优先使用扩展选项中的服务实现类型）。</param>
        /// <param name="descriptor">输出 <see cref="ServiceDescriptor"/>。</param>
        /// <returns>返回 <see cref="IExtensionBuilder"/>。</returns>
        public virtual IExtensionBuilder TryAddOrReplaceService(Type serviceType, Type implementationType,
            out ServiceDescriptor descriptor)
        {
            if (!Options.ServiceCharacteristics.TryGetValue(serviceType, out var characteristic))
                characteristic = ServiceCharacteristic.Singleton(serviceType);

            if (Options.ReplacedServices.TryGetValue(serviceType, out var replacedType))
                implementationType = replacedType;

            descriptor = ServiceDescriptor.Describe(serviceType, implementationType, characteristic.Lifetime);
            Services.TryAddOrReplaceByCharacteristic(characteristic, descriptor);

            return this;
        }


        /// <summary>
        /// 通过服务特征实现添加或替换服务。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <param name="factory">给定的服务方法。</param>
        /// <returns>返回 <see cref="IExtensionBuilder"/>。</returns>
        public virtual IExtensionBuilder TryAddOrReplaceService<TService>(
            Func<IServiceProvider, TService> factory)
            where TService : class
            => TryAddOrReplaceService(factory, out _);

        /// <summary>
        /// 通过服务特征实现添加或替换服务。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <param name="factory">给定的服务方法。</param>
        /// <param name="descriptor">输出 <see cref="ServiceDescriptor"/>。</param>
        /// <returns>返回 <see cref="IExtensionBuilder"/>。</returns>
        public virtual IExtensionBuilder TryAddOrReplaceService<TService>(
            Func<IServiceProvider, TService> factory, out ServiceDescriptor descriptor)
            where TService : class
            => TryAddOrReplaceService(typeof(TService), factory, out descriptor);

        /// <summary>
        /// 通过服务特征实现添加或替换服务。
        /// </summary>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <param name="factory">给定的服务方法。</param>
        /// <returns>返回 <see cref="IExtensionBuilder"/>。</returns>
        public virtual IExtensionBuilder TryAddOrReplaceService(Type serviceType,
            Func<IServiceProvider, object> factory)
            => TryAddOrReplaceService(serviceType, factory, out _);

        /// <summary>
        /// 通过服务特征实现添加或替换服务。
        /// </summary>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <param name="factory">给定的服务方法。</param>
        /// <param name="descriptor">输出 <see cref="ServiceDescriptor"/>。</param>
        /// <returns>返回 <see cref="IExtensionBuilder"/>。</returns>
        public virtual IExtensionBuilder TryAddOrReplaceService(Type serviceType,
            Func<IServiceProvider, object> factory, out ServiceDescriptor descriptor)
        {
            if (!Options.ServiceCharacteristics.TryGetValue(serviceType, out var characteristic))
                characteristic = ServiceCharacteristic.Singleton(serviceType);

            descriptor = ServiceDescriptor.Singleton(serviceType, factory);
            Services.TryAddOrReplaceByCharacteristic(characteristic, descriptor);

            return this;
        }


        /// <summary>
        /// 通过服务特征实现添加或替换服务。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <param name="instance">给定的服务实例。</param>
        /// <returns>返回 <see cref="IExtensionBuilder"/>。</returns>
        public virtual IExtensionBuilder TryAddOrReplaceService<TService>(TService instance)
            where TService : class
            => TryAddOrReplaceService(instance, out _);

        /// <summary>
        /// 通过服务特征实现添加或替换服务。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <param name="instance">给定的服务实例。</param>
        /// <param name="descriptor">输出 <see cref="ServiceDescriptor"/>。</param>
        /// <returns>返回 <see cref="IExtensionBuilder"/>。</returns>
        public virtual IExtensionBuilder TryAddOrReplaceService<TService>(TService instance,
            out ServiceDescriptor descriptor)
            where TService : class
            => TryAddOrReplaceService(typeof(TService), instance, out descriptor);

        /// <summary>
        /// 通过服务特征实现添加或替换服务。
        /// </summary>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <param name="instance">给定的服务实例。</param>
        /// <returns>返回 <see cref="IExtensionBuilder"/>。</returns>
        public virtual IExtensionBuilder TryAddOrReplaceService(Type serviceType, object instance)
            => TryAddOrReplaceService(serviceType, instance, out _);

        /// <summary>
        /// 通过服务特征实现添加或替换服务。
        /// </summary>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <param name="instance">给定的服务实例。</param>
        /// <param name="descriptor">输出 <see cref="ServiceDescriptor"/>。</param>
        /// <returns>返回 <see cref="IExtensionBuilder"/>。</returns>
        public virtual IExtensionBuilder TryAddOrReplaceService(Type serviceType, object instance,
            out ServiceDescriptor descriptor)
        {
            // 虽然对象服务实例都是单例，但此处需要使用服务特征的是否替换功能
            if (!Options.ServiceCharacteristics.TryGetValue(serviceType, out var characteristic))
                characteristic = ServiceCharacteristic.Singleton(serviceType);

            descriptor = ServiceDescriptor.Singleton(serviceType, instance);
            Services.TryAddOrReplaceByCharacteristic(characteristic, descriptor);

            return this;
        }

        #endregion


        #region TryAddEnumerable

        /// <summary>
        /// 通过特征尝试将单个服务添加为可枚举服务集合（默认会忽略已注册的服务类型与实现类型）。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <typeparam name="TImplementation">指定的实现类型。</typeparam>
        /// <returns>返回 <see cref="IServiceCollection"/>。</returns>
        public IExtensionBuilder TryAddEnumerableServices<TService, TImplementation>()
            where TService : class
            where TImplementation : TService
            => TryAddEnumerableServices<TService, TImplementation>(out _);

        /// <summary>
        /// 通过特征尝试将单个服务添加为可枚举服务集合（默认会忽略已注册的服务类型与实现类型）。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <typeparam name="TImplementation">指定的实现类型。</typeparam>
        /// <param name="descriptor">输出 <see cref="ServiceDescriptor"/>。</param>
        /// <returns>返回 <see cref="IServiceCollection"/>。</returns>
        public IExtensionBuilder TryAddEnumerableServices<TService, TImplementation>(out ServiceDescriptor descriptor)
            where TService : class
            where TImplementation : TService
            => TryAddEnumerableServices(typeof(TService), typeof(TImplementation), out descriptor);

        /// <summary>
        /// 通过特征尝试将单个服务添加为可枚举服务集合（默认会忽略已注册的服务类型与实现类型）。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <param name="implementationType">给定的实现类型。</param>
        /// <returns>返回 <see cref="IServiceCollection"/>。</returns>
        public IExtensionBuilder TryAddEnumerableServices<TService>(Type implementationType)
            where TService : class
            => TryAddEnumerableServices<TService>(implementationType, out _);

        /// <summary>
        /// 通过特征尝试将单个服务添加为可枚举服务集合（默认会忽略已注册的服务类型与实现类型）。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <param name="implementationType">给定的实现类型。</param>
        /// <param name="descriptor">输出 <see cref="ServiceDescriptor"/>。</param>
        /// <returns>返回 <see cref="IServiceCollection"/>。</returns>
        public IExtensionBuilder TryAddEnumerableServices<TService>(Type implementationType,
            out ServiceDescriptor descriptor)
            where TService : class
            => TryAddEnumerableServices(typeof(TService), implementationType, out descriptor);

        /// <summary>
        /// 通过特征尝试将单个服务添加为可枚举服务集合（默认会忽略已注册的服务类型与实现类型）。
        /// </summary>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <param name="implementationType">给定的实现类型。</param>
        /// <returns>返回 <see cref="IServiceCollection"/>。</returns>
        public IExtensionBuilder TryAddEnumerableServices(Type serviceType, Type implementationType)
            => TryAddEnumerableServices(serviceType, implementationType, out _);

        /// <summary>
        /// 通过特征尝试将单个服务添加为可枚举服务集合（默认会忽略已注册的服务类型与实现类型）。
        /// </summary>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <param name="implementationType">给定的实现类型。</param>
        /// <param name="descriptor">输出 <see cref="ServiceDescriptor"/>。</param>
        /// <returns>返回 <see cref="IServiceCollection"/>。</returns>
        public IExtensionBuilder TryAddEnumerableServices(Type serviceType, Type implementationType,
            out ServiceDescriptor descriptor)
        {
            if (!Options.ServiceCharacteristics.TryGetValue(serviceType, out var characteristic))
                characteristic = ServiceCharacteristic.Singleton(serviceType);

            Services.TryAddEnumerableByCharacteristic(characteristic, implementationType, out descriptor);

            return this;
        }


        /// <summary>
        /// 通过特征尝试添加可枚举服务集合（默认会忽略已注册的服务类型与实现类型）。如果特征不存在此服务类型，则默认使用单例注册服务。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <param name="implementationTypes">给定的实现类型集合。</param>
        /// <returns>返回 <see cref="IServiceCollection"/>。</returns>
        public IExtensionBuilder TryAddEnumerableServices<TService>(IEnumerable<Type> implementationTypes)
            where TService : class
            => TryAddEnumerableServices<TService>(implementationTypes, out _);

        /// <summary>
        /// 通过特征尝试添加可枚举服务集合（默认会忽略已注册的服务类型与实现类型）。如果特征不存在此服务类型，则默认使用单例注册服务。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <param name="implementationTypes">给定的实现类型集合。</param>
        /// <param name="descriptors">输出 <see cref="IEnumerable{ServiceDescriptor}"/>。</param>
        /// <returns>返回 <see cref="IServiceCollection"/>。</returns>
        public IExtensionBuilder TryAddEnumerableServices<TService>(IEnumerable<Type> implementationTypes,
            out IEnumerable<ServiceDescriptor> descriptors)
            where TService : class
            => TryAddEnumerableServices(typeof(TService), implementationTypes, out descriptors);

        /// <summary>
        /// 通过特征尝试添加可枚举服务集合（默认会忽略已注册的服务类型与实现类型）。如果特征不存在此服务类型，则默认使用单例注册服务。
        /// </summary>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <param name="implementationTypes">给定的实现类型集合。</param>
        /// <returns>返回 <see cref="IServiceCollection"/>。</returns>
        public IExtensionBuilder TryAddEnumerableServices(Type serviceType, IEnumerable<Type> implementationTypes)
            => TryAddEnumerableServices(serviceType, implementationTypes, out _);

        /// <summary>
        /// 通过特征尝试添加可枚举服务集合（默认会忽略已注册的服务类型与实现类型）。如果特征不存在此服务类型，则默认使用单例注册服务。
        /// </summary>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <param name="implementationTypes">给定的实现类型集合。</param>
        /// <param name="descriptors">输出 <see cref="IEnumerable{ServiceDescriptor}"/>。</param>
        /// <returns>返回 <see cref="IServiceCollection"/>。</returns>
        public IExtensionBuilder TryAddEnumerableServices(Type serviceType, IEnumerable<Type> implementationTypes,
            out IEnumerable<ServiceDescriptor> descriptors)
        {
            if (!Options.ServiceCharacteristics.TryGetValue(serviceType, out var characteristic))
                characteristic = ServiceCharacteristic.Singleton(serviceType);

            Services.TryAddEnumerableByCharacteristic(characteristic, implementationTypes, out descriptors);

            return this;
        }

        #endregion

    }
}
