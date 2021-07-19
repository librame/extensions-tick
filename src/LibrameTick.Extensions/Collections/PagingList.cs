#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Librame.Extensions.Collections
{
    /// <summary>
    /// 定义一个支持内存或可查询分页的列表。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    public class PagingList<T> : IPagingList<T>
    {
        /// <summary>
        /// 空分页列表。
        /// </summary>
        public readonly static PagingList<T> Empty
            = new PagingList<T>(Enumerable.Empty<T>());

        private readonly List<T>? _collection;
        private readonly PagingInfo _info;

        private IQueryable<T>? _queryable;
        private IEnumerable<T>? _current;


        /// <summary>
        /// 使用 <see cref="IEnumerable{T}"/> 可枚举内存集合构造一个 <see cref="PagingList{T}"/>。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="collection"/> is null.
        /// </exception>
        /// <param name="collection">给定的 <see cref="IEnumerable{T}"/>。</param>
        public PagingList(IEnumerable<T> collection)
        {
            _collection = new List<T>(collection); // 使用副本集合
            _info = new PagingInfo(_collection.Count);
        }

        /// <summary>
        /// 使用 <see cref="IQueryable{T}"/> 可查询集合构造一个 <see cref="PagingList{T}"/>。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="queryable"/> is null.
        /// </exception>
        /// <param name="queryable">给定的 <see cref="IOrderedQueryable{T}"/>。</param>
        public PagingList(IOrderedQueryable<T> queryable)
        {
            _queryable = queryable.NotNull(nameof(queryable));
            _info = new PagingInfo(queryable.Count());
        }


        /// <summary>
        /// 分页信息（需要先分页后才有效）。
        /// </summary>
        public PagingInfo Info
            => _info;


        /// <summary>
        /// 筛选列表（通常用于内存分页）。
        /// </summary>
        /// <param name="func">给定的筛选方法。</param>
        public void Filtrate(Func<IEnumerable<T>, IEnumerable<T>> func)
        {
            if (_current is not null)
            {
                _current = func.Invoke(_current);
                return;
            }

            _current = func.Invoke(_collection.NotNull(nameof(_collection)));
        }

        /// <summary>
        /// 筛选列表（通常用于动态分页）。
        /// </summary>
        /// <param name="func">给定的筛选方法。</param>
        public void Filtrate(Func<IQueryable<T>, IQueryable<T>> func)
        {
            _queryable = func.Invoke(_queryable.NotNull(nameof(_queryable)));
        }


        /// <summary>
        /// 通过页索引进行分页。
        /// </summary>
        /// <param name="index">给定的页索引。</param>
        /// <param name="size">给定的页大小。</param>
        /// <returns>返回 <see cref="PagingInfo"/>。</returns>
        public PagingInfo PageByIndex(int index, int size)
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
        public PagingInfo PageBySkip(int skip, int take)
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
                    _current = (_current ?? _collection)
                        .Skip(_info.Skip)
                        .Take(_info.Size);
                }
                else
                {
                    _current = _current ?? _collection;
                }
            }

            // 可查询分页
            if (_queryable is not null)
            {
                // 如果需要分页
                if (_info.Total > _info.Size)
                {
                    _current = _queryable
                        .Skip(_info.Skip)
                        .Take(_info.Size)
                        .ToList();
                }
                else
                {
                    _current = _queryable.ToList();
                }
            }
        }


        #region IEnumerable<T> 实现

        /// <summary>
        /// 返回一个循环访问集合的枚举器。
        /// </summary>
        /// <returns>返回枚举器。</returns>
        public IEnumerator<T> GetEnumerator()
        {
            if (_current is null)
                throw new InvalidOperationException("You need to run the “PageByIndex()” or “PageBySkip()” method first.");

            return _current.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        #endregion

    }
}
