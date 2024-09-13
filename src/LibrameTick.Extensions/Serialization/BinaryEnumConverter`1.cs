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
/// 定义继承 <see cref="AbstractBinaryConverter{TConverted}"/> 的泛型二进制枚举转换器。
/// </summary>
/// <typeparam name="TEnum">指定要转换的目标枚举类型。</typeparam>
/// <param name="namedFunc">给定的命名方法（可选）。</param>
public class BinaryEnumConverter<TEnum>(Func<string, string>? namedFunc = null) : AbstractBinaryConverter<TEnum>
    where TEnum : struct, Enum
{
    /// <summary>
    /// 获取转换器名称。
    /// </summary>
    /// <value>
    /// 返回名称字符串。
    /// </value>
    public override string ConverterName
        => namedFunc?.Invoke(base.ConverterName) ?? base.ConverterName;


    /// <summary>
    /// 通过读取器读取指定类型实例核心。
    /// </summary>
    /// <param name="reader">给定的 <see cref="BinaryReader"/>。</param>
    /// <param name="member">给定的 <see cref="BinaryMemberInfo"/>。</param>
    /// <returns>返回枚举项。</returns>
    protected override TEnum ReadCore(BinaryReader reader, BinaryMemberInfo member)
        => Enum.Parse<TEnum>(reader.ReadString());

    /// <summary>
    /// 通过写入器写入指定类型实例核心。
    /// </summary>
    /// <param name="writer">给定的 <see cref="BinaryWriter"/>。</param>
    /// <param name="value">给定要写入的枚举项。</param>
    /// <param name="member">给定的 <see cref="BinaryMemberInfo"/>。</param>
    protected override void WriteCore(BinaryWriter writer, TEnum value, BinaryMemberInfo member)
        => writer.Write(Enum.GetName(value) ?? value.ToString());

}
