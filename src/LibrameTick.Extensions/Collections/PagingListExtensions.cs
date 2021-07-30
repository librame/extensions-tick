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
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Collections
{
    /// <summary>
    /// <see cref="PagingList{T}"/> 静态扩展。
    /// </summary>
    public static class PagingListExtensions
    {

        /// <summary>
        /// 可枚举内存分页。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="collection"/> is null.
        /// </exception>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="collection">给定的 <see cref="IEnumerable{T}"/>。</param>
        /// <param name="action">给定的分页动作。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <see cref="PagingList{T}"/> 的异步操作。</returns>
        public static Task<PagingList<T>> PageAsync<T>(this IEnumerable<T> collection, Action<PagingList<T>> action,
            CancellationToken cancellationToken = default)
            => cancellationToken.RunTask(() => collection.Page(action));

        /// <summary>
        /// 可查询分页。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="queryable"/> is null.
        /// </exception>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="queryable">给定的 <see cref="IOrderedQueryable{T}"/>。</param>
        /// <param name="action">给定的分页动作。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <see cref="PagingList{T}"/> 的异步操作。</returns>
        public static Task<PagingList<T>> PageAsync<T>(this IOrderedQueryable<T> queryable, Action<PagingList<T>> action,
            CancellationToken cancellationToken = default)
            => cancellationToken.RunTask(() => queryable.Page(action));


        /// <summary>
        /// 可枚举内存分页。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="collection"/> is null.
        /// </exception>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="collection">给定的 <see cref="IEnumerable{T}"/>。</param>
        /// <param name="action">给定的分页动作。</param>
        /// <returns>返回 <see cref="PagingList{T}"/>。</returns>
        public static PagingList<T> Page<T>(this IEnumerable<T> collection, Action<PagingList<T>> action)
        {
            var list = new PagingList<T>(collection);
            action.Invoke(list);

            return list;
        }

        /// <summary>
        /// 可查询分页。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="queryable"/> is null.
        /// </exception>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="queryable">给定的 <see cref="IOrderedQueryable{T}"/>。</param>
        /// <param name="action">给定的分页动作。</param>
        /// <returns>返回 <see cref="PagingList{T}"/>。</returns>
        public static PagingList<T> Page<T>(this IOrderedQueryable<T> queryable, Action<PagingList<T>> action)
        {
            var list = new PagingList<T>(queryable);
            action.Invoke(list);

            return list;
        }

    }
}
