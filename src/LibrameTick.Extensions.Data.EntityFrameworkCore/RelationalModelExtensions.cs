#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Librame.Extensions.Data;

/// <summary>
/// 定义 <see cref="RelationalModel"/> 静态扩展。
/// </summary>
public static class RelationalModelExtensions
{
    private static Func<AnnotatableBase, SortedDictionary<string, Annotation>>? _annotationsFunc;

    private static Action<RelationalModel,
        RelationalModel, IEntityType, IRelationalTypeMappingSource>? _addDefaultMappingsAction;

    private static Action<RelationalModel,
        RelationalModel, IEntityType, IRelationalTypeMappingSource>? _addTablesAction;

    private static Action<RelationalModel,
        RelationalModel, IEntityType, IRelationalTypeMappingSource>? _addViewsAction;

    private static Action<RelationalModel,
        RelationalModel, IEntityType>? _addSqlQueriesAction;

    private static Action<RelationalModel,
        RelationalModel, IEntityType>? _addMappedFunctionsAction;

    private static Action<RelationalModel,
        RelationalModel, IEntityType, IRelationalTypeMappingSource>? _addStoredProceduresAction;

    private static Action<RelationalModel, TableBase>? _populateRowInternalForeignKeysAction;
    private static Action<RelationalModel, Table, bool>? _populateTableConfigurationAction;
    private static Action<RelationalModel, Table>? _populateForeignKeyConstraints;


#pragma warning disable EF1001 // Internal EF Core API usage.

    /// <summary>
    /// 填充实体类型。
    /// </summary>
    /// <param name="model"></param>
    /// <param name="newEntityType"></param>
    /// <param name="srcEntityType"></param>
    /// <param name="configurationSource"></param>
    /// <param name="annotationProvider"></param>
    /// <param name="typeMappingSource"></param>
    public static void PopulateTable(this RelationalModel model, EntityType newEntityType, EntityType srcEntityType,
        ConfigurationSource configurationSource, IRelationalAnnotationProvider annotationProvider, IRelationalTypeMappingSource typeMappingSource)
    {
        _addDefaultMappingsAction ??= "AddDefaultMappings".GetMethodActionByExpression<RelationalModel,
            RelationalModel, IEntityType, IRelationalTypeMappingSource>();

        _addTablesAction ??= "AddTables".GetMethodActionByExpression<RelationalModel,
            RelationalModel, IEntityType, IRelationalTypeMappingSource>();

        _addViewsAction ??= "AddViews".GetMethodActionByExpression<RelationalModel,
            RelationalModel, IEntityType, IRelationalTypeMappingSource>();

        _addSqlQueriesAction ??= "AddSqlQueries".GetMethodActionByExpression<RelationalModel,
            RelationalModel, IEntityType>();

        _addMappedFunctionsAction ??= "AddMappedFunctions".GetMethodActionByExpression<RelationalModel,
            RelationalModel, IEntityType>();

        _addStoredProceduresAction ??= "AddStoredProcedures".GetMethodActionByExpression<RelationalModel,
            RelationalModel, IEntityType, IRelationalTypeMappingSource>();

        _addDefaultMappingsAction(model, model, newEntityType, typeMappingSource);

        RepairDefaultMappings(model, newEntityType, srcEntityType, configurationSource);

        var mappedTableName = newEntityType.GetTableName();
        var mappedSchema = newEntityType.GetSchema();

        _addTablesAction(model, model, newEntityType, typeMappingSource);
        _addViewsAction(model, model, newEntityType, typeMappingSource);
        _addSqlQueriesAction(model, model, newEntityType);
        _addMappedFunctionsAction(model, model, newEntityType);
        _addStoredProceduresAction(model, model, newEntityType, typeMappingSource);

        if (model.Tables.TryGetValue((newEntityType.GetTableName()!, newEntityType.GetSchema()), out var newTable))
        {
            var srcTable = model.Tables.GetValueOrDefault((srcEntityType.GetTableName()!, srcEntityType.GetSchema()))!;

            PopulateTable(model, annotationProvider, newTable, srcTable, designTime: false);
        }
    }

