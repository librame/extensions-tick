#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Resources;

namespace Librame.Extensions.Data;

/// <summary>
/// 定义抽象实现 <see cref="IUpdation{TUpdatedBy}"/>。
/// </summary>
/// <typeparam name="TUpdatedBy">指定的更新者。</typeparam>
[NotMapped]
public abstract class AbstractUpdation<TUpdatedBy>
    : AbstractUpdation<TUpdatedBy, DateTimeOffset>, IUpdation<TUpdatedBy>
    where TUpdatedBy : IEquatable<TUpdatedBy>
{
    /// <summary>
    /// 构造一个 <see cref="AbstractUpdation{TId, TUpdatedBy}"/>。
    /// </summary>
    protected AbstractUpdation()
    {
        UpdatedTime = DateTimeOffset.UtcNow;
        UpdatedTimeTicks = UpdatedTime.Ticks;
    }


    /// <summary>
    /// 更新时间周期数。
    /// </summary>
    [Display(Name = nameof(UpdatedTimeTicks), ResourceType = typeof(DataResource))]
    public virtual long UpdatedTimeTicks { get; set; }


    /// <summary>
    /// 设置对象更新时间。
    /// </summary>
    /// <param name="newUpdatedTime">给定的新更新时间对象。</param>
    /// <returns>返回日期与时间（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）。</returns>
    public override object SetObjectUpdatedTime(object newUpdatedTime)
    {
        UpdatedTime = ToUpdatedTime(newUpdatedTime, nameof(newUpdatedTime));
        UpdatedTimeTicks = UpdatedTime.Ticks;

        return newUpdatedTime;
    }

    /// <summary>
    /// 异步设置对象更新时间。
    /// </summary>
    /// <param name="newUpdatedTime">给定的新更新时间对象。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含日期与时间（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）的异步操作。</returns>
    public override ValueTask<object> SetObjectUpdatedTimeAsync(object newUpdatedTime,
        CancellationToken cancellationToken = default)
    {
        var updatedTime = ToUpdatedTime(newUpdatedTime, nameof(newUpdatedTime));

        return cancellationToken.RunValueTask(() =>
        {
            UpdatedTime = updatedTime;
            UpdatedTimeTicks = UpdatedTime.Ticks;

            return newUpdatedTime;
        });
    }


    /// <summary>
    /// 转换为标识键值对字符串。
    /// </summary>
    /// <returns>返回字符串。</returns>
    public override string ToString()
        => $"{base.ToString()};{nameof(UpdatedTimeTicks)}={UpdatedTimeTicks}";

}


