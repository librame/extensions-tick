#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Specifications;

/// <summary>
/// 定义一个用于 <see cref="DbSet{TEntity}"/> 筛选的表达式规约接口。
/// </summary>
/// <typeparam name="TEntity">指定的规约类型。</typeparam>
public interface IDbSetEntitySpecification<TEntity>
    where TEntity : class
{
    /// <summary>
    /// 检查本地缓存（如果启用，则优先检测本地缓存数据，如果不存在才会检测数据库数据）。
    /// </summary>
    bool CheckLocal { get; init; }

    /// <summary>
    /// 判断依据表达式。
    /// </summary>
    Expression<Func<TEntity, bool>>? Criterion { get; }


    /// <summary>
    /// 升序排列方法。
    /// </summary>
    Expression<Func<TEntity, object>>? OrderBy { get; }

    /// <summary>
    /// 降序排列方法。
    /// </summary>
    Expression<Func<TEntity, object>>? OrderByDescending { get; }

    /// <summary>
    /// 出具提供方法。
    /// </summary>
    Func<IEnumerable<TEntity>, TEntity>? Provider { get; }


    /// <summary>
    /// 判断是否存在指定条件的对象（如果本地缓存不为空时，支持查找本地缓存）。
    /// </summary>
    /// <param name="dbSet">给定的 <see cref="DbSet{TEntity}"/>。</param>
    /// <returns>返回布尔值。</returns>
    bool Exists(DbSet<TEntity> dbSet);

    /// <summary>
    /// 异步判断是否存在指定条件的对象（如果本地缓存不为空时，支持查找本地缓存）。
    /// </summary>
    /// <param name="dbSet">给定的 <see cref="DbSet{TEntity}"/>。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含布尔值的异步操作。</returns>
    Task<bool> ExistsAsync(DbSet<TEntity> dbSet,
        CancellationToken cancellationToken = default);


    /// <summary>
    /// 评估可查询对象。
    /// </summary>
    /// <param name="dbSet">给定的 <see cref="DbSet{TEntity}"/>。</param>
    /// <returns>返回 <see cref="IEnumerable{TEntity}"/>。</returns>
    IEnumerable<TEntity> Evaluate(DbSet<TEntity> dbSet);

    /// <summary>
    /// 出具可查询对象。
    /// </summary>
    /// <param name="queryable">给定的 <see cref="IEnumerable{TEntity}"/>。</param>
    /// <returns>返回 <typeparamref name="TEntity"/>。</returns>
    TEntity Issue(IEnumerable<TEntity> queryable);

    /// <summary>
    /// 出具经过评估的可查询对象。
    /// </summary>
    /// <param name="dbSet">给定的 <see cref="DbSet{TEntity}"/>。</param>
    /// <returns>返回 <typeparamref name="TEntity"/>。</returns>
    TEntity IssueEvaluate(DbSet<TEntity> dbSet);


    /// <summary>
    /// 是否满足规约要求。
    /// </summary>
    /// <param name="value">给定的实例。</param>
    /// <returns>返回布尔值。</returns>
    bool IsSatisfiedBy(TEntity value);


    /// <summary>
    /// 设置升序排列表达式。
    /// </summary>
    /// <param name="orderBy">给定的升序排列表达式。</param>
    /// <returns>返回 <see cref="IDbSetEntitySpecification{TEntity}"/>。</returns>
    IDbSetEntitySpecification<TEntity> SetOrderBy(Expression<Func<TEntity, object>> orderBy);

    /// <summary>
    /// 设置降序排列表达式。
    /// </summary>
    /// <param name="orderByDescending">给定的降序排列表达式。</param>
    /// <returns>返回 <see cref="IDbSetEntitySpecification{TEntity}"/>。</returns>
    IDbSetEntitySpecification<TEntity> SetOrderByDescending(Expression<Func<TEntity, object>> orderByDescending);

    /// <summary>
    /// 设置出具提供方法。
    /// </summary>
    /// <param name="provider">给定的出具提供方法。</param>
    /// <returns>返回 <see cref="IDbSetEntitySpecification{TEntity}"/>。</returns>
    IDbSetEntitySpecification<TEntity> SetProvider(Func<IEnumerable<TEntity>, TEntity> provider);
}
