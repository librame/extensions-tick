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

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 定义 <see cref="IServiceInitializer"/> 激活器。
    /// </summary>
    public class ServiceInitializerActivator
    {
        private readonly IServiceProvider _services;
        private readonly List<Type>? _initializerTypes;


        /// <summary>
        /// 构造一个 <see cref="ServiceInitializerActivator"/>。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceProvider"/>。</param>
        /// <param name="loadingOptions">给定的 <see cref="AssemblyLoadingOptions"/>（可选）。</param>
        public ServiceInitializerActivator(IServiceProvider services,
            AssemblyLoadingOptions? loadingOptions = null)
        {
            _services = services.NotNull(nameof(services));
            _initializerTypes = ServiceInitializerLoader.LoadInitializerTypes(loadingOptions);
        }


        /// <summary>
        /// 服务提供程序。
        /// </summary>
        public IServiceProvider Services
            => _services;

        /// <summary>
        /// 初始化器类型列表。
        /// </summary>
        public IReadOnlyList<Type>? InitializerTypes
            => _initializerTypes;


        /// <summary>
        /// 激活服务初始化器。
        /// </summary>
        /// <param name="initializerFunc">给定的服务初始化器方法。</param>
        /// <returns>返回 <see cref="ServiceInitializerActivator"/>。</returns>
        public ServiceInitializerActivator Activate(Func<IServiceProvider, Type, IServiceInitializer?> initializerFunc)
        {
            if (InitializerTypes != null)
            {
                foreach (var type in InitializerTypes)
                {
                    var initializer = initializerFunc.Invoke(Services, type);
                    initializer?.Initialize(Services);
                }
            }

            return this;
        }

    }
}
