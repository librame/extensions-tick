#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Data.Storing;

namespace Librame.Extensions.Data.Auditing;

class InternalAuditingManager : IAuditingManager
{
    private readonly Type _notAuditedType = typeof(NotAuditedAttribute);

    private readonly IIdentificationGeneratorFactory _idGeneratorFactory;


    public InternalAuditingManager(IIdentificationGeneratorFactory idGeneratorFactory)
    {
        _idGeneratorFactory = idGeneratorFactory;
    }


    public IReadOnlyList<Audit> GetAudits(IEnumerable<EntityEntry> entityEntries)
    {
        var audits = new List<Audit>();

        foreach (var entry in entityEntries)
        {
            // 过滤标记不审计特性的实体
            if (entry.Metadata.ClrType.IsDefined(_notAuditedType, inherit: false))
                continue;

            var audit = new Audit();

            audit.Id = _idGeneratorFactory.GetNewId<string>();
            audit.TableName = GetEntityTableName(entry.Metadata);
            audit.EntityTypeName = GetTypeName(entry.Metadata.ClrType);
            audit.EntityId = GetEntityId(entry);
            audit.StateName = entry.State.ToString();

            PopulateProperties(audit, entry);

            audits.Add(audit);
        }

        return audits;
    }

    private void PopulateProperties(Audit audit, EntityEntry entityEntry)
    {
        foreach (var property in entityEntry.CurrentValues.Properties)
        {
            if (property.IsConcurrencyToken)
                continue;

            var auditProperty = new AuditProperty();

            auditProperty.Id = _idGeneratorFactory.GetNewId<string>();
            auditProperty.AuditId = audit.Id;
            auditProperty.PropertyName = property.Name;
            auditProperty.PropertyTypeName = GetTypeName(property.ClrType);

            switch (entityEntry.State)
            {
                case EntityState.Added:
                    auditProperty.NewValue = entityEntry.Property(property.Name).CurrentValue?.ToString();
                    break;

                case EntityState.Deleted:
                    auditProperty.OldValue = entityEntry.Property(property.Name).OriginalValue?.ToString();
                    break;

                case EntityState.Modified:
                    {
                        var currentValue = entityEntry.Property(property.Name).CurrentValue?.ToString();
                        var originalValue = entityEntry.Property(property.Name).OriginalValue?.ToString();

                        if (currentValue != originalValue)
                        {
                            auditProperty.NewValue = currentValue;
                            auditProperty.OldValue = originalValue;
                        }
                    }
                    break;
            }

            // 当前属性的新值与旧值均为空时，则不被审计
            if (!(string.IsNullOrEmpty(auditProperty.NewValue) && string.IsNullOrEmpty(auditProperty.OldValue)))
                audit.Properties.Add(auditProperty);
        }
    }
        
    private string GetEntityTableName(IEntityType entityType)
    {
        var tableName = entityType.GetTableName();
        if (string.IsNullOrEmpty(tableName))
            tableName = entityType.ClrType.Name.AsPluralize();

        var schema = entityType.GetSchema();
        if (string.IsNullOrEmpty(schema))
            return tableName;

        return $"{schema}.{tableName}";
    }

    private string GetTypeName(Type clrType)
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

    private string GetEntityId(EntityEntry entityEntry)
    {
        if (entityEntry.Entity is IObjectIdentifier identifier)
            return identifier.GetObjectId().ToString()!;

        var key = entityEntry.Metadata.FindPrimaryKey();
        if (key is not null)
            return GetPropertyValues(key.Properties, entityEntry).JoinString(',');

        var uniqueIndexes = entityEntry.Metadata.GetIndexes().Where(i => i.IsUnique);
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
