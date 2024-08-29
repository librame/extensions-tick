#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Infrastructure;

namespace Librame.Extensions.Serialization;

/// <summary>
/// 定义二进制静态扩展。
/// </summary>
public static class BinaryExtensions
{

    #region BinaryReader and BinaryWriter Type

    /// <summary>
    /// 读取序列化类型字符串。
    /// </summary>
    /// <param name="reader">给定的 <see cref="BinaryReader"/>。</param>
    /// <returns>返回类型字符串。</returns>
    public static string ReadTypeString(this BinaryReader reader)
        => reader.ReadString();


    /// <summary>
    /// 写入序列化类型字符串。
    /// </summary>
    /// <typeparam name="T">指定的序列化类型。</typeparam>
    /// <param name="writer">给定的 <see cref="BinaryWriter"/>。</param>
    public static void WriteTypeString<T>(this BinaryWriter writer)
        => writer.WriteTypeString(typeof(T));

    /// <summary>
    /// 写入序列化类型字符串。
    /// </summary>
    /// <param name="writer">给定的 <see cref="BinaryWriter"/>。</param>
    /// <param name="type">给定的序列化类型。</param>
    public static void WriteTypeString(this BinaryWriter writer, Type type)
        => writer.Write(type.GetFriendlyName());

    #endregion


    #region BinaryReader and BinaryWriter Version

    private static readonly string _useVersionKey = $"[{nameof(BinarySerializerOptions.UseVersion)}]";


    /// <summary>
    /// 读取版本信息。
    /// </summary>
    /// <param name="reader">给定的 <see cref="BinaryReader"/>。</param>
    /// <returns>返回 <see cref="BinarySerializerVersion"/>。</returns>
    public static BinarySerializerVersion? ReadVersion(this BinaryReader reader)
        => TryReadVersion(reader, out var version) ? version : null;

    /// <summary>
    /// 写入版本信息。
    /// </summary>
    /// <param name="writer">给定的 <see cref="BinaryWriter"/>。</param>
    /// <param name="options">给定的 <see cref="BinarySerializerOptions"/>。</param>
    public static void WriteVersion(this BinaryWriter writer, BinarySerializerOptions options)
    {
        writer.Write(_useVersionKey);
        writer.Write(options.UseVersion is not null);

        if (options.UseVersion is not null)
        {
            writer.Write(options.UseVersion.Version);
            writer.Write(Enum.GetName(options.UseVersion.Comparison) ?? options.UseVersion.Comparison.ToString());
        }
    }

    /// <summary>
    /// 尝试读取版本信息。
    /// </summary>
    /// <param name="reader">给定的 <see cref="BinaryReader"/>。</param>
    /// <param name="version">输出可能存在的 <see cref="BinarySerializerVersion"/>。</param>
    /// <returns>返回是否成功读取的布尔值。</returns>
    public static bool TryReadVersion(this BinaryReader reader,
        [NotNullWhen(true)] out BinarySerializerVersion? version)
    {
        string? useVersionKey;
        bool hasUseVersion;

        try
        {
            // Try read version key
            useVersionKey = reader.ReadString();

            // Try read version is not null
            hasUseVersion = reader.ReadBoolean();
        }
        catch
        {
            useVersionKey = null;
            hasUseVersion = false;
        }

        if (_useVersionKey.Equals(useVersionKey, StringComparison.Ordinal) && hasUseVersion)
        {
            var number = reader.ReadDouble();
            var comparison = Enum.Parse<BinaryVersionComparison>(reader.ReadString());

            version = new(number, comparison);
            return true;
        }

        version = null;
        return false;
    }

    #endregion


    #region IsAttributeDefined

    private static readonly Type _ignoreAttributeType = typeof(BinaryIgnoreAttribute);


    /// <summary>
    /// 判断成员是否定义了忽略特性。
    /// </summary>
    /// <param name="member">给定的 <see cref="MemberInfo"/>。</param>
    /// <returns>返回是否定义的布尔值。</returns>
    public static bool IsIgnoreAttributeDefined(this MemberInfo member)
        => member.IsDefined(_ignoreAttributeType, inherit: false);


