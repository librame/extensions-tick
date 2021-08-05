#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data
{
    /// <summary>
    /// <see cref="IIdentificationGeneratorFactory"/> 静态扩展。
    /// </summary>
    public static class IdentificationGeneratorFactoryExtensions
    {

        /// <summary>
        /// 获取新标识。
        /// </summary>
        /// <typeparam name="TId">指定的标识类型。</typeparam>
        /// <param name="factory">给定的 <see cref="IIdentificationGeneratorFactory"/>。</param>
        /// <returns>返回 <typeparamref name="TId"/>。</returns>
        public static TId GetNewId<TId>(this IIdentificationGeneratorFactory factory)
            => factory.GetIdGenerator<TId>().GenerateId();

        /// <summary>
        /// 异步获取新标识。
        /// </summary>
        /// <typeparam name="TId">指定的标识类型。</typeparam>
        /// <param name="factory">给定的 <see cref="IIdentificationGeneratorFactory"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TId"/> 的异步操作。</returns>
        public static Task<TId> GetNewIdAsync<TId>(this IIdentificationGeneratorFactory factory,
            CancellationToken cancellationToken = default)
            => factory.GetIdGenerator<TId>().GenerateIdAsync(cancellationToken);

    }
}
