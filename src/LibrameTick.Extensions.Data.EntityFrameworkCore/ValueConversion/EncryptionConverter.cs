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

namespace Librame.Extensions.Data.ValueConversion;

/// <summary>
/// 定义实现 <see cref="ValueConverter{TModel, TProvider}"/> 用于数据加密的加密转换器。
/// </summary>
/// <typeparam name="TValue">指定的值类型。</typeparam>
public class EncryptionConverter<TValue> : ValueConverter<TValue, TValue>
{
    /// <summary>
    /// 构造一个 <see cref="EncryptionConverter{TValue}"/>。
    /// </summary>
    /// <param name="provider">给定的 <see cref="IEncryptionProvider{TValue}"/>。</param>
    public EncryptionConverter(IEncryptionProvider<TValue> provider)
        : base(s => provider.Encrypt(s), s => provider.Decrypt(s))
    {
    }

}
