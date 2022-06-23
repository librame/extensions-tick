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
/// 定义抽象实现 <see cref="IIdGenerator{TId}"/>。
/// </summary>
/// <typeparam name="TId">指定的标识类型。</typeparam>
public abstract class AbstractIdGenerator<TId> : IIdGenerator<TId>
    where TId : IEquatable<TId>
{
    /// <summary>
    /// 标识类型。
    /// </summary>
    public virtual Type IdType
        => typeof(TId);


    /// <summary>
    /// 生成标识。
    /// </summary>
    /// <returns>返回 <typeparamref name="TId"/>。</returns>
    public abstract TId GenerateId();

    /// <summary>
    /// 异步生成标识。
    /// </summary>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含 <typeparamref name="TId"/> 的异步操作。</returns>
    public abstract Task<TId> GenerateIdAsync(CancellationToken cancellationToken = default);


    /// <summary>
    /// 生成标识对象。
    /// </summary>
    /// <returns>返回标识对象。</returns>
    public virtual object GenerateObjectId()
    {
        var id = GenerateId();
        if (id is null)
            throw new ArgumentException($"The {nameof(GenerateId)}() method generate id is null.");

        return id;
    }

    /// <summary>
    /// 异步生成标识对象。
    /// </summary>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含标识对象的异步操作。</returns>
    public virtual async Task<object> GenerateObjectIdAsync(CancellationToken cancellationToken = default)
    {
        var id = await GenerateIdAsync(cancellationToken).DisableAwaitContext();
        if (id is null)
            throw new ArgumentException($"The {nameof(GenerateId)}() method generate id is null.");

        return id;
    }

}
