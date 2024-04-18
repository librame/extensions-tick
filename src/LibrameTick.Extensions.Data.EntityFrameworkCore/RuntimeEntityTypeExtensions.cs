#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data;

/// <summary>
/// 定义 <see cref="RuntimeEntityType"/> 静态扩展。
/// </summary>
public static class RuntimeEntityTypeExtensions
{

#pragma warning disable EF1001 // Internal EF Core API usage.

    private static Func<RuntimeModelConvention,
        IProperty, RuntimeTypeBase, RuntimeProperty>? _createPropertyFunc;

    private static Func<RuntimeModelConvention,
        RuntimeProperty, IElementType, bool, RuntimeElementType>? _createElementTypeFunc;


    private static Func<RuntimeModelConvention,
        IServiceProperty, RuntimeEntityType, RuntimeServiceProperty>? _createServicePropertyFunc;

    private static Func<RuntimeModelConvention,
        ParameterBinding, RuntimeEntityType, ParameterBinding>? _createParameterFunc;


    private static Func<RuntimeModelConvention,
        IComplexProperty, RuntimeEntityType, RuntimeComplexProperty>? _createComplexPropertyFunc;

    private static Func<RuntimeModelConvention,
        IKey, RuntimeEntityType, RuntimeKey>? _createKeyFunc;

    private static Func<RuntimeModelConvention,
        IIndex, RuntimeEntityType, RuntimeIndex>? _createIndexFunc;

    private static Func<RuntimeModelConvention,
        ITrigger, RuntimeEntityType, RuntimeTrigger>? _createTriggerFunc;

    private static Func<RuntimeModelConvention,
        InstantiationBinding?, RuntimeEntityType, InstantiationBinding?>? _createInstantiationFunc;


    private static Func<RuntimeModelConvention,
        IForeignKey, RuntimeEntityType, RuntimeForeignKey>? _createForeignKeyFunc;

    private static Func<RuntimeModelConvention,
        INavigation, RuntimeForeignKey, RuntimeNavigation>? _createNavigationFunc;

    private static Func<RuntimeModelConvention,
        ISkipNavigation, RuntimeEntityType, RuntimeSkipNavigation>? _createSkipNavigationFunc;


    private static Action<RuntimeModelConvention,
        Dictionary<string, object?>, IProperty, RuntimeProperty, bool>? _processPropertyAnnotationsAction;

    private static Action<RuntimeModelConvention,
        Dictionary<string, object?>, IElementType, RuntimeElementType, bool>? _processElementTypeAnnotationsAction;

    private static Action<RuntimeModelConvention,
        Dictionary<string, object?>, IServiceProperty, RuntimeServiceProperty, bool>? _processServicePropertyAnnotationsAction;

    private static Action<RuntimeModelConvention,
        Dictionary<string, object?>, IComplexProperty, RuntimeComplexProperty, bool>? _processComplexPropertyAnnotationsAction;

    private static Action<RuntimeModelConvention,
        Dictionary<string, object?>, IKey, RuntimeKey, bool>? _processKeyAnnotationsAction;

    private static Action<RuntimeModelConvention,
        Dictionary<string, object?>, IIndex, RuntimeIndex, bool>? _processIndexAnnotationsAction;

    private static Action<RuntimeModelConvention,
        Dictionary<string, object?>, ITrigger, RuntimeTrigger, bool>? _processTriggerAnnotationsAction;


    private static Action<RuntimeModelConvention,
        Dictionary<string, object?>, IForeignKey, RuntimeForeignKey, bool>? _processForeignKeyAnnotationsAction;

    private static Action<RuntimeModelConvention,
        Dictionary<string, object?>, INavigation, RuntimeNavigation, bool>? _processNavigationAnnotationsAction;

    private static Action<RuntimeModelConvention,
        Dictionary<string, object?>, ISkipNavigation, RuntimeSkipNavigation, bool>? _processSkipNavigationAnnotationsAction;

    private static Action<RuntimeModelConvention,
        Dictionary<string, object?>, IEntityType, RuntimeEntityType, bool>? _processEntityTypeAnnotationsAction;


