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
    /// 定义服务初始化器接口。
    /// </summary>
    public interface IServiceInitializer
    {
        /// <summary>
        /// 初始化服务。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceProvider"/>。</param>
        void Initialize(IServiceProvider services);

        /// <summary>
        /// 异步初始化服务。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceProvider"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个异步操作。</returns>
        Task InitializeAsync(IServiceProvider services, CancellationToken cancellationToken = default);
    }
}
