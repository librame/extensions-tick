#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Data.Accessing;
using System.Linq.Expressions;

namespace Librame.Extensions.Data.Storing
{
    /// <summary>
    /// 定义实现 <see cref="IStore{T}"/> 的泛型基础商店。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    public class BaseStore<T> : IStore<T>
        where T : class
    {
        /// <summary>
        /// 构造一个 <see cref="BaseStore{T}"/>。
        /// </summary>
        /// <param name="accessors">给定的 <see cref="IAccessorManager"/>。</param>
        /// <param name="idGeneratorFactory">给定的 <see cref="IIdentificationGeneratorFactory"/>。</param>
        public BaseStore(IAccessorManager accessors,
            IIdentificationGeneratorFactory idGeneratorFactory)
        {
            Accessors = accessors;
            IdGeneratorFactory = idGeneratorFactory;
        }


        /// <summary>
        /// <see cref="IAccessor"/> 管理器。
        /// </summary>
        protected IAccessorManager Accessors { get; init; }

        /// <summary>
        /// <see cref="IIdentificationGenerator{TId}"/> 工厂。
        /// </summary>
        public IIdentificationGeneratorFactory IdGeneratorFactory { get; init; }


        /// <summary>
        /// 获取访问器。
        /// </summary>
        /// <param name="group">给定的所属群组（可选；默认使用初始访问器）。</param>
        /// <param name="fromWriteAccessor">强制从写入访问器查询（可选；默认不强制）。</param>
        /// <returns>返回 <see cref="IAccessor"/>。</returns>
        public virtual IAccessor GetAccessor(int? group = null, bool fromWriteAccessor = false)
            => fromWriteAccessor ? Accessors.GetWriteAccessor(group) : Accessors.GetReadAccessor(group);

        /// <summary>
        /// 获取可查询接口（支持强制从写入访问器查询）。
        /// </summary>
        /// <param name="group">给定的所属群组（可选；默认使用初始访问器）。</param>
        /// <param name="fromWriteAccessor">强制从写入访问器查询（可选；默认不强制）。</param>
        /// <returns>返回 <see cref="IQueryable{T}"/>。</returns>
        public virtual IQueryable<T> GetQueryable(int? group = null, bool fromWriteAccessor = false)
            => GetAccessor(group, fromWriteAccessor).GetQueryable<T>();


        #region Find

        /// <summary>
        /// 通过标识查找类型实例（支持强制从写入访问器查询）。
        /// </summary>
        /// <param name="id">给定的标识。</param>
        /// <param name="group">给定的所属群组（可选；默认使用初始访问器）。</param>
        /// <param name="fromWriteAccessor">强制从写入访问器查询（可选；默认不强制）。</param>
        /// <returns>返回 <typeparamref name="T"/>。</returns>
        public virtual T? FindById(object id, int? group = null, bool fromWriteAccessor = false)
            => GetAccessor(group, fromWriteAccessor).Find<T>(id);

        #endregion


        #region Add

        /// <summary>
        /// 如果不存在则添加类型实例（仅支持写入访问器）。
        /// </summary>
        /// <param name="item">给定要添加的类型实例。</param>
        /// <param name="predicate">给定用于判定是否存在的工厂方法。</param>
        /// <param name="group">给定的所属群组（可选；默认使用初始访问器）。</param>
        public virtual void AddIfNotExists(T item, Expression<Func<T, bool>> predicate, int? group = null)
            => Accessors.GetWriteAccessor(group).AddIfNotExists(item, predicate);

        /// <summary>
        /// 添加类型实例集合（仅支持写入访问器）。
        /// </summary>
        /// <param name="group">给定的所属群组（可选；默认使用初始访问器）。</param>
        /// <param name="entities">给定的类型实例数组集合。</param>
        public virtual void Add(int? group = null, params T[] entities)
            => Accessors.GetWriteAccessor(group).AddRange(entities);

        /// <summary>
        /// 添加类型实例集合（仅支持写入访问器）。
        /// </summary>
        /// <param name="entities">给定的 <see cref="IEnumerable{T}"/>。</param>
        /// <param name="group">给定的所属群组（可选；默认使用初始访问器）。</param>
        public virtual void Add(IEnumerable<T> entities, int? group = null)
            => Accessors.GetWriteAccessor(group).AddRange(entities);

        #endregion


        #region Remove

        /// <summary>
        /// 移除类型实例集合（仅支持写入访问器）。
        /// </summary>
        /// <param name="group">给定的所属群组（可选；默认使用初始访问器）。</param>
        /// <param name="entities">给定的类型实例数组集合。</param>
        public virtual void Remove(int? group = null, params T[] entities)
            => Accessors.GetWriteAccessor(group).RemoveRange(entities);

        /// <summary>
        /// 移除类型实例集合（仅支持写入访问器）。
        /// </summary>
        /// <param name="entities">给定的 <see cref="IEnumerable{T}"/>。</param>
        /// <param name="group">给定的所属群组（可选；默认使用初始访问器）。</param>
        public virtual void Remove(IEnumerable<T> entities, int? group = null)
            => Accessors.GetWriteAccessor(group).RemoveRange(entities);

        #endregion


        #region Update

        /// <summary>
        /// 更新类型实例集合（仅支持写入访问器）。
        /// </summary>
        /// <param name="group">给定的所属群组（可选；默认使用初始访问器）。</param>
        /// <param name="entities">给定的类型实例数组集合。</param>
        public virtual void Update(int? group = null, params T[] entities)
            => Accessors.GetWriteAccessor(group).UpdateRange(entities);

        /// <summary>
        /// 更新类型实例集合（仅支持写入访问器）。
        /// </summary>
        /// <param name="entities">给定的 <see cref="IEnumerable{T}"/>。</param>
        /// <param name="group">给定的所属群组（可选；默认使用初始访问器）。</param>
        public virtual void Update(IEnumerable<T> entities, int? group = null)
            => Accessors.GetWriteAccessor(group).UpdateRange(entities);

        #endregion


        #region SaveChanges

        /// <summary>
        /// 保存更改（仅支持写入访问器）。
        /// </summary>
        /// <param name="group">给定的所属群组。</param>
        /// <returns>返回受影响的行数。</returns>
        public virtual int SaveChanges(int? group = null)
            => Accessors.GetWriteAccessor(group).SaveChanges();

        /// <summary>
        /// 异步保存更改（仅支持写入访问器）。
        /// </summary>
        /// <param name="group">给定的所属群组。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含受影响行数的异步操作。</returns>
        public virtual Task<int> SaveChangesAsync(int? group = null, CancellationToken cancellationToken = default)
            => Accessors.GetWriteAccessor(group).SaveChangesAsync(cancellationToken);

        #endregion

    }
}