    private static void CreateProperty(RuntimeEntityType newEntityType, IProperty srcProperty,
        RuntimeModelConvention convention)
    {
        _createPropertyFunc ??= "Create".GetMethodFuncByExpression<RuntimeModelConvention,
            IProperty, RuntimeTypeBase, RuntimeProperty>();

        _createElementTypeFunc ??= "Create".GetMethodFuncByExpression<RuntimeModelConvention,
            RuntimeProperty, IElementType, bool, RuntimeElementType>();

        _processPropertyAnnotationsAction ??= "ProcessPropertyAnnotations".GetMethodActionByExpression<RuntimeModelConvention,
            Dictionary<string, object?>, IProperty, RuntimeProperty, bool>();

        _processElementTypeAnnotationsAction ??= "ProcessElementTypeAnnotations".GetMethodActionByExpression<RuntimeModelConvention,
            Dictionary<string, object?>, IElementType, RuntimeElementType, bool>();

        var newProperty = _createPropertyFunc(convention, srcProperty, newEntityType);

        CreateAnnotations(convention, srcProperty, newProperty,
            static (convention, annotations, source, target, runtime) =>
                _processPropertyAnnotationsAction(convention, annotations, source, target, runtime));

        var elementType = srcProperty.GetElementType();
        if (elementType != null)
        {
            var runtimeElementType = _createElementTypeFunc(convention, newProperty, elementType, newProperty.IsPrimitiveCollection);

            CreateAnnotations(convention, elementType, runtimeElementType,
                static (convention, annotations, source, target, runtime) =>
                    _processElementTypeAnnotationsAction(convention, annotations, source, target, runtime));
        }

        //var columnName = srcProperty.GetColumnName();

        //var newColumn = new Column(columnName, srcProperty.GetColumnType(), newTable)
        //{
        //    IsNullable = srcProperty.IsColumnNullable()
        //};
        //newTable.Columns.Add(columnName, newColumn);

        //var newTableMapping = new TableMapping(newEntityType, newTable, includesDerivedTypes: true);

        //var newColumnMapping = new ColumnMapping(newProperty, newColumn, newTableMapping);
        //newTableMapping.AddColumnMapping(newColumnMapping);
        //newColumn.AddPropertyMapping(newColumnMapping);

        //RelationalModel.CreateColumnMapping(newColumn, newProperty, newTableMapping);
    }

    private static void CreateServiceProperty(RuntimeEntityType newEntityType, IServiceProperty srcServiceProperty,
        RuntimeModelConvention convention)
    {
        _createServicePropertyFunc ??= "Create".GetMethodFuncByExpression<RuntimeModelConvention,
            IServiceProperty, RuntimeEntityType, RuntimeServiceProperty>();

        _createParameterFunc ??= "Create".GetMethodFuncByExpression<RuntimeModelConvention,
            ParameterBinding, RuntimeEntityType, ParameterBinding>();

        _processServicePropertyAnnotationsAction ??= "ProcessServicePropertyAnnotations".GetMethodActionByExpression<RuntimeModelConvention,
            Dictionary<string, object?>, IServiceProperty, RuntimeServiceProperty, bool>();

        var newServiceProperty = _createServicePropertyFunc(convention, srcServiceProperty, newEntityType);

        CreateAnnotations(convention, srcServiceProperty, newServiceProperty,
            static (convention, annotations, source, target, runtime) =>
                _processServicePropertyAnnotationsAction(convention, annotations, source, target, runtime));

        newServiceProperty.ParameterBinding =
            (ServiceParameterBinding)_createParameterFunc(convention, srcServiceProperty.ParameterBinding, newEntityType);
    }

