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
/// 定义二进制类型解析器接口。
/// </summary>
public interface IBinaryTypeResolver
{
    /// <summary>
    /// 获取二进制序列化选项。
    /// </summary>
    /// <value>
    /// 返回 <see cref="BinarySerializerOptions"/>。
    /// </value>
    BinarySerializerOptions Options { get; }


    /// <summary>
    /// 解析指定类型的成员信息。
    /// </summary>
    /// <param name="inputType">给定要解析的输入类型。</param>
    /// <returns>返回 <see cref="BinaryMemberInfo"/> 数组。</returns>
    /// <exception cref="NotSupportedException">
    /// Resolving member type other than field and property is not supported.
    /// </exception>
    BinaryMemberInfo[] ResolveMembers(Type inputType);
}
