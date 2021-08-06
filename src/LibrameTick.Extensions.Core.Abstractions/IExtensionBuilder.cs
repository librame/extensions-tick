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
using System;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 定义实现 <see cref="IExtensionInfo"/> 的扩展构建器接口。
    /// </summary>
    public interface IExtensionBuilder : IExtensionInfo
    {
        /// <summary>
        /// 服务集合。
        /// </summary>
        IServiceCollection Services { get; }

        /// <summary>
        /// 扩展选项。
        /// </summary>
        IExtensionOptions Options { get; }

        /// <summary>
        /// 父级构建器。
        /// </summary>
        IExtensionBuilder? ParentBuilder { get; }


        #region AddOrReplaceByCharacteristic

        /// <summary>
        /// 通过服务特征实现添加或替换服务（支持扩展选项的替换服务字典集合）。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <typeparam name="TImplementation">指定的实现类型。</typeparam>
        /// <returns>返回 <see cref="IExtensionBuilder"/>。</returns>
        IExtensionBuilder TryAddOrReplace<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService;

        /// <summary>
        /// 通过服务特征实现添加或替换服务（支持扩展选项的替换服务字典集合）。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <param name="implementationType">给定的实现类型。</param>
        /// <returns>返回 <see cref="IExtensionBuilder"/>。</returns>
        IExtensionBuilder TryAddOrReplace<TService>(Type implementationType)
            where TService : class;

        /// <summary>
        /// 通过服务特征实现添加或替换服务（支持扩展选项的替换服务字典集合）。
        /// </summary>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <param name="implementationType">给定的实现类型。</param>
        /// <returns>返回 <see cref="IExtensionBuilder"/>。</returns>
        IExtensionBuilder TryAddOrReplace(Type serviceType, Type implementationType);


        /// <summary>
        /// 通过服务特征实现添加或替换服务。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <param name="factory">给定的服务方法。</param>
        /// <returns>返回 <see cref="IExtensionBuilder"/>。</returns>
        IExtensionBuilder TryAddOrReplace<TService>(Func<IServiceProvider, TService> factory)
            where TService : class;

        /// <summary>
        /// 通过服务特征实现添加或替换服务。
        /// </summary>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <param name="factory">给定的服务方法。</param>
        /// <returns>返回 <see cref="IExtensionBuilder"/>。</returns>
        IExtensionBuilder TryAddOrReplace(Type serviceType, Func<IServiceProvider, object> factory);


        /// <summary>
        /// 通过服务特征实现添加或替换服务。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <param name="instance">给定的服务实例。</param>
        /// <returns>返回 <see cref="IExtensionBuilder"/>。</returns>
        IExtensionBuilder TryAddOrReplace<TService>(TService instance)
            where TService : class;

        /// <summary>
        /// 通过服务特征实现添加或替换服务。
        /// </summary>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <param name="instance">给定的服务实例。</param>
        /// <returns>返回 <see cref="IExtensionBuilder"/>。</returns>
        IExtensionBuilder TryAddOrReplace(Type serviceType, object instance);

        #endregion

    }
}
