#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;
using System.Reflection.Emit;

namespace Librame.Extensions;

/// <summary>
/// Emit 静态扩展。
/// </summary>
public static class EmitExtensions
{
    private static readonly string _getPropertyMethodNamePrefix = "get_";
    private static readonly string _setPropertyMethodNamePrefix = "set_";
    private static readonly ConstructorInfo _defaultObjectConstructor = typeof(object).GetConstructor(Type.EmptyTypes)!;
    private static readonly Regex _regBackingField = new Regex("(?<__backingFieldName>k__BackingField)$");


    //#region TValue

    ///// <summary>
    ///// 创建程序集，并从来源类型集合生成指定名称的新类型副本集合。
    ///// </summary>
    ///// <param name="sourceTypes">给定的来源类型集合。</param>
    ///// <param name="newTypeNameFunc">给定的新类型名称方法。</param>
    ///// <param name="assemblyName">给定的程序集名称。</param>
    ///// <param name="access">给定的 <see cref="AssemblyBuilderAccess"/>（可选；默认 <see cref="AssemblyBuilderAccess.Run"/>）。</param>
    ///// <param name="attributes">给定的 <see cref="IEnumerable{CustomAttributeBuilder}"/>（可选）。</param>
    ///// <returns>返回新类型集合副本。</returns>
    //public static IDictionary<Type, TValue> BuildCopyFromTypes<TValue>(this IDictionary<Type, TValue> sourceTypes,
    //    Func<Type, string> newTypeNameFunc, string assemblyName,
    //    AssemblyBuilderAccess access = AssemblyBuilderAccess.Run,
    //    IEnumerable<CustomAttributeBuilder>? attributes = null)
    //    => sourceTypes.BuildCopyFromTypes(newTypeNameFunc, assemblyName, out _, access, attributes);

    ///// <summary>
    ///// 创建程序集，并从来源类型集合生成指定名称的新类型副本集合。
    ///// </summary>
    ///// <param name="sourceTypes">给定的来源类型集合。</param>
    ///// <param name="assemblyName">给定的程序集名称。</param>
    ///// <param name="access">给定的 <see cref="AssemblyBuilderAccess"/>（可选；默认 <see cref="AssemblyBuilderAccess.Run"/>）。</param>
    ///// <param name="attributes">给定的 <see cref="IEnumerable{CustomAttributeBuilder}"/>（可选）。</param>
    ///// <returns>返回新类型集合副本。</returns>
    //public static IDictionary<Type, TValue> BuildCopyFromTypes<TValue>(this IDictionary<Type, IEnumerable<(string, TValue)>> sourceTypes,
    //    string assemblyName, AssemblyBuilderAccess access = AssemblyBuilderAccess.Run,
    //    IEnumerable<CustomAttributeBuilder>? attributes = null)
    //    => sourceTypes.BuildCopyFromTypes(assemblyName, out _, access, attributes);


    ///// <summary>
    ///// 创建程序集，并从来源类型集合生成指定名称的新类型副本集合。
    ///// </summary>
    ///// <param name="sourceTypes">给定的来源类型集合。</param>
    ///// <param name="newTypeNameFunc">给定的新类型名称方法。</param>
    ///// <param name="assemblyName">给定的程序集名称。</param>
    ///// <param name="moduleBuilder">输出 <see cref="ModuleBuilder"/>。</param>
    ///// <param name="access">给定的 <see cref="AssemblyBuilderAccess"/>（可选；默认 <see cref="AssemblyBuilderAccess.Run"/>）。</param>
    ///// <param name="attributes">给定的 <see cref="IEnumerable{CustomAttributeBuilder}"/>（可选）。</param>
    ///// <returns>返回新类型集合副本。</returns>
    //public static IDictionary<Type, TValue> BuildCopyFromTypes<TValue>(this IDictionary<Type, TValue> sourceTypes,
    //    Func<Type, string> newTypeNameFunc, string assemblyName, out ModuleBuilder moduleBuilder,
    //    AssemblyBuilderAccess access = AssemblyBuilderAccess.Run,
    //    IEnumerable<CustomAttributeBuilder>? attributes = null)
    //{
    //    moduleBuilder = assemblyName.BuildModule(out _, access, attributes);

