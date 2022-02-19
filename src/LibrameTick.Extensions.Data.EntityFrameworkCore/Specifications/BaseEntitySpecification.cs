#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Specifications;

namespace Librame.Extensions.Data.Specifications;

/// <summary>
/// 定义实现 <see cref="IEntitySpecification{TEntity}"/> 的基础实体规约。
/// </summary>
/// <typeparam name="TEntity">指定的实体类型。</typeparam>
public class BaseEntitySpecification<TEntity> : BaseExpressionSpecification<TEntity>, IEntitySpecification<TEntity>
    where TEntity : class
{
    private readonly List<Expression<Func<TEntity, object>>> _foreignKeys
        = new List<Expression<Func<TEntity, object>>>();


    /// <summary>
    /// 构造一个 <see cref="BaseEntitySpecification{TEntity}"/> 实例。
    /// </summary>
    public BaseEntitySpecification()
        : base()
    {
    }

    /// <summary>
    /// 使用规约条件构造一个 <see cref="BaseEntitySpecification{TEntity}"/> 实例。
    /// </summary>
    /// <param name="criterion">给定的判断依据表达式。</param>
    public BaseEntitySpecification(Expression<Func<TEntity, bool>>? criterion)
        : base(criterion)
    {
    }


    /// <summary>
    /// 外键表达式集合。
    /// </summary>
    public ICollection<Expression<Func<TEntity, object>>> ForeignKeys
        => _foreignKeys;


    /// <summary>
    /// 添加外键表达式。
    /// </summary>
    /// <param name="foreignKey">给定的外键表达式。</param>
    /// <returns>返回 <see cref="IDbSetEntitySpecification{T}"/>。</returns>
    public IEntitySpecification<TEntity> AddForeignKey(Expression<Func<TEntity, object>> foreignKey)
    {
        _foreignKeys.Add(foreignKey);
        return this;
    }


    /// <summary>
    /// 评估可查询对象。
    /// </summary>
    /// <param name="queryable">给定的 <see cref="IQueryable{T}"/>。</param>
    /// <returns>返回 <see cref="IQueryable{T}"/>。</returns>
    public override IQueryable<TEntity> Evaluate(IQueryable<TEntity> queryable)
    {
        queryable = base.Evaluate(queryable);

        if (ForeignKeys.Count > 0)
        {
            queryable = ForeignKeys.Aggregate(queryable,
                (current, foreignKey) => current.Include(foreignKey));
        }

        return queryable;
    }

}
