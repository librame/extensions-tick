#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions;

/// <summary>
/// 定义 Emit 静态扩展。
/// </summary>
public static class EmitExtensions
{
    private const string _getPropertyMethodNamePrefix = "get_";
    private const string _setPropertyMethodNamePrefix = "set_";
    private static readonly ConstructorInfo _defaultObjectConstructor = typeof(object).GetConstructor(Type.EmptyTypes)!;
    private static readonly Regex _regBackingField = new("(?<__backingFieldName>k__BackingField)$");


    /// <summary>
    /// 根据来源类型复制出新类型集合。
    /// </summary>
    /// <param name="moduleBuilder">给定的 <see cref="ModuleBuilder"/>。</param>
    /// <param name="sourceType">给定的来源类型。</param>
    /// <param name="newTypeNames">给定的新类型名称。</param>
    /// <returns>返回新类型集合。</returns>
    public static IEnumerable<Type> CopyTypes(this ModuleBuilder moduleBuilder, Type sourceType, IEnumerable<string> newTypeNames)
    {
        foreach (var name in newTypeNames)
        {
            yield return moduleBuilder.CopyType(sourceType, name);
        }
    }

    /// <summary>
    /// 根据来源类型复制出新类型。
    /// </summary>
    /// <param name="moduleBuilder">给定的 <see cref="ModuleBuilder"/>。</param>
    /// <param name="sourceType">给定的来源类型。</param>
    /// <param name="newTypeName">给定的新类型名称。</param>
    /// <returns>返回新类型。</returns>
    public static Type CopyType(this ModuleBuilder moduleBuilder, Type sourceType, string newTypeName)
    {
        var typeBuilder = moduleBuilder.DefineType(newTypeName, sourceType.Attributes,
            sourceType.BaseType, sourceType.GetInterfaces());

        var sourceTypeInfo = sourceType.GetTypeInfo();

        // 定义属性字段
        var propertyFields = new Dictionary<string, FieldBuilder>();

        // 复制字段
        foreach (var field in sourceTypeInfo.DeclaredFields)
        {
            var fieldBuilder = typeBuilder.DefineField(field.Name, field.FieldType, field.Attributes);

            if (field.CustomAttributes.Any())
            {
                foreach (var attributeData in field.CustomAttributes)
                {
                    fieldBuilder.SetCustomAttribute(CreateCustomAttributeBuilder(attributeData));
                }
            }

            // 提取属性私有字段
            if (_regBackingField.IsMatch(field.Name))
            {
                var fieldName = _regBackingField.Replace(field.Name, string.Empty);
                fieldName = fieldName[1..(fieldName.Length - 1)];

                propertyFields.Add(fieldName, fieldBuilder);
            }
            else
            {
                propertyFields.Add(field.Name, fieldBuilder);
            }
        }

        // 复制构造函数
        foreach (var ctor in sourceTypeInfo.DeclaredConstructors)
        {
            var ctorParamTypes = ctor.GetParameters().Select(static p => p.ParameterType).ToArray();
            var ctorBuilder = typeBuilder.DefineConstructor(ctor.Attributes, ctor.CallingConvention, ctorParamTypes);

            if (ctor.CustomAttributes.Any())
            {
                foreach (var attributeData in ctor.CustomAttributes)
                {
                    ctorBuilder.SetCustomAttribute(CreateCustomAttributeBuilder(attributeData));
                }
            }

            var ctorIL = ctorBuilder.GetILGenerator();

            ctorIL.Emit(OpCodes.Ldarg_0);

            var baseCtor = sourceType.BaseType?.GetConstructor(ctorParamTypes);
            if (baseCtor is not null)
                ctorIL.Emit(OpCodes.Call, baseCtor);
            else
                ctorIL.Emit(OpCodes.Call, _defaultObjectConstructor);

            ctorIL.Emit(OpCodes.Ret);
        }

        Type[] parameterTypes = [typeof(int)];

        var canReadOrWriteProperties = sourceTypeInfo.DeclaredProperties.Where(p => p.CanRead || p.CanWrite).ToArray();
        var getPropertyMethods = new Dictionary<string, MethodBuilder>();
        var setPropertyMethods = new Dictionary<string, MethodBuilder>();

        // 复制方法
        foreach (var method in sourceTypeInfo.DeclaredMethods)
        {
            var methodParamTypes = method.GetParameters().Select(static p => p.ParameterType).ToArray();

            // 判定是否为属性方法
            var propertyMethodName = method.Name[_getPropertyMethodNamePrefix.Length..];

            // GET 属性方法
            if (method.Name.StartsWith(_getPropertyMethodNamePrefix) && canReadOrWriteProperties.Any(p
                => p.Name.Equals(propertyMethodName, StringComparison.Ordinal)))
            {
                var getMethodBuilder = typeBuilder.DefineMethod(method.Name, method.Attributes, method.ReturnType, Type.EmptyTypes);
                var getMethodIL = getMethodBuilder.GetILGenerator();

                getPropertyMethods.Add(propertyMethodName, getMethodBuilder);

                if (!propertyFields.TryGetValue(propertyMethodName, out var fieldBuilder))
                {
                    // 尝试匹配普通字段
                    var matchFields = sourceTypeInfo.DeclaredFields.Where(f => f.FieldType == method.ReturnType).ToArray();

                    var matchField = matchFields.Length > 1
                        ? matchFields.FirstOrDefault(p => p.Name.Contains(propertyMethodName, StringComparison.OrdinalIgnoreCase))
                        : matchFields.FirstOrDefault();

                    if (matchField is null)
                    {
                        foreach (var field in sourceTypeInfo.DeclaredFields)
                        {
                            if (method.ReturnType.IsAssignableFrom(field.FieldType))
                            {
                                matchField = field;
                                break;
                            }
                        }
                    }

                    if (matchField is null) continue;

                    propertyFields.TryGetValue(matchField.Name, out fieldBuilder);
                }

                if (fieldBuilder is not null)
                {
                    getMethodIL.Emit(OpCodes.Ldarg_0);
                    getMethodIL.Emit(OpCodes.Ldfld, fieldBuilder);
                    getMethodIL.Emit(OpCodes.Ret);
                }
            }
            // SET 属性方法
            else if (method.Name.StartsWith(_setPropertyMethodNamePrefix) && canReadOrWriteProperties.Any(p
                => p.Name.Equals(propertyMethodName, StringComparison.Ordinal)))
            {
                var setMethodBuilder = typeBuilder.DefineMethod(method.Name, method.Attributes, null, methodParamTypes);
                var setMethodIL = setMethodBuilder.GetILGenerator();

                setPropertyMethods.Add(propertyMethodName, setMethodBuilder);

                if (propertyFields.TryGetValue(propertyMethodName, out var fieldBuilder))
                {
                    setMethodIL.Emit(OpCodes.Ldarg_0);
                    setMethodIL.Emit(OpCodes.Ldarg_1);
                    setMethodIL.Emit(OpCodes.Stfld, fieldBuilder);
                    setMethodIL.Emit(OpCodes.Ret);
                }
            }
            // 普通方法
            else
            {
                var methodBuilder = typeBuilder.DefineMethod(method.Name, method.Attributes, method.ReturnType, methodParamTypes);

                if (method.CustomAttributes.Any())
                {
                    foreach (var attributeData in method.CustomAttributes)
                    {
                        methodBuilder.SetCustomAttribute(CreateCustomAttributeBuilder(attributeData));
                    }
                }

                var methodIL = methodBuilder.GetILGenerator();

                methodIL.Emit(OpCodes.Ldarg_0);
                methodIL.Emit(OpCodes.Call, method);
                methodIL.Emit(OpCodes.Ret);
            }
        }

        // 复制属性
        foreach (var property in canReadOrWriteProperties)
        {
            var propertyBuilder = typeBuilder.DefineProperty(property.Name, property.Attributes, property.PropertyType, null);

            if (property.CustomAttributes.Any())
            {
                foreach (var attributeData in property.CustomAttributes)
                {
                    propertyBuilder.SetCustomAttribute(CreateCustomAttributeBuilder(attributeData));
                }
            }

            if (getPropertyMethods.TryGetValue(property.Name, out var methodBuilder))
            {
                propertyBuilder.SetGetMethod(methodBuilder);
            }

            if (setPropertyMethods.TryGetValue(property.Name, out methodBuilder))
            {
                propertyBuilder.SetSetMethod(methodBuilder);
            }
        }

        return typeBuilder.CreateType();
    }

