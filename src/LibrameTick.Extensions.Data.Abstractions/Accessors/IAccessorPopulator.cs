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

namespace Librame.Extensions.Data.Accessors
{
    /// <summary>
    /// 定义 <see cref="IAccessor"/> 填充器接口。
    /// </summary>
    public interface IAccessorPopulator
    {
        /// <summary>
        /// 填充访问器。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceProvider"/>。</param>
        /// <returns>返回受影响的行数。</returns>
        int Populate(IServiceProvider services);

        /// <summary>
        /// 异步填充访问器。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceProvider"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含受影响行数的异步操作。</returns>
        Task<int> PopulateAsync(IServiceProvider services,
            CancellationToken cancellationToken = default);
    }
}
