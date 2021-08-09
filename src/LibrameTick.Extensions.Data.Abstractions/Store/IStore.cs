#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Data.Access;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Threading;

namespace Librame.Extensions.Data.Store
{
    /// <summary>
    /// 定义泛型商店接口。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    public interface IStore<T>
    {
        /// <summary>
        /// <see cref="IIdentificationGenerator{TId}"/> 工厂。
        /// </summary>
        IIdentificationGeneratorFactory IdGeneratorFactory { get; init; }


        /// <summary>
        /// 获取访问器。
        /// </summary>
        /// <param name="group">给定的所属群组（可选；默认使用初始访问器）。</param>
        /// <param name="fromWriteAccessor">强制从写入访问器查询（可选；默认不强制）。</param>
        /// <returns>返回 <see cref="IAccessor"/>。</returns>
        IAccessor GetAccessor(int? group = null, bool fromWriteAccessor = false);

        /// <summary>
        /// 获取可查询接口（支持强制从写入访问器查询）。
        /// </summary>
        /// <param name="group">给定的所属群组（可选；默认使用初始访问器）。</param>
        /// <param name="fromWriteAccessor">强制从写入访问器查询（可选；默认不强制）。</param>
        /// <returns>返回 <see cref="IQueryable{T}"/>。</returns>
        IQueryable<T> GetQueryable(int? group = null, bool fromWriteAccessor = false);


        #region Find

        /// <summary>
        /// 通过标识查找类型实例（支持强制从写入访问器查询）。
        /// </summary>
        /// <param name="id">给定的标识。</param>
        /// <param name="group">给定的所属群组（可选；默认使用初始访问器）。</param>
        /// <param name="fromWriteAccessor">强制从写入访问器查询（可选；默认不强制）。</param>
        /// <returns>返回 <typeparamref name="T"/>。</returns>
        T? FindById(object id, int? group = null, bool fromWriteAccessor = false);

        #endregion


        #region Add

        /// <summary>
        /// 如果不存在则添加类型实例（仅支持写入访问器）。
        /// </summary>
        /// <param name="item">给定要添加的类型实例。</param>
        /// <param name="predicate">给定用于判定是否存在的工厂方法。</param>
        /// <param name="group">给定的所属群组（可选；默认使用初始访问器）。</param>
        void AddIfNotExists(T item, Expression<Func<T, bool>> predicate, int? group = null);

        /// <summary>
        /// 添加类型实例集合（仅支持写入访问器）。
        /// </summary>
        /// <param name="group">给定的所属群组（可选；默认使用初始访问器）。</param>
        /// <param name="items">给定的类型实例数组集合。</param>
        void Add(int? group = null, params T[] items);

        /// <summary>
        /// 添加类型实例集合（仅支持写入访问器）。
        /// </summary>
        /// <param name="items">给定的 <see cref="IEnumerable{T}"/>。</param>
        /// <param name="group">给定的所属群组（可选；默认使用初始访问器）。</param>
        void Add(IEnumerable<T> items, int? group = null);

        #endregion


        #region Remove

        /// <summary>
        /// 移除类型实例集合（仅支持写入访问器）。
        /// </summary>
        /// <param name="group">给定的所属群组（可选；默认使用初始访问器）。</param>
        /// <param name="items">给定的类型实例数组集合。</param>
        void Remove(int? group = null, params T[] items);

        /// <summary>
        /// 移除类型实例集合（仅支持写入访问器）。
        /// </summary>
        /// <param name="items">给定的 <see cref="IEnumerable{T}"/>。</param>
        /// <param name="group">给定的所属群组（可选；默认使用初始访问器）。</param>
        void Remove(IEnumerable<T> items, int? group = null);

        #endregion


        #region Update

        /// <summary>
        /// 更新类型实例集合（仅支持写入访问器）。
        /// </summary>
        /// <param name="group">给定的所属群组（可选；默认使用初始访问器）。</param>
        /// <param name="items">给定的类型实例数组集合。</param>
        void Update(int? group = null, params T[] items);

        /// <summary>
        /// 更新类型实例集合（仅支持写入访问器）。
        /// </summary>
        /// <param name="items">给定的 <see cref="IEnumerable{T}"/>。</param>
        /// <param name="group">给定的所属群组（可选；默认使用初始访问器）。</param>
        void Update(IEnumerable<T> items, int? group = null);

        #endregion


        #region SaveChanges

        /// <summary>
        /// 保存更改（仅支持写入访问器）。
        /// </summary>
        /// <param name="group">给定的所属群组。</param>
        /// <returns>返回受影响的行数。</returns>
        int SaveChanges(int? group = null);

        /// <summary>
        /// 异步保存更改（仅支持写入访问器）。
        /// </summary>
        /// <param name="group">给定的所属群组。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含受影响行数的异步操作。</returns>
        Task<int> SaveChangesAsync(int? group = null, CancellationToken cancellationToken = default);

        #endregion

    }
}
