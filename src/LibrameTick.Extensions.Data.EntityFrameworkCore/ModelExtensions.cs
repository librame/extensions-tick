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

namespace Librame.Extensions.Data;

/// <summary>
/// 定义 <see cref="Model"/> 静态扩展。
/// </summary>
public static class ModelExtensions
{

#pragma warning disable EF1001 // Internal EF Core API usage.

    /// <summary>
    /// 复制实体类型。
    /// </summary>
    /// <typeparam name="TModel">指定的模型类型。</typeparam>
    /// <param name="model">给定的 <typeparamref name="TModel"/>。</param>
    /// <param name="sourceType">给定要复制的源类型。</param>
    /// <param name="newType">给定的新类型。</param>
    /// <param name="otherEntityTypes">给定的其他复制实体类型集合。</param>
    /// <returns>返回 <see cref="EntityType"/>。</returns>
    public static EntityType? CopyEntityType<TModel>(this TModel model, Type sourceType, Type newType,
        IEnumerable<EntityType>? otherEntityTypes)
        where TModel : Model
    {
        var srcEntityType = model.FindEntityType(sourceType);
        if (srcEntityType is null)
            return null;

        var newEntityType = model.AddEntityType(newType, srcEntityType.IsOwned(), srcEntityType.GetConfigurationSource());
        if (newEntityType is null)
            return null;

        // AddAnnotations
        newEntityType.AddAnnotations(srcEntityType.GetAnnotations());

        // AddCheckConstraints
        foreach (var srcCheckConstraint in ((IMutableEntityType)srcEntityType).GetCheckConstraints())
        {
            newEntityType.AddCheckConstraint(srcCheckConstraint.Name!, srcCheckConstraint.Sql);
        }

        // AddData
        var srcData = srcEntityType.GetRawSeedData();
        if (srcData is not null)
        {
            var newData = srcData.Select(src => ObjectMapper.NewByMapAllPublicProperties(src, newType)).ToArray();
            newEntityType.AddData(newData);
        }

        // AddIgnored
        foreach (var srcIgnored in srcEntityType.GetIgnoredMembers())
        {
            var configurationSource = srcEntityType.FindIgnoredConfigurationSource(srcIgnored) ?? srcEntityType.GetConfigurationSource();
            newEntityType.AddIgnored(srcIgnored, configurationSource);
        }

        // AddProperties
        var newProperties = new List<Property>();
        foreach (var srcProperty in srcEntityType.GetProperties())
        {
            var newProperty = newEntityType.AddProperty(srcProperty.Name, srcProperty.ClrType, srcProperty.GetTypeConfigurationSource(),
                srcProperty.GetConfigurationSource());

            if (newProperty is not null)
                newProperties.Add(newProperty);
        }

        // AddKeys
        var newKeys = new List<Key>();
        foreach (var srcKey in srcEntityType.GetKeys())
        {
            var newKeyProperties = srcKey.Properties.Select(src => newProperties.Single(s => s.Name == src.Name)).ToArray();
            var newKey = newEntityType.AddKey(newKeyProperties, srcKey.GetConfigurationSource());

            if (newKey is not null)
                newKeys.Add(newKey);
        }

        // AddIndexes
        var newIndexes = new List<Microsoft.EntityFrameworkCore.Metadata.Internal.Index>();
        foreach (var srcIndex in srcEntityType.GetIndexes())
        {
            var newIndexProperties = srcIndex.Properties.Select(src => newProperties.Single(s => s.Name == src.Name)).ToArray();
            var newIndex = newEntityType.AddIndex(newIndexProperties, srcIndex.GetConfigurationSource());

            if (newIndex is not null)
                newIndexes.Add(newIndex);
        }

        // AddForeignKeys
        var newForeignKeys = new List<ForeignKey>();
        foreach (var srcFKProperty in srcEntityType.ForeignKeyProperties)
        {
            if (srcFKProperty is not Property srcInternalFKProperty)
                continue;

            var newPrincipalKey = newEntityType.FindPrimaryKey();
            if (newPrincipalKey is null)
                continue;

            //var newFKProperty = srcEntityType == srcInternalFKProperty.GetElementType()
            //    ? newProperties.SingleOrDefault(s => s.Name == srcInternalFKProperty.Name) // 外键是自身实体类型
            //    : otherEntityTypes?.SingleOrDefault(s => s == srcInternalFKProperty.DeclaringEntityType)
            //        ?.FindProperty(srcInternalFKProperty.Name); // 外键是其他复制实体类型

            //if (newFKProperty is null)
            //{
            //    // 外键是普通实体类型
            //    newFKProperty = srcInternalFKProperty;
            //    //newFKProperty = new Property(srcFKProperty.Name, srcFKProperty.ClrType, srcFKProperty.PropertyInfo, srcFKProperty.FieldInfo,
            //    //    newEntityType, srcFKProperty.GetConfigurationSource(), srcFKProperty.GetTypeConfigurationSource());
            //}

            //var newForeignKey = newEntityType.AddForeignKey(newFKProperty, newPrincipalKey, newEntityType,
            //    srcInternalFKProperty.GetCommentConfigurationSource(), srcInternalFKProperty.GetConfigurationSource());
            
            //if (newForeignKey is not null)
            //    newForeignKeys.Add(newForeignKey);
        }

        // AddNavigations
        foreach (var srcNav in srcEntityType.GetNavigations())
        {
            var newForeignKey = newForeignKeys.FirstOrDefault(p => p.PrincipalKey.GetName() == srcNav.ForeignKey.PrincipalKey.GetName());
            if (newForeignKey is not null)
            {
                newEntityType.AddNavigation(srcNav.Name, newForeignKey,
                    pointsToPrincipal: srcNav.DeclaringEntityType != srcNav.ForeignKey.DeclaringEntityType);
            }
        }

        // AddSkipNavigations
        //foreach (var srcSkipNav in srcEntityType.GetSkipNavigations())
        //{
        //    var newProperty = newProperties.SingleOrDefault(s => s.Name == srcSkipNav.Name);
        //    var targetEntityType = otherEntityTypes?.SingleOrDefault(s => s == srcSkipNav.TargetEntityType);

        //    if (targetEntityType is null)
        //        targetEntityType = model.FindEntityType(srcSkipNav.ClrType);

        //    newEntityType.AddSkipNavigation(srcSkipNav.Name, newProperty?.PropertyInfo, targetEntityType!,
        //        srcSkipNav.IsCollection, srcSkipNav.IsOnDependent, srcSkipNav.GetConfigurationSource());
        //}

        // AddServiceProperties
        //foreach (var srcServiceProperty in srcEntityType.GetServiceProperties())
        //{
        //    var newProperty = newProperties.Single(s => s.Name == srcServiceProperty.Name);
        //    newEntityType.AddServiceProperty(newProperty.PropertyInfo!, srcServiceProperty.GetConfigurationSource());
        //}

        // AddTriggers
        foreach (var srcTrigger in srcEntityType.GetDeclaredTriggers())
        {
            newEntityType.AddTrigger(srcTrigger.ModelName, srcTrigger.GetConfigurationSource());
        }

        return newEntityType;
    }

#pragma warning restore EF1001 // Internal EF Core API usage.

}
