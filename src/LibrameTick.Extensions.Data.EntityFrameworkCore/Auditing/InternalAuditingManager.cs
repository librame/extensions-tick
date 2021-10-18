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
    private readonly DataExtensionOptions _options;


    public InternalAuditingManager(IIdentificationGeneratorFactory idGeneratorFactory,
        IOptionsMonitor<DataExtensionOptions> options)
    {
        _idGeneratorFactory = idGeneratorFactory;
        _options = options.CurrentValue;
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

            audit.Id = _idGeneratorFactory.GetSnowflakeIdGenerator().GenerateId();
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

            (var newValue, var oldValue) = GetValues(property, entityEntry);

            // 当前属性的新值与旧值均为空时，则不被审计
            if (string.IsNullOrEmpty(newValue) && string.IsNullOrEmpty(oldValue))
                continue;

            var auditProperty = new AuditProperty();

            auditProperty.Id = _idGeneratorFactory.GetMongoIdGenerator().GenerateId();
            auditProperty.PropertyName = property.Name;
            auditProperty.PropertyTypeName = GetTypeName(property.ClrType);
            auditProperty.NewValue = newValue;
            auditProperty.OldValue = oldValue;

            if (!_options.Store.MapRelationship)
                auditProperty.AuditId = audit.Id;
            else
                auditProperty.Audit = audit;

            audit.Properties.Add(auditProperty);
        }
    }

    private (string? newValue, string? oldValue) GetValues(IProperty property, EntityEntry entityEntry)
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
