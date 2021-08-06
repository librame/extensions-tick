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
        private ServiceCharacteristic? _initializerCharacteristic;


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

            TryAddOrReplace<IIdentificationGeneratorFactory, DefaultIdentificationGeneratorFactory>();

            // Accessors
            TryAddOrReplace<IAccessorAggregator, DefaultAccessorAggregator>();
            TryAddOrReplace<IAccessorManager, DefaultAccessorManager>();
            TryAddOrReplace<IAccessorResolver, DefaultAccessorResolver>();
            TryAddOrReplace<IAccessorSlicer, DefaultAccessorSlicer>();

            // Stores
            TryAddOrReplace(typeof(IStore<>), typeof(Store<>));
        }


        /// <summary>
        /// 添加 <see cref="IAccessorSeeder"/>。
        /// </summary>
        /// <typeparam name="TSeeder">指定的种子机类型。</typeparam>
        /// <returns>返回 <see cref="DataExtensionBuilder"/>。</returns>
        public DataExtensionBuilder AddSeeder<TSeeder>()
            where TSeeder : class, IAccessorSeeder
        {
            
            TryAddOrReplace<IAccessorSeeder, TSeeder>();
            return this;
        }

        /// <summary>
        /// 添加 <see cref="IAccessorInitializer"/>。
        /// </summary>
        /// <typeparam name="TInitializer">指定的初始化器类型。</typeparam>
        /// <returns>返回 <see cref="DataExtensionBuilder"/>。</returns>
        public DataExtensionBuilder AddInitializer<TInitializer>()
            where TInitializer : class, IAccessorInitializer
        {
            if (_initializerCharacteristic == null)
                _initializerCharacteristic = Options.ServiceCharacteristics[typeof(IAccessorInitializer)];

            Services.Add(new ServiceDescriptor(_initializerCharacteristic, typeof(TInitializer), _initializerCharacteristic.Lifetime));
            return this;
        }

    }
}
