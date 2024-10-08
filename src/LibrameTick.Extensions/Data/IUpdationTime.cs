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
/// 定义泛型更新时间接口。
/// </summary>
/// <typeparam name="TUpdatedTime">指定的更新时间类型（提供对 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/> 的支持）。</typeparam>
public interface IUpdationTime<TUpdatedTime> : IEquatable<IUpdationTime<TUpdatedTime>>, IObjectUpdationTime
    where TUpdatedTime : IEquatable<TUpdatedTime>
{
    /// <summary>
    /// 更新时间。
    /// </summary>
    TUpdatedTime UpdatedTime { get; set; }


    /// <summary>
    /// 转换为更新时间。
    /// </summary>
    /// <param name="updatedTime">给定的更新时间对象。</param>
    /// <param name="paramName">给定的参数名（可选；默认为 <paramref name="updatedTime"/> 调用参数名）。</param>
    /// <returns>返回 <typeparamref name="TUpdatedTime"/>。</returns>
    TUpdatedTime ToUpdatedTime(object updatedTime, [CallerArgumentExpression(nameof(updatedTime))] string? paramName = null)
        => updatedTime.As<TUpdatedTime>(paramName);


    /// <summary>
    /// 比较相等。
    /// </summary>
    /// <param name="other">给定的 <see cref="IUpdationTime{TUpdatedTime}"/>。</param>
    /// <returns>返回布尔值。</returns>
    bool IEquatable<IUpdationTime<TUpdatedTime>>.Equals(IUpdationTime<TUpdatedTime>? other)
        => other is not null && UpdatedTime.Equals(other.UpdatedTime);


    #region IObjectUpdationTime

    /// <summary>
    /// 更新时间类型。
    /// </summary>
    [NotMapped]
    Type IObjectUpdationTime.UpdatedTimeType
        => typeof(TUpdatedTime);


    /// <summary>
    /// 获取对象更新时间。
    /// </summary>
    /// <returns>返回日期与时间（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）。</returns>
    object IObjectUpdationTime.GetObjectUpdatedTime()
        => UpdatedTime;

    /// <summary>
    /// 异步获取对象更新时间。
    /// </summary>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
    /// <returns>返回一个包含日期与时间（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）的异步操作。</returns>
    ValueTask<object> IObjectUpdationTime.GetObjectUpdatedTimeAsync(CancellationToken cancellationToken)
        => cancellationToken.SimpleValueTaskResult(GetObjectUpdatedTime);


    /// <summary>
    /// 设置对象更新时间。
    /// </summary>
    /// <param name="newUpdatedTime">给定的新更新时间对象。</param>
    /// <returns>返回日期与时间（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）。</returns>
    object IObjectUpdationTime.SetObjectUpdatedTime(object newUpdatedTime)
    {
        UpdatedTime = ToUpdatedTime(newUpdatedTime, nameof(newUpdatedTime));
        return newUpdatedTime;
    }

    /// <summary>
    /// 异步设置对象更新时间。
    /// </summary>
    /// <param name="newUpdatedTime">给定的新更新时间对象。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
    /// <returns>返回一个包含日期与时间（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）的异步操作。</returns>
    ValueTask<object> IObjectUpdationTime.SetObjectUpdatedTimeAsync(object newUpdatedTime, CancellationToken cancellationToken)
    {
        var realNewUpdatedTime = ToUpdatedTime(newUpdatedTime, nameof(newUpdatedTime));

        return cancellationToken.SimpleValueTaskResult(() =>
        {
            UpdatedTime = realNewUpdatedTime;
            return newUpdatedTime;
        });
    }

    #endregion

}
