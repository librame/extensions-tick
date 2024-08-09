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
/// 定义二进制静态扩展。
/// </summary>
public static class BinaryExtensions
{
    private static readonly Type _ignoreAttributeType = typeof(BinaryIgnoreAttribute);


    #region IsAttributeDefined

    /// <summary>
    /// 判断成员是否定义了忽略特性。
    /// </summary>
    /// <param name="member">给定的 <see cref="MemberInfo"/>。</param>
    /// <returns>返回是否定义的布尔值。</returns>
    public static bool IsIgnoreAttributeDefined(this MemberInfo member)
        => member.IsDefined(_ignoreAttributeType, inherit: false);


    /// <summary>
    /// 判断成员是否定义了表达式映射特性。
    /// </summary>
    /// <param name="member">给定的 <see cref="MemberInfo"/>。</param>
    /// <returns>返回是否定义的布尔值。</returns>
    public static bool IsExpressionMappingAttributeDefined(this MemberInfo member)
        => member.IsExpressionMappingAttributeDefined(out _);

    /// <summary>
    /// 判断成员是否定义了表达式映射特性。
    /// </summary>
    /// <param name="member">给定的 <see cref="MemberInfo"/>。</param>
    /// <param name="attribute">输出 <see cref="BinaryExpressionMappingAttribute"/>。</param>
    /// <returns>返回是否定义的布尔值。</returns>
    public static bool IsExpressionMappingAttributeDefined(this MemberInfo member,
        [NotNullWhen(true)] out BinaryExpressionMappingAttribute? attribute)
    {
        attribute = member.GetCustomAttribute<BinaryExpressionMappingAttribute>(inherit: false);
        return attribute is not null;
    }

    /// <summary>
    /// 判断成员是否定义了对象映射特性。
    /// </summary>
    /// <param name="member">给定的 <see cref="MemberInfo"/>。</param>
    /// <returns>返回是否定义的布尔值。</returns>
    public static bool IsObjectMappingAttributeDefined(this MemberInfo member)
        => member.IsObjectMappingAttributeDefined(out _);

    /// <summary>
    /// 判断成员是否定义了对象映射特性。
    /// </summary>
    /// <param name="member">给定的 <see cref="MemberInfo"/>。</param>
    /// <param name="attribute">输出 <see cref="BinaryObjectMappingAttribute"/>。</param>
    /// <returns>返回是否定义的布尔值。</returns>
    public static bool IsObjectMappingAttributeDefined(this MemberInfo member,
        [NotNullWhen(true)] out BinaryObjectMappingAttribute? attribute)
    {
        attribute = member.GetCustomAttribute<BinaryObjectMappingAttribute>(inherit: false);
        return attribute is not null;
    }

    #endregion


    #region Serialize Binary

    /// <summary>
    /// 将指定对象序列化为字节数组。
    /// </summary>
    /// <param name="obj">给定的对象。</param>
    /// <param name="options">给定的 <see cref="BinarySerializerOptions"/>（可选；默认使用 <see cref="StaticDefaultInitializer{BinarySerializerOptions}.Default"/>）。</param>
    /// <returns>返回二进制字符串。</returns>
    public static byte[] AsBinary(this object obj, BinarySerializerOptions? options = null)
        => BinarySerializer.SerializeObject(obj.GetType(), obj, options: options);

    /// <summary>
    /// 从字节数组反序列化为指定类型的对象。
    /// </summary>
    /// <param name="byteArray">给定的字节数组。</param>
    /// <param name="returnType">给定的返回类型。</param>
    /// <param name="options">给定的 <see cref="BinarySerializerOptions"/>（可选；默认使用 <see cref="StaticDefaultInitializer{BinarySerializerOptions}.Default"/>）。</param>
    /// <returns>返回对象。</returns>
    public static object? FromBinary(this byte[] byteArray, Type returnType, BinarySerializerOptions? options = null)
        => BinarySerializer.DeserializeObject(byteArray, returnType, options: options);

    /// <summary>
    /// 从字节数组反序列化为指定类型的实例。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    /// <param name="byteArray">给定的字节数组。</param>
    /// <param name="options">给定的 <see cref="BinarySerializerOptions"/>（可选；默认使用 <see cref="StaticDefaultInitializer{BinarySerializerOptions}.Default"/>）。</param>
    /// <returns>返回 <typeparamref name="T"/>。</returns>
    public static T? FromBinary<T>(this byte[] byteArray, BinarySerializerOptions? options = null)
        => BinarySerializer.Deserialize<T>(byteArray, options: options);

    #endregion


    #region Serialize BinaryFile

    /// <summary>
    /// 将指定对象序列化为二进制文件。
    /// </summary>
    /// <param name="obj">给定的对象。</param>
    /// <param name="filePath">给定的二进制文件路径。</param>
    /// <param name="options">给定的 <see cref="BinarySerializerOptions"/>（可选；默认使用 <see cref="StaticDefaultInitializer{BinarySerializerOptions}.Default"/>）。</param>
    public static void AsBinaryFile(this object obj, string filePath, BinarySerializerOptions? options = null)
        => BinarySerializer.SerializeObject(filePath, obj.GetType(), obj, options: options);

    /// <summary>
    /// 从二进制文件反序列化为指定类型的对象。
    /// </summary>
    /// <param name="filePath">给定的二进制文件路径。</param>
    /// <param name="returnType">给定的返回类型。</param>
    /// <param name="options">给定的 <see cref="BinarySerializerOptions"/>（可选；默认使用 <see cref="StaticDefaultInitializer{BinarySerializerOptions}.Default"/>）。</param>
    /// <returns>返回对象。</returns>
    public static object? FromBinaryFile(this string filePath, Type returnType, BinarySerializerOptions? options = null)
        => BinarySerializer.DeserializeObject(filePath, returnType, options: options);

    /// <summary>
    /// 从二进制文件反序列化为指定类型的实例。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    /// <param name="filePath">给定的二进制文件路径。</param>
    /// <param name="options">给定的 <see cref="BinarySerializerOptions"/>（可选；默认使用 <see cref="StaticDefaultInitializer{BinarySerializerOptions}.Default"/>）。</param>
    /// <returns>返回 <typeparamref name="T"/>。</returns>
    public static T? FromBinaryFile<T>(this string filePath, BinarySerializerOptions? options = null)
        => BinarySerializer.Deserialize<T>(filePath, options: options);

    #endregion

}
