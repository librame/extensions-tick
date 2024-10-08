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
/// 定义泛型标识符接口。
/// </summary>
/// <typeparam name="TId">指定的标识类型（兼容各种引用与值类型标识）。</typeparam>
public interface IIdentifier<TId> : IEquatable<IIdentifier<TId>>, IObjectIdentifier
    where TId : IEquatable<TId>
{
    /// <summary>
    /// 标识。
    /// </summary>
    TId Id { get; set; }


    /// <summary>
    /// 转换为标识。
    /// </summary>
    /// <param name="id">给定的标识对象。</param>
    /// <param name="paramName">给定的参数名（可选；默认为 <paramref name="id"/> 调用参数名）。</param>
    /// <returns>返回 <typeparamref name="TId"/>。</returns>
    TId ToId(object? id, [CallerArgumentExpression(nameof(id))] string? paramName = null)
        => id.As<TId>(paramName);


    #region IObjectIdentifier

    /// <summary>
    /// 标识类型。
    /// </summary>
    [NotMapped]
    Type IObjectIdentifier.IdType
        => typeof(TId);


    /// <summary>
    /// 获取对象标识。
    /// </summary>
    /// <returns>返回标识（兼容各种引用与值类型标识）。</returns>
    object IObjectIdentifier.GetObjectId()
        => Id;

    /// <summary>
    /// 异步获取对象标识。
    /// </summary>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
    /// <returns>返回一个包含标识（兼容各种引用与值类型标识）的异步操作。</returns>
    async ValueTask<object> IObjectIdentifier.GetObjectIdAsync(CancellationToken cancellationToken)
        => await TaskExtensions.InvokeAsync(GetObjectId, cancellationToken);


    /// <summary>
    /// 设置对象标识。
    /// </summary>
    /// <param name="newId">给定的新标识对象。</param>
    /// <returns>返回标识（兼容各种引用与值类型标识）。</returns>
    object IObjectIdentifier.SetObjectId(object newId)
    {
        Id = ToId(newId, nameof(newId));
        return newId;
    }

    /// <summary>
    /// 异步设置对象标识。
    /// </summary>
    /// <param name="newId">给定的新对象标识。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
    /// <returns>返回一个包含标识（兼容各种引用与值类型标识）的异步操作。</returns>
    async ValueTask<object> IObjectIdentifier.SetObjectIdAsync(object newId,
        CancellationToken cancellationToken)
        => await TaskExtensions.InvokeAsync(() => SetObjectId(newId), cancellationToken);

    #endregion

}