    private static void CreateComplexProperty(RuntimeEntityType newEntityType, IComplexProperty srcComplexProperty,
        RuntimeModelConvention convention)
    {
        _createComplexPropertyFunc ??= "Create".GetMethodFuncByExpression<RuntimeModelConvention,
            IComplexProperty, RuntimeEntityType, RuntimeComplexProperty>();

        _processComplexPropertyAnnotationsAction ??= "ProcessComplexPropertyAnnotations".GetMethodActionByExpression<RuntimeModelConvention,
            Dictionary<string, object?>, IComplexProperty, RuntimeComplexProperty, bool>();

        var newComplexProperty = _createComplexPropertyFunc(convention, srcComplexProperty, newEntityType);

        CreateAnnotations(convention, srcComplexProperty, newComplexProperty,
            static (convention, annotations, source, target, runtime) =>
                _processComplexPropertyAnnotationsAction(convention, annotations, source, target, runtime));
    }

    private static void CreateKey(RuntimeEntityType newEntityType, IKey srcKey, RuntimeModelConvention convention)
    {
        _createKeyFunc ??= "Create".GetMethodFuncByExpression<RuntimeModelConvention, IKey, RuntimeEntityType, RuntimeKey>();

        _processKeyAnnotationsAction ??= "ProcessKeyAnnotations".GetMethodActionByExpression<RuntimeModelConvention,
            Dictionary<string, object?>, IKey, RuntimeKey, bool>();

        var newKey = _createKeyFunc(convention, srcKey, newEntityType);
        if (srcKey.IsPrimaryKey())
        {
            newEntityType.SetPrimaryKey(newKey);
        }

        CreateAnnotations(convention, srcKey, newKey,
            static (convention, annotations, source, target, runtime) =>
                _processKeyAnnotationsAction(convention, annotations, source, target, runtime));
    }

    private static void CreateIndex(RuntimeEntityType newEntityType, IIndex srcIndex, RuntimeModelConvention convention)
    {
        _createIndexFunc ??= "Create".GetMethodFuncByExpression<RuntimeModelConvention, IIndex, RuntimeEntityType, RuntimeIndex>();

        _processIndexAnnotationsAction ??= "ProcessIndexAnnotations".GetMethodActionByExpression<RuntimeModelConvention,
            Dictionary<string, object?>, IIndex, RuntimeIndex, bool>();

        var newIndex = _createIndexFunc(convention, srcIndex, newEntityType);

        CreateAnnotations(convention, srcIndex, newIndex,
            static (convention, annotations, source, target, runtime) =>
                _processIndexAnnotationsAction(convention, annotations, source, target, runtime));
    }

    private static void CreateTrigger(RuntimeEntityType newEntityType, ITrigger srcTrigger, RuntimeModelConvention convention)
    {
        _createTriggerFunc ??= "Create".GetMethodFuncByExpression<RuntimeModelConvention, ITrigger, RuntimeEntityType, RuntimeTrigger>();

        _processTriggerAnnotationsAction ??= "ProcessTriggerAnnotations".GetMethodActionByExpression<RuntimeModelConvention,
            Dictionary<string, object?>, ITrigger, RuntimeTrigger, bool>();

        var newTrigger = _createTriggerFunc(convention, srcTrigger, newEntityType);

        CreateAnnotations(convention, srcTrigger, newTrigger,
            static (convention, annotations, source, target, runtime) =>
                _processTriggerAnnotationsAction(convention, annotations, source, target, runtime));
    }

    private static void CreateInstantiation(RuntimeEntityType newEntityType, IEntityType srcEntityType, RuntimeModelConvention convention)
    {
        _createInstantiationFunc ??= "Create".GetMethodFuncByExpression<RuntimeModelConvention, InstantiationBinding?, RuntimeEntityType, InstantiationBinding?>();

        newEntityType.ConstructorBinding = _createInstantiationFunc(convention, srcEntityType.ConstructorBinding, newEntityType);
        newEntityType.ServiceOnlyConstructorBinding =
            _createInstantiationFunc(convention, ((IRuntimeEntityType)srcEntityType).ServiceOnlyConstructorBinding, newEntityType);
    }

