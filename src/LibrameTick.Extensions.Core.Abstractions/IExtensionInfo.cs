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
using System.Text.Json.Serialization;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 扩展信息接口（<see cref="IExtensionOptions"/>、<see cref="IExtensionBuilder"/> 的公共基础扩展接口）。
    /// </summary>
    public interface IExtensionInfo : INotifyProperty
    {
        /// <summary>
        /// 当前类型。
        /// </summary>
        [JsonIgnore]
        Type CurrentType { get; }

        /// <summary>
        /// 名称。
        /// </summary>
        [JsonIgnore]
        string Name { get; }

        /// <summary>
        /// 父级。
        /// </summary>
        [JsonIgnore]
        IExtensionInfo? Parent { get; }
    }
}
