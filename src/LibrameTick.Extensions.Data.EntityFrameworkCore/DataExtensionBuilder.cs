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
using Librame.Extensions.Setting;

namespace Librame.Extensions.Data;

/// <summary>
/// 定义实现 <see cref="IExtensionBuilder"/> 的数据扩展构建器。
/// </summary>
public class DataExtensionBuilder : AbstractExtensionBuilder<DataExtensionBuilder>
{
    /// <summary>
    /// 构造一个 <see cref="DataExtensionBuilder"/>。
    /// </summary>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="parentBuilder"/> 为空。
    /// </exception>
    /// <param name="parentBuilder">给定的父级 <see cref="IExtensionBuilder"/>。</param>
    public DataExtensionBuilder(IExtensionBuilder parentBuilder)
        : base(parentBuilder)
    {
        ServiceCharacteristics.AddSingleton<IIdGeneratorFactory>();

        // Auditing
        ServiceCharacteristics.AddSingleton<IAuditingParser<EntityEntry, Audit>>();
        ServiceCharacteristics.AddSingleton<IAuditingTracker<EntityEntry>>();
        ServiceCharacteristics.AddSingleton<IAuditingContext<EntityEntry, Audit>>();

        // Accessing
        ServiceCharacteristics.AddScope<IAccessorContext>();
        ServiceCharacteristics.AddScope<IAccessorMigrator>();
        ServiceCharacteristics.AddScope<IAccessorResolver>();

        ServiceCharacteristics.AddScope(typeof(BaseAccessor<>));
        ServiceCharacteristics.AddScope<IAccessorSeeder>();
        ServiceCharacteristics.AddScope<IAccessorInitializer>();

        // Setting
        ServiceCharacteristics.AddSingleton<IShardingSettingProvider>();

        // Sharding
        ServiceCharacteristics.AddSingleton<IShardingContext>();
        ServiceCharacteristics.AddSingleton<IShardingStrategyProvider>();
        ServiceCharacteristics.AddSingleton<IShardingTracker>();

        // Storing
        ServiceCharacteristics.AddScope(typeof(IStore<>));

        // ValueConversion
        ServiceCharacteristics.AddSingleton<IEncryptionConverterFactory>();
    }

}