    private static void CreateForeignKey(RuntimeEntityType newEntityType, IForeignKey srcForeignKey, RuntimeModelConvention convention)
    {
        _createForeignKeyFunc ??= "Create".GetMethodFuncByExpression<RuntimeModelConvention, IForeignKey, RuntimeEntityType, RuntimeForeignKey>();
        _createNavigationFunc ??= "Create".GetMethodFuncByExpression<RuntimeModelConvention, INavigation, RuntimeForeignKey, RuntimeNavigation>();

        _processForeignKeyAnnotationsAction ??= "ProcessForeignKeyAnnotations".GetMethodActionByExpression<RuntimeModelConvention,
            Dictionary<string, object?>, IForeignKey, RuntimeForeignKey, bool>();

        _processNavigationAnnotationsAction ??= "ProcessNavigationAnnotations".GetMethodActionByExpression<RuntimeModelConvention,
            Dictionary<string, object?>, INavigation, RuntimeNavigation, bool>();

        var newForeignKey = _createForeignKeyFunc(convention, srcForeignKey, newEntityType);

        var srcNavigation = srcForeignKey.DependentToPrincipal;
        if (srcNavigation is not null)
        {
            var newNavigation = _createNavigationFunc(convention, srcNavigation, newForeignKey);

            CreateAnnotations(convention, srcNavigation, newNavigation,
                static (convention, annotations, source, target, runtime) =>
                    _processNavigationAnnotationsAction(convention, annotations, source, target, runtime));
        }

        srcNavigation = srcForeignKey.PrincipalToDependent;
        if (srcNavigation is not null)
        {
            var newNavigation = (srcNavigation.IsOnDependent ? newForeignKey.DeclaringEntityType : newForeignKey.PrincipalEntityType)
                .FindNavigation(srcNavigation.Name);

            if (newNavigation is null)
            {
                newNavigation = _createNavigationFunc(convention, srcNavigation, newForeignKey);
                CreateAnnotations(convention, srcNavigation, newNavigation,
                    static (convention, annotations, source, target, runtime) =>
                        _processNavigationAnnotationsAction(convention, annotations, source, target, runtime));
            }
        }

        CreateAnnotations(convention, srcForeignKey, newForeignKey,
            static (convention, annotations, source, target, runtime) =>
                _processForeignKeyAnnotationsAction(convention, annotations, source, target, runtime));
    }

    private static void CreateSkipNavigation(RuntimeEntityType newEntityType, ISkipNavigation srcSkipNavigation, RuntimeModelConvention convention)
    {
        _createSkipNavigationFunc ??= "Create".GetMethodFuncByExpression<RuntimeModelConvention,
            ISkipNavigation, RuntimeEntityType, RuntimeSkipNavigation>();

        _processSkipNavigationAnnotationsAction ??= "ProcessSkipNavigationAnnotations".GetMethodActionByExpression<RuntimeModelConvention,
            Dictionary<string, object?>, ISkipNavigation, RuntimeSkipNavigation, bool>();

        var newSkipNavigation = _createSkipNavigationFunc(convention, srcSkipNavigation, newEntityType);

        var inverse = newSkipNavigation.TargetEntityType.FindSkipNavigation(srcSkipNavigation.Inverse.Name);
        if (inverse != null)
        {
            newSkipNavigation.Inverse = inverse;
            inverse.Inverse = newSkipNavigation;
        }

        CreateAnnotations(convention, srcSkipNavigation, newSkipNavigation,
            static (convention, annotations, source, target, runtime) =>
                _processSkipNavigationAnnotationsAction(convention, annotations, source, target, runtime));
    }


    private static void CreateAnnotations<TSource, TTarget>(RuntimeModelConvention convention, TSource source, TTarget target,
        Action<RuntimeModelConvention, Dictionary<string, object?>, TSource, TTarget, bool> process)
        where TSource : IAnnotatable
        where TTarget : AnnotatableBase
    {
        var annotations = source.GetAnnotations().ToDictionary(a => a.Name, a => a.Value);
        process(convention, annotations, source, target, false);
        target.AddAnnotations(annotations);

        annotations = source.GetRuntimeAnnotations().ToDictionary(a => a.Name, a => a.Value);
        process(convention, annotations, source, target, true);
        target.AddRuntimeAnnotations(annotations);
    }


