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
/// 定义泛型二进制成员映射。
/// </summary>
/// <param name="converter">给定的 <see cref="IBinaryConverter"/>。</param>
/// <param name="memberInfo">给定的 <see cref="BinaryMemberInfo"/>。</param>
/// <param name="memberType">给定的二进制成员类型。</param>
public class BinaryMemberMapping<T>(IBinaryConverter converter, BinaryMemberInfo memberInfo,
    Type memberType) : BinaryMemberMapping(converter, memberInfo, memberType)
{
    /// <summary>
    /// 获取或设置读取泛型动作委托。
    /// </summary>
    /// <value>
    /// 返回 <see cref="MethodInfo"/>。
    /// </value>
    public Delegate? ReadAction { get; set; }

    /// <summary>
    /// 获取或设置写入泛型动作委托。
    /// </summary>
    /// <value>
    /// 返回 <see cref="MethodInfo"/>。
    /// </value>
    public Delegate? WriteAction { get; set; }


    /// <summary>
    /// 读取指定类型实例。
    /// </summary>
    /// <param name="reader">给定的 <see cref="BinaryReader"/>。</param>
    /// <param name="instance">给定的 <typeparamref name="T"/>。</param>
    public void Read(BinaryReader reader, T instance)
    {
        ArgumentNullException.ThrowIfNull(ReadAction);

        ReadAction.DynamicInvoke(instance, Converter, reader, MemberType, MemberInfo);
    }

    /// <summary>
    /// 写入指定类型实例。
    /// </summary>
    /// <param name="writer">给定的 <see cref="BinaryWriter"/>。</param>
    /// <param name="instance">给定的 <typeparamref name="T"/>。</param>
    public void Write(BinaryWriter writer, T instance)
    {
        ArgumentNullException.ThrowIfNull(WriteAction);

        WriteAction.DynamicInvoke(instance, Converter, writer, MemberType, MemberInfo);
    }

}
