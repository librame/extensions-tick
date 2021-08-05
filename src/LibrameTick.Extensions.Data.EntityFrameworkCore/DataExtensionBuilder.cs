#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Core;
using Librame.Extensions.Data.Accessors;
using Librame.Extensions.Data.Stores;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Librame.Extensions.Data
{
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

            AddOrReplaceByCharacteristic<IIdentificationGeneratorFactory, DefaultIdentificationGeneratorFactory>();

            // Accessors
            AddOrReplaceByCharacteristic<IAccessorAggregator, DefaultAccessorAggregator>();
            AddOrReplaceByCharacteristic<IAccessorManager, DefaultAccessorManager>();
            AddOrReplaceByCharacteristic<IAccessorResolver, DefaultAccessorResolver>();
            AddOrReplaceByCharacteristic<IAccessorSlicer, DefaultAccessorSlicer>();

            // Stores
            AddOrReplaceByCharacteristic(typeof(IStore<>), typeof(Store<>));
        }

    }
}
