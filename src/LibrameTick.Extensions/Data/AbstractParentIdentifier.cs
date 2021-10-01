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
/// 定义抽象实现 <see cref="IParentIdentifier{TId}"/>。
/// </summary>
/// <typeparam name="TId">指定的标识类型。</typeparam>
[NotMapped]
public abstract class AbstractParentIdentifier<TId> : AbstractIdentifier<TId>, IParentIdentifier<TId>
    where TId : IEquatable<TId>
{

    /// <summary>
    /// 父标识。
    /// </summary>
    [Display(Name = nameof(ParentId), GroupName = nameof(DataResource.DataGroup), ResourceType = typeof(DataResource))]
    public virtual TId? ParentId { get; set; }


    /// <summary>
    /// 转换为标识。
    /// </summary>
    /// <param name="parentId">给定的标识对象。</param>
    /// <returns>返回 <typeparamref name="TId"/>。</returns>
    public virtual TId? ToParentId(object? parentId)
        => ParentId;


    /// <summary>
    /// 获取对象标识。
    /// </summary>
    /// <returns>返回标识（兼容各种引用与值类型标识）。</returns>
    public virtual object? GetObjectParentId()
        => ParentId;

    /// <summary>
    /// 异步获取对象标识。
    /// </summary>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含标识（兼容各种引用与值类型标识）的异步操作。</returns>
    public virtual ValueTask<object?> GetObjectParentIdAsync(CancellationToken cancellationToken)
        => cancellationToken.RunValueTask(GetObjectParentId);


    /// <summary>
    /// 设置对象标识。
    /// </summary>
    /// <param name="newParentId">给定的新对象标识。</param>
    /// <returns>返回标识（兼容各种引用与值类型标识）。</returns>
    public virtual object? SetObjectParentId(object? newParentId)
    {
        ParentId = ToId(newParentId, nameof(newParentId));
        return newParentId;
    }

    /// <summary>
    /// 异步设置对象标识。
    /// </summary>
    /// <param name="newParentId">给定的新对象标识。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含标识（兼容各种引用与值类型标识）的异步操作。</returns>
    public virtual ValueTask<object?> SetObjectParentIdAsync(object? newParentId, CancellationToken cancellationToken = default)
    {
        return cancellationToken.RunValueTask(() =>
        {
            ParentId = ToParentId(newParentId);
            return newParentId;
        });
    }


    /// <summary>
    /// 转换为标识键值对字符串。
    /// </summary>
    /// <returns>返回字符串。</returns>
    public override string ToString()
        => $"{base.ToString()};{nameof(ParentId)}={ParentId}";

}
