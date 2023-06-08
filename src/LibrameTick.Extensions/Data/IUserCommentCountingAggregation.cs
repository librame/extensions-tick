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
/// 定义实现 <see cref="IUserCountingAggregation{TAggreg, TCount}"/> 的泛型用户评论计数聚合接口。
/// </summary>
/// <typeparam name="TAggreg">指定的计数聚合类型。</typeparam>
/// <typeparam name="TCount">指定的计数类型。</typeparam>
public interface IUserCommentCountingAggregation<TAggreg, TCount> : IUserCountingAggregation<TAggreg, TCount>
    where TAggreg : class, IUserCommentCountingAggregation<TAggreg, TCount>
    where TCount : IAdditionOperators<TCount, TCount, TCount>, ISubtractionOperators<TCount, TCount, TCount>
        , IDecrementOperators<TCount>, IIncrementOperators<TCount>
{
    /// <summary>
    /// 评论条数。
    /// </summary>
    TCount CommentCount { get; set; }

    /// <summary>
    /// 评论人数。
    /// </summary>
    TCount CommenterCount { get; set; }
}
