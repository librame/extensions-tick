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
/// <typeparam name="T">指定的类型。</typeparam>
public class CompositePagingList<T> : PagingList<T>
{
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
        PagingLists = collection;
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
        PagingLists = collection;
    }


    /// <summary>
    /// 分页列表集合。
    /// </summary>
    public IEnumerable<IPagingList<T>> PagingLists { get; init; }


    /// <summary>
    /// 筛选列表（通常用于动态分页）。
    /// </summary>
    /// <param name="func">给定的筛选方法。</param>
    public override void Filtrate(Func<IQueryable<T>, IQueryable<T>> func)
    {
        foreach (var page in PagingLists)
        {
            page.Filtrate(func);
        }
    }


    /// <summary>
    /// 返回一个循环访问集合的枚举器。
    /// </summary>
    /// <returns>返回枚举器。</returns>
    public override IEnumerator<T> GetEnumerator()
        => PagingLists.SelectMany(static s => s).GetEnumerator();


    private static IEnumerable<T> CombineList(IEnumerable<IPagingList<T>> collection)
        => collection.SelectMany(static s => s);

}
