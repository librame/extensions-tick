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
    /// 抽象扩展选项（抽象实现 <see cref="IExtensionOptions"/>）。
    /// </summary>
    public abstract class AbstractExtensionOptions : AbstractExtensionInfo, IExtensionOptions
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractExtensionOptions"/>。
        /// </summary>
        /// <param name="parent">给定的父级 <see cref="IExtensionOptions"/>。</param>
        /// <param name="directories">给定的 <see cref="DirectoryOptions"/>（可选）。</param>
        public AbstractExtensionOptions(IExtensionOptions? parent, DirectoryOptions? directories = null)
            : base(parent)
        {
            Directories = directories ?? new DirectoryOptions();
            ReplacedServices = new Dictionary<Type, Type>();
            ServiceCharacteristics = new ServiceCharacteristicCollection();
        }


        /// <summary>
        /// 目录选项。
        /// </summary>
        public DirectoryOptions Directories { get; private set; }

        /// <summary>
        /// 替换服务字典集合。
        /// </summary>
        [JsonIgnore]
        public IDictionary<Type, Type> ReplacedServices { get; private set; }

        /// <summary>
        /// 服务特征集合。
        /// </summary>
        [JsonIgnore]
        public ServiceCharacteristicCollection ServiceCharacteristics { get; private set; }
    }
}
