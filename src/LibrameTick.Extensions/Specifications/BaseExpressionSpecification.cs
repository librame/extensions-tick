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
/// 定义实现 <see cref="IExpressionSpecification{T}"/> 的基础表达式规约。
/// </summary>
/// <typeparam name="T">指定的规约类型。</typeparam>
public class BaseExpressionSpecification<T> : IExpressionSpecification<T>
{
    /// <summary>
    /// 构造一个 <see cref="BaseExpressionSpecification{T}"/>。
    /// </summary>
    public BaseExpressionSpecification()
    {
    }

    /// <summary>
    /// 使用指定的判断依据构造一个 <see cref="BaseExpressionSpecification{T}"/>。
    /// </summary>
    /// <param name="criterion">给定的判断依据。</param>
    public BaseExpressionSpecification(Expression<Func<T, bool>>? criterion)
    {
        Criterion = criterion;
    }


    /// <summary>
    /// 判断依据表达式。
    /// </summary>
    public Expression<Func<T, bool>>? Criterion { get; init; }


    /// <summary>
    /// 升序排列表达式。
    /// </summary>
    public Expression<Func<T, object>>? OrderBy { get; private set; }

    /// <summary>
    /// 降序排列表达式。
    /// </summary>
    public Expression<Func<T, object>>? OrderByDescending { get; private set; }

    /// <summary>
    /// 出具提供方法。
    /// </summary>
    public Func<IQueryable<T>, T>? Provider { get; private set; }


    /// <summary>
    /// 评估可枚举对象。
    /// </summary>
    /// <param name="queryable">给定的 <see cref="IQueryable{T}"/>。</param>
    /// <returns>返回 <see cref="IQueryable{T}"/>。</returns>
    public virtual IQueryable<T> Evaluate(IQueryable<T> queryable)
    {
        if (Criterion is not null)
            queryable = queryable.Where(Criterion);

        if (OrderBy is not null)
            queryable = queryable.OrderBy(OrderBy);

        if (OrderByDescending is not null)
            queryable = queryable.OrderByDescending(OrderByDescending);

        return queryable;
    }

    /// <summary>
    /// 出具可查询对象。
    /// </summary>
    /// <param name="queryable">给定的 <see cref="IQueryable{T}"/>。</param>
    /// <returns>返回 <typeparamref name="T"/>。</returns>
    public virtual T Issue(IQueryable<T> queryable)
    {
        if (Provider is not null)
            return Provider(queryable);

        return queryable.First();
    }

    /// <summary>
    /// 出具经过评估的可查询对象。
    /// </summary>
    /// <param name="queryable">给定的 <see cref="IQueryable{T}"/>。</param>
    /// <returns>返回 <typeparamref name="T"/>。</returns>
    public virtual T IssueEvaluate(IQueryable<T> queryable)
        => Issue(Evaluate(queryable));


    /// <summary>
    /// 是否满足规约要求。
    /// </summary>
    /// <param name="value">给定的实例。</param>
    /// <returns>返回布尔值。</returns>
    public virtual bool IsSatisfiedBy(T value)
        => true;


    /// <summary>
    /// 设置升序排列表达式。
    /// </summary>
    /// <param name="orderBy">给定的升序排列表达式。</param>
    /// <returns>返回 <see cref="IExpressionSpecification{T}"/>。</returns>
    public IExpressionSpecification<T> SetOrderBy(Expression<Func<T, object>> orderBy)
    {
        OrderBy = orderBy;
        return this;
    }

    /// <summary>
    /// 设置降序排列表达式。
    /// </summary>
    /// <param name="orderByDescending">给定的降序排列表达式。</param>
    /// <returns>返回 <see cref="IExpressionSpecification{T}"/>。</returns>
    public IExpressionSpecification<T> SetOrderByDescending(Expression<Func<T, object>> orderByDescending)
    {
        OrderByDescending = orderByDescending;
        return this;
    }

    /// <summary>
    /// 设置出具提供方法。
    /// </summary>
    /// <param name="provider">给定的出具提供方法。</param>
    /// <returns>返回 <see cref="IExpressionSpecification{T}"/>。</returns>
    public IExpressionSpecification<T> SetProvider(Func<IQueryable<T>, T> provider)
    {
        Provider = provider;
        return this;
    }

}
