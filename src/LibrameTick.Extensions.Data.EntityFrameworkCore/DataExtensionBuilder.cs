#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.DependencyInjection;
using System;

namespace Librame.Extensions.Data
{
    using Core;
    using Data.Accessors;

    /// <summary>
    /// 数据扩展构建器。
    /// </summary>
    public class DataExtensionBuilder : AbstractExtensionBuilder<DataExtensionOptions>
    {
        /// <summary>
        /// 构造一个 <see cref="DataExtensionBuilder"/>。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="parentBuilder"/> 或 <paramref name="options"/> 为空。
        /// </exception>
        /// <param name="parentBuilder">给定的父级 <see cref="IExtensionBuilder"/>。</param>
        /// <param name="options">给定的 <see cref="DataExtensionOptions"/>。</param>
        public DataExtensionBuilder(IExtensionBuilder parentBuilder, DataExtensionOptions options)
            : base(parentBuilder, options)
        {
            Services.AddSingleton(this);

            // Accessors
            AddOrReplaceByCharacteristic<IAccessorAggregator, AccessorAggregator>();
            AddOrReplaceByCharacteristic<IAccessorManager, AccessorManager>();
            AddOrReplaceByCharacteristic<IAccessorResolver, AccessorResolver>();
            AddOrReplaceByCharacteristic<IAccessorSlicer, AccessorSlicer>();
        }

    }
}
