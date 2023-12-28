#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Data.Accessing;

namespace Librame.Extensions.Data;

internal sealed class InternalModelCreator : IModelCreator
{
    public void PreCreating(DataContext dataContext, ModelBuilder modelBuilder)
    {
        var dataExtensionOptions = dataContext.CurrentServices.DataOptions;

        // 支持迁移程序集模型
        var migrationsAssembly = dataContext.CurrentServices.ContextRelationalOptions?.MigrationsAssembly;
        if (dataExtensionOptions.Access.AutoMapping && !string.IsNullOrEmpty(migrationsAssembly))
            modelBuilder.CreateAssembliesModels(migrationsAssembly);

        // 支持数据审计
        if (dataExtensionOptions.Audit.Enabling)
            modelBuilder.CreateAuditingModels(dataContext);
    }

    public void PostCreating(DataContext dataContext, ModelBuilder modelBuilder)
    {
        var dataExtensionOptions = dataContext.CurrentServices.DataOptions;

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.ClrType.GetProperties())
            {
                // 启用对实体加密属性的支持
                if (dataExtensionOptions.Access.AutoEncryption && property.IsEncrypted())
                    entityType.UseEncryption(property, dataContext);

                // 启用对强类型属性的支持
                if (dataExtensionOptions.Access.AutoStronglyTyped && property.IsStronglyTypedIdentifier(out var valueType))
                    entityType.UseStronglyTypedIdentifier(property, valueType!);

                // 支持将 GUID 类型统一处理为 Chars 类型（跨数据库支持 GUID）
                if (dataExtensionOptions.Access.AutoGuidToChars && property.IsGuidOrNullable(out var isNullable))
                    entityType.UseGuidToChars(property, modelBuilder, isNullable);

                // 支持数据版本标识以支持并发功能
                if (dataExtensionOptions.Access.AutoRowVersion && entityType.IsRowVersionType())
                    entityType.UseRowVersion(property, modelBuilder);

                dataExtensionOptions.PostConfigureEntityTypePropertyAction?.Invoke(dataContext, modelBuilder, entityType, property);
            }

            // 全局查询过滤器
            entityType.UseQueryFilters(dataExtensionOptions.QueryFilters, dataContext);

            dataExtensionOptions.PostConfigureEntityTypeAction?.Invoke(dataContext, modelBuilder, entityType);
        }
    }

    public void ExceptionCreating(DataContext dataContext, ModelBuilder modelBuilder, Exception exception)
    {
    }

}
