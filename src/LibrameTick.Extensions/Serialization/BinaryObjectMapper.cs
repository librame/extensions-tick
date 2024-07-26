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
/// 定义二进制对象映射器。
/// </summary>
public static class BinaryObjectMapper
{
    private static readonly ConcurrentDictionary<Type, List<BinaryMemberMapping>> _mappings = [];


    /// <summary>
    /// 获取二进制成员映射集合。
    /// </summary>
    /// <param name="inputType">给定的输入类型。</param>
    /// <param name="options">给定的 <see cref="BinarySerializerOptions"/>。</param>
    /// <returns>返回 <see cref="BinaryMemberMapping"/> 列表。</returns>
    public static List<BinaryMemberMapping> GetMappings(Type inputType, BinarySerializerOptions options)
    {
        return _mappings.GetOrAdd(inputType, initialMappings);

        List<BinaryMemberMapping> initialMappings(Type key)
        {
            var mappings = new List<BinaryMemberMapping>();

            var members = options.TypeResolver.ResolveMembers(inputType, options);
            for (var i = 0; i < members.Length; i++)
            {
                var member = members[i];
                var memberType = member.GetRequiredType();

                if (!member.CanRead || memberType.IsSameOrNullableUnderlyingType(inputType))
                {
                    continue; // 排除自引用类型
                }

                var converter = options.GetBinaryConverter(memberType,
                    member.GetCustomAttribute<BinaryConverterAttribute>());

                if (converter is null)
                {
                    continue; // 排除不支持的类型
                }

                mappings.Add(new(converter, member, memberType)
                {
                    ReadMethod = converter.GetReadMethod(),
                    WriteMethod= converter.GetWriteMethod()
                });
            }

            return mappings;
        }
    }

}
