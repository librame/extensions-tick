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
/// 定义一个实现 <see cref="ISpecification{T}"/> 的复合规约。
/// </summary>
/// <typeparam name="T">指定的规约类型。</typeparam>
public class CompositeSpecification<T> : ISpecification<T>
{
    private readonly IEnumerable<ISpecification<T>> _specifications;


    /// <summary>
    /// 构造一个 <see cref="CompositeSpecification{T}"/>。
    /// </summary>
    /// <param name="specifications">给定的 <see cref="ISpecification{T}"/> 数组。</param>
    public CompositeSpecification(params ISpecification<T>[] specifications)
        : this((IEnumerable<ISpecification<T>>)specifications)
    {
    }

    /// <summary>
    /// 构造一个 <see cref="CompositeSpecification{T}"/>。
    /// </summary>
    /// <param name="specifications">给定的 <see cref="ISpecification{T}"/> 集合。</param>
    /// <param name="addCriterion">给定要增加的判断依据（可选）。</param>
    public CompositeSpecification(IEnumerable<ISpecification<T>> specifications,
        Func<T, bool>? addCriterion = null)
    {
        _specifications = specifications;
        Criterion = addCriterion;
    }


    /// <summary>
    /// 增加的判断依据。
    /// </summary>
    public Func<T, bool>? Criterion { get; init; }


    /// <summary>
    /// 增加的升序排列方法。
    /// </summary>
    public Func<T, object>? OrderBy { get; private set; }

    /// <summary>
    /// 增加的降序排列方法。
    /// </summary>
    public Func<T, object>? OrderByDescending { get; private set; }

    /// <summary>
    /// 出具提供程序。
    /// </summary>
    public Func<IEnumerable<T>, T>? Provider { get; private set; }


    /// <summary>
    /// 评估可枚举对象。
    /// </summary>
    /// <param name="enumerable">给定的 <see cref="IEnumerable{T}"/>。</param>
    /// <returns>返回 <see cref="IEnumerable{T}"/>。</returns>
    public virtual IEnumerable<T> Evaluate(IEnumerable<T> enumerable)
    {
        if (Criterion is not null)
            enumerable = enumerable.Where(Criterion);

        if (OrderBy is not null)
            enumerable = enumerable.OrderBy(OrderBy);

        if (OrderByDescending is not null)
            enumerable = enumerable.OrderByDescending(OrderByDescending);

        foreach (var specification in _specifications)
        {
            enumerable = specification.Evaluate(enumerable);
        }

        return enumerable;
    }

    /// <summary>
    /// 出具可枚举对象。
    /// </summary>
    /// <param name="enumerable">给定的 <see cref="IEnumerable{T}"/>。</param>
    /// <returns>返回 <typeparamref name="T"/>。</returns>
    public virtual T Issue(IEnumerable<T> enumerable)
    {
        var list = new List<T>();

        if (Provider is not null)
            list.Add(Provider(enumerable));

        foreach (var specification in _specifications)
        {
            list.Add(specification.Issue(enumerable));
        }

        return list.Distinct().First();
    }

    /// <summary>
    /// 出具经过评估的可枚举对象。
    /// </summary>
    /// <param name="enumerable">给定的 <see cref="IEnumerable{T}"/>。</param>
    /// <returns>返回 <typeparamref name="T"/>。</returns>
    public virtual T IssueEvaluate(IEnumerable<T> enumerable)
        => Issue(Evaluate(enumerable));


    /// <summary>
    /// 是否满足规约要求。
    /// </summary>
    /// <param name="value">给定的实例。</param>
    /// <returns>返回布尔值。</returns>
    public virtual bool IsSatisfiedBy(T value)
        => true;


    /// <summary>
    /// 设置升序排列方法。
    /// </summary>
    /// <param name="orderBy">给定的升序排列方法。</param>
    /// <returns>返回 <see cref="ISpecification{T}"/>。</returns>
    public ISpecification<T> SetOrderBy(Func<T, object> orderBy)
    {
        OrderBy = orderBy;
        return this;
    }

    /// <summary>
    /// 设置降序排列方法。
    /// </summary>
    /// <param name="orderByDescending">给定的降序排列方法。</param>
    /// <returns>返回 <see cref="ISpecification{T}"/>。</returns>
    public ISpecification<T> SetOrderByDescending(Func<T, object> orderByDescending)
    {
        OrderByDescending = orderByDescending;
        return this;
    }

    /// <summary>
    /// 设置出具提供方法。
    /// </summary>
    /// <param name="provider">给定的出具提供方法。</param>
    /// <returns>返回 <see cref="ISpecification{T}"/>。</returns>
    public ISpecification<T> SetProvider(Func<IEnumerable<T>, T> provider)
    {
        Provider = provider;
        return this;
    }

}
