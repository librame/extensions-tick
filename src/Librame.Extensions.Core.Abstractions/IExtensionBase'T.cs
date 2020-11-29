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
    /// 扩展基础接口。
    /// </summary>
    /// <typeparam name="T">指定的扩展类型。</typeparam>
    public interface IExtensionBase<T> : IExtensionBase
        where T : IExtensionBase
    {
        /// <summary>
        /// 父级。
        /// </summary>
        T? Parent { get; }
    }
}
