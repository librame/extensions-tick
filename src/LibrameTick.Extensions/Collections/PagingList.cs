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
/// 定义一个支持内存或可查询分页的列表。
/// </summary>
/// <typeparam name="T">指定的类型。</typeparam>
public class PagingList<T> : IPagingList<T>
{
    /// <summary>
    /// 空分页列表。
    /// </summary>
    public readonly static IPagingList<T> Empty
        = new PagingList<T>(Enumerable.Empty<T>());

    private readonly IEnumerable<T>? _collection;
    private readonly PagingInfo _info;

    private IQueryable<T>? _queryable;
    private IEnumerable<T>? _filter;


    /// <summary>
    /// 使用 <see cref="IEnumerable{T}"/> 可枚举内存集合的副本构造一个 <see cref="PagingList{T}"/>。
    /// </summary>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="collection"/> is null.
    /// </exception>
    /// <param name="collection">给定的 <see cref="IEnumerable{T}"/>。</param>
    public PagingList(IEnumerable<T> collection)
    {
        // 默认使用副本集合
        var copy = new List<T>(collection);
        _info = new(copy.Count);
        _collection = copy;
    }

    /// <summary>
    /// 使用 <see cref="IEnumerable{T}"/> 可枚举内存集合构造一个 <see cref="PagingList{T}"/>。
    /// </summary>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="collection"/> is null.
    /// </exception>
    /// <param name="collection">给定的 <see cref="IEnumerable{T}"/>。</param>
    /// <param name="total">给定的总数据条数。</param>
    /// <param name="useCopy">使用副本集合。</param>
    public PagingList(IEnumerable<T> collection, long total, bool useCopy)
    {
        if (useCopy)
            _collection = new List<T>(collection);
        else
            _collection = collection;

        _info = new(total);
    }


    /// <summary>
    /// 使用 <see cref="IQueryable{T}"/> 可查询集合构造一个 <see cref="PagingList{T}"/>。
    /// </summary>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="queryable"/> is null.
    /// </exception>
    /// <param name="queryable">给定的 <see cref="IQueryable{T}"/>。</param>
    public PagingList(IQueryable<T> queryable)
    {
        _queryable = queryable;
        _info = new(queryable.NonEnumeratedCount());
    }

    /// <summary>
    /// 使用 <see cref="IQueryable{T}"/> 可查询集合构造一个 <see cref="PagingList{T}"/>。
    /// </summary>
    /// <param name="queryable">给定的 <see cref="IQueryable{T}"/>。</param>
    /// <param name="total">给定的总数据条数。</param>
    public PagingList(IQueryable<T> queryable, long total)
    {
        _queryable = queryable;
        _info = new(total);
    }


    /// <summary>
    /// 分页信息。
    /// </summary>
    public virtual PagingInfo Info
        => _info;

    /// <summary>
    /// 列表长度（未分页返回总条数）。
    /// </summary>
    public virtual int Length
        => _filter?.Count() ?? (int)_info.Total;


    /// <summary>
    /// 筛选列表（通常用于内存分页）。
    /// </summary>
    /// <param name="func">给定的筛选方法。</param>
    public virtual void Filtrate(Func<IEnumerable<T>, IEnumerable<T>> func)
    {
        if (_filter is not null)
        {
            _filter = func(_filter);
            return;
        }

        _filter = func(_collection!);
    }

    /// <summary>
    /// 筛选列表（通常用于动态分页）。
    /// </summary>
    /// <param name="func">给定的筛选方法。</param>
    public virtual void Filtrate(Func<IQueryable<T>, IQueryable<T>> func)
    {
        _queryable = func(_queryable!);
    }


    /// <summary>
    /// 通过页索引进行分页。
    /// </summary>
    /// <param name="index">给定的页索引。</param>
    /// <param name="size">给定的页大小。</param>
    /// <returns>返回 <see cref="PagingInfo"/>。</returns>
    public virtual PagingInfo PageByIndex(int index, int size)
    {
        _info.ComputeByIndex(index, size);

        PageCore();

        return _info;
    }

    /// <summary>
    /// 通过跳过的条数进行分页。
    /// </summary>
    /// <param name="skip">给定的跳过条数。</param>
    /// <param name="take">给定的取得条数。</param>
    /// <returns>返回 <see cref="PagingInfo"/>。</returns>
    public virtual PagingInfo PageBySkip(int skip, int take)
    {
        _info.ComputeBySkip(skip, take);

        PageCore();

        return _info;
    }

    private void PageCore()
    {
        // 内存分页
        if (_collection is not null)
        {
            // 如果需要分页
            if (_info.Total > _info.Size)
            {
                _filter = (_filter ?? _collection)
                    .Skip(_info.Skip)
                    .Take(_info.Size);
            }
            else
            {
                _filter = _filter ?? _collection;
            }
        }

        // 可查询分页
        if (_queryable is not null)
        {
            // 如果需要分页
            if (_info.Total > _info.Size)
            {
                _filter = _queryable
                    .Skip(_info.Skip)
                    .Take(_info.Size)
                    .ToList();
            }
            else
            {
                _filter = _queryable.ToList();
            }
        }
    }


    #region IEnumerable<T> 实现

    /// <summary>
    /// 返回一个循环访问集合的枚举器。
    /// </summary>
    /// <returns>返回枚举器。</returns>
    public virtual IEnumerator<T> GetEnumerator()
    {
        if (_filter is null)
            throw new InvalidOperationException($"You need to run the '${nameof(PageByIndex)}()' or '${nameof(PageBySkip)}()' method first.");

        return _filter.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();

    #endregion

}
