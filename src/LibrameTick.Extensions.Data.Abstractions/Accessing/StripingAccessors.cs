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
    /// <param name="accessors">给定要分散的 <see cref="IEnumerable{IAccessor}"/>。</param>
    public StripingAccessors(IEnumerable<IAccessor> accessors)
        : base(new TransactionTraversalDispatcher<IAccessor>(accessors))
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
        => ReadingEquilizer.InvokeGetLast(a => a.Query<TEntity>());

    /// <summary>
    /// 创建指定实体类型的可查询接口。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="name">要使用的共享类型实体类型的名称。</param>
    /// <returns>返回 <see cref="IQueryable{TEntity}"/>。</returns>
    public override IQueryable<TEntity> Query<TEntity>(string name)
        where TEntity : class
        => ReadingEquilizer.InvokeGetLast(a => a.Query<TEntity>(name));


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
        => ReadingEquilizer.InvokeGetLast(a => a.QueryBySql<TEntity>(sql, parameters));

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
        => ReadingEquilizer.InvokeGetLast(a => a.QueryBySql<TEntity>(name, sql, parameters));

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
        => ReadingEquilizer.InvokeGetLast(a => a.Exists(predicate, checkLocal));

    /// <summary>
    /// 异步在本地缓存或数据库中是否存在指定断定方法的实体。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="predicate">给定的断定方法表达式。</param>
    /// <param name="checkLocal">是否检查本地缓存（可选；默认启用检查）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含布尔值的异步操作。</returns>
    public override Task<bool> ExistsAsync<TEntity>(Expression<Func<TEntity, bool>> predicate,
        bool checkLocal = true, CancellationToken cancellationToken = default)
        where TEntity : class
        => ReadingEquilizer.InvokeGetLast(a => a.ExistsAsync(predicate, checkLocal, cancellationToken));

    #endregion


    #region Find

    /// <summary>
    /// 查找带有规约的实体集合。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="specification">给定的 <see cref="IEntitySpecification{TEntity}"/>（可选）。</param>
    /// <returns>返回 <see cref="IList{TEntity}"/>。</returns>
    public override IList<TEntity> FindListWithSpecification<TEntity>(IEntitySpecification<TEntity>? specification = null)
        where TEntity : class
        => ReadingEquilizer.InvokeGetLast(a => a.FindListWithSpecification(specification));

    /// <summary>
    /// 异步查找带有规约的实体集合。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="specification">给定的 <see cref="IEntitySpecification{TEntity}"/>（可选）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含 <see cref="IList{TEntity}"/> 的异步操作。</returns>
    public override Task<IList<TEntity>> FindListWithSpecificationAsync<TEntity>(IEntitySpecification<TEntity>? specification = null,
        CancellationToken cancellationToken = default)
        where TEntity : class
        => ReadingEquilizer.InvokeGetLast(a => a.FindListWithSpecificationAsync(specification, cancellationToken));


    /// <summary>
    /// 查找实体分页集合。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="pageAction">给定的分页动作。</param>
    /// <returns>返回 <see cref="IPagingList{TEntity}"/>。</returns>
    public override IPagingList<TEntity> FindPagingList<TEntity>(Action<IPagingList<TEntity>> pageAction)
        where TEntity : class
        => ReadingEquilizer.InvokeGetLast(a => a.FindPagingList(pageAction));

    /// <summary>
    /// 异步查找实体分页集合。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="pageAction">给定的分页动作。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含 <see cref="IPagingList{TEntity}"/> 的异步操作。</returns>
    public override Task<IPagingList<TEntity>> FindPagingListAsync<TEntity>(Action<IPagingList<TEntity>> pageAction,
        CancellationToken cancellationToken = default)
        where TEntity : class
        => ReadingEquilizer.InvokeGetLast(a => a.FindPagingListAsync(pageAction, cancellationToken));


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
        => ReadingEquilizer.InvokeGetLast(a => a.FindPagingListWithSpecification(pageAction, specification));

    /// <summary>
    /// 异步查找带有规约的实体分页集合。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="pageAction">给定的分页动作。</param>
    /// <param name="specification">给定的 <see cref="IEntitySpecification{TEntity}"/>（可选）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含 <see cref="IPagingList{TEntity}"/> 的异步操作。</returns>
    public override Task<IPagingList<TEntity>> FindPagingListWithSpecificationAsync<TEntity>(Action<IPagingList<TEntity>> pageAction,
        IEntitySpecification<TEntity>? specification = null, CancellationToken cancellationToken = default)
        where TEntity : class
        => ReadingEquilizer.InvokeGetLast(a => a.FindPagingListWithSpecificationAsync(pageAction, specification, cancellationToken));

    #endregion

}
