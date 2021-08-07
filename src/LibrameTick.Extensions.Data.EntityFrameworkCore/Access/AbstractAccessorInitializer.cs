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

namespace Librame.Extensions.Data.Access
{
    /// <summary>
    /// 抽象 <see cref="IAccessorInitializer"/> 实现。
    /// </summary>
    /// <typeparam name="TAccessor">指定已实现 <see cref="AbstractAccessor"/> 的访问器类型。</typeparam>
    public abstract class AbstractAccessorInitializer<TAccessor> : IAccessorInitializer
        where TAccessor : AbstractAccessor
    {
        /// <summary>
        /// 使用数据库上下文构造一个 <see cref="AbstractAccessorInitializer{TAccessor}"/>。
        /// </summary>
        /// <param name="accessor">给定的 <typeparamref name="TAccessor"/>。</param>
        protected AbstractAccessorInitializer(TAccessor accessor)
        {
            Accessor = accessor;
        }


        /// <summary>
        /// 访问器。
        /// </summary>
        protected TAccessor Accessor { get; }


        /// <summary>
        /// 初始化访问器。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceProvider"/>。</param>
        public virtual void Initialize(IServiceProvider services)
        {
            var builder = services.GetRequiredService<DataExtensionBuilder>();
            if (builder.Options.Access.InitializeDatabase)
                Accessor.Database.EnsureCreated();

            Populate(services, builder.Options);
        }

        /// <summary>
        /// 异步初始化访问器。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceProvider"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个异步操作。</returns>
        public virtual async Task InitializeAsync(IServiceProvider services,
            CancellationToken cancellationToken = default)
        {
            var builder = services.GetRequiredService<DataExtensionBuilder>();
            if (builder.Options.Access.InitializeDatabase)
                await Accessor.Database.EnsureCreatedAsync(cancellationToken);

            await PopulateAsync(services, builder.Options, cancellationToken);
        }


        /// <summary>
        /// 填充访问器。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceProvider"/>。</param>
        /// <param name="options">给定的 <see cref="DataExtensionOptions"/>。</param>
        protected abstract void Populate(IServiceProvider services, DataExtensionOptions options);

        /// <summary>
        /// 异步填充访问器。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceProvider"/>。</param>
        /// <param name="options">给定的 <see cref="DataExtensionOptions"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个异步操作。</returns>
        protected abstract Task PopulateAsync(IServiceProvider services, DataExtensionOptions options,
            CancellationToken cancellationToken = default);
    }
}
