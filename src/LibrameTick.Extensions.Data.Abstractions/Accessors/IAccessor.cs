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
    /// 定义表示数据访问的访问器接口（主要用于适配数据实现层的访问对象；如 EFCore 实现层的 DbContext 对象）。
    /// </summary>
    public interface IAccessor : ISortable
    {
        /// <summary>
        /// 访问器类型。
        /// </summary>
        Type AccessorType { get; }
    }
}
