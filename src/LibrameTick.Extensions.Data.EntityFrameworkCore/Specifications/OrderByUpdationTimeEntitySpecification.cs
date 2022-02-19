#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data.Specifications;

/// <summary>
/// 定义一个实现 <see cref="IEntitySpecification{T}"/> 并按更新时间降序排列的实体规约。
/// </summary>
/// <typeparam name="T">指定的类型。</typeparam>
/// <typeparam name="TUpdatedTime">指定的更新时间类型（提供对 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/> 的支持）。</typeparam>
public class OrderByUpdationTimeEntitySpecification<T, TUpdatedTime> : BaseEntitySpecification<T>
    where T : class, IUpdationTime<TUpdatedTime>
    where TUpdatedTime : struct
{
    /// <summary>
    /// 构造一个 <see cref="OrderByUpdationTimeEntitySpecification{T, TUpdatedTime}"/>。
    /// </summary>
    public OrderByUpdationTimeEntitySpecification()
    {
        SetOrderByDescending(s => s.UpdatedTime);
    }

    /// <summary>
    /// 使用规约条件构造一个 <see cref="OrderByUpdationTimeEntitySpecification{T, TUpdatedTime}"/> 实例。
    /// </summary>
    /// <param name="criterion">给定的判断依据表达式。</param>
    public OrderByUpdationTimeEntitySpecification(Expression<Func<T, bool>> criterion)
        : base(criterion)
    {
        SetOrderByDescending(s => s.UpdatedTime);
    }

}
