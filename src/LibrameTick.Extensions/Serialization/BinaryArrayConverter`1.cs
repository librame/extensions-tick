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
/// 定义继承 <see cref="BinaryConverter{T}"/> 的泛型二进制数组转换器。
/// </summary>
/// <typeparam name="TArray">指定要转换的目标数组类型。</typeparam>
public class BinaryArrayConverter<TArray>(Func<BinaryReader, BinaryMemberInfo, BinaryArrayAttribute, TArray> readFunc,
    Action<BinaryWriter, TArray, BinaryMemberInfo, BinaryArrayAttribute> writeAction,
    Func<string, string>? namedFunc = null) : BinaryConverter<TArray>
{
    /// <summary>
    /// 获取转换器名称。
    /// </summary>
    /// <value>
    /// 返回名称字符串。
    /// </value>
    public override string CurrentName
        => namedFunc?.Invoke(base.CurrentName) ?? base.CurrentName;


    /// <summary>
    /// 通过读取器读取指定类型实例核心。
    /// </summary>
    /// <param name="reader">给定的 <see cref="BinaryReader"/>。</param>
    /// <param name="member">给定的 <see cref="BinaryMemberInfo"/>。</param>
    /// <returns>返回字节数组。</returns>
    protected override TArray ReadCore(BinaryReader reader, BinaryMemberInfo member)
    {
        var attribute = member.GetRequiredAttribute<BinaryArrayAttribute>();

        return readFunc(reader, member, attribute);
    }

    /// <summary>
    /// 通过写入器写入指定类型实例核心。
    /// </summary>
    /// <param name="writer">给定的 <see cref="BinaryWriter"/>。</param>
    /// <param name="value">给定要写入的字节数组。</param>
    /// <param name="member">给定的 <see cref="BinaryMemberInfo"/>。</param>
    protected override void WriteCore(BinaryWriter writer, TArray value, BinaryMemberInfo member)
    {
        var attribute = member.GetRequiredAttribute<BinaryArrayAttribute>();

        writeAction(writer, value, member, attribute);
    }

}