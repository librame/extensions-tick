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
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 定义抽象 <see cref="IRanking{TRank}"/>。
    /// </summary>
    /// <typeparam name="TRank">指定的排名类型（兼容整数、单双精度等结构体的排名字段）。</typeparam>
    public abstract class AbstractRanking<TRank> : IRanking<TRank>
        where TRank : IComparable<TRank>, IEquatable<TRank>
    {

#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。

        /// <summary>
        /// 排名。
        /// </summary>
        [Display(Name = nameof(Rank), GroupName = nameof(DataResource.DataGroup), ResourceType = typeof(DataResource))]
        public virtual TRank Rank { get; set; }

#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。


        /// <summary>
        /// 排名类型。
        /// </summary>
        [NotMapped]
        public virtual Type RankType
            => typeof(TRank);


        /// <summary>
        /// 比较排名。
        /// </summary>
        /// <param name="other">给定的 <see cref="IRanking{TRank}"/>。</param>
        /// <returns>返回 32 位整数。</returns>
        public virtual int CompareTo(IRanking<TRank>? other)
            => other != null ? Rank.CompareTo(other.Rank) : -1;

        /// <summary>
        /// 比较相等。
        /// </summary>
        /// <param name="other">给定的 <see cref="IRanking{TRank}"/>。</param>
        /// <returns>返回布尔值。</returns>
        public virtual bool Equals(IRanking<TRank>? other)
            => other != null && Rank.Equals(other.Rank);


        /// <summary>
        /// 转换为排名。
        /// </summary>
        /// <param name="rank">给定的排名对象。</param>
        /// <param name="paramName">给定的参数名称。</param>
        /// <returns>返回 <typeparamref name="TRank"/>。</returns>
        public virtual TRank ToRank(object rank, string? paramName)
            => rank.AsNotNull<TRank>(paramName);


        /// <summary>
        /// 获取对象排名。
        /// </summary>
        /// <returns>返回排名（兼容整数、单双精度的排名字段）。</returns>
        public virtual object GetObjectRank()
            => Rank;

        /// <summary>
        /// 异步获取对象排名。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含排名（兼容整数、单双精度的排名字段）的异步操作。</returns>
        public virtual ValueTask<object> GetObjectRankAsync(CancellationToken cancellationToken)
            => cancellationToken.RunValueTask(GetObjectRank);


        /// <summary>
        /// 设置对象排名。
        /// </summary>
        /// <param name="newRank">给定的新排名对象。</param>
        /// <returns>返回排名（兼容整数、单双精度的排名字段）。</returns>
        public virtual object SetObjectRank(object newRank)
        {
            Rank = ToRank(newRank, nameof(newRank));
            return newRank;
        }

        /// <summary>
        /// 异步设置对象排名。
        /// </summary>
        /// <param name="newRank">给定的新排名对象。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含排名（兼容整数、单双精度的排名字段）的异步操作。</returns>
        public virtual ValueTask<object> SetObjectRankAsync(object newRank, CancellationToken cancellationToken = default)
        {
            var rank = ToRank(newRank, nameof(newRank));

            return cancellationToken.RunValueTask(() =>
            {
                Rank = rank;
                return newRank;
            });
        }

    }
}
