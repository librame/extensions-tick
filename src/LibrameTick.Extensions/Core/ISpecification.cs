#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core;

/// <summary>
/// 定义规约接口。
/// </summary>
/// <typeparam name="T">指定的类型。</typeparam>
public interface ISpecification<T>
{
    /// <summary>
    /// 判断依据。
    /// </summary>
    Func<T, bool>? Criterion { get; }


    /// <summary>
    /// 升序排列方法。
    /// </summary>
    Func<T, object>? OrderBy { get; }

    /// <summary>
    /// 降序排列方法。
    /// </summary>
    Func<T, object>? OrderByDescending { get; }

    /// <summary>
    /// 出具提供方法。
    /// </summary>
    Func<IEnumerable<T>, T>? Provider { get; }


    /// <summary>
    /// 评估可枚举对象。
    /// </summary>
    /// <param name="enumerable">给定的 <see cref="IEnumerable{T}"/>。</param>
    /// <returns>返回 <see cref="IEnumerable{T}"/>。</returns>
    IEnumerable<T> Evaluate(IEnumerable<T> enumerable);

    /// <summary>
    /// 出具可枚举对象。
    /// </summary>
    /// <param name="enumerable">给定的 <see cref="IEnumerable{T}"/>。</param>
    /// <returns>返回 <typeparamref name="T"/>。</returns>
    T Issue(IEnumerable<T> enumerable);

    /// <summary>
    /// 出具经过评估的可枚举对象。
    /// </summary>
    /// <param name="enumerable">给定的 <see cref="IEnumerable{T}"/>。</param>
    /// <returns>返回 <typeparamref name="T"/>。</returns>
    T IssueEvaluate(IEnumerable<T> enumerable);


    /// <summary>
    /// 是否满足规约要求。
    /// </summary>
    /// <param name="value">给定的实例。</param>
    /// <returns>返回布尔值。</returns>
    bool IsSatisfiedBy(T value);


    /// <summary>
    /// 设置升序排列方法。
    /// </summary>
    /// <param name="orderBy">给定的升序排列方法。</param>
    /// <returns>返回 <see cref="ISpecification{T}"/>。</returns>
    ISpecification<T> SetOrderBy(Func<T, object> orderBy);

    /// <summary>
    /// 设置降序排列方法。
    /// </summary>
    /// <param name="orderByDescending">给定的降序排列方法。</param>
    /// <returns>返回 <see cref="ISpecification{T}"/>。</returns>
    ISpecification<T> SetOrderByDescending(Func<T, object> orderByDescending);

    /// <summary>
    /// 设置出具提供方法。
    /// </summary>
    /// <param name="provider">给定的出具提供方法。</param>
    /// <returns>返回 <see cref="ISpecification{T}"/>。</returns>
    ISpecification<T> SetProvider(Func<IEnumerable<T>, T> provider);
}