/// <summary>
/// 定义抽象实现 <see cref="IUpdation{TUpdatedBy, TUpdatedTime}"/>。
/// </summary>
/// <typeparam name="TUpdatedBy">指定的更新者。</typeparam>
/// <typeparam name="TUpdatedTime">指定的更新时间类型（提供对 DateTime 或 DateTimeOffset 的支持）。</typeparam>
[NotMapped]
public abstract class AbstractUpdation<TUpdatedBy, TUpdatedTime> : IUpdation<TUpdatedBy, TUpdatedTime>
    where TUpdatedBy : IEquatable<TUpdatedBy>
    where TUpdatedTime : struct
{
    /// <summary>
    /// 更新者。
    /// </summary>
    [Display(Name = nameof(UpdatedBy), ResourceType = typeof(DataResource))]
    public virtual TUpdatedBy? UpdatedBy { get; set; }

    /// <summary>
    /// 更新时间。
    /// </summary>
    [Display(Name = nameof(UpdatedTime), ResourceType = typeof(DataResource))]
    public virtual TUpdatedTime UpdatedTime { get; set; }


    /// <summary>
    /// 更新者类型。
    /// </summary>
    [NotMapped]
    public virtual Type UpdatedByType
        => typeof(TUpdatedBy);

    /// <summary>
    /// 更新时间类型。
    /// </summary>
    [NotMapped]
    public virtual Type UpdatedTimeType
        => typeof(TUpdatedTime);


    /// <summary>
    /// 比较更新者相等。
    /// </summary>
    /// <param name="other">给定的 <see cref="IUpdator{TUpdatedBy}"/>。</param>
    /// <returns>返回布尔值。</returns>
    public virtual bool Equals(IUpdator<TUpdatedBy>? other)
        => other is not null && UpdatedBy is not null && UpdatedBy.Equals(other.UpdatedBy);

    /// <summary>
    /// 比较更新时间相等。
    /// </summary>
    /// <param name="other">给定的 <see cref="IUpdationTime{TUpdatedTime}"/>。</param>
    /// <returns>返回布尔值。</returns>
    public virtual bool Equals(IUpdationTime<TUpdatedTime>? other)
        => other is not null && UpdatedTime.Equals(other.UpdatedTime);


    /// <summary>
    /// 转换为更新者。
    /// </summary>
    /// <param name="updatedBy">给定的更新者对象。</param>
    /// <param name="paramName">给定的参数名（可选；默认为 <paramref name="updatedBy"/> 调用参数名）。</param>
    /// <returns>返回 <typeparamref name="TUpdatedBy"/>。</returns>
    public virtual TUpdatedBy ToUpdatedBy(object? updatedBy,
        [CallerArgumentExpression("updatedBy")] string? paramName = null)
        => updatedBy.As<TUpdatedBy>(paramName);

    /// <summary>
    /// 转换为更新时间。
    /// </summary>
    /// <param name="updatedTime">给定的更新时间对象。</param>
    /// <param name="paramName">给定的参数名（可选；默认为 <paramref name="updatedTime"/> 调用参数名）。</param>
    /// <returns>返回 <typeparamref name="TUpdatedTime"/>。</returns>
    public virtual TUpdatedTime ToUpdatedTime(object updatedTime,
        [CallerArgumentExpression("updatedTime")] string? paramName = null)
        => updatedTime.As<TUpdatedTime>(paramName);


    /// <summary>
    /// 获取对象更新者。
    /// </summary>
    /// <returns>返回更新者（兼容标识或字符串）。</returns>
    public virtual object? GetObjectUpdatedBy()
        => UpdatedBy;

    /// <summary>
    /// 异步获取对象更新者。
    /// </summary>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含更新者（兼容标识或字符串）的异步操作。</returns>
    public virtual ValueTask<object?> GetObjectUpdatedByAsync(CancellationToken cancellationToken = default)
        => cancellationToken.RunValueTask(GetObjectUpdatedBy);


    /// <summary>
    /// 获取对象更新时间。
    /// </summary>
    /// <returns>返回日期与时间（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）。</returns>
    public virtual object GetObjectUpdatedTime()
        => UpdatedTime;

    /// <summary>
    /// 异步获取对象更新时间。
    /// </summary>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含日期与时间（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）的异步操作。</returns>
    public virtual ValueTask<object> GetObjectUpdatedTimeAsync(CancellationToken cancellationToken = default)
        => cancellationToken.RunValueTask(GetObjectUpdatedTime);


    /// <summary>
    /// 设置对象更新者。
    /// </summary>
    /// <param name="newUpdatedBy">给定的新更新者对象。</param>
    /// <returns>返回更新者（兼容标识或字符串）。</returns>
    public virtual object? SetObjectUpdatedBy(object? newUpdatedBy)
    {
        UpdatedBy = ToUpdatedBy(newUpdatedBy, nameof(newUpdatedBy));
        return newUpdatedBy;
    }

    /// <summary>
    /// 异步设置对象更新者。
    /// </summary>
    /// <param name="newUpdatedBy">给定的新更新者对象。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含更新者（兼容标识或字符串）的异步操作。</returns>
    public virtual ValueTask<object?> SetObjectUpdatedByAsync(object? newUpdatedBy,
        CancellationToken cancellationToken = default)
    {
        var realNewUpdatedBy = ToUpdatedBy(newUpdatedBy, nameof(newUpdatedBy));

        return cancellationToken.RunValueTask(() =>
        {
            UpdatedBy = realNewUpdatedBy;
            return newUpdatedBy;
        });
    }


    /// <summary>
    /// 设置对象更新时间。
    /// </summary>
    /// <param name="newUpdatedTime">给定的新更新时间对象。</param>
    /// <returns>返回日期与时间（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）。</returns>
    public virtual object SetObjectUpdatedTime(object newUpdatedTime)
    {
        UpdatedTime = ToUpdatedTime(newUpdatedTime, nameof(newUpdatedTime));
        return newUpdatedTime;
    }

    /// <summary>
    /// 异步设置对象更新时间。
    /// </summary>
    /// <param name="newUpdatedTime">给定的新更新时间对象。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含日期与时间（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）的异步操作。</returns>
    public virtual ValueTask<object> SetObjectUpdatedTimeAsync(object newUpdatedTime,
        CancellationToken cancellationToken = default)
    {
        var realNewUpdatedTime = ToUpdatedTime(newUpdatedTime, nameof(newUpdatedTime));

        return cancellationToken.RunValueTask(() =>
        {
            UpdatedTime = realNewUpdatedTime;
            return newUpdatedTime;
        });
    }


    /// <summary>
    /// 转换为标识键值对字符串。
    /// </summary>
    /// <returns>返回字符串。</returns>
    public override string ToString()
        => $"{nameof(UpdatedBy)}={UpdatedBy};{nameof(UpdatedTime)}={UpdatedTime}";

}
