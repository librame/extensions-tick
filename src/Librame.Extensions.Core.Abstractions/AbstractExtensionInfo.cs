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

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 抽象扩展信息（抽象实现 <see cref="IExtensionInfo"/>）。
    /// </summary>
    public abstract class AbstractExtensionInfo : IExtensionInfo
    {
        /// <summary>
        /// 当前类型。
        /// </summary>
        public virtual Type CurrentType
            => GetType();

        /// <summary>
        /// 名称。
        /// </summary>
        public virtual string Name
            => CurrentType.Name;
    }
}
