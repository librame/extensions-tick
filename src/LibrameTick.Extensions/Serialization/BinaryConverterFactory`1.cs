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
/// 定义实现 <see cref="AbstractBinaryConverter{TConverted}"/> 的泛型二进制转换器工厂。
/// </summary>
/// <typeparam name="TConverted">指定要转换的类型。</typeparam>
/// <param name="readFunc">给定的读取方法。</param>
/// <param name="writeAction">给定的写入动作。</param>
/// <param name="namedFunc">给定的命名方法（可选）。</param>
public class BinaryConverterFactory<TConverted>(
    Func<BinaryReader, BinaryMemberInfo, TConverted> readFunc,
    Action<BinaryWriter, TConverted, BinaryMemberInfo> writeAction,
    Func<string, string>? namedFunc = null) : AbstractBinaryConverter<TConverted>
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
    /// <returns>返回 <typeparamref name="TConverted"/>。</returns>
    protected override TConverted ReadCore(BinaryReader reader, BinaryMemberInfo member)
        => readFunc(reader, member);

    /// <summary>
    /// 通过写入器写入指定类型实例核心。
    /// </summary>
    /// <param name="writer">给定的 <see cref="BinaryWriter"/>。</param>
    /// <param name="value">给定要写入的 <typeparamref name="TConverted"/>。</param>
    /// <param name="member">给定的 <see cref="BinaryMemberInfo"/>。</param>
    protected override void WriteCore(BinaryWriter writer, TConverted value, BinaryMemberInfo member)
        => writeAction(writer, value, member);

}