    private static void RepairDefaultMappings(RelationalModel model, EntityType newEntityType, EntityType srcEntityType,
        ConfigurationSource configurationSource)
    {
        // Set TableName & Schema
        _annotationsFunc ??= "_annotations".GetFieldFuncByExpression<AnnotatableBase, SortedDictionary<string, Annotation>>();

        var annotations = _annotationsFunc(newEntityType);
        annotations ??= new SortedDictionary<string, Annotation>(StringComparer.Ordinal);

        annotations[RelationalAnnotationNames.TableName] = new ConventionAnnotation(RelationalAnnotationNames.TableName,
            newEntityType.Name, configurationSource);

        annotations[RelationalAnnotationNames.Schema] = new ConventionAnnotation(RelationalAnnotationNames.Schema,
            srcEntityType.GetSchema(), configurationSource);

        // Populate DefaultMappings
        var isTpc = newEntityType.GetMappingStrategy() == RelationalAnnotationNames.TpcMappingStrategy;
        var isTph = ((IEntityType)newEntityType).FindDiscriminatorProperty() != null;

        var mappedTableName = isTph ? newEntityType.GetRootType().Name : newEntityType.Name;
        var defaultTable = model.DefaultTables.GetValueOrDefault(mappedTableName)!;

        var tableMappings = (TableMappingBase<ColumnMappingBase>)defaultTable.EntityTypeMappings.First();

        foreach (var property in newEntityType.GetProperties())
        {
            // 修复因基类型（即源类型）不等于分片类型导致跳过除主键外的其他属性列创建（property.DeclaringType == newEntityType）
            //var columnName = ((IReadOnlyProperty)property).IsPrimaryKey() || isTpc || isTph || property.DeclaringType == newEntityType
            //    ? property.GetColumnName()
            //    : null;

            // 跳过已创建的主键列
            var columnName = !((IReadOnlyProperty)property).IsPrimaryKey() || isTpc || isTph
                ? property.GetColumnName()
                : null;

            if (columnName == null)
            {
                continue;
            }

            var column = (ColumnBase<ColumnMappingBase>?)defaultTable.FindColumn(columnName);
            if (column == null)
            {
                column = new ColumnBase<ColumnMappingBase>(columnName, property.GetColumnType(), defaultTable)
                {
                    IsNullable = property.IsColumnNullable()
                };
                defaultTable.Columns.Add(columnName, column);
            }
            else if (!property.IsColumnNullable())
            {
                column.IsNullable = false;
            }

            RelationalModel.CreateColumnMapping(column, property, tableMappings);
        }
    }

    private static void PopulateTable(RelationalModel model, IRelationalAnnotationProvider annotationProvider,
        Table newTable, Table srcTable, bool designTime)
    {
        _populateRowInternalForeignKeysAction ??= "PopulateRowInternalForeignKeys".GetMethodActionByExpression<RelationalModel,
            TableBase>(genTypes => [typeof(ColumnMapping)]);

        _populateTableConfigurationAction ??= "PopulateTableConfiguration".GetMethodActionByExpression<RelationalModel, Table, bool>();

        _populateForeignKeyConstraints ??= "PopulateForeignKeyConstraints".GetMethodActionByExpression<RelationalModel, Table>();

        _populateRowInternalForeignKeysAction(model, newTable);
        _populateTableConfigurationAction(model, newTable, designTime);

        if (annotationProvider is not null)
        {
            //PopulateAnnotations(annotationProvider, newTable.Columns, srcTable.Columns, designTime);

            foreach (Column column in newTable.Columns.Values)
            {
                column.AddAnnotations(annotationProvider.For(column, designTime));
            }

            foreach (var constraint in newTable.UniqueConstraints.Values)
            {
                constraint.AddAnnotations(annotationProvider.For(constraint, designTime));
            }

            foreach (var index in newTable.Indexes.Values)
            {
                index.AddAnnotations(annotationProvider.For(index, designTime));
            }

            if (designTime)
            {
                foreach (var checkConstraint in newTable.CheckConstraints.Values)
                {
                    checkConstraint.AddAnnotations(annotationProvider.For(checkConstraint, designTime));
                }
            }

            foreach (var trigger in newTable.Triggers.Values)
            {
                ((AnnotatableBase)trigger).AddAnnotations(annotationProvider.For(trigger, designTime));
            }
        }

        _populateForeignKeyConstraints(model, newTable);

        if (annotationProvider != null)
        {
            foreach (var constraint in newTable.ForeignKeyConstraints)
            {
                constraint.AddAnnotations(annotationProvider.For(constraint, designTime));
            }

            newTable.AddAnnotations(annotationProvider.For(newTable, designTime));
        }
    }

    //private static void PopulateAnnotations<TAnnotation>(IRelationalAnnotationProvider annotationProvider,
    //    SortedDictionary<string, TAnnotation> newDict, SortedDictionary<string, TAnnotation> srcDict, bool designTime)
    //    where TAnnotation : IAnnotatable
    //{
    //    var count = srcDict.Count();

    //    for (var i = 0; i < count; i++)
    //    {

    //    }

    //    //foreach (Column column in newTable.Columns.Values)
    //    //{
    //    //    column.AddAnnotations(annotationProvider.For(column, designTime));
    //    //}
    //}

#pragma warning restore EF1001 // Internal EF Core API usage.

}
