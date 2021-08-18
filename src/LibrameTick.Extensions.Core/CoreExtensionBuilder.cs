#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Core.Cryptography;
using Librame.Extensions.Core.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 核心扩展构建器。
    /// </summary>
    public class CoreExtensionBuilder : AbstractExtensionBuilder<CoreExtensionOptions, CoreExtensionBuilder>
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
            // Cryptography
            TryAddOrReplaceService<IAlgorithmParameterGenerator, InternalAlgorithmParameterGenerator>();
            TryAddOrReplaceService<IAsymmetricAlgorithm, InternalAsymmetricAlgorithm>();
            TryAddOrReplaceService<ISymmetricAlgorithm, InternalSymmetricAlgorithm>();

            // Storage
            TryAddOrReplaceService<IFileManager, InternalFileManager>();
            TryAddOrReplaceService<IFilePermission, InternalFilePermission>();
            TryAddOrReplaceService<IFileTransmission, InternalFileTransmission>();

            TryAddOrReplaceService<IProcessorManager, InternalProcessorManager>();

            if (options.EnableAutoloaderActivator)
            {
                AutoloaderActivator = new AssemblyAutoloaderActivator(options.AssemblyLoadings);
                AutoloaderActivator.RegisterContainer(type => services.TryAddScoped(type));
            }
        }


        /// <summary>
        /// <see cref="IAutoloader"/> 激活器。
        /// </summary>
        public AssemblyAutoloaderActivator? AutoloaderActivator { get; init; }
    }
}
