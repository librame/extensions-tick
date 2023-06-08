#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.IdGenerators;

/// <summary>
/// 定义泛型标识生成器接口。
/// </summary>
/// <typeparam name="TId">指定的标识类型。</typeparam>
public interface IIdGenerator<TId> : IObjectIdGenerator
    where TId : IEquatable<TId>
{
    /// <summary>
    /// 生成标识。
    /// </summary>
    /// <returns>返回 <typeparamref name="TId"/>。</returns>
    TId GenerateId();

    /// <summary>
    /// 异步生成标识。
    /// </summary>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含 <typeparamref name="TId"/> 的异步操作。</returns>
    ValueTask<TId> GenerateIdAsync(CancellationToken cancellationToken = default);
}
