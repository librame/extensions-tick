#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Serialization.Internal;

internal sealed class BinaryTypeResolver : IBinaryTypeResolver
{
    private readonly Type _ignoreTypeAttribute = typeof(BinaryIgnoreAttribute);


    public BinaryMemberInfo[] ResolveMembers(Type inputType, BinarySerializerOptions options)
    {
        if (options.MemberType != MemberTypes.Field && options.MemberType != MemberTypes.Property)
        {
            throw new NotSupportedException($"Resolving member type '{Enum.GetName(options.MemberType)}' other than field and property is not supported.");
        }

        var infos = inputType.GetMembers();

        var members = infos.Where(m => m.MemberType == options.MemberType && !m.IsDefined(_ignoreTypeAttribute, inherit: false))
            .Select((m, i) => new BinaryMemberInfo(m, options, options.OrderByMembers(m, i, infos.Length)))
            .OrderBy(m => m.OrderId)
            .ToArray();

        return members;
    }

}
