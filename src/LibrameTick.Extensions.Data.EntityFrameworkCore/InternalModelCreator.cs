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
    public void PreCreating(BaseDataContext dataContext, ModelBuilder modelBuilder)
    {
        // 支持迁移程序集模型
        if (dataContext.DataExtOptions.Access.AutoMapping && !string.IsNullOrEmpty(dataContext.RelationalExtension?.MigrationsAssembly))
            modelBuilder.CreateAssembliesModels(dataContext.RelationalExtension.MigrationsAssembly);

        // 支持数据审计
        if (dataContext.DataExtOptions.Audit.Enabling)
            modelBuilder.CreateAuditingModels(dataContext);
    }

    public void PostCreating(BaseDataContext dataContext, ModelBuilder modelBuilder)
    {
        var options = dataContext.DataExtOptions.Access;

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.ClrType.GetProperties())
            {
                // 启用对实体加密属性的支持
                if (options.AutoEncryption && property.IsEncrypted())
                    entityType.UseEncryption(property, dataContext);

                // 启用对强类型属性的支持
                if (options.AutoStronglyTyped && property.IsStronglyTypedIdentifier(out var valueType))
                    entityType.UseStronglyTypedIdentifier(property, valueType!);

                // 支持将 GUID 类型统一处理为 Chars 类型（跨数据库支持 GUID）
                if (options.AutoGuidToChars && property.IsGuidOrNullable(out var isNullable))
                    entityType.UseGuidToChars(property, modelBuilder, isNullable);

                // 支持数据版本标识以支持并发功能
                if (options.AutoRowVersion && entityType.IsRowVersionType())
                    entityType.UseRowVersion(property, modelBuilder);

                dataContext.PostModelCreatingPropertyAction?.Invoke(dataContext, modelBuilder, entityType, property);
            }

            // 全局查询过滤器
            entityType.UseQueryFilters(dataContext.DataExtOptions.QueryFilters, dataContext);

            dataContext.PostModelCreatingAction?.Invoke(dataContext, modelBuilder, entityType);
        }
    }

    public void ExceptionCreating(BaseDataContext dataContext, ModelBuilder modelBuilder, Exception exception)
    {
    }

}
