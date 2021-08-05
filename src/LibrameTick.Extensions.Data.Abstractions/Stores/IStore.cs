#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Data.Specifications;
using Librame.Extensions.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Threading;

namespace Librame.Extensions.Data.Stores
{
    /// <summary>
    /// 定义泛型商店接口。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    public interface IStore<T>
    {
        /// <summary>
        /// 可查询接口。
        /// </summary>
        IQueryable<T> Queryable { get; }


        #region Find

        /// <summary>
        /// 通过标识查找类型实例。
        /// </summary>
        /// <param name="id">给定的标识。</param>
        /// <returns>返回 <typeparamref name="T"/>。</returns>
        T? FindById(object id);


        /// <summary>
        /// 查找带有规约的类型实例集合。
        /// </summary>
        /// <param name="specification">给定的 <see cref="ISpecification{T}"/>（可选）。</param>
        /// <returns>返回 <see cref="IList{T}"/>。</returns>
        IList<T> FindWithSpecification(ISpecification<T>? specification = null);

        /// <summary>
        /// 异步查找带有规约的类型实例集合。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <param name="specification">给定的 <see cref="ISpecification{T}"/>（可选）。</param>
        /// <returns>返回一个包含 <see cref="IList{T}"/> 的异步操作。</returns>
        Task<IList<T>> FindWithSpecificationAsync(CancellationToken cancellationToken = default,
            ISpecification<T>? specification = null);


        /// <summary>
        /// 查找类型实例分页集合。
        /// </summary>
        /// <param name="pageAction">给定的分页动作。</param>
        /// <returns>返回 <see cref="IPagingList{T}"/>。</returns>
        IPagingList<T> FindPaging(Action<IPagingList<T>> pageAction);

        /// <summary>
        /// 异步查找类型实例分页集合。
        /// </summary>
        /// <param name="pageAction">给定的分页动作。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <see cref="IPagingList{T}"/> 的异步操作。</returns>
        Task<IPagingList<T>> FindPagingAsync(Action<IPagingList<T>> pageAction,
            CancellationToken cancellationToken = default);


        /// <summary>
        /// 查找带有规约的类型实例分页集合。
        /// </summary>
        /// <param name="pageAction">给定的分页动作。</param>
        /// <param name="specification">给定的 <see cref="ISpecification{T}"/>（可选）。</param>
        /// <returns>返回 <see cref="IPagingList{T}"/>。</returns>
        IPagingList<T> FindPagingWithSpecification(Action<IPagingList<T>> pageAction,
            ISpecification<T>? specification = null);

        /// <summary>
        /// 异步查找带有规约的类型实例分页集合。
        /// </summary>
        /// <param name="pageAction">给定的分页动作。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <param name="specification">给定的 <see cref="ISpecification{T}"/>（可选）。</param>
        /// <returns>返回一个包含 <see cref="IPagingList{T}"/> 的异步操作。</returns>
        Task<IPagingList<T>> FindPagingWithSpecificationAsync(Action<IPagingList<T>> pageAction,
            CancellationToken cancellationToken = default, ISpecification<T>? specification = null);

        #endregion


        #region Add

        /// <summary>
        /// 如果不存在则添加类型实例。
        /// </summary>
        /// <param name="item">给定要添加的类型实例。</param>
        /// <param name="predicate">给定用于判定是否存在的工厂方法。</param>
        void AddIfNotExists(T item, Expression<Func<T, bool>> predicate);

        /// <summary>
        /// 添加类型实例集合。
        /// </summary>
        /// <param name="items">给定的类型实例数组集合。</param>
        void Add(params T[] items);

        /// <summary>
        /// 添加类型实例集合。
        /// </summary>
        /// <param name="items">给定的 <see cref="IEnumerable{T}"/>。</param>
        void Add(IEnumerable<T> items);

        #endregion


        #region Remove

        /// <summary>
        /// 移除类型实例集合。
        /// </summary>
        /// <param name="items">给定的类型实例数组集合。</param>
        void Remove(params T[] items);

        /// <summary>
        /// 移除类型实例集合。
        /// </summary>
        /// <param name="items">给定的 <see cref="IEnumerable{T}"/>。</param>
        void Remove(IEnumerable<T> items);

        #endregion


        #region Update

        /// <summary>
        /// 更新类型实例集合。
        /// </summary>
        /// <param name="items">给定的类型实例数组集合。</param>
        void Update(params T[] items);

        /// <summary>
        /// 更新类型实例集合。
        /// </summary>
        /// <param name="items">给定的 <see cref="IEnumerable{T}"/>。</param>
        void Update(IEnumerable<T> items);

        #endregion

    }
}
