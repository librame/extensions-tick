#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Data;

namespace Librame.Extensions.Specifications;

/// <summary>
/// 定义一个实现 <see cref="IEntitySpecification{T}"/> 并按发表时间降序排列的实体规约。
/// </summary>
/// <typeparam name="T">指定的类型。</typeparam>
/// <typeparam name="TPublishedTime">指定的发表时间类型（提供对 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/> 的支持）。</typeparam>
public class OrderByPublicationTimeEntitySpecification<T, TPublishedTime> : BaseEntitySpecification<T>
    where T : class, IPublicationTime<TPublishedTime>
    where TPublishedTime : struct
{
    /// <summary>
    /// 构造一个 <see cref="OrderByPublicationTimeEntitySpecification{T, TPublishedTime}"/>。
    /// </summary>
    public OrderByPublicationTimeEntitySpecification()
    {
        SetOrderByDescending(s => s.PublishedTime);
    }

    /// <summary>
    /// 使用规约条件构造一个 <see cref="OrderByPublicationTimeEntitySpecification{T, TPublishedTime}"/> 实例。
    /// </summary>
    /// <param name="criterion">给定的判断依据表达式。</param>
    public OrderByPublicationTimeEntitySpecification(Expression<Func<T, bool>> criterion)
        : base(criterion)
    {
        SetOrderByDescending(s => s.PublishedTime);
    }

}
