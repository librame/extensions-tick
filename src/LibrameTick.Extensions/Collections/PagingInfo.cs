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

namespace Librame.Extensions.Collections
{
    /// <summary>
    /// 分页信息。
    /// </summary>
    public class PagingInfo : IPagingInfo
    {
        /// <summary>
        /// 使用页索引与页大小构造一个 <see cref="PagingInfo"/>。
        /// </summary>
        /// <param name="total">给定的总条数。</param>
        public PagingInfo(long total)
        {
            Total = total;
        }


        /// <summary>
        /// 通过索引计算分页信息。
        /// </summary>
        /// <param name="index">给定的页索引。</param>
        /// <param name="size">给定的页大小。</param>
        /// <returns>返回 <see cref="PagingInfo"/>。</returns>
        public PagingInfo ComputeByIndex(int index, int size)
        {
            Index = index < 1 ? 1 : index;
            Size = size < 1 ? 1 : size;

            // 计算跳过的条数
            if (Index > 1)
                Skip = (Index - 1) * Size;
            else
                Skip = 0; // 当前页索引小于等于1表示不跳过

            // 计算分页数
            if (Total > 0)
                Pages = Total / Size + (Total % Size > 0 ? 1 : 0);

            if (!IsPaged)
                IsPaged = true;

            return this;
        }

        /// <summary>
        /// 通过跳过的条数计算分页信息。
        /// </summary>
        /// <param name="skip">给定的跳过条数。</param>
        /// <param name="take">给定的取得条数。</param>
        /// <returns>返回 <see cref="PagingInfo"/>。</returns>
        public PagingInfo ComputeBySkip(int skip, int take)
        {
            Take = take < 1 ? 1 : take;
            Skip = skip;

            // 计算页索引
            Index = ((int)Math.Round((double)skip / take)) + 1;
            Size = take;

            // 计算分页数
            if (Total > 0)
                Pages = Total / take + (Total % take > 0 ? 1 : 0);

            if (!IsPaged)
                IsPaged = true;

            return this;
        }


        /// <summary>
        /// 总条数。
        /// </summary>
        public long Total { get; }

        /// <summary>
        /// 页数。
        /// </summary>
        public long Pages { get; private set; }

        /// <summary>
        /// 是否已分页。
        /// </summary>
        public bool IsPaged { get; private set; }


        /// <summary>
        /// 取得的条数。
        /// </summary>
        public int Take { get; private set; }

        /// <summary>
        /// 跳过的条数。
        /// </summary>
        public int Skip { get; private set; }


        /// <summary>
        /// 页索引。
        /// </summary>
        public int Index { get; private set; }

        /// <summary>
        /// 页大小。
        /// </summary>
        public int Size { get; private set; }
    }
}
