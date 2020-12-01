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
    /// 扩展信息接口。
    /// </summary>
    /// <typeparam name="TInfo">指定的扩展信息类型。</typeparam>
    public interface IExtensionInfo<TInfo> : IExtensionInfo
        where TInfo : IExtensionInfo
    {
        /// <summary>
        /// 父级。
        /// </summary>
        new TInfo? Parent { get; }
    }
}
