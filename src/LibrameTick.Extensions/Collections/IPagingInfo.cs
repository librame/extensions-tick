#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Collections
{
    /// <summary>
    /// 分页信息接口。
    /// </summary>
    public interface IPagingInfo
    {
        /// <summary>
        /// 总条数。
        /// </summary>
        long Total { get; }

        /// <summary>
        /// 页数。
        /// </summary>
        long Pages { get; }

        /// <summary>
        /// 是否已分页。
        /// </summary>
        bool IsPaged { get; }


        /// <summary>
        /// 取得的条数。
        /// </summary>
        int Take { get; }

        /// <summary>
        /// 跳过的条数。
        /// </summary>
        int Skip { get; }


        /// <summary>
        /// 页大小。
        /// </summary>
        int Size { get; }

        /// <summary>
        /// 页索引。
        /// </summary>
        int Index { get; }
    }
}