    private static void CreateTableMappings(RuntimeEntityType newRuntimeEntityType, EntityType newEntityType, IEntityType srcEntityType)
    {
        var srcTableMappings = (List<TableMapping>)srcEntityType.GetTableMappings();
        var newTableMappings = (List<TableMapping>)newEntityType.GetTableMappings();

        if (newTableMappings.Count == srcTableMappings.Count + 1)
        {
            // 移除分片实体表映射包含的第一项源实体表映射关系
            newTableMappings.RemoveAt(0);
        }

        if (newTableMappings.Count == srcTableMappings.Count)
        {
            // 填充表映射列集合
            for (var i = 0; i < srcTableMappings.Count; i++)
            {
                var newTableMapping = newTableMappings[i];
                var srcTableMapping = srcTableMappings[i];

                var newColumnMappings = ((Microsoft.EntityFrameworkCore.Metadata.ITableMapping)newTableMapping).ColumnMappings;
                var srcColumnMappings = ((Microsoft.EntityFrameworkCore.Metadata.ITableMapping)srcTableMapping).ColumnMappings;

                if (newColumnMappings.Count() != srcColumnMappings.Count())
                {
                    // 填充表映射列集合
                    foreach (var srcColumn in srcColumnMappings)
                    {
                        if (!newColumnMappings.Any(p => p.Column.Name == srcColumn.Column.Name))
                        {
                            var property = newEntityType.FindProperty(srcColumn.Column.Name);

                            var column = new Column(srcColumn.Column.Name, srcColumn.Property.GetColumnType(), (Table)newTableMapping.Table);
                            var columnMapping = new ColumnMapping(property!, column, newTableMapping);

                            newTableMapping.AddColumnMapping(columnMapping);
                        }
                    }
                }
            }
        }

        newRuntimeEntityType.AddRuntimeAnnotation(RelationalAnnotationNames.TableMappings, newTableMappings);
    }


    /// <summary>
    /// 使用来源实体填充新实体。
    /// </summary>
    /// <param name="newRuntimeEntityType">给定的 <see cref="RuntimeEntityType"/>。</param>
    /// <param name="newEntityType">给定的 <see cref="EntityType"/>。</param>
    /// <param name="srcEntityType">给定的 <see cref="IEntityType"/>。</param>
    /// <param name="convention">给定的 <see cref="RuntimeModelConvention"/>。</param>
    public static void PopulateTable(this RuntimeEntityType newRuntimeEntityType, EntityType newEntityType, IEntityType srcEntityType,
        RuntimeModelConvention convention)
    {
        CreateTableMappings(newRuntimeEntityType, newEntityType, srcEntityType);

        foreach (var srcProperty in srcEntityType.GetDeclaredProperties())
        {
            CreateProperty(newRuntimeEntityType, srcProperty, convention);
        }

        foreach (var srcServiceProperty in srcEntityType.GetDeclaredServiceProperties())
        {
            CreateServiceProperty(newRuntimeEntityType, srcServiceProperty, convention);
        }

        foreach (var srcComplexProperty in srcEntityType.GetDeclaredComplexProperties())
        {
            CreateComplexProperty(newRuntimeEntityType, srcComplexProperty, convention);
        }

        foreach (var srcKey in srcEntityType.GetDeclaredKeys())
        {
            CreateKey(newRuntimeEntityType, srcKey, convention);
        }

        foreach (var srcIndex in srcEntityType.GetDeclaredIndexes())
        {
            CreateIndex(newRuntimeEntityType, srcIndex, convention);
        }

        foreach (var srcTrigger in srcEntityType.GetDeclaredTriggers())
        {
            CreateTrigger(newRuntimeEntityType, srcTrigger, convention);
        }

        CreateInstantiation(newRuntimeEntityType, srcEntityType, convention);

        foreach (var srcForeignKey in srcEntityType.GetDeclaredForeignKeys())
        {
            CreateForeignKey(newRuntimeEntityType, srcForeignKey, convention);
        }

        foreach (var srcSkipNavigation in srcEntityType.GetDeclaredSkipNavigations())
        {
            CreateSkipNavigation(newRuntimeEntityType, srcSkipNavigation, convention);
        }

        _processEntityTypeAnnotationsAction ??= "ProcessEntityTypeAnnotations".GetMethodActionByExpression<RuntimeModelConvention,
            Dictionary<string, object?>, IEntityType, RuntimeEntityType, bool>();

        CreateAnnotations(convention, srcEntityType, newRuntimeEntityType,
            static (convention, annotations, source, target, runtime) =>
                _processEntityTypeAnnotationsAction(convention, annotations, source, target, runtime));
    }


