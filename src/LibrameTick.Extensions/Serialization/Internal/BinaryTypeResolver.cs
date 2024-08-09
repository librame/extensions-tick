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

internal sealed class BinaryTypeResolver(BinarySerializerOptions options) : IBinaryTypeResolver
{
    public BinarySerializerOptions Options { get; init; } = options;


    public BinaryMemberInfo[] ResolveMembers(Type inputType)
    {
        var infos = inputType.GetMembers();

        var members = infos.Where(Options.FilterMembers)
            .Select((m, i) => new BinaryMemberInfo(m, Options, Options.OrderByMembersFunc(m, i, infos.Length)))
            .OrderBy(m => m.OrderId)
            .ToArray();

        return members;
    }

}
