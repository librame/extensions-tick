#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Serialization;

/// <summary>
/// 定义泛型二进制表达式映射器。
/// </summary>
/// <typeparam name="T">指定要序列化的类型。</typeparam>
public static class BinaryExpressionMapper<T>
{
    private static readonly Type _actionGenericTypeDefinition = typeof(Action<,,,,>);

    private static readonly ParameterExpression _readerExpr = Expression.Parameter(typeof(BinaryReader), "reader");
    private static readonly ParameterExpression _writerExpr = Expression.Parameter(typeof(BinaryWriter), "writer");
    private static readonly ParameterExpression _infoExpr = Expression.Parameter(typeof(BinaryMemberInfo), "info");
    private static readonly ParameterExpression _typeExpr = Expression.Parameter(typeof(Type), "type");
    private static readonly ParameterExpression _objExpr = Expression.Parameter(typeof(T), "obj");

    private static readonly ConcurrentDictionary<Type, (Expression, int)> _cacheCascadeExprs = [];
    private static readonly ConcurrentDictionary<Type, List<BinaryMemberMapping<T>>> _cacheMaps = [];


    /// <summary>
    /// 清除映射缓存。
    /// </summary>
    public static void ClearCache()
    {
        _cacheCascadeExprs.Clear();
        _cacheMaps.Clear();
    }


    /// <summary>
    /// 获取泛型二进制成员映射集合。
    /// </summary>
    /// <param name="options">给定的 <see cref="BinarySerializerOptions"/>。</param>
    /// <param name="useVersion">给定要使用的版本（可选）。</param>
    /// <returns>返回 <see cref="BinaryMemberMapping"/> 列表。</returns>
    public static List<BinaryMemberMapping<T>> GetMappings(BinarySerializerOptions options,
        BinarySerializerVersion? useVersion)
    {
        return _cacheMaps.GetOrAdd(_objExpr.Type, key =>
        {
            var mappings = LookupMappings(options, useVersion, key);

            mappings = [.. mappings.OrderBy(ks => ks.OrderId).OrderBy(ks => ks.DeclaringTypeCascadeId)];

            return mappings;
        });
    }

    private static List<BinaryMemberMapping<T>> LookupMappings(BinarySerializerOptions options,
        BinarySerializerVersion? useVersion, Type parentType, BinaryParentExpression? parentExpression = null)
    {
        (var parentExpr, var parentCascadeId) = _cacheCascadeExprs.GetOrAdd(parentType, key =>
        {
            var expr = parentExpression?.CurrentMember as Expression;
            var cascadeId = expr?.ToString().Split('.').Length - 1;

            return (expr ?? _objExpr, cascadeId ?? 0);
        });

        var mappings = new List<BinaryMemberMapping<T>>();

        var members = options.TypeResolver.ResolveMembers(parentType, fromExpression: true,
            useVersion, parentExpression?.ParentMember);

        for (var i = 0; i < members.Length; i++)
        {
            var member = members[i];
            var memberType = member.GetRequiredType();

            // 绑定成员声明类型级联标识
            member.DeclaringTypeCascadeId = parentCascadeId;

            var containSameTypeReference = _cacheCascadeExprs.Count == 0
                ? memberType.IsSameOrNullableArgumentType(parentType)
                : _cacheCascadeExprs.Keys.Any(memberType.IsSameOrNullableArgumentType);

            // 排除自引用类型
            if (containSameTypeReference) continue;

            var cascadeMemberExprs = member.BuildExpression(parentExpr);

            var converter = options.ConverterResolver.ResolveConverter(memberType,
                member.GetCustomAttribute<BinaryConverterAttribute>());

            if (converter is null)
            {
                if (parentExpression is null)
                {
                    parentExpression = new(parentType, _objExpr, cascadeMemberExprs, member);
                }
                else
                {
                    parentExpression = parentExpression with
                    {
                        CurrentMember = cascadeMemberExprs,
                        ParentMember = member
                    };
                }

                // 级联查找子级成员映射
                var referenceMappings = LookupMappings(options, useVersion, memberType, parentExpression);
                if (referenceMappings.Count > 0)
                {
                    mappings.AddRange(referenceMappings);
                }

                continue; // 排除不支持的类型
            }

            var converterType = converter.GetType();
            var converterExpr = Expression.Parameter(converterType, "converter");

            // Read: (obj, converter, reader, type, info)
            //      => obj.Member = converter.Read(reader, type, info)
            var readMethod = converter.GetReadMethod();
            var readMethodParamsExps = new Expression[]
            {
                    _readerExpr,
                    _typeExpr,
                    _infoExpr
            };

            var readMethodCallExpr = Expression.Call(converterExpr, readMethod, readMethodParamsExps);

            var readDelegateType = _actionGenericTypeDefinition.MakeGenericType(parentExpression?.ParentType ?? parentType,
                converterType, _readerExpr.Type, _typeExpr.Type, _infoExpr.Type);

            var readAssignExpr = Expression.Assign(cascadeMemberExprs, readMethodCallExpr);

            var readLambda = Expression.Lambda(readDelegateType, readAssignExpr,
                [parentExpression?.ParentParameter ?? _objExpr, converterExpr, _readerExpr, _typeExpr, _infoExpr]);

            var readAction = readLambda.Compile();

            // Write: (obj, converter, writer, type, info)
            //      => converter.Write(writer, type, obj.Member, info)
            var writeMethod = converter.GetWriteMethod();
            var writeMethodParamsExps = new Expression[]
            {
                    _writerExpr,
                    _typeExpr,
                    cascadeMemberExprs,
                    _infoExpr
            };

            var writeMethodCallExpr = Expression.Call(converterExpr, writeMethod, writeMethodParamsExps);

            var writeDelegateType = _actionGenericTypeDefinition.MakeGenericType(parentExpression?.ParentType ?? parentType,
                converterType, _writerExpr.Type, _typeExpr.Type, _infoExpr.Type);

            var writeLambda = Expression.Lambda(writeDelegateType, writeMethodCallExpr,
                [parentExpression?.ParentParameter ?? _objExpr, converterExpr, _writerExpr, _typeExpr, _infoExpr]);

            var writeAction = writeLambda.Compile();

            var mapping = new BinaryMemberMapping<T>(converter, member, memberType)
            {
                ReadMethod = readMethod,
                ReadAction = readAction,
                WriteMethod = writeMethod,
                WriteAction = writeAction
            };

            mappings.Add(mapping);
        }

        return mappings;
    }

}