    ///// <summary>
    ///// 复制实体类型。
    ///// </summary>
    ///// <param name="model">给定的 <see cref="Model"/>。</param>
    ///// <param name="srcType">给定要复制的源类型。</param>
    ///// <param name="newType">给定的新类型。</param>
    ///// <param name="newName">给定的新名称。</param>
    ///// <param name="otherEntityTypes">给定的其他复制实体类型集合。</param>
    ///// <returns>返回 <see cref="EntityType"/>。</returns>
    //public static EntityType? CopyEntityType(this Model model,
    //    Type srcType, Type newType, string newName, IEnumerable<EntityType>? otherEntityTypes)
    //{
    //    var srcEntityType = model.FindEntityType(srcType);
    //    if (srcEntityType is null) return null;

    //    var newEntityType = model.AddEntityType(newName, newType,
    //        srcEntityType.IsOwned(), srcEntityType.GetConfigurationSource());

    //    if (newEntityType is null) return null;

    //    // AddAnnotations
    //    newEntityType.AddAnnotations(srcEntityType.GetAnnotations());

    //    // AddCheckConstraints
    //    foreach (var srcCheckConstraint in ((IMutableEntityType)srcEntityType).GetCheckConstraints())
    //    {
    //        newEntityType.AddCheckConstraint(srcCheckConstraint.Name!, srcCheckConstraint.Sql);
    //    }

    //    // AddData
    //    var srcData = srcEntityType.GetRawSeedData();
    //    if (srcData is not null)
    //    {
    //        var newData = srcData.Select(src => ObjectMapper.NewByMapAllPublicProperties(src, newType)).ToArray();
    //        newEntityType.AddData(newData);
    //    }

    //    // AddIgnored
    //    foreach (var srcIgnored in srcEntityType.GetIgnoredMembers())
    //    {
    //        var configurationSource = srcEntityType.FindIgnoredConfigurationSource(srcIgnored) ?? srcEntityType.GetConfigurationSource();
    //        newEntityType.AddIgnored(srcIgnored, configurationSource);
    //    }

    //    // AddProperties
    //    var newProperties = new List<Property>();
    //    foreach (var srcProperty in srcEntityType.GetProperties())
    //    {
    //        var newProperty = newEntityType.AddProperty(srcProperty.Name, srcProperty.ClrType, srcProperty.GetTypeConfigurationSource(),
    //            srcProperty.GetConfigurationSource());

    //        if (newProperty is not null)
    //            newProperties.Add(newProperty);
    //    }

    //    // AddKeys
    //    var newKeys = new List<Key>();
    //    foreach (var srcKey in srcEntityType.GetKeys())
    //    {
    //        var newKeyProperties = srcKey.Properties.Select(src => newProperties.Single(s => s.Name == src.Name)).ToArray();
    //        var newKey = newEntityType.AddKey(newKeyProperties, srcKey.GetConfigurationSource());

    //        if (newKey is not null)
    //            newKeys.Add(newKey);
    //    }

    //    // AddIndexes
    //    var newIndexes = new List<Microsoft.EntityFrameworkCore.Metadata.Internal.Index>();
    //    foreach (var srcIndex in srcEntityType.GetIndexes())
    //    {
    //        var newIndexProperties = srcIndex.Properties.Select(src => newProperties.Single(s => s.Name == src.Name)).ToArray();
    //        var newIndex = newEntityType.AddIndex(newIndexProperties, srcIndex.GetConfigurationSource());

