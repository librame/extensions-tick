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
using Librame.Extensions.Data.Accessing;
using Librame.Extensions.Data.Auditing;
using Librame.Extensions.Data.Sharding;
using Librame.Extensions.Data.Storing;
using Librame.Extensions.Data.ValueConversion;

namespace Librame.Extensions.Data;

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
        TryAddOrReplaceService<IAuditingManager, InternalAuditingManager>();
        TryAddOrReplaceService<IIdentificationGeneratorFactory, InternalIdentificationGeneratorFactory>();

        // Accessing
        TryAddOrReplaceService<IAccessorAggregator, InternalAccessorAggregator>();
        TryAddOrReplaceService<IAccessorManager, InternalAccessorManager>();
        TryAddOrReplaceService<IAccessorMigrator, InternalAccessorMigrator>();
        TryAddOrReplaceService<IAccessorResolver, InternalAccessorResolver>();
        TryAddOrReplaceService<IAccessorSlicer, InternalAccessorSlicer>();

        // Sharding
        TryAddOrReplaceService<IShardingManager, InternalShardingManager>();

        // Storing
        TryAddOrReplaceService(typeof(IStore<>), typeof(BaseStore<>));

        // ValueConversion
        TryAddOrReplaceService<IEncryptionConverterFactory, InternalEncryptionConverterFactory>();
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
    /// 添加 <typeparamref name="TSeeder"/>。
    /// </summary>
    /// <typeparam name="TSeeder">指定的种子机类型。</typeparam>
    /// <returns>返回 <see cref="DataExtensionBuilder"/>。</returns>
    public DataExtensionBuilder AddSeeder<TSeeder>()
        where TSeeder : class, IAccessorSeeder
    {
        TryAddOrReplaceService<IAccessorSeeder, TSeeder>();
        return this;
    }

}
