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
    /// 定义泛型排名接口。
    /// </summary>
    /// <typeparam name="TRank">指定的排序类型（兼容整数、单双精度等结构体的排序字段）。</typeparam>
    public interface IRanking<TRank> : IObjectRanking
        where TRank : struct
    {
        /// <summary>
        /// 排名。
        /// </summary>
        TRank Rank { get; set; }


        /// <summary>
        /// 转换为排名。
        /// </summary>
        /// <param name="rank">给定的排名对象。</param>
        /// <param name="paramName">给定的参数名称。</param>
        /// <returns>返回 <typeparamref name="TRank"/>。</returns>
        TRank ToRank(object rank, string? paramName);
    }
}
