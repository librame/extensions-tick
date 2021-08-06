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
    /// 定义支持（如从程序集中）扫描自启动加载类型的自加载器接口。
    /// </summary>
    public interface IAutoloader
    {
        /// <summary>
        /// 是否已自启动实例。
        /// </summary>
        bool IsAutoloaded { get; }


        /// <summary>
        /// 自启动实例。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceProvider"/>。</param>
        void Autoload(IServiceProvider services);

        /// <summary>
        /// 异步自启动实例。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceProvider"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个异步操作。</returns>
        Task AutoloadAsync(IServiceProvider services, CancellationToken cancellationToken = default);
    }
}
