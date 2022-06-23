#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Specifications;

namespace Microsoft.EntityFrameworkCore;

/// <summary>
/// <see cref="IDbSetEntitySpecification{TEntity}"/> 静态扩展。
/// </summary>
public static class DbSetEntitySpecificationExtensions
{

    /// <summary>
    /// 通过表达式规约进行筛选并取得第一项（如果本地缓存不为空时，支持查找本地缓存）。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="dbSet">给定的 <see cref="DbSet{TEntity}"/>。</param>
    /// <param name="predicate">给定的断定方法表达式。</param>
    /// <param name="checkLocal">是否检查本地缓存（可选；默认启用检查）。</param>
    /// <returns>返回 <typeparamref name="TEntity"/>。</returns>
    public static TEntity FirstBySpecification<TEntity>(this DbSet<TEntity> dbSet,
        Expression<Func<TEntity, bool>>? predicate = null, bool checkLocal = true)
        where TEntity : class
        => new DbSetEntitySpecification<TEntity>(predicate, checkLocal).IssueEvaluate(dbSet);

    /// <summary>
    /// 异步通过表达式规约进行筛选并取得第一项（如果本地缓存不为空时，支持查找本地缓存）。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="dbSet">给定的 <see cref="DbSet{TEntity}"/>。</param>
    /// <param name="predicate">给定的断定方法表达式。</param>
    /// <param name="checkLocal">是否检查本地缓存（可选；默认启用检查）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回 <typeparamref name="TEntity"/>。</returns>
    public static TEntity FirstBySpecificationAsync<TEntity>(this DbSet<TEntity> dbSet,
        Expression<Func<TEntity, bool>>? predicate = null, bool checkLocal = true,
        CancellationToken cancellationToken = default)
        where TEntity : class
        => new DbSetEntitySpecification<TEntity>(predicate, checkLocal)
            .IssueEvaluateAsync(dbSet, cancellationToken);


    /// <summary>
    /// 通过表达式规约判断是否存在指定条件的对象（如果本地缓存不为空时，支持查找本地缓存）。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="dbSet">给定的 <see cref="DbSet{TEntity}"/>。</param>
    /// <param name="predicate">给定的断定方法表达式。</param>
    /// <param name="checkLocal">是否检查本地缓存（可选；默认启用检查）。</param>
    /// <returns>返回布尔值。</returns>
    public static bool ExistsBySpecification<TEntity>(this DbSet<TEntity> dbSet,
        Expression<Func<TEntity, bool>>? predicate, bool checkLocal = true)
        where TEntity : class
        => new DbSetEntitySpecification<TEntity>(predicate, checkLocal).Exists(dbSet);

    /// <summary>
    /// 异步通过表达式规约判断是否存在指定条件的对象（如果本地缓存不为空时，支持查找本地缓存）。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="dbSet">给定的 <see cref="DbSet{TEntity}"/>。</param>
    /// <param name="predicate">给定的断定方法表达式。</param>
    /// <param name="checkLocal">是否检查本地缓存（可选；默认启用检查）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含布尔值的异步操作。</returns>
    public static Task<bool> ExistsBySpecificationAsync<TEntity>(this DbSet<TEntity> dbSet,
        Expression<Func<TEntity, bool>>? predicate, bool checkLocal = true,
        CancellationToken cancellationToken = default)
        where TEntity : class
        => new DbSetEntitySpecification<TEntity>(predicate, checkLocal)
            .ExistsAsync(dbSet, cancellationToken);


    /// <summary>
    /// 通过表达式规约进行筛选（如果本地缓存不为空时，支持查找本地缓存）。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="dbSet">给定的 <see cref="DbSet{TEntity}"/>。</param>
    /// <param name="predicate">给定的断定方法表达式。</param>
    /// <param name="checkLocal">是否检查本地缓存（可选；默认启用检查）。</param>
    /// <returns>返回 <see cref="IEnumerable{TEntity}"/>。</returns>
    public static IEnumerable<TEntity> WhereBySpecification<TEntity>(this DbSet<TEntity> dbSet,
        Expression<Func<TEntity, bool>>? predicate = null, bool checkLocal = true)
        where TEntity : class
        => new DbSetEntitySpecification<TEntity>(predicate, checkLocal).Evaluate(dbSet);

    /// <summary>
    /// 异步通过表达式规约进行筛选（如果本地缓存不为空时，支持查找本地缓存）。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="dbSet">给定的 <see cref="DbSet{TEntity}"/>。</param>
    /// <param name="predicate">给定的断定方法表达式。</param>
    /// <param name="checkLocal">是否检查本地缓存（可选；默认启用检查）。</param>
    /// <returns>返回 <see cref="IAsyncEnumerable{TEntity}"/>。</returns>
    public static IAsyncEnumerable<TEntity> WhereBySpecificationAsync<TEntity>(this DbSet<TEntity> dbSet,
        Expression<Func<TEntity, bool>>? predicate = null, bool checkLocal = true)
        where TEntity : class
        => new DbSetEntitySpecification<TEntity>(predicate, checkLocal).EvaluateAsync(dbSet);

}
