#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Specifications;

/// <summary>
/// 定义一个用于 <see cref="IQueryable{T}"/> 筛选的表达式规约接口。
/// </summary>
/// <typeparam name="T">指定的规约类型。</typeparam>
public interface IExpressionSpecification<T>
{
    /// <summary>
    /// 判断依据表达式。
    /// </summary>
    Expression<Func<T, bool>>? Criterion { get; }


    /// <summary>
    /// 升序排列方法。
    /// </summary>
    Expression<Func<T, object>>? OrderBy { get; }

    /// <summary>
    /// 降序排列方法。
    /// </summary>
    Expression<Func<T, object>>? OrderByDescending { get; }

    /// <summary>
    /// 出具提供方法。
    /// </summary>
    Func<IQueryable<T>, T>? Provider { get; }


    /// <summary>
    /// 评估可查询对象。
    /// </summary>
    /// <param name="queryable">给定的 <see cref="IQueryable{T}"/>。</param>
    /// <returns>返回 <see cref="IQueryable{T}"/>。</returns>
    IQueryable<T> Evaluate(IQueryable<T> queryable);

    /// <summary>
    /// 出具可查询对象。
    /// </summary>
    /// <param name="queryable">给定的 <see cref="IQueryable{T}"/>。</param>
    /// <returns>返回 <typeparamref name="T"/>。</returns>
    T Issue(IQueryable<T> queryable);

    /// <summary>
    /// 出具经过评估的可查询对象。
    /// </summary>
    /// <param name="queryable">给定的 <see cref="IQueryable{T}"/>。</param>
    /// <returns>返回 <typeparamref name="T"/>。</returns>
    T IssueEvaluate(IQueryable<T> queryable);


    /// <summary>
    /// 是否满足规约要求。
    /// </summary>
    /// <param name="value">给定的实例。</param>
    /// <returns>返回布尔值。</returns>
    bool IsSatisfiedBy(T value);


    /// <summary>
    /// 设置升序排列表达式。
    /// </summary>
    /// <param name="orderBy">给定的升序排列表达式。</param>
    /// <returns>返回 <see cref="IExpressionSpecification{T}"/>。</returns>
    IExpressionSpecification<T> SetOrderBy(Expression<Func<T, object>> orderBy);

    /// <summary>
    /// 设置降序排列表达式。
    /// </summary>
    /// <param name="orderByDescending">给定的降序排列表达式。</param>
    /// <returns>返回 <see cref="IExpressionSpecification{T}"/>。</returns>
    IExpressionSpecification<T> SetOrderByDescending(Expression<Func<T, object>> orderByDescending);

    /// <summary>
    /// 设置出具提供方法。
    /// </summary>
    /// <param name="provider">给定的出具提供方法。</param>
    /// <returns>返回 <see cref="IExpressionSpecification{T}"/>。</returns>
    IExpressionSpecification<T> SetProvider(Func<IQueryable<T>, T> provider);
}