    /// <summary>
    /// 判断成员是否定义了映射特性。
    /// </summary>
    /// <param name="member">给定的 <see cref="MemberInfo"/>。</param>
    /// <returns>返回是否定义的布尔值。</returns>
    public static bool IsMappingAttributeDefined(this MemberInfo member)
        => member.IsMappingAttributeDefined(out _);

    /// <summary>
    /// 判断成员是否定义了映射特性。
    /// </summary>
    /// <param name="member">给定的 <see cref="MemberInfo"/>。</param>
    /// <param name="attribute">输出 <see cref="BinaryMappingAttribute"/>。</param>
    /// <returns>返回是否定义的布尔值。</returns>
    public static bool IsMappingAttributeDefined(this MemberInfo member,
        [NotNullWhen(true)] out BinaryMappingAttribute? attribute)
    {
        attribute = member.GetCustomAttribute<BinaryMappingAttribute>(inherit: false);
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
        => BinarySerializer.SerializeObject(obj, obj.GetType(), options);

    /// <summary>
    /// 从字节数组反序列化为指定类型的对象。
    /// </summary>
    /// <param name="byteArray">给定的字节数组。</param>
    /// <param name="returnType">给定的返回类型。</param>
    /// <param name="options">给定的 <see cref="BinarySerializerOptions"/>（可选；默认使用 <see cref="StaticDefaultInitializer{BinarySerializerOptions}.Default"/>）。</param>
    /// <returns>返回对象。</returns>
    public static object? FromBinary(this byte[] byteArray, Type returnType, BinarySerializerOptions? options = null)
        => BinarySerializer.DeserializeObject(byteArray, returnType, options);

    /// <summary>
    /// 从字节数组反序列化为指定类型的实例。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    /// <param name="byteArray">给定的字节数组。</param>
    /// <param name="options">给定的 <see cref="BinarySerializerOptions"/>（可选；默认使用 <see cref="StaticDefaultInitializer{BinarySerializerOptions}.Default"/>）。</param>
    /// <returns>返回 <typeparamref name="T"/>。</returns>
    public static T? FromBinary<T>(this byte[] byteArray, BinarySerializerOptions? options = null)
        => BinarySerializer.Deserialize<T>(byteArray, options);

    #endregion


    #region Serialize BinaryFile

    /// <summary>
    /// 将指定对象序列化为二进制文件。
    /// </summary>
    /// <param name="filePath">给定的二进制文件路径。</param>
    /// <param name="obj">给定的对象。</param>
    /// <param name="options">给定的 <see cref="BinarySerializerOptions"/>（可选；默认使用 <see cref="StaticDefaultInitializer{BinarySerializerOptions}.Default"/>）。</param>
    public static void AsBinaryFile(this FluentFilePath filePath, object obj, BinarySerializerOptions? options = null)
        => BinarySerializer.SerializeObject(filePath, obj, obj.GetType(), options);

    /// <summary>
    /// 从二进制文件反序列化为指定类型的对象。
    /// </summary>
    /// <param name="filePath">给定的二进制文件路径。</param>
    /// <param name="returnType">给定的返回类型。</param>
    /// <param name="options">给定的 <see cref="BinarySerializerOptions"/>（可选；默认使用 <see cref="StaticDefaultInitializer{BinarySerializerOptions}.Default"/>）。</param>
    /// <returns>返回对象。</returns>
    public static object? FromBinaryFile(this FluentFilePath filePath, Type returnType, BinarySerializerOptions? options = null)
        => BinarySerializer.DeserializeObject(filePath, returnType, options);

    /// <summary>
    /// 从二进制文件反序列化为指定类型的实例。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    /// <param name="filePath">给定的二进制文件路径。</param>
    /// <param name="options">给定的 <see cref="BinarySerializerOptions"/>（可选；默认使用 <see cref="StaticDefaultInitializer{BinarySerializerOptions}.Default"/>）。</param>
    /// <returns>返回 <typeparamref name="T"/>。</returns>
    public static T? FromBinaryFile<T>(this FluentFilePath filePath, BinarySerializerOptions? options = null)
        => BinarySerializer.Deserialize<T>(filePath, options);

    #endregion

}
