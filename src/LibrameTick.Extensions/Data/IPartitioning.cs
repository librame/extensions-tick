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
/// 定义实现 <see cref="IObjectPartitioning"/> 的泛型分区接口。
/// </summary>
/// <typeparam name="TPartition">指定的分区类型（兼容整数等结构体的分区字段）。</typeparam>
public interface IPartitioning<TPartition> : IEquatable<IPartitioning<TPartition>>, IObjectPartitioning
    where TPartition : IEquatable<TPartition>
{
    /// <summary>
    /// 分区。
    /// </summary>
    TPartition Partition { get; set; }


    /// <summary>
    /// 转换为分区。
    /// </summary>
    /// <param name="rank">给定的分区对象。</param>
    /// <param name="paramName">给定的参数名（可选；默认为 <paramref name="rank"/> 调用参数名）。</param>
    /// <returns>返回 <typeparamref name="TPartition"/>。</returns>
    TPartition ToPartition(object rank, [CallerArgumentExpression(nameof(rank))] string? paramName = null)
        => rank.As<TPartition>(paramName);


    /// <summary>
    /// 比较相等。
    /// </summary>
    /// <param name="other">给定的 <see cref="IPartitioning{TPartition}"/>。</param>
    /// <returns>返回布尔值。</returns>
    bool IEquatable<IPartitioning<TPartition>>.Equals(IPartitioning<TPartition>? other)
        => other is not null && Partition.Equals(other.Partition);


    #region IObjectPartitioning

    /// <summary>
    /// 分区类型。
    /// </summary>
    [NotMapped]
    Type IObjectPartitioning.PartitionType => typeof(TPartition);


    /// <summary>
    /// 获取对象分区。
    /// </summary>
    /// <returns>返回分区。</returns>
    object IObjectPartitioning.GetObjectPartition()
        => Partition;

    /// <summary>
    /// 异步获取对象分区。
    /// </summary>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
    /// <returns>返回一个包含分区的异步操作。</returns>
    ValueTask<object> IObjectPartitioning.GetObjectPartitionAsync(CancellationToken cancellationToken)
        => cancellationToken.SimpleValueTaskResult(GetObjectPartition);


    /// <summary>
    /// 设置对象分区。
    /// </summary>
    /// <param name="newPartition">给定的新分区对象。</param>
    /// <returns>返回分区。</returns>
    object IObjectPartitioning.SetObjectPartition(object newPartition)
    {
        Partition = ToPartition(newPartition, nameof(newPartition));
        return newPartition;
    }

    /// <summary>
    /// 异步设置对象分区。
    /// </summary>
    /// <param name="newPartition">给定的新分区对象。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
    /// <returns>返回一个包含分区的异步操作。</returns>
    ValueTask<object> IObjectPartitioning.SetObjectPartitionAsync(object newPartition, CancellationToken cancellationToken)
    {
        var rank = ToPartition(newPartition, nameof(newPartition));

        return cancellationToken.SimpleValueTaskResult(() =>
        {
            Partition = rank;
            return newPartition;
        });
    }

    #endregion

}
