#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Data.Sharding;
using Librame.Extensions.Setting;

namespace Librame.Extensions.Data.Saving;

/// <summary>
/// 定义继承 <see cref="AbstractSavingChangesHandler"/> 的分片保存变化处理程序。
/// </summary>
public sealed class ShardingSavingChangesHandler : AbstractSavingChangesHandler
{
    private static Func<DbContext, object, EntityEntry>? _addFunc;

    private static Func<Model, SortedDictionary<string, EntityType>>? _entityTypesFunc;
    private static Func<Model, Dictionary<Type, (ConfigurationSource ConfigurationSource, SortedSet<EntityType> Types)>>? _sharedTypesFunc;

    private static Func<Model, IList<IModelFinalizedConvention>?>? _modelFinalizedConventionsFunc;

    private static Func<RuntimeModelConvention, IEntityType, RuntimeModel, RuntimeEntityType>? _createRuntimeEntityTypeFunc;

    private static Func<EntityType, SortedDictionary<IReadOnlyList<IReadOnlyProperty>, Key>>? _entityKeysFunc;

    private static Func<TypeBase, SortedSet<TypeBase>>? _directlyDerivedTypesFunc;

    private static Action<EntityType, ConfigurationSource>? _updateBaseTypeConfigurationSourceAction;

    private static FieldInfo? _baseTypeInfo;


    /// <summary>
    /// 预处理保存上下文。
    /// </summary>
    /// <param name="context">给定的 <see cref="ISavingChangesContext"/>。</param>
    protected override void PreHandlingCore(ISavingChangesContext context)
    {
        var descriptorSettings = context.DataContext.CurrentServices.
            ShardingContext.ShardingTables(context.DataContext);

        if (descriptorSettings is null) return;

        // 将分表类型与映射对象添加到上下文
        AddContext(context.DataContext, descriptorSettings);
    }

    private static void AddContext(DataContext context,
        Dictionary<ShardingDescriptor, List<ShardingItemSetting>> descriptorSettings)
    {

#pragma warning disable EF1001 // Internal EF Core API usage.

        var model = (RuntimeModel)context.Model;
        var relationalModel = (RelationalModel)context.Model.GetRelationalModel();

        var modelSource = (DataModelSource)context.GetService<IModelSource>();
        var mutableModel = modelSource.CurrentMutableModel;
        if (mutableModel is null) return;

        var stateManager = context.GetDependencies().StateManager;

        var typeMappingSource = (IRelationalTypeMappingSource)context.GetService<ProviderConventionSetBuilderDependencies>().TypeMappingSource;
        var annotationProvider = context.GetService<RelationalModelRuntimeInitializerDependencies>().RelationalAnnotationProvider;

        _addFunc ??= "Add".GetMethodFuncByExpression<DbContext, object, EntityEntry>();

        _entityTypesFunc ??= "_entityTypes".GetFieldFuncByExpression<Model, SortedDictionary<string, EntityType>>();
        _sharedTypesFunc ??= "_sharedTypes".GetFieldFuncByExpression<Model, Dictionary<Type, (ConfigurationSource ConfigurationSource, SortedSet<EntityType> Types)>>();
        _modelFinalizedConventionsFunc ??= "_modelFinalizedConventions".GetFieldFuncByExpression<Model, IList<IModelFinalizedConvention>?>();
        _createRuntimeEntityTypeFunc ??= "Create".GetMethodFuncByExpression<RuntimeModelConvention, IEntityType, RuntimeModel, RuntimeEntityType>();

        _entityKeysFunc ??= "_keys".GetFieldFuncByExpression<EntityType, SortedDictionary<IReadOnlyList<IReadOnlyProperty>, Key>>();

        var runtimeModelConvention = _modelFinalizedConventionsFunc(mutableModel)?.OfType<RuntimeModelConvention>().FirstOrDefault();
        if (runtimeModelConvention is null) return;

        var entityTypes = _entityTypesFunc(mutableModel);
        var sharedTypes = _sharedTypesFunc(mutableModel);

        var dbSetMethod = context.ContextType.GetMethod(nameof(DbContext.Set), Type.EmptyTypes)!;

        foreach (var (descriptor, settings) in descriptorSettings)
        {
            foreach (var setting in settings)
            {
                var srcEntityType = mutableModel.FindEntityType(descriptor.SourceType)!;

                var configurationSource = (srcEntityType.IsOwned()
                    ? mutableModel.FindIsOwnedConfigurationSource(descriptor.SourceType)
                    : mutableModel.FindIsSharedConfigurationSource(descriptor.SourceType)
                    ) ?? ConfigurationSource.Explicit;

                var shardedType = setting.ShardedType!.GenericTypeArguments.First();

                var shardedEntityType = new EntityType(shardedType, mutableModel, srcEntityType.IsOwned(), configurationSource);

                SetBaseType(shardedEntityType, srcEntityType, configurationSource);

                AddMutableModel(shardedEntityType, entityTypes, sharedTypes, mutableModel);

                var entityKeys = _entityKeysFunc(shardedEntityType);

                AddPrimaryKey(shardedEntityType, srcEntityType, entityKeys);

                relationalModel.PopulateTable(shardedEntityType, srcEntityType, configurationSource, annotationProvider, typeMappingSource);

                // Create RuntimeEntityType
                var shardedRuntimeEntityType = _createRuntimeEntityTypeFunc(runtimeModelConvention, shardedEntityType, model);

                shardedRuntimeEntityType.PopulateTable(shardedEntityType, srcEntityType, runtimeModelConvention);

                var entry = _addFunc(context, setting.Sharded!);

                var props = shardedEntityType.GetFlattenedProperties();
            }
        }

#pragma warning restore EF1001 // Internal EF Core API usage.

    }


#pragma warning disable EF1001 // Internal EF Core API usage.

