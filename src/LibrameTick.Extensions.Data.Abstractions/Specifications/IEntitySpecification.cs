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

///// <summary>
///// 定义继承 <see cref="IExpressionSpecification{T}"/> 的实体规约接口。
///// </summary>
///// <typeparam name="TEntity">指定的实体类型。</typeparam>
//public interface IEntitySpecification<TEntity> : IExpressionSpecification<TEntity>
//{
//    /// <summary>
//    /// 外键表达式集合。
//    /// </summary>
//    ICollection<Expression<Func<TEntity, object>>> ForeignKeys { get; }


//    /// <summary>
//    /// 添加外键表达式。
//    /// </summary>
//    /// <param name="foreignKey">给定的外键表达式。</param>
//    /// <returns>返回 <see cref="IExpressionSpecification{T}"/>。</returns>
//    IEntitySpecification<TEntity> AddForeignKey(Expression<Func<TEntity, object>> foreignKey);
//}
