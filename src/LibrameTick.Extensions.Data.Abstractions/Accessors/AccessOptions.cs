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
    using Core;

    /// <summary>
    /// 访问选项。
    /// </summary>
    public class AccessOptions
    {
        /// <summary>
        /// 构造一个 <see cref="AccessOptions"/>。
        /// </summary>
        /// <param name="notifyProperty">给定的 <see cref="INotifyProperty"/>。</param>
        public AccessOptions(INotifyProperty notifyProperty)
        {
            NotifyProperty = notifyProperty;
        }


        /// <summary>
        /// 通知属性。
        /// </summary>
        protected INotifyProperty NotifyProperty { get; init; }


        /// <summary>
        /// 多访问器访问关系（默认使用 <see cref="AccessorsRelationship.Slicing"/>）。
        /// </summary>
        public AccessorsRelationship Relationship { get; set; }
            = AccessorsRelationship.Slicing;

        /// <summary>
        /// 默认切片方法（当多访问器访问关系设定为 <see cref="AccessorsRelationship.Slicing"/> 访问时有效，详情参见 <see cref="IAccessorManager.CustomSliceFunc"/>）。
        /// </summary>
        public Func<IAccessor, bool>? DefaultSliceFunc { get; set; }
    }
}
