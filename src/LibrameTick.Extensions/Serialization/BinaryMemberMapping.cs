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
/// 定义二进制成员映射。
/// </summary>
/// <param name="converter">给定的 <see cref="IBinaryConverter"/>。</param>
/// <param name="member">给定的 <see cref="BinaryMemberInfo"/>。</param>
/// <param name="memberType">给定的二进制成员类型。</param>
public class BinaryMemberMapping(IBinaryConverter converter,
    BinaryMemberInfo member, Type memberType)
{
    /// <summary>
    /// 获取二进制转换器。
    /// </summary>
    /// <value>
    /// 返回 <see cref="IBinaryConverter"/>。
    /// </value>
    public IBinaryConverter Converter { get; init; } = converter;

    /// <summary>
    /// 获取或设置声明类型级联标识。
    /// </summary>
    /// <value>
    /// 返回整数。
    /// </value>
    public int DeclaringTypeCascadeId
    {
        get => Member.DeclaringTypeCascadeId;
        set => Member.DeclaringTypeCascadeId = value;
    }

    /// <summary>
    /// 获取声明类型。
    /// </summary>
    /// <value>
    /// 返回类型实例。
    /// </value>
    public Type? DeclaringType { get; init; } = member.DeclaringType;

    /// <summary>
    /// 获取二进制成员信息。
    /// </summary>
    /// <value>
    /// 返回 <see cref="BinaryMemberInfo"/>。
    /// </value>
    public BinaryMemberInfo Member { get; init; } = member;

    /// <summary>
    /// 获取二进制成员类型。
    /// </summary>
    /// <value>
    /// 返回成员类型实例。
    /// </value>
    public Type MemberType { get; init; } = memberType;

    /// <summary>
    /// 获取二进制成员名称。
    /// </summary>
    /// <value>
    /// 返回名称字符串。
    /// </value>
    public string MemberName { get; init; } = member.Name;

    /// <summary>
    /// 获取顺序标识。
    /// </summary>
    /// <value>
    /// 返回可空整数。
    /// </value>
    public int? OrderId { get; init; } = member.OrderId;

    /// <summary>
    /// 获取或设置读取方法。
    /// </summary>
    /// <value>
    /// 返回 <see cref="MethodInfo"/>。
    /// </value>
    public MethodInfo? ReadMethod { get; set; }

    /// <summary>
    /// 获取或设置写入方法。
    /// </summary>
    /// <value>
    /// 返回 <see cref="MethodInfo"/>。
    /// </value>
    public MethodInfo? WriteMethod { get; set; }

    /// <summary>
    /// 获取或设置父级调用子级对象的方法。传入参数依次为 <see cref="BinaryMemberMapping"/>、父级对象。
    /// </summary>
    /// <value>
    /// 返回 <see cref="Func{BinaryMemberMapping, Object, Object}"/> 方法委托。
    /// </value>
    public Func<BinaryMemberMapping, object, object>? CascadeChildrenInvokeFunc { get; set; }


    /// <summary>
    /// 读取指定对象的成员值。
    /// </summary>
    /// <param name="reader">给定的 <see cref="BinaryReader"/>。</param>
    /// <param name="obj">给定要读取的默认对象。</param>
    /// <exception cref="ArgumentNullException">
    /// <see cref="ReadMethod"/> is null.
    /// </exception>
    public void ReadObject(BinaryReader reader, object obj)
    {
        ArgumentNullException.ThrowIfNull(ReadMethod);

        var value = ReadMethod.Invoke(Converter, [reader, MemberType, Member]);

        Member.SetValue(CascadeChildrenInvokeFunc?.Invoke(this, obj) ?? obj, value);
    }

    /// <summary>
    /// 写入指定对象的成员值。
    /// </summary>
    /// <param name="writer">给定的 <see cref="BinaryWriter"/>。</param>
    /// <param name="obj">给定要写入的对象。</param>
    /// <exception cref="ArgumentNullException">
    /// <see cref="WriteMethod"/> is null.
    /// </exception>
    public void WriteObject(BinaryWriter writer, object obj)
    {
        ArgumentNullException.ThrowIfNull(WriteMethod);

        var value = Member.GetValue(CascadeChildrenInvokeFunc?.Invoke(this, obj) ?? obj);

        WriteMethod.Invoke(Converter, [writer, MemberType, value, Member]);
    }

}
