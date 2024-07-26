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
    private static readonly Type _binaryMemberInfoType = typeof(BinaryMemberInfo);
    private static readonly Type _binaryReaderType = typeof(BinaryReader);
    private static readonly Type _binaryWriterType = typeof(BinaryWriter);
    private static readonly Type _typeType = typeof(Type);

    private static readonly List<BinaryMemberMapping<T>> _mappings = [];


    /// <summary>
    /// 获取泛型二进制成员映射集合。
    /// </summary>
    /// <param name="inputType">给定的输入类型。</param>
    /// <param name="options">给定的 <see cref="BinarySerializerOptions"/>。</param>
    /// <returns>返回 <see cref="BinaryMemberMapping{T}"/> 列表。</returns>
    public static List<BinaryMemberMapping<T>> GetMappings(Type inputType, BinarySerializerOptions options)
    {
        if (_mappings.Count == 0)
        {
            var readerExp = Expression.Parameter(_binaryReaderType, "reader");
            var writerExp = Expression.Parameter(_binaryWriterType, "writer");
            var infoExp = Expression.Parameter(_binaryMemberInfoType, "info");
            var typeExp = Expression.Parameter(_typeType, "type");
            var inputExp = Expression.Parameter(inputType, "obj");

            var members = options.TypeResolver.ResolveMembers(inputType, options);
            for (var i = 0; i < members.Length; i++)
            {
                var member = members[i];
                var memberType = member.GetRequiredType();

                if (!member.CanRead || memberType.IsSameOrNullableUnderlyingType(inputType))
                {
                    continue; // 排除自引用类型
                }

                var memberExp = member.BuildExpression(inputExp);
                var converter = options.GetBinaryConverter(memberType,
                    member.GetCustomAttribute<BinaryConverterAttribute>());

                if (converter is null)
                {
                    continue; // 排除不支持的类型
                }

                var converterType = converter.GetType();
                var converterExp = Expression.Parameter(converterType, "converter");

                // Read: (obj, converter, reader, type, info)
                //      => obj.Member = converter.Read(reader, type, info)
                var readMethod = converter.GetReadMethod();
                var readMethodParamsExps = new Expression[]
                {
                    readerExp,
                    typeExp,
                    infoExp
                };

                var readMethodCallExp = Expression.Call(converterExp, readMethod, readMethodParamsExps);

                var readDelegateType = _actionGenericTypeDefinition.MakeGenericType(inputType, converterType,
                    readerExp.Type, typeExp.Type, infoExp.Type);

                var readAssignExp = Expression.Assign(memberExp, readMethodCallExp);

                var readLambda = Expression.Lambda(readDelegateType, readAssignExp,
                    [inputExp, converterExp, readerExp, typeExp, infoExp]);

                // Write: (obj, converter, writer, type, info)
                //      => converter.Write(writer, type, obj.Member, info)
                var writeMethod = converter.GetWriteMethod();
                var writeMethodParamsExps = new Expression[]
                {
                    writerExp,
                    typeExp,
                    memberExp,
                    infoExp
                };

                var writeMethodCallExp = Expression.Call(converterExp, writeMethod, writeMethodParamsExps);

                var writeDelegateType = _actionGenericTypeDefinition.MakeGenericType(inputType, converterType,
                    writerExp.Type, typeExp.Type, infoExp.Type);

                var writeLambda = Expression.Lambda(writeDelegateType, writeMethodCallExp,
                    [inputExp, converterExp, writerExp, typeExp, infoExp]);

                _mappings.Add(new(converter, member, memberType)
                {
                    ReadMethod = readMethod,
                    ReadAction = readLambda.Compile(),
                    WriteMethod = writeMethod,
                    WriteAction = writeLambda.Compile()
                });
            }
        }

        return _mappings;
    }

}
