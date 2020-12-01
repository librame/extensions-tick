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

namespace Librame.Extensions.Core.Builders
{
    using Options;

    /// <summary>
    /// 核心扩展构建器。
    /// </summary>
    /// <typeparam name="TOptions">指定的扩展选项类型。</typeparam>
    public class CoreExtensionBuilder<TOptions> : AbstractExtensionBuilder<TOptions>
        where TOptions : CoreExtensionOptions
    {
        /// <summary>
        /// 构造一个 <see cref="CoreExtensionBuilder{TOptions}"/>。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="options">给定的 <typeparamref name="TOptions"/>。</param>
        public CoreExtensionBuilder(IServiceCollection services, TOptions options)
            : base(services, options, parent: null)
        {
        }

    }
}
