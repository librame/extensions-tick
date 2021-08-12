#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Librame.Extensions.Data
{
    /// <summary>
    /// <see cref="DbSet{TEntity}"/> 静态扩展。
    /// </summary>
    public static class DbSetExtensions
    {
        private static readonly object _locker = new object();


        #region LocalOrDbAny

        /// <summary>
        /// 确定本地缓存或数据库序列中是否包含任何元素。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <param name="dbSet">给定的 <see cref="DbSet{TEntity}"/>。</param>
        /// <param name="checkLocal">是否检查本地缓存（可选；默认优先检查本地缓存）。</param>
        /// <returns>返回是否存在的布尔值。</returns>
        public static bool LocalOrDbAny<TEntity>(this DbSet<TEntity> dbSet, bool checkLocal = true)
            where TEntity : class
        {
            lock (_locker)
            {
                if (checkLocal)
                    return dbSet.Local.Any() || dbSet.Any();

                return dbSet.Any();
            }
        }

        /// <summary>
        /// 确定本地缓存或数据库序列中是否包含任何元素。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <param name="dbSet">给定的 <see cref="DbSet{TEntity}"/>。</param>
        /// <param name="checkLocal">是否检查本地缓存（可选；默认优先检查本地缓存）。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回是否存在的布尔值。</returns>
        public static async Task<bool> LocalOrDbAnyAsync<TEntity>(this DbSet<TEntity> dbSet,
            bool checkLocal = true, CancellationToken cancellationToken = default)
            where TEntity : class
        {
            if (!checkLocal)
                return await dbSet.AnyAsync(cancellationToken);

            var localAny = false;
            lock (_locker)
            {
                localAny = dbSet.Local.Any();
            }

            return localAny || await dbSet.AnyAsync(cancellationToken);
        }


        /// <summary>
        /// 确定本地缓存或数据库序列中的任何元素是否满足条件。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <param name="dbSet">给定的 <see cref="DbSet{TEntity}"/>。</param>
        /// <param name="predicate">给定的断定方法。</param>
        /// <param name="checkLocal">是否检查本地缓存（可选；默认优先检查本地缓存）。</param>
        /// <returns>返回是否存在的布尔值。</returns>
        public static bool LocalOrDbAny<TEntity>(this DbSet<TEntity> dbSet,
            Expression<Func<TEntity, bool>> predicate, bool checkLocal = true)
            where TEntity : class
        {
            lock (_locker)
            {
                if (checkLocal)
                    return dbSet.Local.Any(predicate.Compile()) || dbSet.Any(predicate);

                return dbSet.Any(predicate);
            }
        }

        /// <summary>
        /// 确定本地缓存或数据库序列中的任何元素是否满足条件。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <param name="dbSet">给定的 <see cref="DbSet{TEntity}"/>。</param>
        /// <param name="predicate">给定的断定方法。</param>
        /// <param name="checkLocal">是否检查本地缓存（可选；默认优先检查本地缓存）。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回是否存在的布尔值。</returns>
        public static async Task<bool> LocalOrDbAnyAsync<TEntity>(this DbSet<TEntity> dbSet,
            Expression<Func<TEntity, bool>> predicate, bool checkLocal = true,
            CancellationToken cancellationToken = default)
            where TEntity : class
        {
            if (!checkLocal)
                return await dbSet.AnyAsync(predicate, cancellationToken);

            var localAny = false;
            lock (_locker)
            {
                localAny = dbSet.Local.Any(predicate.Compile());
            }

            return localAny || await dbSet.AnyAsync(predicate, cancellationToken);
        }

        #endregion


        #region LocalAndAny

        /// <summary>
        /// 确定本地缓存与数据库序列中是否包含任何元素。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <param name="dbSet">给定的 <see cref="DbSet{TEntity}"/>。</param>
        /// <returns>返回是否存在的布尔值。</returns>
        public static bool LocalAndDbAny<TEntity>(this DbSet<TEntity> dbSet)
            where TEntity : class
        {
            lock (_locker)
            {
                return dbSet.Local.Any() && dbSet.Any();
            }
        }

        /// <summary>
        /// 确定本地缓存与数据库序列中是否包含任何元素。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <param name="dbSet">给定的 <see cref="DbSet{TEntity}"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回是否存在的布尔值。</returns>
        public static async Task<bool> LocalAndDbAnyAsync<TEntity>(this DbSet<TEntity> dbSet,
            CancellationToken cancellationToken = default)
            where TEntity : class
        {
            var localAny = false;
            lock (_locker)
            {
                localAny = dbSet.Local.Any();
            }

            return localAny && await dbSet.AnyAsync(cancellationToken);
        }


        /// <summary>
        /// 确定本地缓存与数据库序列中的任何元素是否满足条件。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <param name="dbSet">给定的 <see cref="DbSet{TEntity}"/>。</param>
        /// <param name="predicate">给定的断定方法。</param>
        /// <returns>返回是否存在的布尔值。</returns>
        public static bool LocalAndDbAny<TEntity>(this DbSet<TEntity> dbSet,
            Expression<Func<TEntity, bool>> predicate)
            where TEntity : class
        {
            lock (_locker)
            {
                return dbSet.Local.Any(predicate.Compile()) && dbSet.Any(predicate);
            }
        }

        /// <summary>
        /// 确定本地缓存与数据库序列中的任何元素是否满足条件。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <param name="dbSet">给定的 <see cref="DbSet{TEntity}"/>。</param>
        /// <param name="predicate">给定的断定方法。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回是否存在的布尔值。</returns>
        public static async Task<bool> LocalAndDbAnyAsync<TEntity>(this DbSet<TEntity> dbSet,
            Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
            where TEntity : class
        {
            var localAny = false;
            lock (_locker)
            {
                localAny = dbSet.Local.Any(predicate.Compile());
            }

            return localAny && await dbSet.AnyAsync(predicate, cancellationToken);
        }

        #endregion

    }
}
