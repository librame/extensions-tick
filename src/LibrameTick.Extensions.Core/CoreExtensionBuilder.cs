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
    using Cryptography;

    /// <summary>
    /// 核心扩展构建器。
    /// </summary>
    public class CoreExtensionBuilder : AbstractExtensionBuilder<CoreExtensionOptions>
    {
        /// <summary>
        /// 构造一个 <see cref="CoreExtensionBuilder"/>。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="services"/> 或 <paramref name="options"/> 为空。
        /// </exception>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="options">给定的 <see cref="CoreExtensionOptions"/>。</param>
        public CoreExtensionBuilder(IServiceCollection services, CoreExtensionOptions options)
            : base(services, options)
        {
            Services.AddSingleton(this);

            // Cryptography
            AddOrReplaceByCharacteristic<IAlgorithmParameterGenerator, DefaultAlgorithmParameterGenerator>();
            AddOrReplaceByCharacteristic<ISymmetricAlgorithm, DefaultSymmetricAlgorithm>();
        }

    }
}
