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
/// 定义二进制序列化器。
/// </summary>
public static class BinarySerializer
{
    private static readonly Type _binaryConverterGenericTypeDefinition = typeof(BinaryConverter<>);


    #region FileStream

    /// <summary>
    /// 将文件反序列化为指定类型的实例。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    /// <param name="filePath">给定的文件路径。</param>
    /// <param name="initial">给定的初始实例（可选；默认使用 <see cref="Activator.CreateInstance{T}()"/> 新建）。</param>
    /// <param name="options">给定的 <see cref="BinarySerializerOptions"/>（可选；默认使用 <see cref="Infrastructure.StaticDefaultInitializer{BinarySerializerOptions}.Default"/>）。</param>
    /// <returns>返回对象。</returns>
    public static T? Deserialize<T>(string filePath, T? initial = default, BinarySerializerOptions? options = null)
    {
        using var stream = File.Open(filePath, FileMode.Open);

        return Deserialize(stream, initial, options);
    }

    /// <summary>
    /// 将文件反序列化为指定类型的对象。
    /// </summary>
    /// <param name="filePath">给定的文件路径。</param>
    /// <param name="inputType">给定的输入类型。</param>
    /// <param name="initial">给定的初始对象（可选；默认使用 <see cref="Activator.CreateInstance(Type)"/> 新建）。</param>
    /// <param name="options">给定的 <see cref="BinarySerializerOptions"/>（可选；默认使用 <see cref="Infrastructure.StaticDefaultInitializer{BinarySerializerOptions}.Default"/>）。</param>
    /// <returns>返回对象。</returns>
    public static object? Deserialize(string filePath, Type inputType, object? initial = null,
        BinarySerializerOptions? options = null)
    {
        using var stream = File.Open(filePath, FileMode.Open);

        return Deserialize(stream, inputType, initial, options);
    }


    /// <summary>
    /// 将指定实例序列化为文件。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    /// <param name="filePath">给定的文件路径。</param>
    /// <param name="instance">给定的类型实例。</param>
    /// <param name="options">给定的 <see cref="BinarySerializerOptions"/>（可选；默认使用 <see cref="Infrastructure.StaticDefaultInitializer{BinarySerializerOptions}.Default"/>）。</param>
    public static void Serialize<T>(string filePath, T instance, BinarySerializerOptions? options = null)
    {
        using var stream = File.Open(filePath, FileMode.Create);

        Serialize(stream, instance, options);
    }

    /// <summary>
    /// 将指定对象序列化为文件。
    /// </summary>
    /// <param name="filePath">给定的文件路径。</param>
    /// <param name="inputType">给定的输入类型。</param>
    /// <param name="obj">给定的对象。</param>
    /// <param name="options">给定的 <see cref="BinarySerializerOptions"/>（可选；默认使用 <see cref="Infrastructure.StaticDefaultInitializer{BinarySerializerOptions}.Default"/>）。</param>
    public static void Serialize(string filePath, Type inputType, object obj, BinarySerializerOptions? options = null)
    {
        using var stream = File.Open(filePath, FileMode.Create);

        Serialize(stream, inputType, obj, options);
    }

    #endregion


    #region Stream

    /// <summary>
    /// 将二进制流反序列化为指定类型的实例。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    /// <param name="stream">给定的 <see cref="Stream"/>。</param>
    /// <param name="initial">给定的初始实例（可选；默认使用 <see cref="Activator.CreateInstance{T}()"/> 新建）。</param>
    /// <param name="options">给定的 <see cref="BinarySerializerOptions"/>（可选；默认使用 <see cref="Infrastructure.StaticDefaultInitializer{BinarySerializerOptions}.Default"/>）。</param>
    /// <returns>返回对象。</returns>
    public static T? Deserialize<T>(Stream stream, T? initial = default, BinarySerializerOptions? options = null)
    {
        initial ??= Activator.CreateInstance<T>() ?? throw new ArgumentNullException(nameof(initial));
        options ??= BinarySerializerOptions.Default;

        using var reader = new BinaryReader(stream, options.Encoding);

        var inputType = typeof(T);
        var mappings = BinaryExpressionMapper<T>.GetMappings(inputType, options);

        for (var i = 0; i < mappings.Count; i++)
        {
            mappings[i].Read(reader, initial);
        }

        return initial;
    }

    /// <summary>
    /// 将二进制流反序列化为指定类型的对象。
    /// </summary>
    /// <param name="stream">给定的 <see cref="Stream"/>。</param>
    /// <param name="inputType">给定的输入类型。</param>
    /// <param name="initial">给定的初始对象（可选；默认使用 <see cref="Activator.CreateInstance(Type)"/> 新建）。</param>
    /// <param name="options">给定的 <see cref="BinarySerializerOptions"/>（可选；默认使用 <see cref="Infrastructure.StaticDefaultInitializer{BinarySerializerOptions}.Default"/>）。</param>
    /// <returns>返回对象。</returns>
    public static object? Deserialize(Stream stream, Type inputType, object? initial = null,
        BinarySerializerOptions? options = null)
    {
        initial ??= Activator.CreateInstance(inputType) ?? throw new ArgumentNullException(nameof(initial));
        options ??= BinarySerializerOptions.Default;

        using var reader = new BinaryReader(stream, options.Encoding);

        var mappings = BinaryObjectMapper.GetMappings(inputType, options);

        for (var i = 0; i < mappings.Count; i++)
        {
            mappings[i].ReadObject(reader, initial);
        }

        return initial;
    }


    /// <summary>
    /// 将指定实例序列化为二进制流。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    /// <param name="stream">给定的 <see cref="Stream"/>。</param>
    /// <param name="instance">给定的类型实例。</param>
    /// <param name="options">给定的 <see cref="BinarySerializerOptions"/>（可选；默认使用 <see cref="Infrastructure.StaticDefaultInitializer{BinarySerializerOptions}.Default"/>）。</param>
    public static void Serialize<T>(Stream stream, T instance, BinarySerializerOptions? options = null)
    {
        var inputType = typeof(T);
        options ??= BinarySerializerOptions.Default;

        using var writer = new BinaryWriter(stream, options.Encoding);

        var mappings = BinaryExpressionMapper<T>.GetMappings(inputType, options);

        for (var i = 0; i < mappings.Count; i++)
        {
            mappings[i].Write(writer, instance);
        }
    }

    /// <summary>
    /// 将指定对象序列化为二进制流。
    /// </summary>
    /// <param name="stream">给定的 <see cref="Stream"/>。</param>
    /// <param name="inputType">给定的输入类型。</param>
    /// <param name="obj">给定的对象。</param>
    /// <param name="options">给定的 <see cref="BinarySerializerOptions"/>（可选；默认使用 <see cref="Infrastructure.StaticDefaultInitializer{BinarySerializerOptions}.Default"/>）。</param>
    public static void Serialize(Stream stream, Type inputType, object obj, BinarySerializerOptions? options = null)
    {
        options ??= BinarySerializerOptions.Default;

        using var writer = new BinaryWriter(stream, options.Encoding);

        var mappings = BinaryObjectMapper.GetMappings(inputType, options);

        for (var i = 0; i < mappings.Count; i++)
        {
            mappings[i].WriteObject(writer, obj);
        }
    }

    #endregion

}
