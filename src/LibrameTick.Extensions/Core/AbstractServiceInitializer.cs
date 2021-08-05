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
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 定义抽象实现 <see cref="IServiceInitializer"/>。
    /// </summary>
    public abstract class AbstractServiceInitializer : IServiceInitializer
    {
        /// <summary>
        /// 是否已初始化。
        /// </summary>
        public bool IsInitialized { get; private set; }


        /// <summary>
        /// 初始化服务。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceProvider"/>。</param>
        public virtual void Initialize(IServiceProvider services)
        {
            if (!IsInitialized)
            {
                OnInitialize(services);

                IsInitialized = true;
            }
        }

        /// <summary>
        /// 异步初始化服务。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceProvider"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个异步操作。</returns>
        public virtual async Task InitializeAsync(IServiceProvider services,
            CancellationToken cancellationToken = default)
        {
            if (!IsInitialized)
            {
                await OnInitializeAsync(services, cancellationToken);

                IsInitialized = true;
            }
        }


        /// <summary>
        /// 在初始化服务。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceProvider"/>。</param>
        protected abstract void OnInitialize(IServiceProvider services);

        /// <summary>
        /// 异步在初始化服务。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceProvider"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个异步操作。</returns>
        protected abstract Task OnInitializeAsync(IServiceProvider services,
            CancellationToken cancellationToken = default);
    }
}