    //    return moduleBuilder.BuildCopyFromTypes(sourceTypes, newTypeNameFunc);
    //}

    ///// <summary>
    ///// 创建程序集，并从来源类型集合生成指定名称的新类型副本集合。
    ///// </summary>
    ///// <param name="sourceTypes">给定的来源类型集合。</param>
    ///// <param name="assemblyName">给定的程序集名称。</param>
    ///// <param name="moduleBuilder">输出 <see cref="ModuleBuilder"/>。</param>
    ///// <param name="access">给定的 <see cref="AssemblyBuilderAccess"/>（可选；默认 <see cref="AssemblyBuilderAccess.Run"/>）。</param>
    ///// <param name="attributes">给定的 <see cref="IEnumerable{CustomAttributeBuilder}"/>（可选）。</param>
    ///// <returns>返回新类型集合副本。</returns>
    //public static IDictionary<Type, TValue> BuildCopyFromTypes<TValue>(this IDictionary<Type, IEnumerable<(string, TValue)>> sourceTypes,
    //    string assemblyName, out ModuleBuilder moduleBuilder,
    //    AssemblyBuilderAccess access = AssemblyBuilderAccess.Run,
    //    IEnumerable<CustomAttributeBuilder>? attributes = null)
    //{
    //    moduleBuilder = assemblyName.BuildModule(out _, access, attributes);

    //    return moduleBuilder.BuildCopyFromTypes(sourceTypes);
    //}


    ///// <summary>
    ///// 从来源类型集合生成指定名称的新类型副本集合。
    ///// </summary>
    ///// <param name="moduleBuilder">给定的 <see cref="ModuleBuilder"/>。</param>
    ///// <param name="sourceTypes">给定的来源类型集合。</param>
    ///// <param name="newTypeNameFunc">给定的新类型名称方法。</param>
    ///// <returns>返回新类型集合副本。</returns>
    //public static IDictionary<Type, TValue> BuildCopyFromTypes<TValue>(this ModuleBuilder moduleBuilder,
    //    IDictionary<Type, TValue> sourceTypes, Func<Type, string> newTypeNameFunc)
    //{
    //    var dict = new Dictionary<Type, TValue>();

    //    foreach (var sourceType in sourceTypes)
    //    {
    //        var newTypeName = newTypeNameFunc(sourceType.Key);

    //        var newType = moduleBuilder.CreateType(sourceType.Key, newTypeName);
    //        dict.Add(newType, sourceType.Value);
    //    }

    //    return dict;
    //}

    ///// <summary>
    ///// 从来源类型与新名称的字典集合生成指定名称的新类型副本集合。
    ///// </summary>
    ///// <param name="moduleBuilder">给定的 <see cref="ModuleBuilder"/>。</param>
    ///// <param name="sourceTypes">给定的来源类型集合。</param>
    ///// <returns>返回新类型集合副本。</returns>
    //public static IDictionary<Type, TValue> BuildCopyFromTypes<TValue>(this ModuleBuilder moduleBuilder,
    //    IDictionary<Type, IEnumerable<(string, TValue)>> sourceTypes)
    //{
    //    var dict = new Dictionary<Type, TValue>();

    //    foreach (var sourceType in sourceTypes)
    //    {
    //        foreach (var (newTypeName, value) in sourceType.Value)
    //        {
    //            var newType = moduleBuilder.CreateType(sourceType.Key, newTypeName);
    //            dict.Add(newType, value);
    //        }
    //    }

    //    return dict;
    //}

    //#endregion


