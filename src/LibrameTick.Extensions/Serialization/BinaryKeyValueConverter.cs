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
/// 定义继承 <see cref="AbstractBinaryConverter{TConverted}"/> 的泛型二进制列表转换器。
/// </summary>
/// <typeparam name="TKey">指定要转换的目标键类型。</typeparam>
/// <typeparam name="TValue">指定要转换的目标值类型。</typeparam>
/// <param name="namedFunc">给定的命名方法（可选）。</param>
public class BinaryKeyValueConverter<TKey, TValue>(Func<string, string>? namedFunc = null)
    : AbstractBinaryConverter<KeyValuePair<TKey, TValue>>
{
    private readonly Type _keyType = typeof(TKey);
    private readonly Type _valueType = typeof(TValue);

    private ArgumentException KeyConverterNotFoundException(BinaryMemberInfo member)
        => new($"Cannot resolve converter for '{base.BeConvertedType.Name}' key type '{_keyType}'. The member '{member.GetRequiredType()} {member.Info.Name}' whether lacks a '{nameof(BinaryMappingAttribute)}' annotation.");

    private ArgumentException ValueConverterNotFoundException(BinaryMemberInfo member)
        => new($"Cannot resolve converter for '{base.BeConvertedType.Name}' value type '{_valueType}'. The member '{member.GetRequiredType()} {member.Info.Name}' whether lacks a '{nameof(BinaryMappingAttribute)}' annotation.");


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
        var keyConverter = member.Options.ConverterResolver.ResolveConverter(_keyType) as AbstractBinaryConverter<TKey>;
        var valueConverter = member.Options.ConverterResolver.ResolveConverter(_valueType) as AbstractBinaryConverter<TValue>;

        var useVersion = BinarySerializerVersion.FromAttribute(member.GetCustomAttribute<BinaryVersionAttribute>());

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
            value = ReadValue(reader, member, useVersion);
        }
        else if (valueConverter is not null)
        {
            key = ReadKey(reader, member, useVersion);
            value = valueConverter.Read(reader, _keyType, member)!;
        }
        else
        {
            key = ReadKey(reader, member, useVersion);
            value = ReadValue(reader, member, useVersion);
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
        var keyConverter = member.Options.ConverterResolver.ResolveConverter(_keyType) as AbstractBinaryConverter<TKey>;
        var valueConverter = member.Options.ConverterResolver.ResolveConverter(_valueType) as AbstractBinaryConverter<TValue>;

        var useVersion = BinarySerializerVersion.FromAttribute(member.GetCustomAttribute<BinaryVersionAttribute>());

        if (keyConverter is not null && valueConverter is not null)
        {
            keyConverter.Write(writer, _keyType, value.Key, member);
            valueConverter.Write(writer, _valueType, value.Value, member);
        }
        else if (keyConverter is not null)
        {
            keyConverter.Write(writer, _keyType, value.Key, member);
            WriteValue(writer, value.Value, member, useVersion);
        }
        else if (valueConverter is not null)
        {
            WriteKey(writer, value.Key, member, useVersion);
            valueConverter.Write(writer, _keyType, value.Value, member);
        }
        else
        {
            WriteKey(writer, value.Key, member, useVersion);
            WriteValue(writer, value.Value, member, useVersion);
        }
    }


    private TKey ReadKey(BinaryReader reader, BinaryMemberInfo member, BinarySerializerVersion? useVersion)
    {
        var key = Activator.CreateInstance<TKey>();

        var isKeyMappingDefined = member.Info.IsMappingAttributeDefined(out var attr) && attr?.ForKey == true;

        if (isKeyMappingDefined && member.FromExpression)
        {
            var mappings = BinaryExpressionMapper<TKey>.GetMappings(member.Options, useVersion);
            for (var i = 0; i < mappings.Count; i++)
            {
                mappings[i].Read(reader, key);
            }
        }
        else if (isKeyMappingDefined && !member.FromExpression)
        {
            var mappings = BinaryObjectMapper.GetMappings(_keyType, member.Options, useVersion);
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

    private TValue ReadValue(BinaryReader reader, BinaryMemberInfo member, BinarySerializerVersion? useVersion)
    {
        var value = Activator.CreateInstance<TValue>();

        var isValueMappingDefined = member.Info.IsMappingAttributeDefined(out var attr) && attr?.ForValue == true;

        if (isValueMappingDefined && member.FromExpression)
        {
            var mappings = BinaryExpressionMapper<TValue>.GetMappings(member.Options, useVersion);
            for (var i = 0; i < mappings.Count; i++)
            {
                mappings[i].Read(reader, value);
            }
        }
        else if (isValueMappingDefined && !member.FromExpression)
        {
            var mappings = BinaryObjectMapper.GetMappings(_keyType, member.Options, useVersion);
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


    private void WriteKey(BinaryWriter writer, TKey key, BinaryMemberInfo member, BinarySerializerVersion? useVersion)
    {
        var isKeyMappingDefined = member.Info.IsMappingAttributeDefined(out var attr) && attr?.ForKey == true;

        if (isKeyMappingDefined && member.FromExpression)
        {
            var mappings = BinaryExpressionMapper<TKey>.GetMappings(member.Options, useVersion);
            for (var i = 0; i < mappings.Count; i++)
            {
                mappings[i].Write(writer, key);
            }
        }
        else if (isKeyMappingDefined && !member.FromExpression)
        {
            var mappings = BinaryObjectMapper.GetMappings(_keyType, member.Options, useVersion);
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

    private void WriteValue(BinaryWriter writer, TValue value, BinaryMemberInfo member, BinarySerializerVersion? useVersion)
    {
        var isValueMappingDefined = member.Info.IsMappingAttributeDefined(out var attr) && attr?.ForValue == true;

        if (isValueMappingDefined && member.FromExpression)
        {
            var mappings = BinaryExpressionMapper<TValue>.GetMappings(member.Options, useVersion);
            for (var i = 0; i < mappings.Count; i++)
            {
                mappings[i].Write(writer, value);
            }
        }
        else if (isValueMappingDefined && !member.FromExpression)
        {
            var mappings = BinaryObjectMapper.GetMappings(_valueType, member.Options, useVersion);
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
