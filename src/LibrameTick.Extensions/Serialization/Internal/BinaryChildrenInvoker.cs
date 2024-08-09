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

internal static class BinaryChildrenInvoker
{

    public static object CascadeInvoke(BinaryMemberMapping mapping, object obj,
        List<BinaryMemberInfo> parentMembers)
    {
        if (mapping.DeclaringTypeCascadeId == 0) return obj;

        var parentValues = new object[mapping.DeclaringTypeCascadeId];
        for (var i = 0; i < parentValues.Length; i++)
        {
            var currentParentValue = i == 0 ? obj : parentValues[i - 1];
            parentValues[i] = parentMembers[i].GetValue(currentParentValue)!;
        }

        return parentValues.Last();
    }

}
