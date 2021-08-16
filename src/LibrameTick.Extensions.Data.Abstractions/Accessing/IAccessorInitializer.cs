#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data.Accessing
{
    /// <summary>
    /// 定义 <see cref="IAccessor"/> 初始化器接口。
    /// </summary>
    public interface IAccessorInitializer
    {
        /// <summary>
        /// 初始化访问器。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceProvider"/>。</param>
        void Initialize(IServiceProvider services);

        /// <summary>
        /// 异步初始化访问器。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceProvider"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个异步操作。</returns>
        Task InitializeAsync(IServiceProvider services,
            CancellationToken cancellationToken = default);
    }
}