    private static CustomAttributeBuilder CreateCustomAttributeBuilder(CustomAttributeData attributeData)
    {
        var ctorArgs = attributeData.ConstructorArguments.Select(s => s.Value).ToArray();

        var fields = attributeData.NamedArguments.Where(p => p.IsField).ToArray();
        var fieldInfos = fields.Select(s => (FieldInfo)s.MemberInfo).ToArray();
        var fieldValues = fields.Select(s => s.TypedValue.Value).ToArray();

        var properties = attributeData.NamedArguments.Where(p => !p.IsField).ToArray();
        var propertyInfos = properties.Select(s => (PropertyInfo)s.MemberInfo).ToArray();
        var propertyValues = properties.Select(s => s.TypedValue.Value).ToArray();

        return new CustomAttributeBuilder(attributeData.Constructor, ctorArgs,
            propertyInfos, propertyValues, fieldInfos, fieldValues);
    }


    /// <summary>
    /// 根据来源类型派生出新类型集合。
    /// </summary>
    /// <param name="moduleBuilder">给定的 <see cref="ModuleBuilder"/>。</param>
    /// <param name="sourceType">给定的来源类型。</param>
    /// <param name="newTypeNames">给定的新类型名称。</param>
    /// <returns>返回新类型集合。</returns>
    public static IEnumerable<Type> DeriveTypes(this ModuleBuilder moduleBuilder, Type sourceType, IEnumerable<string> newTypeNames)
    {
        foreach (var name in newTypeNames)
        {
            yield return moduleBuilder.DeriveType(sourceType, name);
        }
    }

