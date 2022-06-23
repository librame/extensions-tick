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
/// 定义专用于 <see cref="DbSet{TEntity}"/> 的表达式规约。
/// </summary>
/// <typeparam name="TEntity">指定的类型。</typeparam>
public class DbSetEntitySpecification<TEntity> : IDbSetEntitySpecification<TEntity>
    where TEntity : class
{
    /// <summary>
    /// 构造一个 <see cref="DbSetEntitySpecification{TEntity}"/>。
    /// </summary>
    public DbSetEntitySpecification()
    {
    }

    /// <summary>
    /// 使用指定的判断依据构造一个 <see cref="DbSetEntitySpecification{TEntity}"/>。
    /// </summary>
    /// <param name="criterion">给定的判断依据。</param>
    /// <param name="checkLocal">检查本地缓存（如果启用，则优先检测本地缓存数据，如果不存在才会检测数据库数据；默认启用）。</param>
    public DbSetEntitySpecification(Expression<Func<TEntity, bool>>? criterion, bool checkLocal = true)
    {
        Criterion = criterion;
        CheckLocal = checkLocal;
    }


    /// <summary>
    /// 检查本地缓存（如果启用，则优先检测本地缓存数据，如果不存在才会检测数据库数据）。
    /// </summary>
    public bool CheckLocal { get; init; }

    /// <summary>
    /// 判断依据表达式。
    /// </summary>
    public Expression<Func<TEntity, bool>>? Criterion { get; init; }


    /// <summary>
    /// 升序排列表达式。
    /// </summary>
    public Expression<Func<TEntity, object>>? OrderBy { get; private set; }

    /// <summary>
    /// 降序排列表达式。
    /// </summary>
    public Expression<Func<TEntity, object>>? OrderByDescending { get; private set; }

    /// <summary>
    /// 出具提供方法。
    /// </summary>
    public Func<IEnumerable<TEntity>, TEntity>? Provider { get; private set; }

    /// <summary>
    /// 异步出具提供方法。
    /// </summary>
    public Func<IAsyncEnumerable<TEntity>, TEntity>? AsyncProvider { get; private set; }


    /// <summary>
    /// 判断是否存在指定条件的对象（如果本地缓存不为空时，支持查找本地缓存）。
    /// </summary>
    /// <param name="dbSet">给定的 <see cref="DbSet{TEntity}"/>。</param>
    /// <returns>返回布尔值。</returns>
    public virtual bool Exists(DbSet<TEntity> dbSet)
    {
        if (CheckLocal && dbSet.Local.Any())
        {
            if (Criterion is null)
                return true;

            if (dbSet.Local.Any(Criterion.Compile()))
                return true;
        }

        if (Criterion is null)
            return dbSet.Any();

        return dbSet.Any(Criterion);
    }

    /// <summary>
    /// 异步判断是否存在指定条件的对象（如果本地缓存不为空时，支持查找本地缓存）。
    /// </summary>
    /// <param name="dbSet">给定的 <see cref="DbSet{TEntity}"/>。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含布尔值的异步操作。</returns>
    public virtual async Task<bool> ExistsAsync(DbSet<TEntity> dbSet,
        CancellationToken cancellationToken = default)
    {
        if (CheckLocal && dbSet.Local.Any())
        {
            if (Criterion is null)
                return true;

            if (dbSet.Local.Any(Criterion.Compile()))
                return true;
        }

        if (Criterion is null)
            return await dbSet.AnyAsync(cancellationToken);

        return await dbSet.AnyAsync(Criterion, cancellationToken);
    }


    /// <summary>
    /// 评估可枚举对象。
    /// </summary>
    /// <param name="dbSet">给定的 <see cref="DbSet{TEntity}"/>。</param>
    /// <returns>返回 <see cref="IEnumerable{TEntity}"/>。</returns>
    public virtual IEnumerable<TEntity> Evaluate(DbSet<TEntity> dbSet)
    {
        (var enumerable, var queryable) = EvaluateCore(dbSet);

        return enumerable ?? queryable!.ToList();
    }

    /// <summary>
    /// 异步评估可枚举对象。
    /// </summary>
    /// <param name="dbSet">给定的 <see cref="DbSet{TEntity}"/>。</param>
    /// <returns>返回 <see cref="IAsyncEnumerable{TEntity}"/>。</returns>
    public virtual IAsyncEnumerable<TEntity> EvaluateAsync(DbSet<TEntity> dbSet)
    {
        (var enumerable, var queryable) = EvaluateCore(dbSet);

        if (enumerable is not null)
            return EnumerableExtensions.AsAsyncEnumerable(enumerable);

        return queryable!.AsAsyncEnumerable();
    }

    /// <summary>
    /// 评估实体数据集。
    /// </summary>
    /// <param name="dbSet">给定的 <see cref="DbSet{TEntity}"/>。</param>
    /// <returns>返回包含 <see cref="IEnumerable{TEntity}"/> 与 <see cref="IQueryable{TEntity}"/> 的元组。</returns>
    protected virtual (IEnumerable<TEntity>? enumerable, IQueryable<TEntity>? queryable) EvaluateCore(DbSet<TEntity> dbSet)
    {
        IEnumerable<TEntity> enumerable = dbSet.Local;
        IQueryable<TEntity> queryable = dbSet;

        if (Criterion is not null)
        {
            if (CheckLocal && dbSet.Local.Any())
                enumerable = enumerable.Where(Criterion.Compile());

            if (enumerable is null)
                queryable = queryable.Where(Criterion);
        }

        if (OrderBy is not null)
        {
            if (enumerable is not null)
                enumerable = enumerable.OrderBy(OrderBy.Compile());

            if (queryable is not null)
                queryable = queryable.OrderBy(OrderBy);
        }

        if (OrderByDescending is not null)
        {
            if (enumerable is not null)
                enumerable = enumerable.OrderByDescending(OrderByDescending.Compile());

            if (queryable is not null)
                queryable = queryable.OrderByDescending(OrderByDescending);
        }

        return (enumerable, queryable);
    }


    /// <summary>
    /// 出具可查询对象。
    /// </summary>
    /// <param name="enumerable">给定的 <see cref="IEnumerable{TEntity}"/>。</param>
    /// <returns>返回 <typeparamref name="TEntity"/>。</returns>
    public virtual TEntity Issue(IEnumerable<TEntity> enumerable)
    {
        if (Provider is not null)
            return Provider(enumerable);

        return enumerable.First();
    }

    /// <summary>
    /// 异步出具可查询对象。
    /// </summary>
    /// <param name="enumerable">给定的 <see cref="IAsyncEnumerable{TEntity}"/>。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回 <typeparamref name="TEntity"/>。</returns>
    public virtual TEntity IssueAsync(IAsyncEnumerable<TEntity> enumerable,
        CancellationToken cancellationToken = default)
    {
        if (AsyncProvider is not null)
            return AsyncProvider(enumerable);

        return enumerable.GetAsyncEnumerator(cancellationToken).Current;
    }


    /// <summary>
    /// 出具经过评估的可查询对象。
    /// </summary>
    /// <param name="dbSet">给定的 <see cref="DbSet{TEntity}"/>。</param>
    /// <returns>返回 <typeparamref name="TEntity"/>。</returns>
    public virtual TEntity IssueEvaluate(DbSet<TEntity> dbSet)
        => Issue(Evaluate(dbSet));

    /// <summary>
    /// 异步出具经过评估的可查询对象。
    /// </summary>
    /// <param name="dbSet">给定的 <see cref="DbSet{TEntity}"/>。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回 <typeparamref name="TEntity"/>。</returns>
    public virtual TEntity IssueEvaluateAsync(DbSet<TEntity> dbSet,
        CancellationToken cancellationToken = default)
        => IssueAsync(EvaluateAsync(dbSet), cancellationToken);


    /// <summary>
    /// 是否满足规约要求。
    /// </summary>
    /// <param name="value">给定的实例。</param>
    /// <returns>返回布尔值。</returns>
    public virtual bool IsSatisfiedBy(TEntity value)
        => true;


    /// <summary>
    /// 设置升序排列表达式。
    /// </summary>
    /// <param name="orderBy">给定的升序排列表达式。</param>
    /// <returns>返回 <see cref="IDbSetEntitySpecification{TEntity}"/>。</returns>
    public IDbSetEntitySpecification<TEntity> SetOrderBy(Expression<Func<TEntity, object>> orderBy)
    {
        OrderBy = orderBy;
        return this;
    }

    /// <summary>
    /// 设置降序排列表达式。
    /// </summary>
    /// <param name="orderByDescending">给定的降序排列表达式。</param>
    /// <returns>返回 <see cref="IDbSetEntitySpecification{TEntity}"/>。</returns>
    public IDbSetEntitySpecification<TEntity> SetOrderByDescending(Expression<Func<TEntity, object>> orderByDescending)
    {
        OrderByDescending = orderByDescending;
        return this;
    }

    /// <summary>
    /// 设置出具提供方法。
    /// </summary>
    /// <param name="provider">给定的出具提供方法。</param>
    /// <returns>返回 <see cref="IDbSetEntitySpecification{TEntity}"/>。</returns>
    public IDbSetEntitySpecification<TEntity> SetProvider(Func<IEnumerable<TEntity>, TEntity> provider)
    {
        Provider = provider;
        return this;
    }

    /// <summary>
    /// 设置异步出具提供方法。
    /// </summary>
    /// <param name="asyncProvider">给定的异步出具提供方法。</param>
    /// <returns>返回 <see cref="IDbSetEntitySpecification{TEntity}"/>。</returns>
    public IDbSetEntitySpecification<TEntity> SetProviderAsync(Func<IAsyncEnumerable<TEntity>, TEntity> asyncProvider)
    {
        AsyncProvider = asyncProvider;
        return this;
    }

}