    //        if (newIndex is not null)
    //            newIndexes.Add(newIndex);
    //    }

    //    // AddForeignKeys
    //    var newForeignKeys = new List<ForeignKey>();
    //    foreach (var srcFKProperty in srcEntityType.ForeignKeyProperties)
    //    {
    //        if (srcFKProperty is not Property srcInternalFKProperty)
    //            continue;

    //        var newPrincipalKey = newEntityType.FindPrimaryKey();
    //        if (newPrincipalKey is null)
    //            continue;

    //        //var newFKProperty = srcEntityType == srcInternalFKProperty.GetElementType()
    //        //    ? newProperties.SingleOrDefault(s => s.Name == srcInternalFKProperty.Name) // 外键是自身实体类型
    //        //    : otherEntityTypes?.SingleOrDefault(s => s == srcInternalFKProperty.DeclaringEntityType)
    //        //        ?.FindProperty(srcInternalFKProperty.Name); // 外键是其他复制实体类型

    //        //if (newFKProperty is null)
    //        //{
    //        //    // 外键是普通实体类型
    //        //    newFKProperty = srcInternalFKProperty;
    //        //    //newFKProperty = new Property(srcFKProperty.Name, srcFKProperty.ClrType, srcFKProperty.PropertyInfo, srcFKProperty.FieldInfo,
    //        //    //    newEntityType, srcFKProperty.GetConfigurationSource(), srcFKProperty.GetTypeConfigurationSource());
    //        //}

    //        //var newForeignKey = newEntityType.AddForeignKey(newFKProperty, newPrincipalKey, newEntityType,
    //        //    srcInternalFKProperty.GetCommentConfigurationSource(), srcInternalFKProperty.GetConfigurationSource());

    //        //if (newForeignKey is not null)
    //        //    newForeignKeys.Add(newForeignKey);
    //    }

    //    // AddNavigations
    //    foreach (var srcNav in srcEntityType.GetNavigations())
    //    {
    //        var newForeignKey = newForeignKeys.FirstOrDefault(p => p.PrincipalKey.GetName() == srcNav.ForeignKey.PrincipalKey.GetName());
    //        if (newForeignKey is not null)
    //        {
    //            newEntityType.AddNavigation(srcNav.Name, newForeignKey,
    //                pointsToPrincipal: srcNav.DeclaringEntityType != srcNav.ForeignKey.DeclaringEntityType);
    //        }
    //    }

    //    // AddSkipNavigations
    //    //foreach (var srcSkipNav in srcEntityType.GetSkipNavigations())
    //    //{
    //    //    var newProperty = newProperties.SingleOrDefault(s => s.Name == srcSkipNav.Name);
    //    //    var targetEntityType = otherEntityTypes?.SingleOrDefault(s => s == srcSkipNav.TargetEntityType);

    //    //    if (targetEntityType is null)
    //    //        targetEntityType = model.FindEntityType(srcSkipNav.ClrType);

    //    //    newEntityType.AddSkipNavigation(srcSkipNav.Name, newProperty?.PropertyInfo, targetEntityType!,
    //    //        srcSkipNav.IsCollection, srcSkipNav.IsOnDependent, srcSkipNav.GetConfigurationSource());
    //    //}

    //    // AddServiceProperties
    //    //foreach (var srcServiceProperty in srcEntityType.GetServiceProperties())
    //    //{
    //    //    var newProperty = newProperties.Single(s => s.Name == srcServiceProperty.Name);
    //    //    newEntityType.AddServiceProperty(newProperty.PropertyInfo!, srcServiceProperty.GetConfigurationSource());
    //    //}

    //    // AddTriggers
    //    foreach (var srcTrigger in srcEntityType.GetDeclaredTriggers())
    //    {
    //        newEntityType.AddTrigger(srcTrigger.ModelName, srcTrigger.GetConfigurationSource());
    //    }

    //    return newEntityType;
    //}

#pragma warning restore EF1001 // Internal EF Core API usage.

}
