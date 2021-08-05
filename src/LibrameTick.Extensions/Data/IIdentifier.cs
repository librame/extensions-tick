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
    /// 定义泛型标识符接口。
    /// </summary>
    /// <typeparam name="TId">指定的标识类型（兼容各种引用与值类型标识）。</typeparam>
    public interface IIdentifier<TId> : IObjectIdentifier
        where TId : IEquatable<TId>
    {
        /// <summary>
        /// 标识。
        /// </summary>
        TId Id { get; set; }


        /// <summary>
        /// 转换为标识。
        /// </summary>
        /// <param name="id">给定的标识对象。</param>
        /// <param name="paramName">给定的参数名称。</param>
        /// <returns>返回 <typeparamref name="TId"/>。</returns>
        TId ToId(object? id, string? paramName);
    }
}
