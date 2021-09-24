#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Data.Accessing;

namespace Librame.Extensions.Data.ValueConversion;

/// <summary>
/// 定义用于统一管理 <see cref="EncryptionConverter{TValue}"/> 的加密转换器接口。
/// </summary>
public interface IEncryptionConverterFactory
{
    /// <summary>
    /// 获取指定访问器的属性类型值转换器。
    /// </summary>
    /// <param name="accessor">给定的 <see cref="IDataAccessor"/>。</param>
    /// <param name="propertyType">给定的属性类型。</param>
    /// <returns>返回 <see cref="ValueConverter"/>。</returns>
    ValueConverter GetConverter(IDataAccessor accessor, Type propertyType);
}
