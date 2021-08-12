#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Data.Access;
using Librame.Extensions.Data.Cryptography;
using Librame.Extensions.Data.ValueConversion;
using Microsoft.EntityFrameworkCore;

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 定义 <see cref="ModelBuilder"/> 与 <see cref="IAccessor"/> 静态扩展。
    /// </summary>
    public static class ModelBuilderAccessorExtensions
    {
        private static readonly Type _encryptedAttributeType = typeof(EncryptedAttribute);


        /// <summary>
        /// 对使用 <see cref="EncryptedAttribute"/> 的属性应用加密功能。
        /// </summary>
        /// <param name="modelBuilder">给定的 <see cref="ModelBuilder"/>。</param>
        /// <param name="converterFactory">给定的 <see cref="IEncryptionConverterFactory"/>。</param>
        /// <param name="accessorType">给定的访问器类型。</param>
        /// <returns>返回 <see cref="ModelBuilder"/>。</returns>
        public static ModelBuilder UseEncryption(this ModelBuilder modelBuilder,
            IEncryptionConverterFactory converterFactory, Type accessorType)
        {
            foreach (var metadata in modelBuilder.Model.GetEntityTypes())
            {
                var encryptedProperties = metadata.ClrType.GetProperties()
                    .Where(p => Attribute.IsDefined(p, _encryptedAttributeType));

                foreach (var property in encryptedProperties)
                {
                    var converter = converterFactory.GetConverter(accessorType, property.PropertyType);
                    metadata.GetProperty(property.Name).SetValueConverter(converter);
                }
            }

            return modelBuilder;
        }

    }
}
