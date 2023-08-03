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
/// 定义 <see cref="EntityType"/> 静态扩展。
/// </summary>
public static class EntityTypeExtensions
{

    public static TModel CopyEntityType<TModel>(this TModel model, Type sourceType, Type newType)
        where TModel : Model
    {
        var sourceEntityType = model.FindEntityType(sourceType);
        if (sourceEntityType is null)
            return model;

        var newEntityType = model.AddEntityType(newType, sourceEntityType.IsOwned(), sourceEntityType.GetConfigurationSource())!;

        newEntityType.AddAnnotations(sourceEntityType.GetAnnotations());

        foreach (var property in sourceEntityType.ForeignKeyProperties)
        {
            newEntityType.AddForeignKey();
        }

        newEntityType.AddCheckConstraint();
        newEntityType.AddData();
        newEntityType.AddIgnored();
        newEntityType.AddIndex();
        newEntityType.AddKey();
        newEntityType.AddNavigation();
        newEntityType.AddProperty();
        newEntityType.AddServiceProperty();
        newEntityType.AddSkipNavigation();
        newEntityType.AddTrigger();

        return model;
    }

}
