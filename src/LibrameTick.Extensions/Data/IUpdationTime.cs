#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 定义泛型更新时间接口。
    /// </summary>
    /// <typeparam name="TUpdatedTime">指定的更新时间类型（提供对 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/> 的支持）。</typeparam>
    public interface IUpdationTime<TUpdatedTime> : ICreationTime<TUpdatedTime>, IObjectUpdationTime
        where TUpdatedTime : struct
    {
        /// <summary>
        /// 更新时间。
        /// </summary>
        TUpdatedTime UpdatedTime { get; set; }
    }
}
