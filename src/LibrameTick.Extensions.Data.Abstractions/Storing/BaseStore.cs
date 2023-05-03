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
using Librame.Extensions.Data.Accessing;
using Librame.Extensions.Dispatchers;
using Librame.Extensions.IdGenerators;
using Librame.Extensions.Specifications;

namespace Librame.Extensions.Data.Storing;

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
    /// <param name="accessorManager">给定的 <see cref="IAccessorManager"/>。</param>
    /// <param name="idGeneratorFactory">给定的 <see cref="IIdGeneratorFactory"/>。</param>
    public BaseStore(IAccessorManager accessorManager, IIdGeneratorFactory idGeneratorFactory)
    {
        AccessorManager = accessorManager;
        IdGeneratorFactory = idGeneratorFactory;

        CurrentReadAccessor = accessorManager.GetReadAccessors();
        CurrentWriteAccessor = accessorManager.GetWriteAccessors();
    }


    /// <summary>
    /// <see cref="IAccessor"/> 管理器。
    /// </summary>
    public IAccessorManager AccessorManager { get; init; }

    /// <summary>
    /// <see cref="IIdGenerator{TId}"/> 工厂。
    /// </summary>
    public IIdGeneratorFactory IdGeneratorFactory { get; init; }

    /// <summary>
    /// 当前读取可调度存取器。
    /// </summary>
    public IDispatchableAccessors CurrentReadAccessor { get; set; }

    /// <summary>
    /// 当前写入可调度存取器。
    /// </summary>
    public IDispatchableAccessors CurrentWriteAccessor { get; set; }


    /// <summary>
    /// 使用读取与写入访问器。
    /// </summary>
    /// <param name="accessorName">给定的 <see cref="IAccessor"/> 名称。</param>
    /// <returns>返回 <see cref="IStore{T}"/>。</returns>
    public virtual IStore<T> UseAccessor(string accessorName)
    {
        CurrentWriteAccessor = CurrentReadAccessor =
            AccessorManager.GetAccessors(new NamedAccessorSpecification(accessorName));
        return this;
    }

    /// <summary>
    /// 使用读取访问器。
    /// </summary>
    /// <param name="specification">给定的 <see cref="ISpecification{IAccessor}"/>（可选；默认使用 <see cref="ReadAccessAccessorSpecification"/> 规约）。</param>
    /// <returns>返回 <see cref="IStore{T}"/>。</returns>
    public virtual IStore<T> UseReadAccessor(ISpecification<IAccessor>? specification = null)
    {
        CurrentReadAccessor = AccessorManager.GetReadAccessors(specification);
        return this;
    }

    /// <summary>
    /// 使用写入访问器。
    /// </summary>
    /// <param name="specification">给定的 <see cref="ISpecification{IAccessor}"/>（可选；默认使用 <see cref="WriteAccessAccessorSpecification"/> 规约）。</param>
    /// <returns>返回 <see cref="IStore{T}"/>。</returns>
    public virtual IStore<T> UseWriteAccessor(ISpecification<IAccessor>? specification = null)
    {
        CurrentWriteAccessor = AccessorManager.GetWriteAccessors(specification);
        return this;
    }


    /// <summary>
    /// 获取读取的存取器调度器。
    /// </summary>
    /// <param name="specification">给定的 <see cref="ISpecification{IAccessor}"/>（可选）。</param>
    /// <returns>返回 <see cref="IDispatcher{IAccessor}"/>。</returns>
    protected virtual IDispatcher<IAccessor> GetReadingDispatcher(ISpecification<IAccessor>? specification)
        => (AccessorManager.GetReadAccessors(specification) ?? CurrentReadAccessor).ReadingDispatcher;

    /// <summary>
    /// 获取写入的存取器调度器。
    /// </summary>
    /// <param name="specification">给定的 <see cref="ISpecification{IAccessor}"/>（可选）。</param>
    /// <returns>返回 <see cref="IDispatcher{IAccessor}"/>。</returns>
    protected virtual IDispatcher<IAccessor> GetWritingDispatcher(ISpecification<IAccessor>? specification)
        => (AccessorManager.GetWriteAccessors(specification) ?? CurrentWriteAccessor).WritingDispatcher;


    #region Query

    /// <summary>
    /// 获取可查询接口。
    /// </summary>
    /// <param name="specification">给定的 <see cref="ISpecification{IAccessor}"/>（可选；默认使用 <see cref="ReadAccessAccessorSpecification"/> 规约）。</param>
    /// <returns>返回 <see cref="IQueryable{T}"/>。</returns>
    public virtual IQueryable<T> Query(ISpecification<IAccessor>? specification = null)
    {
        IQueryable<T>? query = null;

        foreach (var accessor in GetReadingDispatcher(specification))
        {
            query = accessor.Query<T>();
        }

        return query!;
    }

    /// <summary>
    /// 获取可查询接口。
    /// </summary>
    /// <param name="name">要使用的共享类型实体类型的名称。</param>
    /// <param name="specification">给定的 <see cref="ISpecification{IAccessor}"/>（可选；默认使用 <see cref="ReadAccessAccessorSpecification"/> 规约）。</param>
    /// <returns>返回 <see cref="IQueryable{T}"/>。</returns>
    public virtual IQueryable<T> Query(string name, ISpecification<IAccessor>? specification = null)
    {
        IQueryable<T>? query = null;

        foreach (var accessor in GetReadingDispatcher(specification))
        {
            query = accessor.Query<T>(name);
        }

        return query!;
    }


    /// <summary>
    /// 通过 SQL 语句获取可查询接口。
    /// </summary>
    /// <param name="sql">给定的 SQL 语句（可使用“${Schema}、${Table}/${TableName}”模板关键字分别代替架构、表名等参数值）。</param>
    /// <param name="parameters">给定的参数数组。</param>
    /// <returns>返回 <see cref="IQueryable{T}"/>。</returns>
    public virtual IQueryable<T> QueryBySql(string sql, params object[] parameters)
    {
        IQueryable<T>? query = null;

        foreach (var accessor in CurrentReadAccessor.ReadingDispatcher)
        {
            query = accessor.QueryBySql<T>(sql, parameters);
        }

        return query!;
    }

    /// <summary>
    /// 通过 SQL 语句获取可查询接口。
    /// </summary>
    /// <param name="sql">给定的 SQL 语句（可使用“${Schema}、${Table}/${TableName}”模板关键字分别代替架构、表名等参数值）。</param>
    /// <param name="parameters">给定的参数数组。</param>
    /// <param name="specification">给定的 <see cref="ISpecification{IAccessor}"/>。</param>
    /// <returns>返回 <see cref="IQueryable{T}"/>。</returns>
    public virtual IQueryable<T> QueryBySql(string sql, object[] parameters,
        ISpecification<IAccessor> specification)
    {
        IQueryable<T>? query = null;

        foreach (var accessor in GetReadingDispatcher(specification))
        {
            query = accessor.QueryBySql<T>(sql, parameters);
        }

        return query!;
    }


    /// <summary>
    /// 通过 SQL 语句获取可查询接口。
    /// </summary>
    /// <param name="name">要使用的共享类型实体类型的名称。</param>
    /// <param name="sql">给定的 SQL 语句（可使用“${Schema}、${Table}/${TableName}”模板关键字分别代替架构、表名等参数值）。</param>
    /// <param name="parameters">给定的参数数组。</param>
    /// <returns>返回 <see cref="IQueryable{T}"/>。</returns>
    public virtual IQueryable<T> QueryBySql(string name, string sql, params object[] parameters)
    {
        IQueryable<T>? query = null;

        foreach (var accessor in CurrentReadAccessor.ReadingDispatcher)
        {
            query = accessor.QueryBySql<T>(name, sql, parameters);
        }

        return query!;
    }

    /// <summary>
    /// 通过 SQL 语句获取可查询接口。
    /// </summary>
    /// <param name="name">要使用的共享类型实体类型的名称。</param>
    /// <param name="sql">给定的 SQL 语句（可使用“${Schema}、${Table}/${TableName}”模板关键字分别代替架构、表名等参数值）。</param>
    /// <param name="parameters">给定的参数数组。</param>
    /// <param name="specification">给定的 <see cref="ISpecification{IAccessor}"/>。</param>
    /// <returns>返回 <see cref="IQueryable{T}"/>。</returns>
    public virtual IQueryable<T> QueryBySql(string name, string sql, object[] parameters,
        ISpecification<IAccessor> specification)
    {
        IQueryable<T>? query = null;

        foreach (var accessor in GetReadingDispatcher(specification))
        {
            query = accessor.QueryBySql<T>(name, sql, parameters);
        }

        return query!;
    }

    #endregion


    #region Exists

    /// <summary>
    /// 在本地缓存或数据库中是否存在指定断定方法的实体。
    /// </summary>
    /// <param name="predicate">给定的断定方法表达式。</param>
    /// <param name="checkLocal">是否检查本地缓存（可选；默认启用检查）。</param>
    /// <param name="specification">给定的 <see cref="ISpecification{IAccessor}"/>（可选；默认使用 <see cref="ReadAccessAccessorSpecification"/> 规约）。</param>
    /// <returns>返回布尔值。</returns>
    public virtual bool Exists(Expression<Func<T, bool>> predicate,
        bool checkLocal = true, ISpecification<IAccessor>? specification = null)
    {
        foreach (var accessor in GetReadingDispatcher(specification))
        {
            if (accessor.Exists(predicate, checkLocal))
                return true;
        }

        return false;
    }

    /// <summary>
    /// 异步在本地缓存或数据库中是否存在指定断定方法的实体。
    /// </summary>
    /// <param name="predicate">给定的断定方法表达式。</param>
    /// <param name="checkLocal">是否检查本地缓存（可选；默认启用检查）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <param name="specification">给定的 <see cref="ISpecification{IAccessor}"/>（可选；默认使用 <see cref="ReadAccessAccessorSpecification"/> 规约）。</param>
    /// <returns>返回一个包含布尔值的异步操作。</returns>
    public virtual async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate,
        bool checkLocal = true, CancellationToken cancellationToken = default,
        ISpecification<IAccessor>? specification = null)
    {
        foreach (var accessor in GetReadingDispatcher(specification))
        {
            if (await accessor.ExistsAsync(predicate, checkLocal, cancellationToken))
                return true;
        }

        return false;
    }

    #endregion


    #region Find

    /// <summary>
    /// 通过标识查找类型实例。
    /// </summary>
    /// <param name="id">给定的标识。</param>
    /// <param name="specification">给定的 <see cref="ISpecification{IAccessor}"/>（可选；默认使用 <see cref="ReadAccessAccessorSpecification"/> 规约）。</param>
    /// <returns>返回 <typeparamref name="T"/>。</returns>
    public virtual T? FindById(object id, ISpecification<IAccessor>? specification = null)
    {
        foreach (var accessor in GetReadingDispatcher(specification))
        {
            var one = accessor.CurrentContext.Find<T>(id);
            if (one is not null)
                return one;
        }

        return null;
    }


    /// <summary>
    /// 通过指定断定条件查找类型实例集合。
    /// </summary>
    /// <param name="predicate">给定的断定条件（可选；为空表示查询所有）</param>
    /// <param name="specification">给定的 <see cref="ISpecification{IAccessor}"/>（可选；默认使用 <see cref="ReadAccessAccessorSpecification"/> 规约）。</param>
    /// <returns>返回 <see cref="IList{T}"/>。</returns>
    public virtual IList<T>? FindList(Expression<Func<T, bool>>? predicate = null,
        ISpecification<IAccessor>? specification = null)
    {
        foreach (var accessor in GetReadingDispatcher(specification))
        {
            var query = accessor.Query<T>();

            if (predicate is not null)
                query = query.Where(predicate);

            var list = query.ToList();
            if (list.Count > 0)
                return list;
        }

        return null;
    }

    /// <summary>
    /// 异步通过指定断定条件查找类型实例集合。
    /// </summary>
    /// <param name="predicate">给定的断定条件（可选；为空表示查询所有）</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <param name="specification">给定的 <see cref="ISpecification{IAccessor}"/>（可选；默认使用 <see cref="ReadAccessAccessorSpecification"/> 规约）。</param>
    /// <returns>返回一个包含 <see cref="IList{T}"/> 的异步操作。</returns>
    public virtual Task<IList<T>?> FindListAsync(Expression<Func<T, bool>>? predicate = null,
        CancellationToken cancellationToken = default, ISpecification<IAccessor>? specification = null)
        => cancellationToken.RunTask(() => FindList(predicate, specification));


    /// <summary>
    /// 查找带有规约的类型实例集合。
    /// </summary>
    /// <param name="entitySpecification">给定的 <see cref="ISpecification{T}"/>（可选）。</param>
    /// <param name="specification">给定的 <see cref="ISpecification{IAccessor}"/>（可选；默认使用 <see cref="ReadAccessAccessorSpecification"/> 规约）。</param>
    /// <returns>返回 <see cref="IList{T}"/>。</returns>
    public virtual IList<T>? FindListWithSpecification(ISpecification<T>? entitySpecification = null,
        ISpecification<IAccessor>? specification = null)
    {
        foreach (var accessor in GetReadingDispatcher(specification))
        {
            var list = accessor.FindsWithSpecification(entitySpecification);
            if (list.Count > 0)
                return list;
        }

        return null;
    }

    /// <summary>
    /// 异步查找带有规约的类型实例集合。
    /// </summary>
    /// <param name="entitySpecification">给定的 <see cref="ISpecification{T}"/>（可选）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <param name="specification">给定的 <see cref="ISpecification{IAccessor}"/>（可选；默认使用 <see cref="ReadAccessAccessorSpecification"/> 规约）。</param>
    /// <returns>返回一个包含 <see cref="IList{T}"/> 的异步操作。</returns>
    public virtual async Task<IList<T>?> FindListWithSpecificationAsync(ISpecification<T>? entitySpecification = null,
        CancellationToken cancellationToken = default, ISpecification<IAccessor>? specification = null)
    {
        foreach (var accessor in GetReadingDispatcher(specification))
        {
            var list = await accessor.FindsWithSpecificationAsync(entitySpecification, cancellationToken);
            if (list.Count > 0)
                return list;
        }

        return null;
    }


    /// <summary>
    /// 查找类型实例分页集合。
    /// </summary>
    /// <param name="pageAction">给定的分页动作。</param>
    /// <param name="specification">给定的 <see cref="ISpecification{IAccessor}"/>（可选；默认使用 <see cref="ReadAccessAccessorSpecification"/> 规约）。</param>
    /// <returns>返回 <see cref="IPagingList{T}"/>。</returns>
    public virtual IPagingList<T>? FindPagingList(Action<IPagingList<T>> pageAction,
        ISpecification<IAccessor>? specification = null)
    {
        foreach (var accessor in GetReadingDispatcher(specification))
        {
            var list = accessor.FindPagingList(pageAction);
            if (list.Length > 0)
                return list;
        }

        return null;
    }

    /// <summary>
    /// 异步查找类型实例分页集合。
    /// </summary>
    /// <param name="pageAction">给定的分页动作。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <param name="specification">给定的 <see cref="ISpecification{IAccessor}"/>（可选；默认使用 <see cref="ReadAccessAccessorSpecification"/> 规约）。</param>
    /// <returns>返回一个包含 <see cref="IPagingList{T}"/> 的异步操作。</returns>
    public virtual async Task<IPagingList<T>?> FindPagingListAsync(Action<IPagingList<T>> pageAction,
        CancellationToken cancellationToken = default, ISpecification<IAccessor>? specification = null)
    {
        foreach (var accessor in GetReadingDispatcher(specification))
        {
            var list = await accessor.FindPagingListAsync(pageAction, cancellationToken);
            if (list.Length > 0)
                return list;
        }

        return null;
    }


    /// <summary>
    /// 查找带有规约的类型实例分页集合。
    /// </summary>
    /// <param name="pageAction">给定的分页动作。</param>
    /// <param name="entitySpecification">给定的 <see cref="ISpecification{T}"/>（可选）。</param>
    /// <param name="specification">给定的 <see cref="ISpecification{IAccessor}"/>（可选；默认使用 <see cref="ReadAccessAccessorSpecification"/> 规约）。</param>
    /// <returns>返回 <see cref="IPagingList{T}"/>。</returns>
    public virtual IPagingList<T>? FindPagingListWithSpecification(Action<IPagingList<T>> pageAction,
        ISpecification<T>? entitySpecification = null, ISpecification<IAccessor>? specification = null)
    {
        foreach (var accessor in GetReadingDispatcher(specification))
        {
            var list = accessor.FindPagingListWithSpecification(pageAction, entitySpecification);
            if (list.Length > 0)
                return list;
        }

        return null;
    }

    /// <summary>
    /// 异步查找带有规约的类型实例分页集合。
    /// </summary>
    /// <param name="pageAction">给定的分页动作。</param>
    /// <param name="entitySpecification">给定的 <see cref="ISpecification{T}"/>（可选）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <param name="specification">给定的 <see cref="ISpecification{IAccessor}"/>（可选；默认使用 <see cref="ReadAccessAccessorSpecification"/> 规约）。</param>
    /// <returns>返回一个包含 <see cref="IPagingList{T}"/> 的异步操作。</returns>
    public virtual async Task<IPagingList<T>?> FindPagingListWithSpecificationAsync(Action<IPagingList<T>> pageAction,
        ISpecification<T>? entitySpecification = null, CancellationToken cancellationToken = default,
        ISpecification<IAccessor>? specification = null)
    {
        foreach (var accessor in GetReadingDispatcher(specification))
        {
            var list = await accessor.FindPagingListWithSpecificationAsync(pageAction, entitySpecification, cancellationToken);
            if (list.Length > 0)
                return list;
        }

        return null;
    }

    #endregion


    #region Add

    /// <summary>
    /// 如果不存在则添加类型实例（仅支持写入存取器）。
    /// </summary>
    /// <param name="item">给定要添加的类型实例。</param>
    /// <param name="predicate">给定用于判定是否存在的工厂方法。</param>
    /// <param name="specification">给定的 <see cref="ISpecification{IAccessor}"/>（可选；默认使用 <see cref="WriteAccessAccessorSpecification"/> 规约）。</param>
    public virtual void AddIfNotExists(T item, Expression<Func<T, bool>> predicate,
        ISpecification<IAccessor>? specification = null)
    {
        foreach (var accessor in GetWritingDispatcher(specification))
        {
            var one = accessor.AddIfNotExists(item, predicate);
            if (one is not null)
                return;
        }

        return;
    }

    /// <summary>
    /// 添加类型实例集合（仅支持写入存取器）。
    /// </summary>
    /// <param name="entities">给定的类型实例数组集合。</param>
    public virtual void Add(params T[] entities)
    {
        foreach (var accessor in CurrentWriteAccessor.WritingDispatcher)
        {
            accessor.CurrentContext.AddRange(entities);
        }
    }

    /// <summary>
    /// 添加类型实例集合（仅支持写入存取器）。
    /// </summary>
    /// <param name="entities">给定的 <see cref="IEnumerable{T}"/>。</param>
    /// <param name="specification">给定的 <see cref="ISpecification{IAccessor}"/>（可选；默认使用 <see cref="WriteAccessAccessorSpecification"/> 规约）。</param>
    public virtual void Add(IEnumerable<T> entities, ISpecification<IAccessor>? specification = null)
    {
        foreach (var accessor in GetWritingDispatcher(specification))
        {
            accessor.CurrentContext.AddRange(entities);
        }
    }

    #endregion


    #region Remove

    /// <summary>
    /// 移除类型实例集合（仅支持写入存取器）。
    /// </summary>
    /// <param name="entities">给定的类型实例数组集合。</param>
    public virtual void Remove(params T[] entities)
    {
        foreach (var accessor in CurrentWriteAccessor.WritingDispatcher)
        {
            accessor.CurrentContext.RemoveRange(entities);
        }
    }

    /// <summary>
    /// 移除类型实例集合（仅支持写入存取器）。
    /// </summary>
    /// <param name="entities">给定的 <see cref="IEnumerable{T}"/>。</param>
    /// <param name="specification">给定的 <see cref="ISpecification{IAccessor}"/>（可选；默认使用 <see cref="WriteAccessAccessorSpecification"/> 规约）。</param>
    public virtual void Remove(IEnumerable<T> entities, ISpecification<IAccessor>? specification = null)
    {
        foreach (var accessor in GetWritingDispatcher(specification))
        {
            accessor.CurrentContext.RemoveRange(entities);
        }
    }

    #endregion


    #region Update

    /// <summary>
    /// 更新类型实例集合（仅支持写入存取器）。
    /// </summary>
    /// <param name="entities">给定的类型实例数组集合。</param>
    public virtual void Update(params T[] entities)
    {
        foreach (var accessor in CurrentWriteAccessor.WritingDispatcher)
        {
            accessor.CurrentContext.UpdateRange(entities);
        }
    }

    /// <summary>
    /// 更新类型实例集合（仅支持写入存取器）。
    /// </summary>
    /// <param name="entities">给定的 <see cref="IEnumerable{T}"/>。</param>
    /// <param name="specification">给定的 <see cref="ISpecification{IAccessor}"/>（可选；默认使用 <see cref="WriteAccessAccessorSpecification"/> 规约）。</param>
    public virtual void Update(IEnumerable<T> entities, ISpecification<IAccessor>? specification = null)
    {
        foreach (var accessor in GetWritingDispatcher(specification))
        {
            accessor.CurrentContext.UpdateRange(entities);
        }
    }

    #endregion


    #region SaveChanges

    /// <summary>
    /// 保存更改（仅支持写入存取器；操作结束后将自行切换为读取存取器）。
    /// </summary>
    /// <param name="specification">给定的 <see cref="ISpecification{IAccessor}"/>（可选；默认使用 <see cref="WriteAccessAccessorSpecification"/> 规约）。</param>
    /// <returns>返回受影响的行数。</returns>
    public virtual int SaveChanges(ISpecification<IAccessor>? specification = null)
    {
        var value = -1;

        foreach (var accessor in GetWritingDispatcher(specification))
        {
            value = accessor.CurrentContext.SaveChanges();
        }

        UseReadAccessor(specification);

        return value;
    }

    /// <summary>
    /// 异步保存更改（仅支持写入存取器；操作结束后将自行切换为读取存取器）。
    /// </summary>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <param name="specification">给定的 <see cref="ISpecification{IAccessor}"/>（可选；默认使用 <see cref="WriteAccessAccessorSpecification"/> 规约）。</param>
    /// <returns>返回一个包含受影响行数的异步操作。</returns>
    public virtual async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default,
        ISpecification<IAccessor>? specification = null)
    {
        var value = -1;

        foreach (var accessor in GetWritingDispatcher(specification))
        {
            value = await accessor.CurrentContext.SaveChangesAsync(cancellationToken);
        }

        UseReadAccessor(specification);

        return value;
    }

    #endregion

}
