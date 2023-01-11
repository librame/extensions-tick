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
using Librame.Extensions.Dispatchers;
using Librame.Extensions.Specifications;

namespace Librame.Extensions.Data.Accessing;

/// <summary>
/// 定义一个表示数据访问集合的分布式存取器（支持针对读取与写入事务遍历等功能）。
/// </summary>
public class StripingAccessors : RedundableAccessors
{
    /// <summary>
    /// 构造一个 <see cref="MirroringAccessors"/>。
    /// </summary>
    /// <param name="accessors">给定的 <see cref="IEnumerable{IAccessor}"/>。</param>
    /// <param name="dispatcherOptions">给定的 <see cref="DispatcherOptions"/>。</param>
    public StripingAccessors(IEnumerable<IAccessor> accessors, DispatcherOptions dispatcherOptions)
        : base(new TransactionTraversalDispatcher<IAccessor>(accessors, dispatcherOptions))
    {
        // 分割需要从多库聚合数据
    }


    #region Query

    /// <summary>
    /// 创建指定实体类型的可查询接口。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <returns>返回 <see cref="IQueryable{TEntity}"/>。</returns>
    public override IQueryable<TEntity> Query<TEntity>()
        where TEntity : class
        => ReadingDispatcher.InvokeFunc(a => a.CurrentSource!.Query<TEntity>()).First();

    /// <summary>
    /// 创建指定实体类型的可查询接口。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="name">要使用的共享类型实体类型的名称。</param>
    /// <returns>返回 <see cref="IQueryable{TEntity}"/>。</returns>
    public override IQueryable<TEntity> Query<TEntity>(string name)
        where TEntity : class
        => ReadingDispatcher.InvokeFunc(a => a.CurrentSource!.Query<TEntity>(name)).First();


    /// <summary>
    /// 通过 SQL 语句创建指定实体类型的可查询接口。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="sql">给定的 SQL 语句（可使用“${Schema}、${Table}/${TableName}”模板关键字分别代替架构、表名等参数值）。</param>
    /// <param name="parameters">给定的参数数组。</param>
    /// <returns>返回 <see cref="IQueryable{TEntity}"/>。</returns>
    public override IQueryable<TEntity> QueryBySql<TEntity>(string sql,
        params object[] parameters)
        where TEntity : class
        => ReadingDispatcher.InvokeFunc(a => a.CurrentSource!.QueryBySql<TEntity>(sql, parameters)).First();

    /// <summary>
    /// 通过 SQL 语句创建指定实体类型的可查询接口。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="name">要使用的共享类型实体类型的名称。</param>
    /// <param name="sql">给定的 SQL 语句（可使用“${Schema}、${Table}/${TableName}”模板关键字分别代替架构、表名等参数值）。</param>
    /// <param name="parameters">给定的参数数组。</param>
    /// <returns>返回 <see cref="IQueryable{TEntity}"/>。</returns>
    public override IQueryable<TEntity> QueryBySql<TEntity>(string name,
        string sql, params object[] parameters)
        where TEntity : class
        => ReadingDispatcher.InvokeFunc(a => a.CurrentSource!.QueryBySql<TEntity>(name, sql, parameters)).First();

    #endregion


    #region Exists

    /// <summary>
    /// 在本地缓存或数据库中是否存在指定断定方法的实体。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="predicate">给定的断定方法表达式。</param>
    /// <param name="checkLocal">是否检查本地缓存（可选；默认启用检查）。</param>
    /// <returns>返回布尔值。</returns>
    public override bool Exists<TEntity>(Expression<Func<TEntity, bool>> predicate,
        bool checkLocal = true)
        where TEntity : class
        => ReadingDispatcher.InvokeFunc(a => a.CurrentSource.Exists(predicate, checkLocal)).First();

    /// <summary>
    /// 异步在本地缓存或数据库中是否存在指定断定方法的实体。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="predicate">给定的断定方法表达式。</param>
    /// <param name="checkLocal">是否检查本地缓存（可选；默认启用检查）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含布尔值的异步操作。</returns>
    public override async Task<bool> ExistsAsync<TEntity>(Expression<Func<TEntity, bool>> predicate,
        bool checkLocal = true, CancellationToken cancellationToken = default)
        where TEntity : class
    {
        var results = await ReadingDispatcher.InvokeFuncAsync(a => a.CurrentSource.ExistsAsync(predicate, checkLocal, cancellationToken));

        return results.First();
    }

