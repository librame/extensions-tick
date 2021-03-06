﻿#region License

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
    /// 抽象排名。
    /// </summary>
    /// <typeparam name="TRank">指定的排名类型（兼容整数、单双精度等结构体的排名字段）。</typeparam>
    public abstract class AbstractRanking<TRank> : IRanking<TRank>
        where TRank : struct
    {
        /// <summary>
        /// 排名。
        /// </summary>
        [Display(Name = nameof(Rank), GroupName = nameof(DataResource.DataGroup), ResourceType = typeof(DataResource))]
        public virtual TRank Rank { get; set; }


        /// <summary>
        /// 排名类型。
        /// </summary>
        [NotMapped]
        public virtual Type RankType
            => typeof(TRank);


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
