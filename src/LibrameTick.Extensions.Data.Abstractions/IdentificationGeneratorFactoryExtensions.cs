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
        /// 获取指定类型的新标识（默认已集成 <see cref="string"/> “MongoDB”、<see cref="long"/> “雪花”、<see cref="Guid"/> “COMB for SQL Server” 等标识类型的生成器）。
        /// </summary>
        /// <typeparam name="TId">指定的标识类型。</typeparam>
        /// <param name="factory">给定的 <see cref="IIdentificationGeneratorFactory"/>。</param>
        /// <returns>返回 <typeparamref name="TId"/>。</returns>
        public static TId GetNewId<TId>(this IIdentificationGeneratorFactory factory)
            => factory.GetIdGenerator<TId>().GenerateId();

        /// <summary>
        /// 异步获取指定类型的新标识（默认已集成 <see cref="string"/> “MongoDB”、<see cref="long"/> “雪花”、<see cref="Guid"/> “COMB for SQL Server” 等标识类型的生成器）。
        /// </summary>
        /// <typeparam name="TId">指定的标识类型。</typeparam>
        /// <param name="factory">给定的 <see cref="IIdentificationGeneratorFactory"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TId"/> 的异步操作。</returns>
        public static Task<TId> GetNewIdAsync<TId>(this IIdentificationGeneratorFactory factory,
            CancellationToken cancellationToken = default)
            => factory.GetIdGenerator<TId>().GenerateIdAsync(cancellationToken);


        /// <summary>
        /// 获取指定类型的新标识（默认已集成 <see cref="string"/> “MongoDB”、<see cref="long"/> “雪花”、<see cref="Guid"/> “COMB for SQL Server” 等标识类型的生成器）。
        /// </summary>
        /// <param name="factory">给定的 <see cref="IIdentificationGeneratorFactory"/>。</param>
        /// <param name="idType">给定的标识类型。</param>
        /// <returns>返回标识对象。</returns>
        public static object? GetNewId(this IIdentificationGeneratorFactory factory, Type idType)
            => factory.GetIdGenerator(idType).GenerateObjectId();

        /// <summary>
        /// 异步获取指定类型的新标识（默认已集成 <see cref="string"/> “MongoDB”、<see cref="long"/> “雪花”、<see cref="Guid"/> “COMB for SQL Server” 等标识类型的生成器）。
        /// </summary>
        /// <param name="factory">给定的 <see cref="IIdentificationGeneratorFactory"/>。</param>
        /// <param name="idType">给定的标识类型。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含标识对象的异步操作。</returns>
        public static Task<object?> GetNewIdAsync<TId>(this IIdentificationGeneratorFactory factory, Type idType,
            CancellationToken cancellationToken = default)
            => factory.GetIdGenerator(idType).GenerateObjectIdAsync(cancellationToken);

    }
}