    #endregion


    #region Find

    /// <summary>
    /// 查找带有规约的实体集合。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="specification">给定的 <see cref="IEntitySpecification{TEntity}"/>（可选）。</param>
    /// <returns>返回 <see cref="IList{TEntity}"/>。</returns>
    public override IList<TEntity> FindsWithSpecification<TEntity>(IEntitySpecification<TEntity>? specification = null)
        where TEntity : class
        => ReadingDispatcher.InvokeFunc(a => a.CurrentSource.FindsWithSpecification(specification)).First();

    /// <summary>
    /// 异步查找带有规约的实体集合。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="specification">给定的 <see cref="IEntitySpecification{TEntity}"/>（可选）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含 <see cref="IList{TEntity}"/> 的异步操作。</returns>
    public override async Task<IList<TEntity>> FindsWithSpecificationAsync<TEntity>(IEntitySpecification<TEntity>? specification = null,
        CancellationToken cancellationToken = default)
        where TEntity : class
    {
        var pagings = await ReadingDispatcher.InvokeFuncAsync(a => a.CurrentSource.FindsWithSpecificationAsync(specification, cancellationToken));

        return pagings.First();
    }


    /// <summary>
    /// 查找实体分页集合。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="pageAction">给定的分页动作。</param>
    /// <returns>返回 <see cref="IPagingList{TEntity}"/>。</returns>
    public override IPagingList<TEntity> FindPagingList<TEntity>(Action<IPagingList<TEntity>> pageAction)
        where TEntity : class
        => ReadingDispatcher.InvokeFunc(a => a.CurrentSource.FindPagingList(pageAction)).CompositePaging();

    /// <summary>
    /// 异步查找实体分页集合。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="pageAction">给定的分页动作。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含 <see cref="IPagingList{TEntity}"/> 的异步操作。</returns>
    public override async Task<IPagingList<TEntity>> FindPagingListAsync<TEntity>(Action<IPagingList<TEntity>> pageAction,
        CancellationToken cancellationToken = default)
        where TEntity : class
    {
        var pagings = await ReadingDispatcher.InvokeFuncAsync(a => a.CurrentSource.FindPagingListAsync(pageAction, cancellationToken));

        return await pagings.CompositePagingAsync(cancellationToken);
    }


    /// <summary>
    /// 查找带有规约的实体分页集合。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="pageAction">给定的分页动作。</param>
    /// <param name="specification">给定的 <see cref="IEntitySpecification{TEntity}"/>（可选）。</param>
    /// <returns>返回 <see cref="IPagingList{TEntity}"/>。</returns>
    public override IPagingList<TEntity> FindPagingListWithSpecification<TEntity>(Action<IPagingList<TEntity>> pageAction,
        IEntitySpecification<TEntity>? specification = null)
        where TEntity : class
        => ReadingDispatcher.InvokeFunc(a => a.CurrentSource.FindPagingListWithSpecification(pageAction, specification)).CompositePaging();

    /// <summary>
    /// 异步查找带有规约的实体分页集合。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="pageAction">给定的分页动作。</param>
    /// <param name="specification">给定的 <see cref="IEntitySpecification{TEntity}"/>（可选）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含 <see cref="IPagingList{TEntity}"/> 的异步操作。</returns>
    public override async Task<IPagingList<TEntity>> FindPagingListWithSpecificationAsync<TEntity>(Action<IPagingList<TEntity>> pageAction,
        IEntitySpecification<TEntity>? specification = null, CancellationToken cancellationToken = default)
        where TEntity : class
    {
        var pagings = await ReadingDispatcher.InvokeFuncAsync(a => a.CurrentSource.FindPagingListWithSpecificationAsync(pageAction, specification, cancellationToken));

        return await pagings.CompositePagingAsync(cancellationToken);
    }

    #endregion

}
