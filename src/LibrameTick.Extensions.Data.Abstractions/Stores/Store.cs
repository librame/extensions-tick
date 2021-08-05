#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Collections;
using Librame.Extensions.Data.Accessors;
using Librame.Extensions.Data.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Threading;

namespace Librame.Extensions.Data.Stores
{
    /// <summary>
    /// 实现 <see cref="IStore{T}"/> 的泛型商店。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    public class Store<T> : IStore<T>
        where T : class
    {
        /// <summary>
        /// 构造一个 <see cref="Store{T}"/>。
        /// </summary>
        /// <param name="accessorManager">给定的 <see cref="IAccessorManager"/>。</param>
        public Store(IAccessorManager accessorManager)
        {
            AccessorManager = accessorManager;
        }


        /// <summary>
        /// <see cref="IAccessor"/> 管理器。
        /// </summary>
        protected IAccessorManager AccessorManager { get; init; }

        /// <summary>
        /// 读取 <see cref="IAccessor"/>。
        /// </summary>
        protected virtual IAccessor ReadAccessor
            => AccessorManager.GetReadAccessor();

        /// <summary>
        /// 写入 <see cref="IAccessor"/>。
        /// </summary>
        protected virtual IAccessor WriteAccessor
            => AccessorManager.GetWriteAccessor();


        /// <summary>
        /// 读取可查询接口。
        /// </summary>
        public virtual IQueryable<T> Queryable
            => ReadAccessor.GetQueryable<T>();


        #region Find

        /// <summary>
        /// 通过标识查找类型实例。
        /// </summary>
        /// <param name="id">给定的标识。</param>
        /// <returns>返回 <typeparamref name="T"/>。</returns>
        public virtual T? FindById(object id)
            => ReadAccessor.Find<T>(id);


        /// <summary>
        /// 查找带有规约的类型实例集合。
        /// </summary>
        /// <param name="specification">给定的 <see cref="ISpecification{T}"/>（可选）。</param>
        /// <returns>返回 <see cref="IList{T}"/>。</returns>
        public virtual IList<T> FindWithSpecification(ISpecification<T>? specification = null)
            => ReadAccessor.FindWithSpecification(specification);

        /// <summary>
        /// 异步查找带有规约的类型实例集合。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <param name="specification">给定的 <see cref="ISpecification{T}"/>（可选）。</param>
        /// <returns>返回一个包含 <see cref="IList{T}"/> 的异步操作。</returns>
        public virtual Task<IList<T>> FindWithSpecificationAsync(CancellationToken cancellationToken = default,
            ISpecification<T>? specification = null)
            => ReadAccessor.FindWithSpecificationAsync(cancellationToken, specification);


        /// <summary>
        /// 查找类型实例分页集合。
        /// </summary>
        /// <param name="pageAction">给定的分页动作。</param>
        /// <returns>返回 <see cref="IPagingList{T}"/>。</returns>
        public virtual IPagingList<T> FindPaging(Action<IPagingList<T>> pageAction)
            => ReadAccessor.FindPaging(pageAction);

        /// <summary>
        /// 异步查找类型实例分页集合。
        /// </summary>
        /// <param name="pageAction">给定的分页动作。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <see cref="IPagingList{T}"/> 的异步操作。</returns>
        public virtual Task<IPagingList<T>> FindPagingAsync(Action<IPagingList<T>> pageAction,
            CancellationToken cancellationToken = default)
            => ReadAccessor.FindPagingAsync(pageAction, cancellationToken);


        /// <summary>
        /// 查找带有规约的类型实例分页集合。
        /// </summary>
        /// <param name="pageAction">给定的分页动作。</param>
        /// <param name="specification">给定的 <see cref="ISpecification{T}"/>（可选）。</param>
        /// <returns>返回 <see cref="IPagingList{T}"/>。</returns>
        public virtual IPagingList<T> FindPagingWithSpecification(Action<IPagingList<T>> pageAction,
            ISpecification<T>? specification = null)
            => ReadAccessor.FindPagingWithSpecification(pageAction, specification);

        /// <summary>
        /// 异步查找带有规约的类型实例分页集合。
        /// </summary>
        /// <param name="pageAction">给定的分页动作。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <param name="specification">给定的 <see cref="ISpecification{T}"/>（可选）。</param>
        /// <returns>返回一个包含 <see cref="IPagingList{T}"/> 的异步操作。</returns>
        public virtual Task<IPagingList<T>> FindPagingWithSpecificationAsync(Action<IPagingList<T>> pageAction,
            CancellationToken cancellationToken = default, ISpecification<T>? specification = null)
            => ReadAccessor.FindPagingWithSpecificationAsync(pageAction, cancellationToken, specification);

        #endregion


        #region Add

        /// <summary>
        /// 如果不存在则添加类型实例。
        /// </summary>
        /// <param name="item">给定要添加的类型实例。</param>
        /// <param name="predicate">给定用于判定是否存在的工厂方法。</param>
        public virtual void AddIfNotExists(T item, Expression<Func<T, bool>> predicate)
            => WriteAccessor.AddIfNotExists(item, predicate);

        /// <summary>
        /// 添加类型实例集合。
        /// </summary>
        /// <param name="entities">给定的类型实例数组集合。</param>
        public virtual void Add(params T[] entities)
            => WriteAccessor.AddRange(entities);

        /// <summary>
        /// 添加类型实例集合。
        /// </summary>
        /// <param name="entities">给定的 <see cref="IEnumerable{T}"/>。</param>
        public virtual void Add(IEnumerable<T> entities)
            => WriteAccessor.AddRange(entities);

        #endregion


        #region Remove

        /// <summary>
        /// 移除类型实例集合。
        /// </summary>
        /// <param name="entities">给定的类型实例数组集合。</param>
        public virtual void Remove(params T[] entities)
            => WriteAccessor.RemoveRange(entities);

        /// <summary>
        /// 移除类型实例集合。
        /// </summary>
        /// <param name="entities">给定的 <see cref="IEnumerable{T}"/>。</param>
        public virtual void Remove(IEnumerable<T> entities)
            => WriteAccessor.RemoveRange(entities);

        #endregion


        #region Update

        /// <summary>
        /// 更新类型实例集合。
        /// </summary>
        /// <param name="entities">给定的类型实例数组集合。</param>
        public virtual void Update(params T[] entities)
            => WriteAccessor.UpdateRange(entities);

        /// <summary>
        /// 更新类型实例集合。
        /// </summary>
        /// <param name="entities">给定的 <see cref="IEnumerable{T}"/>。</param>
        public virtual void Update(IEnumerable<T> entities)
            => WriteAccessor.UpdateRange(entities);

        #endregion

    }
}
