#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data.ValueConversion;

/// <summary>
/// 定义实现 <see cref="ValueConverter{TModel, TProvider}"/> 的强类型标识符转换器。
/// </summary>
/// <typeparam name="TValue">指定的值类型。</typeparam>
public class StronglyTypedIdentifierConverter<TValue> : ValueConverter<StronglyTypedIdentifier<TValue>, TValue>
{
    /// <summary>
    /// 构造一个 <see cref="StronglyTypedIdentifier{TValue}"/>。
    /// </summary>
    public StronglyTypedIdentifierConverter()
        : base(static s => s.Value, static v => new(v))
    {
    }

}
