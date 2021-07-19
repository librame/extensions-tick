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

namespace Librame.Extensions.Core.Cryptography
{
    /// <summary>
    /// 抽象算法。
    /// </summary>
    public abstract class AbstractAlgorithm : IAlgorithm
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractAlgorithm"/>。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="parameterGenerator"/> or <paramref name="extensionBuilder"/> 为空。
        /// </exception>
        /// <param name="parameterGenerator">给定的 <see cref="IAlgorithmParameterGenerator"/>。</param>
        /// <param name="extensionBuilder">给定的 <see cref="IExtensionBuilder"/>。</param>
        public AbstractAlgorithm(IAlgorithmParameterGenerator parameterGenerator,
            IExtensionBuilder extensionBuilder)
        {
            ParameterGenerator = parameterGenerator.NotNull(nameof(parameterGenerator));
            ExtensionBuilder = extensionBuilder.NotNull(nameof(extensionBuilder));
        }


        /// <summary>
        /// 参数生成器。
        /// </summary>
        public IAlgorithmParameterGenerator ParameterGenerator { get; private set; }

        /// <summary>
        /// 扩展构建器。
        /// </summary>
        public IExtensionBuilder ExtensionBuilder { get; private set; }
    }
}