    private static void AddPrimaryKey(EntityType newEntityType, EntityType srcEntityType,
        SortedDictionary<IReadOnlyList<IReadOnlyProperty>, Key> entityKeys)
    {
        //mutableModel.Builder.Entity(shardedType, configurationSource ?? ConfigurationSource.Explicit);

        var srcKey = srcEntityType.FindPrimaryKey();
        if (srcKey is null) return;

        var newKeyProperties = newEntityType.GetProperties()
            .Where(p => srcKey.Properties.Any(op => op.Name == p.Name))
            .ToArray();

        var newKey = new Key(newKeyProperties, newEntityType.GetConfigurationSource());
        entityKeys.Add(newKeyProperties, newKey);
    }

    private static EntityType? AddMutableModel(EntityType newEntityType, SortedDictionary<string, EntityType> entityTypes,
        Dictionary<Type, (ConfigurationSource ConfigurationSource, SortedSet<EntityType> Types)> sharedTypes, Model model)
    {
        var entityTypeName = newEntityType.Name;
        if (entityTypes.ContainsKey(entityTypeName))
        {
            throw new InvalidOperationException(CoreStrings.DuplicateEntityType(newEntityType.DisplayName()));
        }

        if (newEntityType.HasSharedClrType)
        {
            if (entityTypes.Any(et => !et.Value.HasSharedClrType && et.Value.ClrType == newEntityType.ClrType))
            {
                throw new InvalidOperationException(
                    CoreStrings.ClashingNonSharedType(newEntityType.Name, model.GetDisplayName(newEntityType.ClrType)));
            }

            if (sharedTypes.TryGetValue(newEntityType.ClrType, out var existingTypes))
            {
                var newConfigurationSource = newEntityType.GetConfigurationSource().Max(existingTypes.ConfigurationSource);
                existingTypes.Types.Add(newEntityType);
                sharedTypes[newEntityType.ClrType] = (newConfigurationSource, existingTypes.Types);
            }
            else
            {
                var types = new SortedSet<EntityType>(TypeBaseNameComparer.Instance) { newEntityType };
                sharedTypes.Add(newEntityType.ClrType, (newEntityType.GetConfigurationSource(), types));
            }
        }
        else if (sharedTypes.ContainsKey(newEntityType.ClrType))
        {
            throw new InvalidOperationException(CoreStrings.ClashingSharedType(newEntityType.DisplayName()));
        }

        entityTypes.Add(entityTypeName, newEntityType);

        return newEntityType;

        //return (EntityType?)model.ConventionDispatcher.OnEntityTypeAdded(entityType.Builder)?.Metadata;
    }

    private static EntityType? SetBaseType(EntityType newEntityType, EntityType srcEntityType,
        ConfigurationSource configurationSource)
    {
        //PropertyBase propertyBase = (from p in srcEntityType.GetMembers()
        //                             select p.Name).SelectMany(FindMembersInHierarchy).FirstOrDefault();
        //if (propertyBase is not null)
        //{
        //    PropertyBase propertyBase2 = newBaseType.FindMembersInHierarchy(propertyBase.Name).Single();
        //    throw new InvalidOperationException(CoreStrings.DuplicatePropertiesOnBase(DisplayName(), newBaseType.DisplayName(), propertyBase.DeclaringType.DisplayName(), propertyBase.Name, propertyBase2.DeclaringType.DisplayName(), propertyBase2.Name));
        //}

        _directlyDerivedTypesFunc ??= "_directlyDerivedTypes".GetFieldFuncByExpression<TypeBase, SortedSet<TypeBase>>();

        _updateBaseTypeConfigurationSourceAction ??= "UpdateBaseTypeConfigurationSource".GetMethodActionByExpression<EntityType, ConfigurationSource>();

        _baseTypeInfo ??= typeof(TypeBase).GetField("_baseType", TypeExtensions.AllMemberFlagsWithStatic);
        _baseTypeInfo?.SetValue(newEntityType, srcEntityType);

        var srcDirectlyDerivedTypes = _directlyDerivedTypesFunc(srcEntityType);
        srcDirectlyDerivedTypes.Add(newEntityType);

        _updateBaseTypeConfigurationSourceAction(newEntityType, configurationSource);
        srcEntityType?.UpdateConfigurationSource(configurationSource);

        return newEntityType;

        //return (EntityType)Model.ConventionDispatcher.OnEntityTypeBaseTypeChanged(Builder, newBaseType, baseType);
    }

#pragma warning restore EF1001 // Internal EF Core API usage.

}
