#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core;

/// <summary>
/// 定义一个表示泛型枚举信息的描述符。
/// </summary>
/// <typeparam name="TEnum">指定的枚举类型。</typeparam>
public class EnumDescriptor<TEnum> : EnumDescriptor
{
    /// <summary>
    /// 构造一个 <see cref="EnumDescriptor{TEnum}"/>。
    /// </summary>
    /// <param name="enumType">给定的枚举类型。</param>
    /// <param name="field">给定的 <see cref="FieldInfo"/>。</param>
    public EnumDescriptor(Type enumType, FieldInfo field)
        : base(enumType, field)
    {
    }


    /// <summary>
    /// 枚举项。
    /// </summary>
    public TEnum EnumItem => (TEnum)ObjValue;
}


/// <summary>
/// 定义一个表示枚举信息的描述符。
/// </summary>
public class EnumDescriptor
{
    /// <summary>
    /// 构造一个 <see cref="EnumDescriptor"/>。
    /// </summary>
    /// <param name="enumType">给定的枚举类型。</param>
    /// <param name="field">给定的字段信息。</param>
    public EnumDescriptor(Type enumType, FieldInfo field)
    {
        EnumType = enumType;
        Field = field;
    }


    /// <summary>
    /// 枚举类型。
    /// </summary>
    public Type EnumType { get; init; }

    /// <summary>
    /// 字段信息。
    /// </summary>
    public FieldInfo Field { get; init; }

    /// <summary>
    /// 枚举名称。
    /// </summary>
    public string Name => Field.Name;

    /// <summary>
    /// 对象枚举值。
    /// </summary>
    public object ObjValue => Field.GetValue(null)!;

    /// <summary>
    /// 枚举常量值。
    /// </summary>
    /// <value>返回 32 位整数。</value>
    public int Value => (int)ObjValue;

    /// <summary>
    /// 所有标注的特性集合。
    /// </summary>
    public IEnumerable<Attribute> AllAttributes => Field.GetCustomAttributes();


    /// <summary>
    /// 获取指定的特性集合。
    /// </summary>
    /// <typeparam name="TAttribute">指定的特性类型。</typeparam>
    /// <returns>返回 <see cref="IEnumerable{TAttribute}"/>。</returns>
    public virtual IEnumerable<TAttribute> GetAttributes<TAttribute>()
        where TAttribute : Attribute
        => Field.GetCustomAttributes<TAttribute>();

}
