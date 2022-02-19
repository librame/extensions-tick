#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Bootstraps;

namespace Librame.Extensions.Specifications;

/// <summary>
/// 定义实现 <see cref="IExpressionSpecification{T}"/> 的复合表达式规约。
/// </summary>
/// <typeparam name="T">指定的规约类型。</typeparam>
public class CompositeExpressionSpecification<T> : IExpressionSpecification<T>
{
    private readonly IEnumerable<IExpressionSpecification<T>> _specifications;
    private bool _isJoined;


    /// <summary>
    /// 构造一个 <see cref="CompositeExpressionSpecification{T}"/>。
    /// </summary>
    /// <param name="specifications">给定的 <see cref="IExpressionSpecification{T}"/> 数组。</param>
    public CompositeExpressionSpecification(params IExpressionSpecification<T>[] specifications)
        : this((IEnumerable<IExpressionSpecification<T>>)specifications)
    {
    }

    /// <summary>
    /// 使用指定的判断依据构造一个 <see cref="CompositeExpressionSpecification{T}"/>。
    /// </summary>
    /// <param name="specifications">给定的 <see cref="IExpressionSpecification{T}"/> 集合。</param>
    /// <param name="addCriterion">给定要增加的判断依据表达式。</param>
    public CompositeExpressionSpecification(IEnumerable<IExpressionSpecification<T>> specifications,
        Expression<Func<T, bool>>? addCriterion = null)
    {
        _specifications = specifications;
        Criterion = addCriterion;
    }


    private void Join()
    {
        Bootstrapper.GetLocker().Lock(i =>
        {
            if (!_isJoined)
            {
                foreach (var specification in _specifications)
                {
                    if (specification.Criterion != null)
                    {
                        if (Criterion is null)
                            Criterion = specification.Criterion;
                        else
                            Criterion = Criterion.AndAlso(specification.Criterion);
                    }

                    if (specification.OrderBy != null)
                    {
                        if (OrderBy is null)
                            OrderBy = specification.OrderBy;
                        else
                            OrderBy = OrderBy.AndAlso(specification.OrderBy);
                    }

                    if (specification.OrderByDescending != null)
                    {
                        if (OrderByDescending is null)
                            OrderByDescending = specification.OrderByDescending;
                        else
                            OrderByDescending = OrderByDescending.AndAlso(specification.OrderByDescending);
                    }
                }

                _isJoined = true;
            }
        });
    }


    /// <summary>
    /// 判断依据表达式。
    /// </summary>
    public Expression<Func<T, bool>>? Criterion { get; private set; }


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
        Join();

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
        var list = new List<T>();

        if (Provider is not null)
            list.Add(Provider(queryable));

        foreach (var specification in _specifications)
        {
            list.Add(specification.Issue(queryable));
        }

        return list.Distinct().First();
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
