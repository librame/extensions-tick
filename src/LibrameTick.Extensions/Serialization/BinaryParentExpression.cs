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
/// 定义二进制父级表达式。
/// </summary>
/// <param name="ParentType">给定的父级类型。</param>
/// <param name="ParentParameter">给定的父级参数表达式。</param>
/// <param name="CurrentMember">给定的当前成员表达式。</param>
public record BinaryParentExpression(Type ParentType,
    ParameterExpression ParentParameter, MemberExpression CurrentMember);