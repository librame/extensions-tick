#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data
{
    using Resources;

    /// <summary>
    /// 抽象更新标识符（继承自抽象创建）。
    /// </summary>
    /// <typeparam name="TId">指定的标识类型。</typeparam>
    /// <typeparam name="TUpdatedBy">指定的更新者。</typeparam>
    [NotMapped]
    public abstract class AbstractUpdationIdentifier<TId, TUpdatedBy>
        : AbstractUpdationIdentifier<TId, TUpdatedBy, DateTimeOffset>
        , IUpdationIdentifier<TId, TUpdatedBy>
        where TId : IEquatable<TId>
        where TUpdatedBy : IEquatable<TUpdatedBy>
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractUpdationIdentifier{TId, TUpdatedBy}"/>。
        /// </summary>
        protected AbstractUpdationIdentifier()
        {
            UpdatedTime = CreatedTime = DateTimeExtensions.GetUtcNow();
            UpdatedTimeTicks = CreatedTimeTicks = UpdatedTime.Ticks;
        }


        /// <summary>
        /// 创建时间周期数。
        /// </summary>
        [Display(Name = nameof(CreatedTimeTicks), ResourceType = typeof(DataResource))]
        public virtual long CreatedTimeTicks { get; set; }

        /// <summary>
        /// 更新时间周期数。
        /// </summary>
        [Display(Name = nameof(UpdatedTimeTicks), ResourceType = typeof(DataResource))]
        public virtual long UpdatedTimeTicks { get; set; }


        /// <summary>
        /// 设置对象创建时间。
        /// </summary>
        /// <param name="newCreatedTime">给定的新创建时间对象。</param>
        /// <returns>返回日期与时间（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）。</returns>
        public override object SetObjectCreatedTime(object newCreatedTime)
        {
            CreatedTime = ToCreatedTime(newCreatedTime, nameof(newCreatedTime));
            CreatedTimeTicks = CreatedTime.Ticks;

            return newCreatedTime;
        }

        /// <summary>
        /// 异步设置对象创建时间。
        /// </summary>
        /// <param name="newCreatedTime">给定的新创建时间对象。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含日期与时间（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）的异步操作。</returns>
        public override ValueTask<object> SetObjectCreatedTimeAsync(object newCreatedTime,
            CancellationToken cancellationToken = default)
        {
            var createdTime = ToCreatedTime(newCreatedTime, nameof(newCreatedTime));

            return cancellationToken.RunValueTask(() =>
            {
                CreatedTime = createdTime;
                CreatedTimeTicks = CreatedTime.Ticks;

                return newCreatedTime;
            });
        }


        /// <summary>
        /// 设置对象更新时间。
        /// </summary>
        /// <param name="newUpdatedTime">给定的新更新时间对象。</param>
        /// <returns>返回日期与时间（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）。</returns>
        public override object SetObjectUpdatedTime(object newUpdatedTime)
        {
            UpdatedTime = ToCreatedTime(newUpdatedTime, nameof(newUpdatedTime));
            UpdatedTimeTicks = UpdatedTime.Ticks;

            return newUpdatedTime;
        }

        /// <summary>
        /// 异步设置对象更新时间。
        /// </summary>
        /// <param name="newUpdatedTime">给定的新更新时间对象。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含日期与时间（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）的异步操作。</returns>
        public override ValueTask<object> SetObjectUpdatedTimeAsync(object newUpdatedTime, CancellationToken cancellationToken = default)
        {
            var updatedTime = ToCreatedTime(newUpdatedTime, nameof(newUpdatedTime));

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
    /// 抽象更新标识符（继承自抽象创建）。
    /// </summary>
    /// <typeparam name="TId">指定的标识类型。</typeparam>
    /// <typeparam name="TUpdatedBy">指定的更新者。</typeparam>
    /// <typeparam name="TUpdatedTime">指定的更新时间类型（提供对 DateTime 或 DateTimeOffset 的支持）。</typeparam>
    [NotMapped]
    public abstract class AbstractUpdationIdentifier<TId, TUpdatedBy, TUpdatedTime>
        : AbstractCreationIdentifier<TId, TUpdatedBy, TUpdatedTime>, IUpdationIdentifier<TId, TUpdatedBy, TUpdatedTime>
        where TId : IEquatable<TId>
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
            => CreatedByType;

        /// <summary>
        /// 更新时间类型。
        /// </summary>
        [NotMapped]
        public virtual Type UpdatedTimeType
            => CreatedTimeType;


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
        public virtual ValueTask<object?> GetObjectUpdatedByAsync(CancellationToken cancellationToken)
            => cancellationToken.RunValueTask(() => (object?)UpdatedBy);


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
        public virtual ValueTask<object> GetObjectUpdatedTimeAsync(CancellationToken cancellationToken)
            => cancellationToken.RunValueTask(() => (object)UpdatedTime);


        /// <summary>
        /// 设置对象更新者。
        /// </summary>
        /// <param name="newUpdatedBy">给定的新更新者对象。</param>
        /// <returns>返回创建者（兼容标识或字符串）。</returns>
        public virtual object? SetObjectUpdatedBy(object? newUpdatedBy)
        {
            UpdatedBy = ToCreatedBy(newUpdatedBy, nameof(newUpdatedBy));
            return newUpdatedBy;
        }

        /// <summary>
        /// 异步设置对象更新者。
        /// </summary>
        /// <param name="newUpdatedBy">给定的新更新者对象。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含创建者（兼容标识或字符串）的异步操作。</returns>
        public virtual ValueTask<object?> SetObjectUpdatedByAsync(object? newUpdatedBy,
            CancellationToken cancellationToken = default)
        {
            var updatedBy = ToCreatedBy(newUpdatedBy, nameof(newUpdatedBy));

            return cancellationToken.RunValueTask(() =>
            {
                UpdatedBy = updatedBy;
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
            UpdatedTime = ToCreatedTime(newUpdatedTime, nameof(newUpdatedTime));
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
            var updatedTime = ToCreatedTime(newUpdatedTime, nameof(newUpdatedTime));

            return cancellationToken.RunValueTask(() =>
            {
                UpdatedTime = updatedTime;
                return newUpdatedTime;
            });
        }


        /// <summary>
        /// 转换为标识键值对字符串。
        /// </summary>
        /// <returns>返回字符串。</returns>
        public override string ToString()
            => $"{base.ToString()};{nameof(UpdatedBy)}={UpdatedBy};{nameof(UpdatedTime)}={UpdatedTime}";

    }
}
