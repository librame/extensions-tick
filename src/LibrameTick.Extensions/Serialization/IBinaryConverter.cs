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
/// 定义二进制转换器接口。
/// </summary>
public interface IBinaryConverter
{
    /// <summary>
    /// 获取被转换的类型。
    /// </summary>
    /// <value>
    /// 返回 <see cref="Type"/>。
    /// </value>
    Type BeConvertedType { get; }

    /// <summary>
    /// 获取转换器类型。
    /// </summary>
    /// <value>
    /// 返回 <see cref="Type"/>。
    /// </value>
    Type ConverterType { get; }

    /// <summary>
    /// 获取转换器名称。
    /// </summary>
    /// <value>
    /// 返回名称字符串。
    /// </value>
    string ConverterName { get; }

    /// <summary>
    /// 是否处理空值。
    /// </summary>
    /// <value>
    /// 返回是否处理的布尔值。
    /// </value>
    bool HandleNull { get; }


    /// <summary>
    /// 判断类型可以转换。
    /// </summary>
    /// <param name="typeToConvert">给定要转换的类型。</param>
    /// <returns>
    /// 返回能转换的布尔值。
    /// </returns>
    bool CanConvert(Type typeToConvert);


    /// <summary>
    /// 获取读取方法。
    /// </summary>
    /// <returns>返回 <see cref="MethodInfo"/>。</returns>
    MethodInfo GetReadMethod();

    /// <summary>
    /// 获取写入方法。
    /// </summary>
    /// <returns>返回 <see cref="MethodInfo"/>。</returns>
    MethodInfo GetWriteMethod();
}