    /// <summary>
    /// 创建程序集，并从来源类型集合生成指定名称的新类型副本集合。
    /// </summary>
    /// <param name="sourceTypes">给定的来源类型集合。</param>
    /// <param name="newTypeNameFunc">给定的新类型名称方法。</param>
    /// <param name="assemblyName">给定的程序集名称。</param>
    /// <param name="access">给定的 <see cref="AssemblyBuilderAccess"/>（可选；默认 <see cref="AssemblyBuilderAccess.Run"/>）。</param>
    /// <param name="attributes">给定的 <see cref="IEnumerable{CustomAttributeBuilder}"/>（可选）。</param>
    /// <returns>返回新类型集合副本。</returns>
    public static IReadOnlyCollection<Type> BuildCopyFromTypes(this IEnumerable<Type> sourceTypes,
        Func<Type, string> newTypeNameFunc, string assemblyName,
        AssemblyBuilderAccess access = AssemblyBuilderAccess.Run,
        IEnumerable<CustomAttributeBuilder>? attributes = null)
        => sourceTypes.BuildCopyFromTypes(newTypeNameFunc, assemblyName, out _, access, attributes);

    /// <summary>
    /// 创建程序集，并从来源类型集合生成指定名称的新类型副本集合。
    /// </summary>
    /// <param name="sourceTypes">给定的来源类型集合。</param>
    /// <param name="assemblyName">给定的程序集名称。</param>
    /// <param name="access">给定的 <see cref="AssemblyBuilderAccess"/>（可选；默认 <see cref="AssemblyBuilderAccess.Run"/>）。</param>
    /// <param name="attributes">给定的 <see cref="IEnumerable{CustomAttributeBuilder}"/>（可选）。</param>
    /// <returns>返回新类型集合副本。</returns>
    public static IReadOnlyCollection<Type> BuildCopyFromTypes(this IDictionary<Type, IEnumerable<string>> sourceTypes,
        string assemblyName, AssemblyBuilderAccess access = AssemblyBuilderAccess.Run,
        IEnumerable<CustomAttributeBuilder>? attributes = null)
        => sourceTypes.BuildCopyFromTypes(assemblyName, out _, access, attributes);


    /// <summary>
    /// 创建程序集，并从来源类型集合生成指定名称的新类型副本集合。
    /// </summary>
    /// <param name="sourceTypes">给定的来源类型集合。</param>
    /// <param name="newTypeNameFunc">给定的新类型名称方法。</param>
    /// <param name="assemblyName">给定的程序集名称。</param>
    /// <param name="moduleBuilder">输出 <see cref="ModuleBuilder"/>。</param>
    /// <param name="access">给定的 <see cref="AssemblyBuilderAccess"/>（可选；默认 <see cref="AssemblyBuilderAccess.Run"/>）。</param>
    /// <param name="attributes">给定的 <see cref="IEnumerable{CustomAttributeBuilder}"/>（可选）。</param>
    /// <returns>返回新类型集合副本。</returns>
    public static IReadOnlyCollection<Type> BuildCopyFromTypes(this IEnumerable<Type> sourceTypes,
        Func<Type, string> newTypeNameFunc, string assemblyName, out ModuleBuilder moduleBuilder,
        AssemblyBuilderAccess access = AssemblyBuilderAccess.Run,
        IEnumerable<CustomAttributeBuilder>? attributes = null)
    {
        moduleBuilder = assemblyName.BuildModule(out _, access, attributes);

        return moduleBuilder.BuildCopyFromTypes(sourceTypes, newTypeNameFunc).AsReadOnlyCollection();
    }

    /// <summary>
    /// 创建程序集，并从来源类型集合生成指定名称的新类型副本集合。
    /// </summary>
    /// <param name="sourceTypes">给定的来源类型集合。</param>
    /// <param name="assemblyName">给定的程序集名称。</param>
    /// <param name="moduleBuilder">输出 <see cref="ModuleBuilder"/>。</param>
    /// <param name="access">给定的 <see cref="AssemblyBuilderAccess"/>（可选；默认 <see cref="AssemblyBuilderAccess.Run"/>）。</param>
    /// <param name="attributes">给定的 <see cref="IEnumerable{CustomAttributeBuilder}"/>（可选）。</param>
    /// <returns>返回新类型集合副本。</returns>
    public static IReadOnlyCollection<Type> BuildCopyFromTypes(this IDictionary<Type, IEnumerable<string>> sourceTypes,
        string assemblyName, out ModuleBuilder moduleBuilder,
        AssemblyBuilderAccess access = AssemblyBuilderAccess.Run,
        IEnumerable<CustomAttributeBuilder>? attributes = null)
    {
        moduleBuilder = assemblyName.BuildModule(out _, access, attributes);

        return moduleBuilder.BuildCopyFromTypes(sourceTypes).AsReadOnlyCollection();
    }


