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
/// 定义实现 <see cref="ICountingAggregation{TAggreg, TCount}"/> 的泛型用户计数聚合接口。
/// </summary>
/// <typeparam name="TAggreg">指定的计数聚合类型。</typeparam>
/// <typeparam name="TCount">指定的计数类型。</typeparam>
public interface IUserCountingAggregation<TAggreg, TCount> : ICountingAggregation<TAggreg, TCount>
    where TAggreg : class, IUserCountingAggregation<TAggreg, TCount>
    where TCount : IAdditionOperators<TCount, TCount, TCount>, ISubtractionOperators<TCount, TCount, TCount>
        , IDecrementOperators<TCount>, IIncrementOperators<TCount>
{
    /// <summary>
    /// 支持人数。
    /// </summary>
    /// <value>返回 <typeparamref name="TCount"/>。</value>
    TCount SupporterCount { get; set; }

    /// <summary>
    /// 反对人数。
    /// </summary>
    /// <value>返回 <typeparamref name="TCount"/>。</value>
    TCount ObjectorCount { get; set; }

    /// <summary>
    /// 收藏人数。
    /// </summary>
    /// <value>返回 <typeparamref name="TCount"/>。</value>
    TCount FavoriteCount { get; set; }

    /// <summary>
    /// 转发次数。
    /// </summary>
    TCount RetweetCount { get; set; }
}
