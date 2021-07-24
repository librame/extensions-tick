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
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 扩展选项接口。
    /// </summary>
    public interface IExtensionOptions : IExtensionInfo
    {
        /// <summary>
        /// 目录选项。
        /// </summary>
        DirectoryOptions Directories { get; }

        /// <summary>
        /// 替换服务字典集合。
        /// </summary>
        [JsonIgnore]
        IDictionary<Type, Type> ReplacedServices { get; }

        /// <summary>
        /// 服务特征集合。
        /// </summary>
        [JsonIgnore]
        ServiceCharacteristicCollection ServiceCharacteristics { get; }

        /// <summary>
        /// 父级选项。
        /// </summary>
        [JsonIgnore]
        IExtensionOptions? ParentOptions { get; }
    }
}