    /// <summary>
    /// 根据来源类型派生出新类型。
    /// </summary>
    /// <param name="moduleBuilder">给定的 <see cref="ModuleBuilder"/>。</param>
    /// <param name="sourceType">给定的来源类型。</param>
    /// <param name="newTypeName">给定的新类型名称。</param>
    /// <returns>返回新类型。</returns>
    public static Type DeriveType(this ModuleBuilder moduleBuilder, Type sourceType, string newTypeName)
    {
        var typeBuilder = moduleBuilder.DefineType(newTypeName, sourceType.Attributes, sourceType);
        
        return typeBuilder.CreateType();
    }


    #region DefineDynamicModule

    /// <summary>
    /// 使用指定的程序集名称定义动态模块。
    /// </summary>
    /// <param name="assemblyName">给定的程序集名称。</param>
    /// <param name="access">给定的 <see cref="AssemblyBuilderAccess"/>（可选；默认 <see cref="AssemblyBuilderAccess.Run"/>）。</param>
    /// <param name="attributes">给定的 <see cref="IEnumerable{CustomAttributeBuilder}"/>（可选）。</param>
    /// <returns>返回 <see cref="ModuleBuilder"/>。</returns>
    public static ModuleBuilder DefineDynamicModule(this string assemblyName,
        AssemblyBuilderAccess access = AssemblyBuilderAccess.Run,
        IEnumerable<CustomAttributeBuilder>? attributes = null)
        => new AssemblyName(assemblyName).DefineDynamicModule(access, attributes);

    /// <summary>
    /// 使用指定的程序集名称定义动态模块。
    /// </summary>
    /// <param name="name">给定的 <see cref="AssemblyName"/>。</param>
    /// <param name="access">给定的 <see cref="AssemblyBuilderAccess"/>（可选；默认 <see cref="AssemblyBuilderAccess.Run"/>）。</param>
    /// <param name="attributes">给定的 <see cref="IEnumerable{CustomAttributeBuilder}"/>（可选）。</param>
    /// <returns>返回 <see cref="ModuleBuilder"/>。</returns>
    public static ModuleBuilder DefineDynamicModule(this AssemblyName name,
        AssemblyBuilderAccess access = AssemblyBuilderAccess.Run,
        IEnumerable<CustomAttributeBuilder>? attributes = null)
        => name.DefineDynamicModule(out _, access, attributes);

    /// <summary>
    /// 使用指定的程序集名称定义动态模块。
    /// </summary>
    /// <param name="name">给定的 <see cref="AssemblyName"/>。</param>
    /// <param name="assemblyBuilder">输出 <see cref="AssemblyBuilder"/>。</param>
    /// <param name="access">给定的 <see cref="AssemblyBuilderAccess"/>（可选；默认 <see cref="AssemblyBuilderAccess.Run"/>）。</param>
    /// <param name="attributes">给定的 <see cref="IEnumerable{CustomAttributeBuilder}"/>（可选）。</param>
    /// <returns>返回 <see cref="ModuleBuilder"/>。</returns>
    public static ModuleBuilder DefineDynamicModule(this AssemblyName name,
        out AssemblyBuilder assemblyBuilder,
        AssemblyBuilderAccess access = AssemblyBuilderAccess.Run,
        IEnumerable<CustomAttributeBuilder>? attributes = null)
    {
        assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(name, access, attributes);
        
        return assemblyBuilder.DefineDynamicModule($"{name.Name}.dll");
    }

    #endregion

}
