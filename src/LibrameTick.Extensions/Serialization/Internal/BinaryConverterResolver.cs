#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Serialization.Internal;

internal sealed class BinaryConverterResolver(BinarySerializerOptions options) : IBinaryConverterResolver
{
    private readonly Type _enumType = typeof(BinaryEnumConverter<>);
    private readonly Type _dictType = typeof(BinaryDictionaryConverter<,,>);
    private readonly Type _listType = typeof(BinaryListConverter<,>);
    private readonly Type _idictType = typeof(IDictionary<,>);
    private readonly Type _ilistType = typeof(IList<>);


    public BinarySerializerOptions Options { get; init; } = options;


    public BinaryConverter<TConverted>? ResolveConverter<TConverted>(BinaryConverterAttribute? attribute)
    {
        var typeToConvert = typeof(TConverted);

        if (attribute?.ConvertedType is not null)
        {
            typeToConvert = attribute.ConvertedType;
        }

        return (BinaryConverter<TConverted>?)ResolveConverter(typeToConvert, attribute?.Name);
    }

    public BinaryConverter<TConverted>? ResolveConverter<TConverted>(string? name = null)
        => (BinaryConverter<TConverted>?)ResolveConverter(typeof(TConverted), name);

    public IBinaryConverter? ResolveConverter(Type typeToConvert, BinaryConverterAttribute? attribute)
    {
        if (attribute?.ConvertedType is not null)
        {
            typeToConvert = attribute.ConvertedType;
        }

        return ResolveConverter(typeToConvert, attribute?.Name);
    }

    public IBinaryConverter? ResolveConverter(Type typeToConvert, string? name = null)
    {
        // 优先解析转换类型
        var converters = Options.Converters.Where(c => c.BeConvertedType == typeToConvert);
        if (name is not null)
        {
            // 如果存在多个转换器，则优先解析名称，否则直接解析转换器名称
            converters = (converters.Count() > 1 ? converters : Options.Converters)
                .Where(c => c.ConverterName == name);
        }

        var converter = converters.FirstOrDefault();
        if (converter is not null)
        {
            return converter;
        }

        // 其次自动解析特定转换类型
        if (typeToConvert.IsEnum)
        {
            converter = AutoResolveEnumConverter(typeToConvert);
        }
        else if (typeToConvert.IsImplementedType(_ilistType, out var resultType))
        {
            converter = AutoResolevListConverter(typeToConvert, resultType.GenericTypeArguments.First());
        }
        else if (typeToConvert.IsImplementedType(_idictType, out resultType))
        {
            converter = AutoResolevDictConverter(typeToConvert, resultType.GenericTypeArguments.First(),
                resultType.GenericTypeArguments.Last());
        }

        return converter;
    }

    private IBinaryConverter? AutoResolveEnumConverter(Type typeToConvert)
    {
        var enumConverter = _enumType.MakeGenericType(typeToConvert);

        var ctor = enumConverter.GetConstructors().First();
        if (ctor.GetParameters().Length == 1)
        {
            Func<string, string>? func = BinaryConverters.InternalConverterNamed;

            return ctor.Invoke([func]) as IBinaryConverter;
        }

        return ctor.Invoke(null) as IBinaryConverter;
    }

    private IBinaryConverter? AutoResolevListConverter(Type typeToConvert, Type itemType)
    {
        var listConverter = _listType.MakeGenericType(typeToConvert, itemType);

        return AutoResolveEnumerableConverter(listConverter);
    }

    private IBinaryConverter? AutoResolevDictConverter(Type typeToConvert, Type keyType, Type valueType)
    {
        var dictConverter = _dictType.MakeGenericType(typeToConvert, keyType, valueType);

        return AutoResolveEnumerableConverter(dictConverter);
    }

    private IBinaryConverter? AutoResolveEnumerableConverter(Type enumerableConverterType)
    {
        var ctor = enumerableConverterType.GetConstructors().First();
        if (ctor.GetParameters().Length == 1)
        {
            Func<string, string>? func = BinaryConverters.InternalConverterNamed;
            return ctor.Invoke([func]) as IBinaryConverter;
        }

        return ctor.Invoke(null) as IBinaryConverter;
    }

}
