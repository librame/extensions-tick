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

///// <summary>
///// 定义实现 <see cref="ISpecification{T}"/> 的基础规约。
///// </summary>
///// <typeparam name="T">指定的规约类型。</typeparam>
//public class BaseSpecification<T> : ISpecification<T>
//{
//    /// <summary>
//    /// 构造一个 <see cref="BaseSpecification{T}"/>。
//    /// </summary>
//    public BaseSpecification()
//    {
//    }

//    /// <summary>
//    /// 使用指定的判断依据构造一个 <see cref="BaseSpecification{T}"/>。
//    /// </summary>
//    /// <param name="criterion">给定的判断依据。</param>
//    public BaseSpecification(Func<T, bool>? criterion)
//    {
//        Criterion = criterion;
//    }


//    /// <summary>
//    /// 判断依据。
//    /// </summary>
//    public Func<T, bool>? Criterion { get; init; }


//    /// <summary>
//    /// 升序排列方法。
//    /// </summary>
//    public Func<T, object>? OrderBy { get; private set; }

//    /// <summary>
//    /// 降序排列方法。
//    /// </summary>
//    public Func<T, object>? OrderByDescending { get; private set; }

//    /// <summary>
//    /// 出具提供程序。
//    /// </summary>
//    public Func<IEnumerable<T>, T>? Provider { get; private set; }


//    /// <summary>
//    /// 评估可枚举对象。
//    /// </summary>
//    /// <param name="enumerable">给定的 <see cref="IEnumerable{T}"/>。</param>
//    /// <returns>返回 <see cref="IEnumerable{T}"/>。</returns>
//    public virtual IEnumerable<T> Evaluate(IEnumerable<T> enumerable)
//    {
//        if (Criterion is not null)
//            enumerable = enumerable.Where(Criterion);

//        if (OrderBy is not null)
//            enumerable = enumerable.OrderBy(OrderBy);

//        if (OrderByDescending is not null)
//            enumerable = enumerable.OrderByDescending(OrderByDescending);

//        return enumerable;
//    }

//    /// <summary>
//    /// 出具可枚举对象。
//    /// </summary>
//    /// <param name="enumerable">给定的 <see cref="IEnumerable{T}"/>。</param>
//    /// <returns>返回 <typeparamref name="T"/>。</returns>
//    public virtual T Issue(IEnumerable<T> enumerable)
//    {
//        if (Provider is not null)
//            return Provider(enumerable);

//        return enumerable.First();
//    }

//    /// <summary>
//    /// 出具经过评估的可枚举对象。
//    /// </summary>
//    /// <param name="enumerable">给定的 <see cref="IEnumerable{T}"/>。</param>
//    /// <returns>返回 <typeparamref name="T"/>。</returns>
//    public virtual T IssueEvaluate(IEnumerable<T> enumerable)
//        => Issue(Evaluate(enumerable));


//    /// <summary>
//    /// 是否满足规约要求。
//    /// </summary>
//    /// <param name="value">给定的实例。</param>
//    /// <returns>返回布尔值。</returns>
//    public virtual bool IsSatisfiedBy(T value)
//        => true;


//    /// <summary>
//    /// 设置升序排列方法。
//    /// </summary>
//    /// <param name="orderBy">给定的升序排列方法。</param>
//    /// <returns>返回 <see cref="ISpecification{T}"/>。</returns>
//    public ISpecification<T> SetOrderBy(Func<T, object> orderBy)
//    {
//        OrderBy = orderBy;
//        return this;
//    }

//    /// <summary>
//    /// 设置降序排列方法。
//    /// </summary>
//    /// <param name="orderByDescending">给定的降序排列方法。</param>
//    /// <returns>返回 <see cref="ISpecification{T}"/>。</returns>
//    public ISpecification<T> SetOrderByDescending(Func<T, object> orderByDescending)
//    {
//        OrderByDescending = orderByDescending;
//        return this;
//    }

//    /// <summary>
//    /// 设置出具提供方法。
//    /// </summary>
//    /// <param name="provider">给定的出具提供方法。</param>
//    /// <returns>返回 <see cref="ISpecification{T}"/>。</returns>
//    public ISpecification<T> SetProvider(Func<IEnumerable<T>, T> provider)
//    {
//        Provider = provider;
//        return this;
//    }

//}
