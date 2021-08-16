#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Core.Cryptography;
using Librame.Extensions.Data.Accessing;
using Librame.Extensions.Data.Cryptography;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Collections.Concurrent;

namespace Librame.Extensions.Data.ValueConversion
{
    class InternalEncryptionConverterFactory : IEncryptionConverterFactory
    {
        private readonly ConcurrentDictionary<Type, List<ValueConverter>> _dictionary;


        public InternalEncryptionConverterFactory(ISymmetricAlgorithm symmetric, IAccessorManager accessors)
        {
            _dictionary = new ConcurrentDictionary<Type, List<ValueConverter>>();

            // 针对每个访问器配置独立的值类型转换器
            foreach (var descr in accessors.Descriptors)
            {
                var converters = new List<ValueConverter>();

                // 以字节数组为基础加密提供程序
                var byteArrayProvider = new ByteArrayEncryptionProvider(symmetric, descr.Algorithms);

                // 支持对字节数组类型加密
                converters.Add(new EncryptionConverter<byte[]>(byteArrayProvider));

                // 支持对字符串类型加密
                converters.Add(new EncryptionConverter<string>(new StringEncryptionProvider(byteArrayProvider, descr.Encoding)));

                _dictionary.AddOrUpdate(descr.ServiceType, converters, (k, ov) => converters);
            }
        }


        public ValueConverter GetConverter(Type accessorType, Type propertyType)
        {
            var valueConverters = _dictionary[accessorType];

            var converter = valueConverters.FirstOrDefault(p => p.ModelClrType == propertyType);
            if (converter == null)
                throw new ArgumentException($"The encryption property type '{propertyType}' of the current accessor '{accessorType}' is not supported.");

            return converter;
        }

    }
}
