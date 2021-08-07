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
using Librame.Extensions.Data.Access;
using Librame.Extensions.Data.Store;
using Librame.Extensions.Data.ValueConversion;
using System;

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 数据扩展构建器。
    /// </summary>
    public class DataExtensionBuilder : AbstractExtensionBuilder<DataExtensionOptions, DataExtensionBuilder>
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
            // Base: IdentificationGenerator
            TryAddOrReplaceService<IIdentificationGeneratorFactory, DefaultIdentificationGeneratorFactory>();

            // Access
            TryAddOrReplaceService<IAccessorAggregator, DefaultAccessorAggregator>();
            TryAddOrReplaceService<IAccessorManager, DefaultAccessorManager>();
            TryAddOrReplaceService<IAccessorResolver, DefaultAccessorResolver>();
            TryAddOrReplaceService<IAccessorSlicer, DefaultAccessorSlicer>();

            // Store
            TryAddOrReplaceService(typeof(IStore<>), typeof(BaseStore<>));

            // ValueConversion
            TryAddOrReplaceService<IEncryptionConverterFactory, DefaultEncryptionConverterFactory>();
        }


        /// <summary>
        /// 添加 <see cref="IAccessorInitializer"/>。
        /// </summary>
        /// <typeparam name="TInitializer">指定的初始化器类型。</typeparam>
        /// <returns>返回 <see cref="DataExtensionBuilder"/>。</returns>
        public DataExtensionBuilder AddInitializer<TInitializer>()
            where TInitializer : class, IAccessorInitializer
        {
            TryAddEnumerableServices<IAccessorInitializer, TInitializer>();
            return this;
        }

        /// <summary>
        /// 添加 <typeparamref name="TMigrator"/>。
        /// </summary>
        /// <typeparam name="TMigrator">指定的移植器类型。</typeparam>
        /// <returns>返回 <see cref="DataExtensionBuilder"/>。</returns>
        public DataExtensionBuilder AddMigrator<TMigrator>()
            where TMigrator : class, IAccessorMigrator
        {
            TryAddOrReplaceService<IAccessorMigrator, TMigrator>();
            return this;
        }

        /// <summary>
        /// 添加 <typeparamref name="TSeeder"/>（仅注册种子机类型，种子机接口仅用于匹配特征）。
        /// </summary>
        /// <typeparam name="TSeeder">指定的种子机类型。</typeparam>
        /// <returns>返回 <see cref="DataExtensionBuilder"/>。</returns>
        public DataExtensionBuilder AddSeeder<TSeeder>()
            where TSeeder : class, IAccessorSeeder
        {
            // 仅注册种子机类型，种子机接口仅用于匹配特征
            TryAddOrReplaceServiceByCharacteristic<IAccessorSeeder, TSeeder>();
            return this;
        }

    }
}
