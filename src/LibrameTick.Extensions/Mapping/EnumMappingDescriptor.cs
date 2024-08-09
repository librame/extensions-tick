#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Mapping;

/// <summary>
/// 定义表示泛型枚举信息映射的描述符。
/// </summary>
/// <typeparam name="TEnum">指定的枚举类型。</typeparam>
/// <remarks>
/// 构造一个 <see cref="EnumMappingDescriptor{TEnum}"/>。
/// </remarks>
/// <param name="enumType">给定的枚举类型。</param>
/// <param name="field">给定的 <see cref="FieldInfo"/>。</param>
public class EnumMappingDescriptor<TEnum>(Type enumType, FieldInfo field)
    : EnumMappingDescriptor(enumType, field)
{
    /// <summary>
    /// 枚举项。
    /// </summary>
    public TEnum EnumItem => (TEnum)ObjValue;
}


/// <summary>
/// 定义表示枚举信息映射的描述符。
/// </summary>
/// <remarks>
/// 构造一个 <see cref="EnumMappingDescriptor"/>。
/// </remarks>
/// <param name="enumType">给定的枚举类型。</param>
/// <param name="field">给定的字段信息。</param>
public class EnumMappingDescriptor(Type enumType, FieldInfo field)
{
    /// <summary>
    /// 枚举类型。
    /// </summary>
    public Type EnumType { get; init; } = enumType;

    /// <summary>
    /// 字段信息。
    /// </summary>
    public FieldInfo Field { get; init; } = field;

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
