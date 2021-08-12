#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 定义扩展信息接口（<see cref="IExtensionOptions"/>、<see cref="IExtensionBuilder"/> 的公共基础扩展接口）。
    /// </summary>
    public interface IExtensionInfo
    {
        /// <summary>
        /// 信息类型。
        /// </summary>
        Type InfoType { get; }

        /// <summary>
        /// 名称。
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 父级信息。
        /// </summary>
        IExtensionInfo? ParentInfo { get; }
    }
}
