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
/// 定义对象分区接口。
/// </summary>
public interface IObjectPartitioning
{
    /// <summary>
    /// 分区类型。
    /// </summary>
    Type PartitionType { get; }


    /// <summary>
    /// 获取对象分区。
    /// </summary>
    /// <returns>返回分区（兼容整数、单双精度的排序字段）。</returns>
    object GetObjectPartition();

    /// <summary>
    /// 异步获取对象分区。
    /// </summary>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含分区（兼容整数、单双精度的排序字段）的异步操作。</returns>
    ValueTask<object> GetObjectPartitionAsync(CancellationToken cancellationToken = default);


    /// <summary>
    /// 设置对象分区。
    /// </summary>
    /// <param name="newPartition">给定的新分区对象。</param>
    /// <returns>返回分区（兼容整数、单双精度的排序字段）。</returns>
    object SetObjectPartition(object newPartition);

    /// <summary>
    /// 异步设置对象分区。
    /// </summary>
    /// <param name="newPartition">给定的新分区对象。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含分区（兼容整数、单双精度的排序字段）的异步操作。</returns>
    ValueTask<object> SetObjectPartitionAsync(object newPartition, CancellationToken cancellationToken = default);


    /// <summary>
    /// 设置对象分区。
    /// </summary>
    /// <param name="newPartitionFactory">给定的新分区对象工厂方法。</param>
    /// <returns>返回分区（兼容整数、单双精度的排序字段）。</returns>
    object SetObjectPartition(Func<object, object> newPartitionFactory)
    {
        var currentPartition = GetObjectPartition();

        return SetObjectPartition(newPartitionFactory(currentPartition));
    }

    /// <summary>
    /// 异步设置对象分区。
    /// </summary>
    /// <param name="newPartitionFactory">给定的新分区对象工厂方法。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含分区（兼容整数、单双精度的排序字段）的异步操作。</returns>
    async ValueTask<object> SetObjectPartitionAsync(Func<object, object> newPartitionFactory,
        CancellationToken cancellationToken = default)
    {
        var currentPartition = await GetObjectPartitionAsync(cancellationToken).DiscontinueCapturedContext();

        return await SetObjectPartitionAsync(newPartitionFactory(currentPartition), cancellationToken)
            .DiscontinueCapturedContext();
    }

}
