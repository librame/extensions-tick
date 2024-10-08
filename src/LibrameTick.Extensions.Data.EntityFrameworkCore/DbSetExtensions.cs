#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data;

/// <summary>
/// <see cref="DbSet{TEntity}"/> 静态扩展。
/// </summary>
public static class DbSetExtensions
{

    /// <summary>
    /// 筛选并取得第一项（如果本地缓存不为空时，支持查找本地缓存）。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="dbSet">给定的 <see cref="DbSet{TEntity}"/>。</param>
    /// <param name="predicate">给定的断定方法表达式。</param>
    /// <param name="checkLocal">是否检查本地缓存（可选；默认启用检查）。</param>
    /// <returns>返回 <typeparamref name="TEntity"/>。</returns>
    public static TEntity? FirstOrDefaultWithLocal<TEntity>(this DbSet<TEntity> dbSet,
        Expression<Func<TEntity, bool>>? predicate = null, bool checkLocal = true)
        where TEntity : class
    {
        if (predicate is null)
        {
            return checkLocal
                ? dbSet.Local.FirstOrDefault()
                : dbSet.FirstOrDefault();
        }

        return checkLocal
                ? dbSet.Local.FirstOrDefault(predicate.Compile())
                : dbSet.FirstOrDefault(predicate);
    }

    /// <summary>
    /// 异步筛选并取得第一项（如果本地缓存不为空时，支持查找本地缓存）。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="dbSet">给定的 <see cref="DbSet{TEntity}"/>。</param>
    /// <param name="predicate">给定的断定方法表达式。</param>
    /// <param name="checkLocal">是否检查本地缓存（可选；默认启用检查）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回 <typeparamref name="TEntity"/>。</returns>
    public static async Task<TEntity?> FirstOrDefaultWithLocalAsync<TEntity>(this DbSet<TEntity> dbSet,
        Expression<Func<TEntity, bool>>? predicate = null, bool checkLocal = true,
        CancellationToken cancellationToken = default)
        where TEntity : class
    {
        if (checkLocal)
        {
            return predicate is null
                ? await cancellationToken.SimpleTaskResult(dbSet.Local.FirstOrDefault)
                : await cancellationToken.SimpleTaskResult(() => dbSet.Local.FirstOrDefault(predicate.Compile()));
        }

        return predicate is null
            ? await dbSet.FirstOrDefaultAsync(cancellationToken)
            : await dbSet.FirstOrDefaultAsync(predicate, cancellationToken);
    }


    /// <summary>
    /// 通过判断是否存在指定条件的对象（如果本地缓存不为空时，支持查找本地缓存）。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="dbSet">给定的 <see cref="DbSet{TEntity}"/>。</param>
    /// <param name="predicate">给定的断定方法表达式。</param>
    /// <param name="checkLocal">是否检查本地缓存（可选；默认启用检查）。</param>
    /// <returns>返回布尔值。</returns>
    public static bool ExistsWithLocal<TEntity>(this DbSet<TEntity> dbSet,
        Expression<Func<TEntity, bool>>? predicate, bool checkLocal = true)
        where TEntity : class
    {
        if (predicate is null)
        {
            return checkLocal
                ? dbSet.Local.Any()
                : dbSet.Any();
        }

        return checkLocal
                ? dbSet.Local.Any(predicate.Compile())
                : dbSet.Any(predicate);
    }

    /// <summary>
    /// 异步判断是否存在指定条件的对象（如果本地缓存不为空时，支持查找本地缓存）。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="dbSet">给定的 <see cref="DbSet{TEntity}"/>。</param>
    /// <param name="predicate">给定的断定方法表达式。</param>
    /// <param name="checkLocal">是否检查本地缓存（可选；默认启用检查）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含布尔值的异步操作。</returns>
    public static async Task<bool> ExistsWithLocalAsync<TEntity>(this DbSet<TEntity> dbSet,
        Expression<Func<TEntity, bool>>? predicate, bool checkLocal = true,
        CancellationToken cancellationToken = default)
        where TEntity : class
    {
        if (checkLocal)
        {
            return predicate is null
                ? await cancellationToken.SimpleTaskResult(dbSet.Local.Any)
                : await cancellationToken.SimpleTaskResult(() => dbSet.Local.Any(predicate.Compile()));
        }

        return predicate is null
            ? await dbSet.AnyAsync(cancellationToken)
            : await dbSet.AnyAsync(predicate, cancellationToken);
    }


    /// <summary>
    /// 带本地缓存的筛选（如果本地缓存不为空时，支持查找本地缓存）。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="dbSet">给定的 <see cref="DbSet{TEntity}"/>。</param>
    /// <param name="predicate">给定的断定方法表达式。</param>
    /// <param name="checkLocal">是否检查本地缓存（可选；默认启用检查）。</param>
    /// <returns>返回 <see cref="IEnumerable{TEntity}"/>。</returns>
    public static IEnumerable<TEntity> WhereWithLocal<TEntity>(this DbSet<TEntity> dbSet,
        Expression<Func<TEntity, bool>>? predicate = null, bool checkLocal = true)
        where TEntity : class
    {
        if (predicate is null)
        {
            return checkLocal
                ? dbSet.Local
                : dbSet.AsEnumerable();
        }

        return checkLocal
                ? dbSet.Local.Where(predicate.Compile())
                : dbSet.Where(predicate);
    }

}
