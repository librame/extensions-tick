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
using System.Collections.Generic;
using System.Linq;

namespace Librame.Extensions.Collections
{
    /// <summary>
    /// 定义分页列表接口。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    public interface IPagingList<T> : IEnumerable<T>
    {
        /// <summary>
        /// 分页信息（需要先分页后才有效）。
        /// </summary>
        PagingInfo Info { get; }


        /// <summary>
        /// 筛选列表（通常用于内存分页）。
        /// </summary>
        /// <param name="func">给定的筛选方法。</param>
        void Filtrate(Func<IEnumerable<T>, IEnumerable<T>> func);

        /// <summary>
        /// 筛选列表（通常用于动态分页）。
        /// </summary>
        /// <param name="func">给定的筛选方法。</param>
        void Filtrate(Func<IQueryable<T>, IQueryable<T>> func);


        /// <summary>
        /// 通过页索引进行分页。
        /// </summary>
        /// <param name="index">给定的页索引。</param>
        /// <param name="size">给定的页大小。</param>
        /// <returns>返回 <see cref="PagingInfo"/>。</returns>
        PagingInfo PageByIndex(int index, int size);

        /// <summary>
        /// 通过跳过的条数进行分页。
        /// </summary>
        /// <param name="skip">给定的跳过条数。</param>
        /// <param name="take">给定的取得条数。</param>
        /// <returns>返回 <see cref="PagingInfo"/>。</returns>
        PagingInfo PageBySkip(int skip, int take);
    }
}
