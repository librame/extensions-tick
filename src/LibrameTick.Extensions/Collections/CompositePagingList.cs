#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Collections;

/// <summary>
/// 定义一个实现 <see cref="IPagingList{T}"/> 的复合分页列表。
/// </summary>
/// <typeparam name="T"></typeparam>
public class CompositePagingList<T> : PagingList<T>
{
    private readonly IEnumerable<IPagingList<T>> _collection;


    /// <summary>
    /// 使用 <see cref="IPagingList{T}"/> 可枚举内存集合的副本构造一个复合 <see cref="PagingList{T}"/>。
    /// </summary>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="collection"/> is null.
    /// </exception>
    /// <param name="collection">给定的 <see cref="IPagingList{T}"/>。</param>
    public CompositePagingList(IEnumerable<IPagingList<T>> collection)
        : base(CombineList(collection))
    {
        _collection = collection;
    }

    /// <summary>
    /// 使用 <see cref="IPagingList{T}"/> 可枚举内存集合构造一个 <see cref="PagingList{T}"/>。
    /// </summary>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="collection"/> is null.
    /// </exception>
    /// <param name="collection">给定的 <see cref="IPagingList{T}"/>。</param>
    /// <param name="total">给定的总数据条数。</param>
    /// <param name="useCopy">使用副本集合。</param>
    public CompositePagingList(IEnumerable<IPagingList<T>> collection, long total, bool useCopy)
        : base(CombineList(collection), total, useCopy)
    {
        _collection = collection;
    }


    /// <summary>
    /// 筛选列表（通常用于动态分页）。
    /// </summary>
    /// <param name="func">给定的筛选方法。</param>
    public override void Filtrate(Func<IQueryable<T>, IQueryable<T>> func)
    {
        foreach (var page in _collection)
        {
            page.Filtrate(func);
        }
    }


    private static IEnumerable<T> CombineList(IEnumerable<IPagingList<T>> collection)
        => collection.SelectMany(s => s.AsEnumerable());

}
