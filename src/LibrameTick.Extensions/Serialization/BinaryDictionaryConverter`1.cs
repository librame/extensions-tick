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
/// 定义继承 <see cref="AbstractBinaryConverter{TConverted}"/> 的泛型二进制字典转换器。
/// </summary>
/// <typeparam name="TDict">指定要转换的目标字典类型。</typeparam>
/// <typeparam name="TKey">指定要转换的目标字典键类型。</typeparam>
/// <typeparam name="TValue">指定要转换的目标字典值类型。</typeparam>
/// <param name="namedFunc">给定的命名方法（可选）。</param>
public class BinaryDictionaryConverter<TDict, TKey, TValue>(
    Func<string, string>? namedFunc = null) : AbstractBinaryConverter<TDict>
    where TDict : IDictionary<TKey, TValue>, new()
{
    private readonly Type _pairType = typeof(KeyValuePair<TKey, TValue>);

    private readonly BinaryKeyValueConverter<TKey, TValue> _pairConverter = new(namedFunc);


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
    /// <returns>返回字节数组。</returns>
    protected override TDict ReadCore(BinaryReader reader, BinaryMemberInfo member)
    {
        var dict = new TDict();

        var count = reader.ReadInt32();
        for (var i = 0; i < count; i++)
        {
            var pair = _pairConverter.Read(reader, _pairType, member);
            dict.Add(pair);
        }

        return dict;
    }

    /// <summary>
    /// 通过写入器写入指定类型实例核心。
    /// </summary>
    /// <param name="writer">给定的 <see cref="BinaryWriter"/>。</param>
    /// <param name="value">给定要写入的字节数组。</param>
    /// <param name="member">给定的 <see cref="BinaryMemberInfo"/>。</param>
    protected override void WriteCore(BinaryWriter writer, TDict value, BinaryMemberInfo member)
    {
        var count = value.Count;
        writer.Write(count);

        for (var i = 0; i < count; i++)
        {
            var pair = value.ElementAt(i);
            _pairConverter.Write(writer, _pairType, pair, member);
        }
    }

}
