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

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 创建时间接口。
    /// </summary>
    /// <typeparam name="TCreatedTime">指定的创建时间类型（提供对 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/> 的支持）。</typeparam>
    public interface ICreationTime<TCreatedTime> : IObjectCreationTime
        where TCreatedTime : struct
    {
        /// <summary>
        /// 创建时间。
        /// </summary>
        TCreatedTime CreatedTime { get; set; }


        /// <summary>
        /// 转换为创建时间。
        /// </summary>
        /// <param name="createdTime">给定的创建时间对象。</param>
        /// <param name="paramName">给定的参数名称。</param>
        /// <returns>返回 <typeparamref name="TCreatedTime"/>。</returns>
        TCreatedTime ToCreatedTime(object createdTime, string? paramName);
    }
}
