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
    /// 定义泛型更新者接口。
    /// </summary>
    /// <typeparam name="TUpdatedBy">指定的更新者类型（提供对整数、字符串、GUID 等类型的支持）。</typeparam>
    public interface IUpdator<TUpdatedBy> : ICreator<TUpdatedBy>, IObjectUpdator
        where TUpdatedBy : IEquatable<TUpdatedBy>
    {
        /// <summary>
        /// 更新者。
        /// </summary>
        TUpdatedBy? UpdatedBy { get; set; }
    }
}
