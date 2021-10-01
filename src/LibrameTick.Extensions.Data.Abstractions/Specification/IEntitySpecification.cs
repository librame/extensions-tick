#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Core;

namespace Librame.Extensions.Data.Specification;

/// <summary>
/// 定义实现 <see cref="IExpressionSpecification{T}"/> 的实体规约接口。
/// </summary>
/// <typeparam name="T">指定的类型。</typeparam>
public interface IEntitySpecification<T> : IExpressionSpecification<T>
{
    /// <summary>
    /// 外键表达式集合。
    /// </summary>
    ICollection<Expression<Func<T, object>>> ForeignKeys { get; }


    /// <summary>
    /// 添加外键表达式。
    /// </summary>
    /// <param name="foreignKey">给定的外键表达式。</param>
    /// <returns>返回 <see cref="IExpressionSpecification{T}"/>。</returns>
    IEntitySpecification<T> AddForeignKey(Expression<Func<T, object>> foreignKey);
}
