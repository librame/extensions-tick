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
    /// 抽象扩展构建器（抽象实现 <see cref="IExtensionBuilder{TOptions}"/>）。
    /// </summary>
    /// <typeparam name="TOptions">指定的扩展选项类型。</typeparam>
    public abstract class AbstractExtensionBuilder<TOptions> : AbstractExtensionBuilder, IExtensionBuilder<TOptions>
        where TOptions : IExtensionOptions
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractExtensionBuilder{TOptions}"/>。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="services"/> 或 <paramref name="options"/> 为空。
        /// </exception>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="options">给定的 <typeparamref name="TOptions"/>。</param>
        /// <param name="parent">给定的可空 <see cref="IExtensionBuilder"/>。</param>
        public AbstractExtensionBuilder(IServiceCollection services, TOptions options,
            IExtensionBuilder? parent)
            : base(services, options, parent)
        {
            Options = options;
        }


        /// <summary>
        /// 扩展选项。
        /// </summary>
        public new TOptions Options { get; private set; }
    }
}
