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

namespace Librame.Extensions.Data.Accessors
{
    /// <summary>
    /// <see cref="IAccessor"/> 管理器。
    /// </summary>
    public interface IAccessorManager
    {
        /// <summary>
        /// 自定义访问器切片方法（可选；仅在 <see cref="AccessOptions.Relationship"/> 设定为 <see cref="AccessorsRelationship.Slicing"/> 时有效）。
        /// </summary>
        Func<IAccessor, bool>? CustomSliceFunc { get; set; }


        /// <summary>
        /// 获取读取访问器。
        /// </summary>
        /// <returns>返回 <see cref="IAccessor"/>。</returns>
        IAccessor GetReadAccessor();

        /// <summary>
        /// 获取写入访问器。
        /// </summary>
        /// <returns>返回 <see cref="IAccessor"/>。</returns>
        IAccessor GetWriteAccessor();
    }
}
