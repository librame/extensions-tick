#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Data.Accessing;
using System.Linq.Expressions;

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 定义查询过滤器接口。
    /// </summary>
    public interface IQueryFilter
    {
        /// <summary>
        /// 启用查询过滤器。
        /// </summary>
        /// <param name="entityType">给定的实体类型。</param>
        /// <returns>返回布尔值。</returns>
        bool Enabling(Type entityType);

        /// <summary>
        /// 获取查询过滤器。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
        /// <returns>返回 <see cref="LambdaExpression"/>。</returns>
        LambdaExpression GetQueryFilter<TEntity>(IAccessor accessor)
            where TEntity : class;
    }
}
