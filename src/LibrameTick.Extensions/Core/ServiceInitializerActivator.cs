#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 定义 <see cref="IServiceInitializer"/> 激活器。
    /// </summary>
    public class ServiceInitializerActivator
    {
        private readonly Type _baseType = typeof(IServiceInitializer);
        private readonly Type[]? _initializerTypes;
        private IServiceProvider? _serviceProvider;


        /// <summary>
        /// 构造一个 <see cref="ServiceInitializerActivator"/>。
        /// </summary>
        /// <param name="loadingOptions">给定的 <see cref="AssemblyLoadingOptions"/>（可选）。</param>
        public ServiceInitializerActivator(AssemblyLoadingOptions? loadingOptions = null)
        {
            _initializerTypes = AssemblyLoader.LoadInstantiableTypesByAssemblies(_baseType, loadingOptions);
        }


        /// <summary>
        /// 初始化器类型列表。
        /// </summary>
        public IReadOnlyList<Type>? InitializerTypes
            => _initializerTypes;


        /// <summary>
        /// 激活服务初始化器。
        /// </summary>
        /// <typeparam name="TInitializer">指定要激活的初始化器类型。</typeparam>
        /// <returns>返回 <see cref="ServiceInitializerActivator"/>。</returns>
        public ServiceInitializerActivator Activate<TInitializer>()
            where TInitializer : IServiceInitializer
        {
            if (InitializerTypes != null)
            {
                if (_serviceProvider == null)
                    throw new ArgumentException($"{nameof(_serviceProvider)} is null. You may need to call the {nameof(ApplyServiceProvider)}() method.");

                var initializerType = typeof(TInitializer);
                var activateTypes = InitializerTypes.Where(p => p.IsAssignableToBaseType(initializerType));

                foreach (var type in activateTypes)
                {
                    var initializer = (TInitializer?)_serviceProvider.GetService(type);
                    initializer?.Initialize(_serviceProvider);
                }
            }

            return this;
        }

        /// <summary>
        /// 激活服务初始化器。
        /// </summary>
        /// <typeparam name="TInitializer">指定要激活的初始化器类型。</typeparam>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="ServiceInitializerActivator"/>。</returns>
        public async Task<ServiceInitializerActivator> ActivateAsync<TInitializer>(CancellationToken cancellationToken = default)
            where TInitializer : IServiceInitializer
        {
            if (InitializerTypes != null)
            {
                if (_serviceProvider == null)
                    throw new ArgumentException($"{nameof(_serviceProvider)} is null. You may need to call the {nameof(ApplyServiceProvider)}() method.");

                var initializerType = typeof(TInitializer);
                var activateTypes = InitializerTypes.Where(p => p.IsAssignableToBaseType(initializerType));

                foreach (var type in activateTypes)
                {
                    var initializer = (TInitializer?)_serviceProvider.GetService(type);
                    if (initializer != null)
                        await initializer.InitializeAsync(_serviceProvider, cancellationToken);
                }
            }

            return this;
        }


        /// <summary>
        /// 应用 <see cref="IServiceProvider"/>。
        /// </summary>
        /// <param name="serviceProvider">给定的 <see cref="IServiceProvider"/>。</param>
        /// <returns>返回 <see cref="ServiceInitializerActivator"/>。</returns>
        public ServiceInitializerActivator ApplyServiceProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            return this;
        }


        /// <summary>
        /// 注册服务初始化器类型。
        /// </summary>
        /// <param name="registAction">给定的注册动作。</param>
        /// <returns>返回 <see cref="ServiceInitializerActivator"/>。</returns>
        public ServiceInitializerActivator Register(Action<Type> registAction)
        {
            if (InitializerTypes != null)
            {
                foreach (var type in InitializerTypes)
                {
                    registAction.Invoke(type);
                }
            }

            return this;
        }

    }
}