    /// <summary>
    /// 从来源类型集合生成指定名称的新类型副本集合。
    /// </summary>
    /// <param name="moduleBuilder">给定的 <see cref="ModuleBuilder"/>。</param>
    /// <param name="sourceTypes">给定的来源类型集合。</param>
    /// <param name="newTypeNameFunc">给定的新类型名称方法。</param>
    /// <returns>返回新类型集合副本。</returns>
    public static IEnumerable<Type> BuildCopyFromTypes(this ModuleBuilder moduleBuilder,
        IEnumerable<Type> sourceTypes, Func<Type, string> newTypeNameFunc)
    {
        foreach (var sourceType in sourceTypes)
        {
            var newTypeName = newTypeNameFunc(sourceType);

            yield return moduleBuilder.CopyType(sourceType, newTypeName);
        }
    }

    /// <summary>
    /// 从来源类型与新名称的字典集合生成指定名称的新类型副本集合。
    /// </summary>
    /// <param name="moduleBuilder">给定的 <see cref="ModuleBuilder"/>。</param>
    /// <param name="sourceTypes">给定的来源类型集合。</param>
    /// <returns>返回新类型集合副本。</returns>
    public static IEnumerable<Type> BuildCopyFromTypes(this ModuleBuilder moduleBuilder,
        IDictionary<Type, IEnumerable<string>> sourceTypes)
    {
        foreach (var type in sourceTypes)
        {
            foreach (var newTypeName in type.Value)
            {
                yield return moduleBuilder.CopyType(type.Key, newTypeName);
            }
        }
    }

    /// <summary>
    /// 从来源类型复制一个具有相同成员的新类型。
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

        Type[] parameterTypes = { typeof(int) };

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

                // 绑定属性字段
                if (propertyFields.TryGetValue(propertyMethodName, out var fieldBuilder))
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
    /// 使用指定的程序集名称构建模块构建器。
    /// </summary>
    /// <param name="assemblyName">给定的程序集名称。</param>
    /// <param name="assemblyBuilder">输出 <see cref="AssemblyBuilder"/>。</param>
    /// <param name="access">给定的 <see cref="AssemblyBuilderAccess"/>（可选；默认 <see cref="AssemblyBuilderAccess.Run"/>）。</param>
    /// <param name="attributes">给定的 <see cref="IEnumerable{CustomAttributeBuilder}"/>（可选）。</param>
    /// <returns>返回 <see cref="ModuleBuilder"/>。</returns>
    public static ModuleBuilder BuildModule(this string assemblyName,
        out AssemblyBuilder assemblyBuilder,
        AssemblyBuilderAccess access = AssemblyBuilderAccess.Run,
        IEnumerable<CustomAttributeBuilder>? attributes = null)
        => new AssemblyName(assemblyName).BuildModule(out assemblyBuilder, access, attributes);

    /// <summary>
    /// 使用指定的程序集名称构建模块构建器。
    /// </summary>
    /// <param name="name">给定的 <see cref="AssemblyName"/>。</param>
    /// <param name="assemblyBuilder">输出 <see cref="AssemblyBuilder"/>。</param>
    /// <param name="access">给定的 <see cref="AssemblyBuilderAccess"/>（可选；默认 <see cref="AssemblyBuilderAccess.Run"/>）。</param>
    /// <param name="attributes">给定的 <see cref="IEnumerable{CustomAttributeBuilder}"/>（可选）。</param>
    /// <returns>返回 <see cref="ModuleBuilder"/>。</returns>
    public static ModuleBuilder BuildModule(this AssemblyName name,
        out AssemblyBuilder assemblyBuilder,
        AssemblyBuilderAccess access = AssemblyBuilderAccess.Run,
        IEnumerable<CustomAttributeBuilder>? attributes = null)
    {
        assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(name, access, attributes);

        return assemblyBuilder.DefineDynamicModule($"{name.Name}.dll");
    }

}
