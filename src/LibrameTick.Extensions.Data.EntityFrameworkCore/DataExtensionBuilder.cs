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
/// 定义实现 <see cref="IExtensionBuilder"/> 的数据扩展构建器。
/// </summary>
public class DataExtensionBuilder : BaseExtensionBuilder<DataExtensionBuilder, DataExtensionOptions>
{
    /// <summary>
    /// 构造一个 <see cref="DataExtensionBuilder"/>。
    /// </summary>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="parentBuilder"/> 为空。
    /// </exception>
    /// <param name="parentBuilder">给定的父级 <see cref="IExtensionBuilder"/>。</param>
    /// <param name="setupOptions">给定用于设置选项的动作（可选；为空则不设置）。</param>
    /// <param name="configOptions">给定使用 <see cref="IConfiguration"/> 的选项配置（可选；为空则不配置）。</param>
    public DataExtensionBuilder(IExtensionBuilder parentBuilder,
        Action<DataExtensionOptions>? setupOptions = null, IConfiguration? configOptions = null)
        : base(parentBuilder, setupOptions, configOptions)
    {
        ServiceCharacteristics.AddSingleton<IIdentificationGeneratorFactory>();
        ServiceCharacteristics.AddSingleton<IAuditingManager>();

        // Accessing
        ServiceCharacteristics.AddScope<IAccessorManager>();
        ServiceCharacteristics.AddScope<IAccessorMigrator>();
        ServiceCharacteristics.AddScope<IAccessorResolver>();

        ServiceCharacteristics.AddScope<IAccessorSeeder>(addImplementationType: true);
        ServiceCharacteristics.AddScope<IAccessorInitializer>();

        // Sharding
        ServiceCharacteristics.AddSingleton<IShardingManager>();

        // Storing
        ServiceCharacteristics.AddScope(typeof(IStore<>));

        // ValueConversion
        ServiceCharacteristics.AddSingleton<IEncryptionConverterFactory>();
    }

}
