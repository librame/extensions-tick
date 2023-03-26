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

namespace Librame.Extensions.Specifications;

///// <summary>
///// <see cref="IEntitySpecification{T}"/> 静态扩展。
///// </summary>
//public static class EntitySpecificationExtensions
//{

//    #region EvaluateList

//    /// <summary>
//    /// 规约求值列表。
//    /// </summary>
//    /// <typeparam name="T">指定的类型。</typeparam>
//    /// <param name="queryable">给定的 <see cref="IQueryable{T}"/>。</param>
//    /// <param name="specification">给定的 <see cref="IEntitySpecification{T}"/>（可选）。</param>
//    /// <returns>返回 <see cref="List{T}"/>。</returns>
//    public static IList<T> EvaluateList<T>(this IQueryable<T> queryable,
//        IEntitySpecification<T>? specification = null)
//        where T : class
//    {
//        if (specification is null)
//            return queryable.ToList();

//        return specification.Evaluate(queryable).ToList();
//    }

//    /// <summary>
//    /// 异步规约求值列表。
//    /// </summary>
//    /// <typeparam name="T">指定的类型。</typeparam>
//    /// <param name="queryable">给定的 <see cref="IQueryable{T}"/>。</param>
//    /// <param name="specification">给定的 <see cref="IEntitySpecification{T}"/>（可选）。</param>
//    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
//    /// <returns>返回一个包含 <see cref="List{T}"/> 的异步操作。</returns>
//    public static async Task<IList<T>> EvaluateListAsync<T>(this IQueryable<T> queryable,
//        IEntitySpecification<T>? specification = null, CancellationToken cancellationToken = default)
//        where T : class
//    {
//        if (specification is null)
//            return await queryable.ToListAsync(cancellationToken);
        
//        return await specification.Evaluate(queryable).ToListAsync(cancellationToken);
//    }

//    #endregion


//    #region EvaluatePagingList

//    /// <summary>
//    /// 规约求值分页列表。
//    /// </summary>
//    /// <typeparam name="T">指定的类型。</typeparam>
//    /// <param name="queryable">给定的 <see cref="IQueryable{T}"/>。</param>
//    /// <param name="pageAction">给定的分页动作。</param>
//    /// <param name="specification">给定的 <see cref="IEntitySpecification{T}"/>（可选）。</param>
//    /// <returns>返回 <see cref="IPagingList{T}"/>。</returns>
//    public static IPagingList<T> EvaluatePagingList<T>(this IQueryable<T> queryable,
//        Action<IPagingList<T>> pageAction, IEntitySpecification<T>? specification = null)
//        where T : class
//    {
//        IPagingList<T> list = new PagingList<T>(queryable);

//        if (specification is not null)
//            list.Filtrate(query => specification.Evaluate(queryable));

//        pageAction(list);

//        return list;
//    }

//    /// <summary>
//    /// 规约求值分页列表。
//    /// </summary>
//    /// <typeparam name="T">指定的类型。</typeparam>
//    /// <param name="queryable">给定的 <see cref="IQueryable{T}"/>。</param>
//    /// <param name="pageAction">给定的分页动作。</param>
//    /// <param name="specification">给定的 <see cref="IEntitySpecification{T}"/>（可选）。</param>
//    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
//    /// <returns>返回一个包含 <see cref="PagingList{T}"/> 的异步操作。</returns>
//    public static Task<IPagingList<T>> EvaluatePagingListAsync<T>(this IQueryable<T> queryable,
//        Action<IPagingList<T>> pageAction, IEntitySpecification<T>? specification = null,
//        CancellationToken cancellationToken = default)
//        where T : class
//    {
//        IPagingList<T> list = new PagingList<T>(queryable);

//        if (specification is not null)
//            list.Filtrate(query => specification.Evaluate(queryable));

//        return cancellationToken.RunTask(() =>
//        {
//            pageAction(list);

//            return list;
//        });
//    }

//    #endregion

//}
