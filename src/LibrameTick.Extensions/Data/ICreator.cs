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
    /// 定义泛型创建者接口。
    /// </summary>
    /// <typeparam name="TCreatedBy">指定的创建者类型（提供对整数、字符串、GUID 等类型的支持）。</typeparam>
    public interface ICreator<TCreatedBy> : IObjectCreator
        where TCreatedBy : IEquatable<TCreatedBy>
    {
        /// <summary>
        /// 创建者。
        /// </summary>
        TCreatedBy? CreatedBy { get; set; }


        /// <summary>
        /// 转换为创建者。
        /// </summary>
        /// <param name="createdBy">给定的创建者对象。</param>
        /// <param name="paramName">给定的参数名称。</param>
        /// <returns>返回 <typeparamref name="TCreatedBy"/>。</returns>
        TCreatedBy ToCreatedBy(object? createdBy, string? paramName);
    }
}
