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

namespace Librame.Extensions.Data.Specification
{
    /// <summary>
    /// 定义一个实现 <see cref="IPublicationTime{TPublishedTime}"/> 接口类型的降序排列规约。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    /// <typeparam name="TPublishedTime">指定的发表时间类型（提供对 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/> 的支持）。</typeparam>
    public class OrderByPublicationTimeSpecification<T, TPublishedTime> : BaseSpecification<T>
        where T : class, IPublicationTime<TPublishedTime>
        where TPublishedTime : struct
    {
        /// <summary>
        /// 构造一个 <see cref="OrderByPublicationTimeSpecification{T, TPublishedTime}"/>。
        /// </summary>
        public OrderByPublicationTimeSpecification()
        {
            SetOrderByDescending(s => s.PublishedTime);
        }

    }
}
