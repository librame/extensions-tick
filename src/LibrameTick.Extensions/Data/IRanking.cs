#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data;

/// <summary>
/// 定义实现 <see cref="IObjectRanking"/> 的泛型排名接口。
/// </summary>
/// <typeparam name="TRank">指定的排序类型（兼容整数、单双精度等结构体的排序字段）。</typeparam>
public interface IRanking<TRank> : IComparable<IRanking<TRank>>, IEquatable<IRanking<TRank>>, IObjectRanking
    where TRank : IComparable<TRank>, IEquatable<TRank>
{
    /// <summary>
    /// 排名。
    /// </summary>
    TRank Rank { get; set; }


    /// <summary>
    /// 转换为排名。
    /// </summary>
    /// <param name="rank">给定的排名对象。</param>
    /// <param name="paramName">给定的参数名称。</param>
    /// <returns>返回 <typeparamref name="TRank"/>。</returns>
    TRank ToRank(object rank, [CallerArgumentExpression(nameof(rank))] string? paramName = null)
        => rank.As<TRank>(paramName);


    /// <summary>
    /// 比较排名。
    /// </summary>
    /// <param name="other">给定的 <see cref="IRanking{TRank}"/>。</param>
    /// <returns>返回 32 位整数。</returns>
    int IComparable<IRanking<TRank>>.CompareTo(IRanking<TRank>? other)
        => other is not null ? Rank.CompareTo(other.Rank) : -1;

    /// <summary>
    /// 比较相等。
    /// </summary>
    /// <param name="other">给定的 <see cref="IRanking{TRank}"/>。</param>
    /// <returns>返回布尔值。</returns>
    bool IEquatable<IRanking<TRank>>.Equals(IRanking<TRank>? other)
        => other is not null && Rank.Equals(other.Rank);


    #region IObjectRanking

    /// <summary>
    /// 排名类型。
    /// </summary>
    [NotMapped]
    Type IObjectRanking.RankType
        => typeof(TRank);


    /// <summary>
    /// 获取对象排名。
    /// </summary>
    /// <returns>返回排名（兼容整数、单双精度的排名字段）。</returns>
    object IObjectRanking.GetObjectRank()
        => Rank;

    /// <summary>
    /// 异步获取对象排名。
    /// </summary>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
    /// <returns>返回一个包含排名（兼容整数、单双精度的排名字段）的异步操作。</returns>
    ValueTask<object> IObjectRanking.GetObjectRankAsync(CancellationToken cancellationToken)
        => cancellationToken.SimpleValueTask(GetObjectRank);


    /// <summary>
    /// 设置对象排名。
    /// </summary>
    /// <param name="newRank">给定的新排名对象。</param>
    /// <returns>返回排名（兼容整数、单双精度的排名字段）。</returns>
    object IObjectRanking.SetObjectRank(object newRank)
    {
        Rank = ToRank(newRank, nameof(newRank));
        return newRank;
    }

    /// <summary>
    /// 异步设置对象排名。
    /// </summary>
    /// <param name="newRank">给定的新排名对象。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
    /// <returns>返回一个包含排名（兼容整数、单双精度的排名字段）的异步操作。</returns>
    ValueTask<object> IObjectRanking.SetObjectRankAsync(object newRank, CancellationToken cancellationToken)
    {
        var rank = ToRank(newRank, nameof(newRank));

        return cancellationToken.SimpleValueTask(() =>
        {
            Rank = rank;
            return newRank;
        });
    }

    #endregion

}
