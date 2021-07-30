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
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data.Accessors
{
    /// <summary>
    /// 抽象 <see cref="IAccessorPopulator"/> 实现。
    /// </summary>
    /// <typeparam name="TAccessor">指定已实现 <see cref="AbstractAccessor"/> 的访问器类型。</typeparam>
    public abstract class AbstractAccessorPopulator<TAccessor> : IAccessorPopulator
        where TAccessor : AbstractAccessor
    {
        /// <summary>
        /// 使用数据库上下文构造一个 <see cref="AbstractAccessorPopulator{TAccessor}"/>。
        /// </summary>
        /// <param name="accessor">给定的 <typeparamref name="TAccessor"/>。</param>
        protected AbstractAccessorPopulator(TAccessor accessor)
        {
            Accessor = accessor.NotNull(nameof(accessor));
        }


        /// <summary>
        /// 访问器。
        /// </summary>
        protected TAccessor Accessor { get; }


        /// <summary>
        /// 填充访问器。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceProvider"/>。</param>
        /// <returns>返回受影响的行数。</returns>
        public virtual int Populate(IServiceProvider services)
        {
            var builder = services.GetRequiredService<DataExtensionBuilder>();
            if (builder.Options.Access.InitializeDatabase)
                Accessor.Database.EnsureCreated();

            return PopulateCore(services);
        }

        /// <summary>
        /// 异步填充访问器。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceProvider"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含受影响行数的异步操作。</returns>
        public virtual async Task<int> PopulateAsync(IServiceProvider services,
            CancellationToken cancellationToken = default)
        {
            var builder = services.GetRequiredService<DataExtensionBuilder>();
            if (builder.Options.Access.InitializeDatabase)
                await Accessor.Database.EnsureCreatedAsync();

            return await PopulateCoreAsync(services, cancellationToken);
        }


        /// <summary>
        /// 填充核心访问器。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceProvider"/>。</param>
        /// <returns>返回受影响的行数。</returns>
        protected abstract int PopulateCore(IServiceProvider services);

        /// <summary>
        /// 异步填充核心访问器。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceProvider"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含受影响行数的异步操作。</returns>
        protected abstract Task<int> PopulateCoreAsync(IServiceProvider services,
            CancellationToken cancellationToken = default);
    }
}
