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
    /// 抽象扩展构建器（抽象实现 <see cref="IExtensionBuilder"/>）。
    /// </summary>
    public abstract class AbstractExtensionBuilder : AbstractExtensionInfo<IExtensionBuilder>, IExtensionBuilder
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractExtensionBuilder"/>。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="options">给定的 <see cref="IExtensionOptions"/>。</param>
        /// <param name="parent">给定的可空 <see cref="IExtensionBuilder"/>。</param>
        public AbstractExtensionBuilder(IServiceCollection services, IExtensionOptions options,
            IExtensionBuilder? parent)
            : base(parent)
        {
            Services = services.NotNull(nameof(services));
            Options = options.NotNull(nameof(options));
        }


        /// <summary>
        /// 服务集合。
        /// </summary>
        public IServiceCollection Services { get; }

        /// <summary>
        /// 扩展选项。
        /// </summary>
        public IExtensionOptions Options { get; }
    }
}
