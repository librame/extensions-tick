﻿#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

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


        #region AddOrReplaceByCharacteristic

        /// <summary>
        /// 通过服务特征实现添加或替换服务（支持扩展选项的替换服务字典集合）。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型（如果扩展选项的替换服务字典集合中存在此服务实现类型，则优先使用扩展选项中的服务实现）。</typeparam>
        /// <returns>返回 <see cref="IExtensionBuilder"/>。</returns>
        public virtual IExtensionBuilder TryAddOrReplace<TService>()
            where TService : class
        {
            var serviceType = typeof(TService);
            return TryAddOrReplace(serviceType, serviceType);
        }

        /// <summary>
        /// 通过服务特征实现添加或替换服务（支持扩展选项的替换服务字典集合）。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <typeparam name="TImplementation">指定的实现类型（如果扩展选项的替换服务字典集合中存在此服务实现类型，则优先使用扩展选项中的服务实现）。</typeparam>
        /// <returns>返回 <see cref="IExtensionBuilder"/>。</returns>
        public virtual IExtensionBuilder TryAddOrReplace<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService
            => TryAddOrReplace(typeof(TService), typeof(TImplementation));

        /// <summary>
        /// 通过服务特征实现添加或替换服务（支持扩展选项的替换服务字典集合）。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <param name="implementationType">给定的实现类型（如果扩展选项的替换服务字典集合中存在此服务实现类型，则优先使用扩展选项中的服务实现）。</param>
        /// <returns>返回 <see cref="IExtensionBuilder"/>。</returns>
        public virtual IExtensionBuilder TryAddOrReplace<TService>(Type implementationType)
            where TService : class
            => TryAddOrReplace(typeof(TService), implementationType);

        /// <summary>
        /// 通过服务特征实现添加或替换服务（支持扩展选项的替换服务字典集合）。
        /// </summary>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <param name="implementationType">给定的实现类型（如果扩展选项的替换服务字典集合中存在此服务实现类型，则优先使用扩展选项中的服务实现）。</param>
        /// <returns>返回 <see cref="IExtensionBuilder"/>。</returns>
        public virtual IExtensionBuilder TryAddOrReplace(Type serviceType, Type implementationType)
        {
            if (!Options.ServiceCharacteristics.TryGetValue(serviceType, out var characteristic))
                characteristic = ServiceCharacteristic.Singleton(serviceType);

            if (Options.ReplacedServices.TryGetValue(serviceType, out var replacedType))
                implementationType = replacedType;

            var descriptor = ServiceDescriptor.Describe(serviceType, implementationType, characteristic.Lifetime);
            Services.TryAddOrReplaceByCharacteristic(characteristic, descriptor);

            return this;
        }


        /// <summary>
        /// 通过服务特征实现添加或替换服务。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <param name="factory">给定的服务方法。</param>
        /// <returns>返回 <see cref="IExtensionBuilder"/>。</returns>
        public virtual IExtensionBuilder TryAddOrReplace<TService>(Func<IServiceProvider, TService> factory)
            where TService : class
            => TryAddOrReplace(typeof(TService), factory);

        /// <summary>
        /// 通过服务特征实现添加或替换服务。
        /// </summary>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <param name="factory">给定的服务方法。</param>
        /// <returns>返回 <see cref="IExtensionBuilder"/>。</returns>
        public virtual IExtensionBuilder TryAddOrReplace(Type serviceType, Func<IServiceProvider, object> factory)
        {
            if (!Options.ServiceCharacteristics.TryGetValue(serviceType, out var characteristic))
                characteristic = ServiceCharacteristic.Singleton(serviceType);

            var descriptor = ServiceDescriptor.Singleton(serviceType, factory);
            Services.TryAddOrReplaceByCharacteristic(characteristic, descriptor);

            return this;
        }


        /// <summary>
        /// 通过服务特征实现添加或替换服务。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <param name="instance">给定的服务实例。</param>
        /// <returns>返回 <see cref="IExtensionBuilder"/>。</returns>
        public virtual IExtensionBuilder TryAddOrReplace<TService>(TService instance)
            where TService : class
            => TryAddOrReplace(typeof(TService), instance);

        /// <summary>
        /// 通过服务特征实现添加或替换服务。
        /// </summary>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <param name="instance">给定的服务实例。</param>
        /// <returns>返回 <see cref="IExtensionBuilder"/>。</returns>
        public virtual IExtensionBuilder TryAddOrReplace(Type serviceType, object instance)
        {
            // 虽然对象服务实例都是单例，但此处需要使用服务特征的是否替换功能
            if (!Options.ServiceCharacteristics.TryGetValue(serviceType, out var characteristic))
                characteristic = ServiceCharacteristic.Singleton(serviceType);

            var descriptor = ServiceDescriptor.Singleton(serviceType, instance);
            Services.TryAddOrReplaceByCharacteristic(characteristic, descriptor);

            return this;
        }

        #endregion


        #region TryAddEnumerable

        /// <summary>
        /// 通过特征尝试添加可枚举服务集合（默认会忽略已注册的服务类型与实现类型）。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <typeparam name="TImplementation">指定的实现类型。</typeparam>
        /// <returns>返回 <see cref="IServiceCollection"/>。</returns>
        public IExtensionBuilder TryAddEnumerable<TService, TImplementation>()
            where TService : class
            where TImplementation : TService
            => TryAddEnumerable(typeof(TService), typeof(TImplementation));

        /// <summary>
        /// 通过特征尝试添加可枚举服务集合（默认会忽略已注册的服务类型与实现类型）。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <param name="implementationType">给定的实现类型。</param>
        /// <returns>返回 <see cref="IServiceCollection"/>。</returns>
        public IExtensionBuilder TryAddEnumerable<TService>(Type implementationType)
            where TService : class
            => TryAddEnumerable(typeof(TService), implementationType);

        /// <summary>
        /// 通过特征尝试添加可枚举服务集合（默认会忽略已注册的服务类型与实现类型）。
        /// </summary>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <param name="implementationType">给定的实现类型。</param>
        /// <returns>返回 <see cref="IServiceCollection"/>。</returns>
        public IExtensionBuilder TryAddEnumerable(Type serviceType, Type implementationType)
        {
            if (!Options.ServiceCharacteristics.TryGetValue(serviceType, out var characteristic))
                characteristic = ServiceCharacteristic.Singleton(serviceType);

            Services.TryAddEnumerableByCharacteristic(characteristic, implementationType);

            return this;
        }


        /// <summary>
        /// 通过特征尝试添加可枚举服务集合（默认会忽略已注册的服务类型与实现类型）。如果特征不存在此服务类型，则默认使用单例注册服务。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <param name="implementationTypes">给定的实现类型集合。</param>
        /// <returns>返回 <see cref="IServiceCollection"/>。</returns>
        public IExtensionBuilder TryAddEnumerable<TService>(IEnumerable<Type> implementationTypes)
            where TService : class
            => TryAddEnumerable(typeof(TService), implementationTypes);

        /// <summary>
        /// 通过特征尝试添加可枚举服务集合（默认会忽略已注册的服务类型与实现类型）。如果特征不存在此服务类型，则默认使用单例注册服务。
        /// </summary>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <param name="implementationTypes">给定的实现类型集合。</param>
        /// <returns>返回 <see cref="IServiceCollection"/>。</returns>
        public IExtensionBuilder TryAddEnumerable(Type serviceType, IEnumerable<Type> implementationTypes)
        {
            if (!Options.ServiceCharacteristics.TryGetValue(serviceType, out var characteristic))
                characteristic = ServiceCharacteristic.Singleton(serviceType);

            Services.TryAddEnumerableByCharacteristic(characteristic, implementationTypes);

            return this;
        }

        #endregion

    }
}
