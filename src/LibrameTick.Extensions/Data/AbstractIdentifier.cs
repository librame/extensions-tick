#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Resources;

namespace Librame.Extensions.Data;

/// <summary>
/// 定义抽象实现 <see cref="IIdentifier{TId}"/>。
/// </summary>
/// <typeparam name="TId">指定的标识类型。</typeparam>
[NotMapped]
public abstract class AbstractIdentifier<TId> : IIdentifier<TId>
    where TId : IEquatable<TId>
{

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    /// <summary>
    /// 标识。
    /// </summary>
    [Display(Name = nameof(Id), GroupName = nameof(DataResource.DataGroup), ResourceType = typeof(DataResource))]
    public virtual TId Id { get; set; }

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.


    /// <summary>
    /// 标识类型。
    /// </summary>
    [NotMapped]
    public virtual Type IdType
        => typeof(TId);


    /// <summary>
    /// 比较相等。
    /// </summary>
    /// <param name="other">给定的 <see cref="IIdentifier{TId}"/>。</param>
    /// <returns>返回布尔值。</returns>
    public virtual bool Equals(IIdentifier<TId>? other)
        => other is not null && Id.Equals(other.Id);


    /// <summary>
    /// 转换为标识。
    /// </summary>
    /// <param name="id">给定的标识对象。</param>
    /// <param name="paramName">给定的参数名称。</param>
    /// <returns>返回 <typeparamref name="TId"/>。</returns>
    public virtual TId ToId(object? id, string? paramName)
        => id.AsNotNull<TId>(paramName);


    /// <summary>
    /// 获取对象标识。
    /// </summary>
    /// <returns>返回标识（兼容各种引用与值类型标识）。</returns>
    public virtual object GetObjectId()
        => Id;

    /// <summary>
    /// 异步获取对象标识。
    /// </summary>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含标识（兼容各种引用与值类型标识）的异步操作。</returns>
    public virtual ValueTask<object> GetObjectIdAsync(CancellationToken cancellationToken)
        => cancellationToken.RunValueTask(() => (object)Id);


    /// <summary>
    /// 设置对象标识。
    /// </summary>
    /// <param name="newId">给定的新标识对象。</param>
    /// <returns>返回标识（兼容各种引用与值类型标识）。</returns>
    public virtual object SetObjectId(object newId)
    {
        Id = ToId(newId, nameof(newId));
        return newId;
    }

    /// <summary>
    /// 异步设置对象标识。
    /// </summary>
    /// <param name="newId">给定的新对象标识。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含标识（兼容各种引用与值类型标识）的异步操作。</returns>
    public virtual ValueTask<object> SetObjectIdAsync(object newId, CancellationToken cancellationToken = default)
    {
        var id = ToId(newId, nameof(newId));

        return cancellationToken.RunValueTask(() =>
        {
            Id = id;
            return newId;
        });
    }


    /// <summary>
    /// 转换为字符串。
    /// </summary>
    /// <returns>返回字符串。</returns>
    public override string ToString()
        => $"{nameof(Id)}={Id}";

}
