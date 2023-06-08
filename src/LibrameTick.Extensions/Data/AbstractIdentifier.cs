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
    /// 比较引用与标识是否相等。
    /// </summary>
    /// <param name="other">给定的 <see cref="IIdentifier{TId}"/>。</param>
    /// <returns>返回布尔值。</returns>
    protected virtual bool ReferenceAndIdEquals(IIdentifier<TId> other)
    {
        if (ReferenceEquals(this, other))
            return true;

        if (GetType() != other.GetType())
            return false;

        return Id.Equals(other.Id);
    }

    /// <summary>
    /// 比较相等。
    /// </summary>
    /// <param name="other">给定的 <see cref="IIdentifier{TId}"/>。</param>
    /// <returns>返回布尔值。</returns>
    public virtual bool Equals(IIdentifier<TId>? other)
        => other is not null && ReferenceAndIdEquals(other);


    /// <summary>
    /// 判断引用对象是否相等。注：默认实例将被认定不相等。
    /// </summary>
    /// <param name="obj">给定要比较的对象。</param>
    /// <returns>返回是否相等的布尔值。</returns>
    public override bool Equals(object? obj)
    {
        if (obj is not IIdentifier<TId> other)
            return false;

        if (!ReferenceAndIdEquals(other))
            return false;

        if (other is not AbstractIdentifier<TId> otherId)
            return true; // 如果不是继承于抽象标识符，则直接返回相等

        // 比较是否为默认实例
        return !(IsTransient() || otherId.IsTransient());
    }

    private bool IsTransient()
        => Id is null || Id.Equals(default);


    /// <summary>
    /// 获取哈希码。
    /// </summary>
    /// <returns>返回整数。</returns>
    public override int GetHashCode()
        => Id.GetHashCode();


    /// <summary>
    /// 转换为字符串。
    /// </summary>
    /// <returns>返回字符串。</returns>
    public override string ToString()
        => $"{nameof(Id)}={Id}";


    /// <summary>
    /// 比较相等。注：同时为空表示相等。
    /// </summary>
    /// <param name="a">给定的 <see cref="AbstractIdentifier{TId}"/>。</param>
    /// <param name="b">给定的 <see cref="AbstractIdentifier{TId}"/>。</param>
    /// <returns>返回是否相等的布尔值。</returns>
    public static bool operator ==(AbstractIdentifier<TId>? a, AbstractIdentifier<TId>? b)
    {
        if (a is null && b is null)
            return true;

        if (a is null || b is null)
            return false;

        return a.Equals(b);
    }

    /// <summary>
    /// 比较不等。注：同时为空表示相等。
    /// </summary>
    /// <param name="a">给定的 <see cref="AbstractIdentifier{TId}"/>。</param>
    /// <param name="b">给定的 <see cref="AbstractIdentifier{TId}"/>。</param>
    /// <returns>返回是否不等的布尔值。</returns>
    public static bool operator !=(AbstractIdentifier<TId>? a, AbstractIdentifier<TId>? b)
        => !(a == b);

}
