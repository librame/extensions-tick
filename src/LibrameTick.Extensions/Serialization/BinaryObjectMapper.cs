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
    private static readonly ConcurrentDictionary<Type, int> _cacheCascadeIds = new();
    private static readonly ConcurrentDictionary<Type, List<BinaryMemberMapping>> _cacheMaps = [];


    /// <summary>
    /// 清除映射缓存。
    /// </summary>
    public static void ClearCache()
    {
        _cacheCascadeIds.Clear();
        _cacheMaps.Clear();
    }


    /// <summary>
    /// 获取二进制成员映射集合。
    /// </summary>
    /// <param name="inputType">给定的输入类型。</param>
    /// <param name="options">给定的 <see cref="BinarySerializerOptions"/>。</param>
    /// <returns>返回 <see cref="BinaryMemberMapping"/> 列表。</returns>
    public static List<BinaryMemberMapping> GetMappings(Type inputType, BinarySerializerOptions options)
    {
        return _cacheMaps.GetOrAdd(inputType, key =>
        {
            var mappings = LookupMappings(options, key);

            mappings = [.. mappings.OrderBy(ks => ks.OrderId).OrderBy(ks => ks.DeclaringTypeCascadeId)];

            return mappings;
        });
    }

    private static List<BinaryMemberMapping> LookupMappings(BinarySerializerOptions options,
        Type parentType, List<BinaryMemberInfo>? parentMembers = null)
    {
        var mappings = new List<BinaryMemberMapping>();

        var members = options.TypeResolver.ResolveMembers(parentType);
        for (var i = 0; i < members.Length; i++)
        {
            var member = members[i];
            var memberType = member.GetRequiredType();

            // 绑定成员声明类型级联标识
            member.DeclaringTypeCascadeId = _cacheCascadeIds.GetOrAdd(parentType, parentMembers?.Count ?? 0);

            var containSameTypeReference = parentMembers is null
                ? memberType.IsSameOrNullableArgumentType(parentType)
                : parentMembers.Any(p => memberType.IsSameOrNullableArgumentType(p.GetRequiredType()));

            if (!member.CanRead || containSameTypeReference)
            {
                continue; // 排除自引用类型
            }

            var converter = options.ConverterResolver.ResolveConverter(memberType,
                member.GetCustomAttribute<BinaryConverterAttribute>());

            if (converter is null)
            {
                if (parentMembers is null)
                {
                    parentMembers = [member];
                }
                else
                {
                    parentMembers.Add(member);
                }

                // 级联查找子级成员映射
                var referenceMappings = LookupMappings(options, memberType, parentMembers);
                if (referenceMappings.Count > 0)
                {
                    mappings.AddRange(referenceMappings);
                }
                
                continue; // 排除不支持的类型
            }

            var mapping = new BinaryMemberMapping(converter, member, memberType)
            {
                ReadMethod = converter.GetReadMethod(),
                WriteMethod = converter.GetWriteMethod()
            };

            if (parentMembers is not null)
            {
                mapping.CascadeChildrenInvokeFunc = (map, obj) => options.CascadeChildrenInvokeFunc(map, obj, parentMembers);
            }

            mappings.Add(mapping);
        }

        return mappings;
    }

}
