#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data.Specifications;

/// <summary>
/// 定义数据状态实体规约。
/// </summary>
/// <typeparam name="T">指定的类型。</typeparam>
public class DataStateEntitySpecification<T> : BaseEntitySpecification<T>
    where T : class, IState<DataStatus>
{
    /// <summary>
    /// 构造一个默认 <see cref="DataStateEntitySpecification{T}"/> 实例。
    /// </summary>
    /// <param name="status">给定的 <see cref="DataStatus"/>。</param>
    public DataStateEntitySpecification(DataStatus? status)
        : base()
    {
        Status = status;
    }

    /// <summary>
    /// 使用规约条件构造一个 <see cref="DataStateEntitySpecification{T}"/> 实例。
    /// </summary>
    /// <param name="status">给定的 <see cref="DataStatus"/>。</param>
    /// <param name="criterion">给定的判断依据表达式。</param>
    public DataStateEntitySpecification(DataStatus? status, Expression<Func<T, bool>>? criterion)
        : base(criterion)
    {
        Status = status;
    }


    /// <summary>
    /// 数据状态。
    /// </summary>
    public DataStatus? Status { get; private set; }


    /// <summary>
    /// 评估可查询对象。
    /// </summary>
    /// <param name="queryable">给定的 <see cref="IQueryable{T}"/>。</param>
    /// <returns>返回 <see cref="IQueryable{T}"/>。</returns>
    public override IQueryable<T> Evaluate(IQueryable<T> queryable)
    {
        if (Status is not null)
            queryable = queryable.Where(p => p.Status == Status);

        return base.Evaluate(queryable);
    }

}
