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
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data.Specifications
{
    /// <summary>
    /// <see cref="ISpecification{T}"/> 求值。
    /// </summary>
    public static class SpecificationEvaluation
    {

        #region EvaluateList

        /// <summary>
        /// 规约求值列表。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="queryable">给定的 <see cref="IQueryable{T}"/>。</param>
        /// <param name="specification">给定的 <see cref="ISpecification{T}"/>（可选）。</param>
        /// <returns>返回 <see cref="List{T}"/>。</returns>
        public static IList<T> EvaluateList<T>(IQueryable<T> queryable,
            ISpecification<T>? specification = null)
            where T : class
        {
            if (specification == null)
                return queryable.ToList();

            return CombineQueryable(queryable, specification).ToList();
        }

        /// <summary>
        /// 异步规约求值列表。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="queryable">给定的 <see cref="IQueryable{T}"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <param name="specification">给定的 <see cref="ISpecification{T}"/>（可选）。</param>
        /// <returns>返回一个包含 <see cref="List{T}"/> 的异步操作。</returns>
        public static async Task<IList<T>> EvaluateListAsync<T>(IQueryable<T> queryable,
            CancellationToken cancellationToken = default, ISpecification<T>? specification = null)
            where T : class
        {
            if (specification == null)
                return await queryable.ToListAsync(cancellationToken);

            return await CombineQueryable(queryable, specification).ToListAsync(cancellationToken);
        }

        #endregion


        #region EvaluatePagingList

        /// <summary>
        /// 规约求值分页列表。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="queryable">给定的 <see cref="IQueryable{T}"/>。</param>
        /// <param name="pageAction">给定的分页动作。</param>
        /// <param name="specification">给定的 <see cref="ISpecification{T}"/>（可选）。</param>
        /// <returns>返回 <see cref="IPagingList{T}"/>。</returns>
        public static IPagingList<T> EvaluatePagingList<T>(IQueryable<T> queryable,
            Action<IPagingList<T>> pageAction,
            ISpecification<T>? specification = null)
            where T : class
        {
            IPagingList<T> list = new PagingList<T>(queryable);

            if (specification != null)
                list.Filtrate(query => CombineQueryable(query, specification));

            pageAction.Invoke(list);

            return list;
        }

        /// <summary>
        /// 规约求值分页列表。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="queryable">给定的 <see cref="IQueryable{T}"/>。</param>
        /// <param name="pageAction">给定的分页动作。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <param name="specification">给定的 <see cref="ISpecification{T}"/>（可选）。</param>
        /// <returns>返回一个包含 <see cref="PagingList{T}"/> 的异步操作。</returns>
        public static Task<IPagingList<T>> EvaluatePagingListAsync<T>(IQueryable<T> queryable,
            Action<IPagingList<T>> pageAction, CancellationToken cancellationToken = default,
            ISpecification<T>? specification = null)
            where T : class
        {
            IPagingList<T> list = new PagingList<T>(queryable);

            if (specification != null)
                list.Filtrate(query => CombineQueryable(query, specification));

            return cancellationToken.RunTask(() =>
            {
                pageAction.Invoke(list);

                return list;
            });
        }

        #endregion


        private static IQueryable<T> CombineQueryable<T>(IQueryable<T> queryable,
            ISpecification<T> specification)
            where T : class
        {
            var query = queryable;

            if (specification.Criteria != null)
                query = query.Where(specification.Criteria);

            if (specification.OrderBy != null)
                query = query.OrderBy(specification.OrderBy);

            if (specification.OrderByDescending != null)
                query = query.OrderByDescending(specification.OrderByDescending);

            if (specification.Includes.Count > 0)
                query = specification.Includes.Aggregate(query, (current, include) => current.Include(include));

            return query;
        }

    }
}
