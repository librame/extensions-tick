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


    public BinaryMemberInfo[] ResolveMembers(Type inputType, bool fromExpression,
        BinarySerializerVersion? useVersion, BinaryMemberInfo? parentMember = null)
    {
        var infos = inputType.GetMembers();

        var members = infos.Where(m => FilterMembers(m, useVersion ?? Options.UseVersion))
            .Select((m, i) => CreateMemberInfo(m, i, infos.Length, fromExpression, parentMember))
            .Where(m => m.CanRead && m.CanWrite) // 只保留可读写的成员
            .OrderBy(m => m.OrderId)
            .ToArray();

        return members;
    }

    private BinaryMemberInfo CreateMemberInfo(MemberInfo member, int memberIndex, int membersCount,
        bool fromExpression, BinaryMemberInfo? parentMember = null)
        => new(member, fromExpression, Options, Options.OrderByMembersFunc(member, memberIndex, membersCount), parentMember);

    private bool FilterMembers(MemberInfo member, BinarySerializerVersion? useVersion)
    {
        var isUnfiltered = member.MemberType == Options.MemberType && !member.IsIgnoreAttributeDefined();

        // 未指定序列化版本，只过滤标注忽略自定义特性的成员
        if (useVersion is null) return isUnfiltered;

        var memberVersion = member.GetCustomAttribute<BinaryVersionAttribute>();

        // 如果未标注版本自定义特性的成员将不会被过滤
        if (memberVersion is null) return isUnfiltered;

        // 根据版本比较方法过滤不满足条件的成员
        return useVersion.IsSupported(memberVersion.Version, isUnfiltered);
    }

}
