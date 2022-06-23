#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data;

/// <summary>
/// <see cref="IParentIdentifier{TId}"/> 静态扩展。
/// </summary>
public static class ParentIdentifierExtensions
{

    /// <summary>
    /// 异步设置父标识。
    /// </summary>
    /// <typeparam name="TId">指定的标识类型（兼容各种引用与值类型标识）。</typeparam>
    /// <param name="parentIdentifier">给定的 <see cref="IParentIdentifier{TId}"/>。</param>
    /// <param name="newParentIdFactory">给定的新 <typeparamref name="TId"/> 工厂方法。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含 <typeparamref name="TId"/> （兼容各种引用与值类型标识）的异步操作。</returns>
    public static ValueTask<TId?> SetParentIdAsync<TId>(this IParentIdentifier<TId> parentIdentifier,
        Func<TId?, TId?> newParentIdFactory, CancellationToken cancellationToken = default)
        where TId : IEquatable<TId>
        => cancellationToken.RunValueTask(() => parentIdentifier.ParentId = newParentIdFactory(parentIdentifier.ParentId));


    /// <summary>
    /// 异步设置对象父标识。
    /// </summary>
    /// <param name="parentIdentifier">给定的 <see cref="IObjectParentIdentifier"/>。</param>
    /// <param name="newParentIdFactory">给定的新对象父标识工厂方法。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含父标识（兼容各种引用与值类型标识）的异步操作。</returns>
    public static async ValueTask<object?> SetObjectParentIdAsync(this IObjectParentIdentifier parentIdentifier,
        Func<object?, object?> newParentIdFactory, CancellationToken cancellationToken = default)
    {
        var currentParentId = await parentIdentifier.GetObjectParentIdAsync(cancellationToken).DisableAwaitContext();

        return await parentIdentifier.SetObjectParentIdAsync(newParentIdFactory(currentParentId), cancellationToken)
            .DisableAwaitContext();
    }

}
