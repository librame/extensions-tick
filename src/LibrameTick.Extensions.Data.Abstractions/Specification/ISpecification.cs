#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Librame.Extensions.Data.Specification
{
    /// <summary>
    /// 定义用于查询的泛型规约接口。
    /// </summary>
    public interface ISpecification<T>
    {
        /// <summary>
        /// 规约条件表达式。
        /// </summary>
        Expression<Func<T, bool>>? Criteria { get; }

        /// <summary>
        /// 包含的外键表达式列表。
        /// </summary>
        IReadOnlyList<Expression<Func<T, object>>> Includes { get; }

        /// <summary>
        /// 升序排列表达式。
        /// </summary>
        Expression<Func<T, object>>? OrderBy { get; }

        /// <summary>
        /// 降序排列表达式。
        /// </summary>
        Expression<Func<T, object>>? OrderByDescending { get; }


        /// <summary>
        /// 添加包含的外键表达式。
        /// </summary>
        /// <param name="include">给定的外键表达式。</param>
        /// <returns>返回 <see cref="ISpecification{T}"/>。</returns>
        ISpecification<T> AddInclude(Expression<Func<T, object>> include);


        /// <summary>
        /// 设置升序排列表达式。
        /// </summary>
        /// <param name="orderBy">给定的升序排列表达式。</param>
        /// <returns>返回 <see cref="ISpecification{T}"/>。</returns>
        ISpecification<T> SetOrderBy(Expression<Func<T, object>> orderBy);

        /// <summary>
        /// 设置降序排列表达式。
        /// </summary>
        /// <param name="orderByDescending">给定的降序排列表达式。</param>
        /// <returns>返回 <see cref="ISpecification{T}"/>。</returns>
        ISpecification<T> SetOrderByDescending(Expression<Func<T, object>> orderByDescending);
    }
}
