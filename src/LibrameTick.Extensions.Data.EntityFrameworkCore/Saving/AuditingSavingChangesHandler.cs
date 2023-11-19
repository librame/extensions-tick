#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Data.Auditing;
using Librame.Extensions.Data.Storing;

namespace Librame.Extensions.Data.Saving;

/// <summary>
/// 定义继承 <see cref="AbstractSavingChangesHandler"/> 的审计保存变化处理程序。
/// </summary>
public sealed class AuditingSavingChangesHandler : AbstractSavingChangesHandler
{
    private static readonly Type _notAuditedType = typeof(NotAuditedAttribute);

    /// <summary>
    /// 保存审计集合。
    /// </summary>
    internal IReadOnlyList<Audit>? SavingAudits { get; private set; }


    /// <summary>
    /// 预处理保存上下文。
    /// </summary>
    /// <param name="context">给定的 <see cref="ISavingChangesContext"/>。</param>
    protected override void PreHandlingCore(ISavingChangesContext context)
    {
        var idGenerator = context.DataContext.GetService<IIdGeneratorFactory>();
        var options = context.DataContext.DataExtOptions;

        SavingAudits = ParseEntities(idGenerator, options, context.ChangesEntities);

        // 保存审计数据
        if (options.Audit.SaveAudits && SavingAudits?.Count > 0)
            options.SavingAuditsAction(context.DataContext, SavingAudits);
    }


    private List<Audit>? ParseEntities(IIdGeneratorFactory idGenerator, DataExtensionOptions options,
        IEnumerable<EntityEntry>? entityEntries)
    {
        if (entityEntries is null)
            return null;

        var audits = new List<Audit>();

        foreach (var entry in entityEntries)
        {
            // 过滤标记不审计特性的实体
            if (entry.Metadata.ClrType.IsDefined(_notAuditedType, inherit: false))
                continue;

            var audit = new Audit();

            audit.Id = idGenerator.GetSnowflakeIdGenerator().GenerateId();
            audit.TableName = GetEntityTableName(entry.Metadata);
            audit.EntityTypeName = GetTypeName(entry.Metadata.ClrType);
            audit.EntityId = GetEntityId(entry);
            audit.StateName = entry.State.ToString();

            PopulateProperties(idGenerator, options, audit, entry);

            audits.Add(audit);
        }

        return audits;
    }

    private void PopulateProperties(IIdGeneratorFactory idGenerator, DataExtensionOptions options,
        Audit audit, EntityEntry entityEntry)
    {
        foreach (var property in entityEntry.CurrentValues.Properties)
        {
            if (property.IsConcurrencyToken)
                continue;

            (var newValue, var oldValue) = GetValues(property, entityEntry);

            // 当前属性的新值与旧值均为空时，则不被审计
            if (string.IsNullOrEmpty(newValue) && string.IsNullOrEmpty(oldValue))
                continue;

            var auditProperty = new AuditProperty();

            auditProperty.Id = idGenerator.GetSnowflakeIdGenerator().GenerateId();
            auditProperty.PropertyName = property.Name;
            auditProperty.PropertyTypeName = GetTypeName(property.ClrType);
            auditProperty.NewValue = newValue;
            auditProperty.OldValue = oldValue;

            if (!options.Store.MapRelationship)
                auditProperty.AuditId = audit.Id;
            else
                auditProperty.Audit = audit;

            audit.AddProperty(auditProperty);
        }
    }

    private static (string? newValue, string? oldValue) GetValues(IProperty property, EntityEntry entityEntry)
    {
        string? newValue = null;
        string? oldValue = null;

        switch (entityEntry.State)
        {
            case EntityState.Added:
                newValue = entityEntry.Property(property.Name).CurrentValue?.ToString();
                break;

            case EntityState.Deleted:
                oldValue = entityEntry.Property(property.Name).OriginalValue?.ToString();
                break;

            case EntityState.Modified:
                {
                    var currentValue = entityEntry.Property(property.Name).CurrentValue?.ToString();
                    var originalValue = entityEntry.Property(property.Name).OriginalValue?.ToString();

                    if (currentValue != originalValue)
                    {
                        newValue = currentValue;
                        oldValue = originalValue;
                    }
                }
                break;
        }

        return (newValue, oldValue);
    }

    private static string GetEntityTableName(IEntityType entityType)
    {
        var tableName = entityType.GetTableName();
        if (string.IsNullOrEmpty(tableName))
            tableName = entityType.ClrType.Name.AsPluralize();

        var schema = entityType.GetSchema();
        if (string.IsNullOrEmpty(schema))
            return tableName;

        return $"{schema}.{tableName}";
    }

    private static string GetTypeName(Type clrType)
    {
        var sb = new StringBuilder(clrType.Name);

        if (clrType.IsGenericType)
        {
            sb.Append("[[");
            sb.Append(clrType.GenericTypeArguments.Select(t => t.Name).JoinString(","));
            sb.Append("]]");
        }

        return sb.ToString();
    }

    private static string GetEntityId(EntityEntry entityEntry)
    {
        if (entityEntry.Entity is IObjectIdentifier identifier)
            return identifier.GetObjectId().ToString()!;

        var key = entityEntry.Metadata.FindPrimaryKey();
        if (key is not null)
            return GetPropertyValues(key.Properties, entityEntry).JoinString(',');

        var uniqueIndexes = entityEntry.Metadata.GetIndexes().Where(static i => i.IsUnique);
        return uniqueIndexes.Select(s => GetPropertyValues(s.Properties, entityEntry).JoinString(',')).JoinString(',');
    }


    private static IEnumerable<object?> GetPropertyValues(IEnumerable<IProperty> properties, EntityEntry entityEntry)
    {
        return properties.Select(s =>
        {
            var property = entityEntry.Property(s.Name);

            if (property.EntityEntry.State is EntityState.Deleted)
                return property.OriginalValue;
            else
                return property.CurrentValue;
        });
    }

}
