#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Internal;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Update.Internal;

namespace Librame.Extensions.Data.Resets;

//internal class MigrationsModelDifferReset : MigrationsModelDiffer
//{
//    public MigrationsModelDifferReset(IRelationalTypeMappingSource typeMappingSource,
//        IMigrationsAnnotationProvider migrationsAnnotationProvider,
//        IRowIdentityMapFactory rowIdentityMapFactory,
//        CommandBatchPreparerDependencies commandBatchPreparerDependencies)
//        : base(typeMappingSource, migrationsAnnotationProvider, rowIdentityMapFactory, commandBatchPreparerDependencies)
//    {
//    }


//    protected override IEnumerable<MigrationOperation> DiffCollection<T>(
//        IEnumerable<T> sources,
//        IEnumerable<T> targets,
//        DiffContext diffContext,
//        Func<T, T, DiffContext, IEnumerable<MigrationOperation>> diff,
//        Func<T, DiffContext, IEnumerable<MigrationOperation>> add,
//        Func<T, DiffContext, IEnumerable<MigrationOperation>> remove,
//        params Func<T, T, DiffContext, bool>[] predicates)
//    {
//        var sourceList = sources.ToList();
//        var targetList = targets.ToList();
//        var pairedList = new List<(T source, T target)>();

//        foreach (var predicate in predicates)
//        {
//            for (var i = sourceList.Count - 1; i >= 0; i--)
//            {
//                var source = sourceList[i];

//                for (var j = targetList.Count - 1; j >= 0; j--)
//                {
//                    var target = targetList[j];

//                    if (predicate(source, target, diffContext))
//                    {
//                        sourceList.RemoveAt(i);
//                        targetList.RemoveAt(j);
//                        pairedList.Add((source, target));
//                        diffContext.AddMapping(source, target);

//                        break;
//                    }
//                }
//            }
//        }

//        foreach (var (source, target) in pairedList)
//        {
//            foreach (var operation in diff(source, target, diffContext))
//            {
//                yield return operation;
//            }
//        }

//        foreach (var source in sourceList)
//        {
//            foreach (var operation in remove(source, diffContext))
//            {
//                yield return operation;
//            }
//        }

//        foreach (var target in targetList)
//        {
//            foreach (var operation in add(target, diffContext))
//            {
//                yield return operation;
//            }
//        }
//    }

//    protected override IEnumerable<MigrationOperation> Diff(
//        ITable source,
//        ITable target,
//        DiffContext diffContext)
//    {
//        if (source.IsExcludedFromMigrations
//            && target.IsExcludedFromMigrations)
//        {
//            // Populate column mapping
//            foreach (var _ in Diff(source.Columns, target.Columns, diffContext))
//            {
//            }

//            yield break;
//        }

//        if (source.Schema != target.Schema
//            || source.Name != target.Name)
//        {
//            //if (source.)
//            //{
//            //    foreach (var operation in Add(target, diffContext))
//            //    {
//            //        yield return operation;
//            //    }
//            //}
//            //else
//            //{
//                var renameTableOperation = new RenameTableOperation
//                {
//                    Schema = source.Schema,
//                    Name = source.Name,
//                    NewSchema = target.Schema,
//                    NewName = target.Name
//                };

//                renameTableOperation.AddAnnotations(MigrationsAnnotationProvider.ForRename(source));

//                yield return renameTableOperation;
//            //}
//        }

//        var sourceMigrationsAnnotations = source.GetAnnotations();
//        var targetMigrationsAnnotations = target.GetAnnotations();

//        if (source.Comment != target.Comment
//            || HasDifferences(sourceMigrationsAnnotations, targetMigrationsAnnotations))
//        {
//            var alterTableOperation = new AlterTableOperation
//            {
//                Name = target.Name,
//                Schema = target.Schema,
//                Comment = target.Comment,
//                OldTable = { Comment = source.Comment }
//            };

//            alterTableOperation.AddAnnotations(targetMigrationsAnnotations);
//            alterTableOperation.OldTable.AddAnnotations(sourceMigrationsAnnotations);

//            yield return alterTableOperation;
//        }

//        var operations = Diff(source.Columns, target.Columns, diffContext)
//            .Concat(Diff(source.UniqueConstraints, target.UniqueConstraints, diffContext))
//            .Concat(Diff(source.Indexes, target.Indexes, diffContext))
//            .Concat(Diff(source.CheckConstraints, target.CheckConstraints, diffContext));

//        foreach (var operation in operations)
//        {
//            yield return operation;
//        }
//    }

//}
