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
/// 定义泛型创建时间接口。
/// </summary>
/// <typeparam name="TCreatedTime">指定的创建时间类型（提供对 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/> 的支持）。</typeparam>
public interface ICreationTime<TCreatedTime> : IEquatable<ICreationTime<TCreatedTime>>, IObjectCreationTime
    where TCreatedTime : IEquatable<TCreatedTime>
{
    /// <summary>
    /// 创建时间。
    /// </summary>
    TCreatedTime CreatedTime { get; set; }


    /// <summary>
    /// 转换为创建时间。
    /// </summary>
    /// <param name="createdTime">给定的创建时间对象。</param>
    /// <param name="paramName">给定的参数名（可选；默认为 <paramref name="createdTime"/> 调用参数名）。</param>
    /// <returns>返回 <typeparamref name="TCreatedTime"/>。</returns>
    TCreatedTime ToCreatedTime(object createdTime, [CallerArgumentExpression(nameof(createdTime))] string? paramName = null)
        => createdTime.As<TCreatedTime>(paramName);


    /// <summary>
    /// 比较相等。
    /// </summary>
    /// <param name="other">给定的 <see cref="ICreationTime{TCreatedTime}"/>。</param>
    /// <returns>返回布尔值。</returns>
    bool IEquatable<ICreationTime<TCreatedTime>>.Equals(ICreationTime<TCreatedTime>? other)
        => other is not null && CreatedTime.Equals(other.CreatedTime);


    #region IObjectCreationTime

    /// <summary>
    /// 创建时间类型。
    /// </summary>
    [NotMapped]
    Type IObjectCreationTime.CreatedTimeType
        => typeof(TCreatedTime);


    /// <summary>
    /// 获取对象创建时间。
    /// </summary>
    /// <returns>返回日期与时间（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）。</returns>
    object IObjectCreationTime.GetObjectCreatedTime()
        => CreatedTime;

    /// <summary>
    /// 异步获取对象创建时间。
    /// </summary>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
    /// <returns>返回一个包含日期与时间（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）的异步操作。</returns>
    ValueTask<object> IObjectCreationTime.GetObjectCreatedTimeAsync(CancellationToken cancellationToken)
        => cancellationToken.SimpleValueTask(GetObjectCreatedTime);


    /// <summary>
    /// 设置对象创建时间。
    /// </summary>
    /// <param name="newCreatedTime">给定的新创建时间对象。</param>
    /// <returns>返回日期与时间（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）。</returns>
    object IObjectCreationTime.SetObjectCreatedTime(object newCreatedTime)
    {
        CreatedTime = ToCreatedTime(newCreatedTime, nameof(newCreatedTime));
        return newCreatedTime;
    }

    /// <summary>
    /// 异步设置对象创建时间。
    /// </summary>
    /// <param name="newCreatedTime">给定的新创建时间对象。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
    /// <returns>返回一个包含日期与时间（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）的异步操作。</returns>
    ValueTask<object> IObjectCreationTime.SetObjectCreatedTimeAsync(object newCreatedTime, CancellationToken cancellationToken)
    {
        var createdTime = ToCreatedTime(newCreatedTime, nameof(newCreatedTime));

        return cancellationToken.SimpleValueTask(() =>
        {
            CreatedTime = createdTime;
            return newCreatedTime;
        });
    }

    #endregion

}
