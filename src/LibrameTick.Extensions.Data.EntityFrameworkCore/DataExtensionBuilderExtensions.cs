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
using Librame.Extensions.Data;
using Librame.Extensions.Data.Accessing;
using Librame.Extensions.Data.Auditing;
using Librame.Extensions.Data.Sharding;
using Librame.Extensions.Data.Storing;
using Librame.Extensions.Data.ValueConversion;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// <see cref="DataExtensionBuilder"/> 静态扩展。
/// </summary>
public static class DataExtensionBuilderExtensions
{

    /// <summary>
    /// 注册 Librame 数据扩展构建器。
    /// </summary>
    /// <param name="parentBuilder">给定的 <see cref="IExtensionBuilder"/>。</param>
    /// <param name="setupOptions">给定可用于设置 <see cref="DataExtensionOptions"/> 选项的动作（可选；为空则不设置）。</param>
    /// <param name="configuration">给定可用于 <see cref="DataExtensionOptions"/> 选项的配置对象（可选；为空则不配置）。</param>
    /// <returns>返回 <see cref="DataExtensionBuilder"/>。</returns>
    public static DataExtensionBuilder AddData(this IExtensionBuilder parentBuilder,
        Action<DataExtensionOptions>? setupOptions = null, IConfiguration? configuration = null)
    {
        // 配置扩展选项
        parentBuilder.ConfigureExtensionOptions(setupOptions, configuration);

        var builder = new DataExtensionBuilder(parentBuilder);

        builder
            .AddAccessing()
            .AddAuditing()
            .AddIdentification()
            .AddSharding()
            .AddStoring()
            .AddValueConversion();

        return builder;
    }


    private static DataExtensionBuilder AddAccessing(this DataExtensionBuilder builder)
    {
        builder.TryAddOrReplaceService<IAccessorManager, InternalAccessorManager>();
        builder.TryAddOrReplaceService<IAccessorMigrator, InternalAccessorMigrator>();
        builder.TryAddOrReplaceService<IAccessorResolver, InternalAccessorResolver>();

        return builder;
    }

    private static DataExtensionBuilder AddAuditing(this DataExtensionBuilder builder)
    {
        builder.TryAddOrReplaceService<IAuditingManager, InternalAuditingManager>();

        return builder;
    }

    private static DataExtensionBuilder AddIdentification(this DataExtensionBuilder builder)
    {
        builder.TryAddOrReplaceService<IIdGeneratorFactory, InternalIdGeneratorFactory>();

        return builder;
    }

    private static DataExtensionBuilder AddSharding(this DataExtensionBuilder builder)
    {
        builder.TryAddOrReplaceService<IShardingManager, InternalShardingManager>();

        return builder;
    }

    private static DataExtensionBuilder AddStoring(this DataExtensionBuilder builder)
    {
        builder.TryAddOrReplaceService(typeof(IStore<>), typeof(BaseStore<>));

        return builder;
    }

    private static DataExtensionBuilder AddValueConversion(this DataExtensionBuilder builder)
    {
        builder.TryAddOrReplaceService<IEncryptionConverterFactory, InternalEncryptionConverterFactory>();

        return builder;
    }


    /// <summary>
    /// 添加 <see cref="IAccessorInitializer"/>（支持多次添加）。
    /// </summary>
    /// <typeparam name="TInitializer">指定的初始化器类型。</typeparam>
    /// <param name="builder">给定的 <see cref="DataExtensionBuilder"/>。</param>
    /// <returns>返回 <see cref="DataExtensionBuilder"/>。</returns>
    public static DataExtensionBuilder AddInitializer<TInitializer>(this DataExtensionBuilder builder)
        where TInitializer : class, IAccessorInitializer
    {
        builder.TryAddEnumerableServices<IAccessorInitializer, TInitializer>();

        return builder;
    }

    /// <summary>
    /// 添加 <typeparamref name="TSeeder"/>。
    /// </summary>
    /// <typeparam name="TSeeder">指定的种子机类型。</typeparam>
    /// <param name="builder">给定的 <see cref="DataExtensionBuilder"/>。</param>
    /// <returns>返回 <see cref="DataExtensionBuilder"/>。</returns>
    public static DataExtensionBuilder AddSeeder<TSeeder>(this DataExtensionBuilder builder)
        where TSeeder : class, IAccessorSeeder
    {
        builder.TryAddOrReplaceService<IAccessorSeeder, TSeeder>();

        return builder;
    }

}
