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
    /// 扩展信息接口。
    /// </summary>
    public interface IExtensionInfo
    {
        /// <summary>
        /// 当前类型。
        /// </summary>
        Type CurrentType { get; }

        /// <summary>
        /// 名称。
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 父级。
        /// </summary>
        IExtensionInfo? Parent { get; }
    }
}
