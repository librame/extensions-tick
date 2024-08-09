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
/// 定义继承 <see cref="BinaryConverter{TConverted}"/> 的泛型二进制列表转换器。
/// </summary>
/// <typeparam name="TKey">指定要转换的目标键类型。</typeparam>
/// <typeparam name="TValue">指定要转换的目标值类型。</typeparam>
/// <param name="namedFunc">给定的命名方法（可选）。</param>
public class BinaryKeyValueConverter<TKey, TValue>(Func<string, string>? namedFunc = null)
    : BinaryConverter<KeyValuePair<TKey, TValue>>
{
    private readonly Type _keyType = typeof(TKey);
    private readonly Type _valueType = typeof(TValue);

    private ArgumentException KeyConverterNotFoundException(BinaryMemberInfo member)
        => new($"Cannot resolve converter for '{base.BeConvertedType.Name}' key type '{_keyType}'. The member '{member.GetRequiredType()} {member.Info.Name}' whether lacks a '{nameof(BinaryExpressionMappingAttribute)}' or '{nameof(BinaryObjectMappingAttribute)}' annotation.");

    private ArgumentException ValueConverterNotFoundException(BinaryMemberInfo member)
        => new($"Cannot resolve converter for '{base.BeConvertedType.Name}' value type '{_valueType}'. The member '{member.GetRequiredType()} {member.Info.Name}' whether lacks a '{nameof(BinaryExpressionMappingAttribute)}' or '{nameof(BinaryObjectMappingAttribute)}' annotation.");


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
    protected override KeyValuePair<TKey, TValue> ReadCore(BinaryReader reader, BinaryMemberInfo member)
    {
        var keyConverter = member.Options.ConverterResolver.ResolveConverter(_keyType) as BinaryConverter<TKey>;
        var valueConverter = member.Options.ConverterResolver.ResolveConverter(_valueType) as BinaryConverter<TValue>;

        TKey key;
        TValue value;

        if (keyConverter is not null && valueConverter is not null)
        {
            key = keyConverter.Read(reader, _keyType, member)!;
            value = valueConverter.Read(reader, _valueType, member)!;
        }
        else if (keyConverter is not null)
        {
            key = keyConverter.Read(reader, _keyType, member)!;
            value = ReadValue(reader, member);
        }
        else if (valueConverter is not null)
        {
            key = ReadKey(reader, member);
            value = valueConverter.Read(reader, _keyType, member)!;
        }
        else
        {
            key = ReadKey(reader, member);
            value = ReadValue(reader, member);
        }

        return new(key, value);
    }

    /// <summary>
    /// 通过写入器写入指定类型实例核心。
    /// </summary>
    /// <param name="writer">给定的 <see cref="BinaryWriter"/>。</param>
    /// <param name="value">给定要写入的字节数组。</param>
    /// <param name="member">给定的 <see cref="BinaryMemberInfo"/>。</param>
    protected override void WriteCore(BinaryWriter writer, KeyValuePair<TKey, TValue> value, BinaryMemberInfo member)
    {
        var keyConverter = member.Options.ConverterResolver.ResolveConverter(_keyType) as BinaryConverter<TKey>;
        var valueConverter = member.Options.ConverterResolver.ResolveConverter(_valueType) as BinaryConverter<TValue>;

        if (keyConverter is not null && valueConverter is not null)
        {
            keyConverter.Write(writer, _keyType, value.Key, member);
            valueConverter.Write(writer, _valueType, value.Value, member);
        }
        else if (keyConverter is not null)
        {
            keyConverter.Write(writer, _keyType, value.Key, member);
            WriteValue(writer, value.Value, member);
        }
        else if (valueConverter is not null)
        {
            WriteKey(writer, value.Key, member);
            valueConverter.Write(writer, _keyType, value.Value, member);
        }
        else
        {
            WriteKey(writer, value.Key, member);
            WriteValue(writer, value.Value, member);
        }
    }


    private TKey ReadKey(BinaryReader reader, BinaryMemberInfo member)
    {
        var key = Activator.CreateInstance<TKey>();

        if (member.Info.IsExpressionMappingAttributeDefined(out var exprAttr) && exprAttr?.ForKey == true)
        {
            var mappings = BinaryExpressionMapper<TKey>.GetMappings(member.Options);
            for (var i = 0; i < mappings.Count; i++)
            {
                mappings[i].Read(reader, key);
            }
        }
        else if (member.Info.IsObjectMappingAttributeDefined(out var objAttr) && objAttr?.ForKey == true)
        {
            var mappings = BinaryObjectMapper.GetMappings(_keyType, member.Options);
            for (var i = 0; i < mappings.Count; i++)
            {
                mappings[i].ReadObject(reader, key!);
            }
        }
        else
        {
            throw KeyConverterNotFoundException(member);
        }

        return key;
    }

    private TValue ReadValue(BinaryReader reader, BinaryMemberInfo member)
    {
        var value = Activator.CreateInstance<TValue>();

        if (member.Info.IsExpressionMappingAttributeDefined(out var exprAttr) && exprAttr?.ForValue == true)
        {
            var mappings = BinaryExpressionMapper<TValue>.GetMappings(member.Options);
            for (var i = 0; i < mappings.Count; i++)
            {
                mappings[i].Read(reader, value);
            }
        }
        else if (member.Info.IsObjectMappingAttributeDefined(out var objAttr) && objAttr?.ForValue == true)
        {
            var mappings = BinaryObjectMapper.GetMappings(_keyType, member.Options);
            for (var i = 0; i < mappings.Count; i++)
            {
                mappings[i].ReadObject(reader, value!);
            }
        }
        else
        {
            throw ValueConverterNotFoundException(member);
        }

        return value;
    }


    private void WriteKey(BinaryWriter writer, TKey key, BinaryMemberInfo member)
    {
        if (member.Info.IsExpressionMappingAttributeDefined(out var exprAttr) && exprAttr?.ForKey == true)
        {
            var mappings = BinaryExpressionMapper<TKey>.GetMappings(member.Options);
            for (var i = 0; i < mappings.Count; i++)
            {
                mappings[i].Write(writer, key);
            }
        }
        else if (member.Info.IsObjectMappingAttributeDefined(out var objAttr) && objAttr?.ForKey == true)
        {
            var mappings = BinaryObjectMapper.GetMappings(_keyType, member.Options);
            for (var i = 0; i < mappings.Count; i++)
            {
                mappings[i].WriteObject(writer, key!);
            }
        }
        else
        {
            throw KeyConverterNotFoundException(member);
        }
    }

    private void WriteValue(BinaryWriter writer, TValue value, BinaryMemberInfo member)
    {
        if (member.Info.IsExpressionMappingAttributeDefined(out var exprAttr) && exprAttr?.ForValue == true)
        {
            var mappings = BinaryExpressionMapper<TValue>.GetMappings(member.Options);
            for (var i = 0; i < mappings.Count; i++)
            {
                mappings[i].Write(writer, value);
            }
        }
        else if (member.Info.IsObjectMappingAttributeDefined(out var objAttr) && objAttr?.ForValue == true)
        {
            var mappings = BinaryObjectMapper.GetMappings(_valueType, member.Options);
            for (var i = 0; i < mappings.Count; i++)
            {
                mappings[i].WriteObject(writer, value!);
            }
        }
        else
        {
            throw ValueConverterNotFoundException(member);
        }
    }

}
