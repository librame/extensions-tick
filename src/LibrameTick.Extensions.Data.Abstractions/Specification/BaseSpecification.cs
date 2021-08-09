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
    /// 定义实现 <see cref="ISpecification{T}"/> 的泛型基础规约。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    public class BaseSpecification<T> : ISpecification<T>
        where T : class
    {
        private readonly List<Expression<Func<T, object>>> _includes
            = new List<Expression<Func<T, object>>>();

        private Expression<Func<T, object>>? _orderBy;
        private Expression<Func<T, object>>? _orderByDescending;


        /// <summary>
        /// 构造一个默认 <see cref="BaseSpecification{T}"/> 实例。
        /// </summary>
        public BaseSpecification()
        {
        }

        /// <summary>
        /// 使用规约条件构造一个 <see cref="BaseSpecification{T}"/> 实例。
        /// </summary>
        /// <param name="criteria">给定的规约条件表达式。</param>
        public BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }


        /// <summary>
        /// 规约条件表达式。
        /// </summary>
        public Expression<Func<T, bool>>? Criteria { get; init; }

        /// <summary>
        /// 包含的外键表达式列表。
        /// </summary>
        public IReadOnlyList<Expression<Func<T, object>>> Includes
            => _includes;

        /// <summary>
        /// 升序排列表达式。
        /// </summary>
        public Expression<Func<T, object>>? OrderBy
            => _orderBy;

        /// <summary>
        /// 降序排列表达式。
        /// </summary>
        public Expression<Func<T, object>>? OrderByDescending
            => _orderByDescending;


        /// <summary>
        /// 添加包含的外键表达式。
        /// </summary>
        /// <param name="include">给定的外键表达式。</param>
        /// <returns>返回 <see cref="ISpecification{T}"/>。</returns>
        public ISpecification<T> AddInclude(Expression<Func<T, object>> include)
        {
            _includes.Add(include);
            return this;
        }


        /// <summary>
        /// 设置升序排列表达式。
        /// </summary>
        /// <param name="orderBy">给定的升序排列表达式。</param>
        /// <returns>返回 <see cref="ISpecification{T}"/>。</returns>
        public ISpecification<T> SetOrderBy(Expression<Func<T, object>> orderBy)
        {
            _orderBy = orderBy;
            return this;
        }

        /// <summary>
        /// 设置降序排列表达式。
        /// </summary>
        /// <param name="orderByDescending">给定的降序排列表达式。</param>
        /// <returns>返回 <see cref="ISpecification{T}"/>。</returns>
        public ISpecification<T> SetOrderByDescending(Expression<Func<T, object>> orderByDescending)
        {
            _orderByDescending = orderByDescending;
            return this;
        }

    }
}
