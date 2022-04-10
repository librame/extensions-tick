#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Cryptography;
using Librame.Extensions.Data.Accessing;

namespace Librame.Extensions.Data.ValueConversion;

class InternalEncryptionConverterFactory : IEncryptionConverterFactory
{
    private readonly ConcurrentDictionary<string, List<ValueConverter>> _dictionary = new();

    private readonly ISymmetricAlgorithm _symmetric;


    public InternalEncryptionConverterFactory(ISymmetricAlgorithm symmetric)
    {
        _symmetric = symmetric;
    }


    public ValueConverter GetConverter(DbContextAccessor accessor, Type propertyType)
    {
        if (!_dictionary.TryGetValue(accessor.AccessorId, out var converters))
        {
            converters = new List<ValueConverter>();

            // 以字节数组为基础加密提供程序
            var byteArrayProvider = new ByteArrayEncryptionProvider(_symmetric,
                accessor.AccessorDescriptor?.Algorithm ?? accessor.CoreOptions.Algorithm);

            // 支持对字节数组类型加密
            converters.Add(new EncryptionConverter<byte[]>(byteArrayProvider));

            // 支持对字符串类型加密
            converters.Add(new EncryptionConverter<string>(new StringEncryptionProvider(byteArrayProvider)));
        }

        var converter = converters.FirstOrDefault(p => p.ModelClrType.IsSameType(propertyType));
        if (converter is null)
            throw new ArgumentException($"The encryption property type '{propertyType}' of the current accessor '{accessor.AccessorType}' is not supported.");

        return converter;
    }

}
