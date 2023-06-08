#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data;

/// <summary>
/// 定义实现 <see cref="IUserCountingAggregation{TAggreg, TCount}"/> 的泛型用户访问计数聚合接口。
/// </summary>
/// <typeparam name="TAggreg">指定的计数聚合类型。</typeparam>
/// <typeparam name="TCount">指定的计数类型。</typeparam>
public interface IUserVisitingCountingAggregation<TAggreg, TCount> : IUserCountingAggregation<TAggreg, TCount>
    where TAggreg : class, IUserVisitingCountingAggregation<TAggreg, TCount>
    where TCount : IAdditionOperators<TCount, TCount, TCount>, ISubtractionOperators<TCount, TCount, TCount>
        , IDecrementOperators<TCount>, IIncrementOperators<TCount>
{
    /// <summary>
    /// 访问次数。
    /// </summary>
    /// <value>返回 <typeparamref name="TCount"/>。</value>
    TCount VisitCount { get; set; }

    /// <summary>
    /// 访问人数。
    /// </summary>
    /// <value>返回 <typeparamref name="TCount"/>。</value>
    TCount VisitorCount { get; set; }
}
