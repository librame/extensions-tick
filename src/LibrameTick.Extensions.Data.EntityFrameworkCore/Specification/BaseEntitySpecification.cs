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
/// 定义实现 <see cref="IEntitySpecification{T}"/> 的基础实体规约。
/// </summary>
/// <typeparam name="T">指定的类型。</typeparam>
public class BaseEntitySpecification<T> : BaseExpressionSpecification<T>, IEntitySpecification<T>
    where T : class
{
    private readonly List<Expression<Func<T, object>>> _foreignKeys
        = new List<Expression<Func<T, object>>>();


    /// <summary>
    /// 构造一个默认 <see cref="BaseExpressionSpecification{T}"/> 实例。
    /// </summary>
    public BaseEntitySpecification()
        : base()
    {
    }

    /// <summary>
    /// 使用规约条件构造一个 <see cref="BaseExpressionSpecification{T}"/> 实例。
    /// </summary>
    /// <param name="criterion">给定的判断依据表达式。</param>
    public BaseEntitySpecification(Expression<Func<T, bool>> criterion)
        : base(criterion)
    {
    }


    /// <summary>
    /// 外键表达式集合。
    /// </summary>
    public ICollection<Expression<Func<T, object>>> ForeignKeys
        => _foreignKeys;


    /// <summary>
    /// 添加外键表达式。
    /// </summary>
    /// <param name="foreignKey">给定的外键表达式。</param>
    /// <returns>返回 <see cref="IExpressionSpecification{T}"/>。</returns>
    public IEntitySpecification<T> AddForeignKey(Expression<Func<T, object>> foreignKey)
    {
        _foreignKeys.Add(foreignKey);
        return this;
    }


    /// <summary>
    /// 评估可查询对象。
    /// </summary>
    /// <param name="queryable">给定的 <see cref="IQueryable{T}"/>。</param>
    /// <returns>返回 <see cref="IQueryable{T}"/>。</returns>
    public override IQueryable<T> Evaluate(IQueryable<T> queryable)
    {
        queryable = base.Evaluate(queryable);

        if (ForeignKeys.Count > 0)
            queryable = ForeignKeys.Aggregate(queryable, (current, foreignKey) => current.Include(foreignKey));

        return queryable;
    }

}
