#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Compression;
using Librame.Extensions.Cryptography;
using Librame.Extensions.Dependency;
using Librame.Extensions.Infrastructure;

namespace Librame.Extensions.Serialization;

/// <summary>
/// 定义二进制序列化器。
/// </summary>
public static class BinarySerializer
{

    #region FileStream

    /// <summary>
    /// 将文件反序列化为指定类型的实例。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    /// <param name="filePath">给定的文件路径。</param>
    /// <param name="initial">给定的初始实例（可选；默认使用 <see cref="Activator.CreateInstance{T}()"/> 新建）。</param>
    /// <param name="options">给定的 <see cref="BinarySerializerOptions"/>（可选；默认使用 <see cref="StaticDefaultInitializer{BinarySerializerOptions}.Default"/>）。</param>
    /// <returns>返回 <typeparamref name="T"/>。</returns>
    public static T? Deserialize<T>(FluentFilePath filePath,
        BinarySerializerOptions? options = null, T? initial = default)
    {
        options ??= BinarySerializerOptions.Default;

        // 从下层流还原对象时默认会自行释放此流，因此无需手动释放
        var fluentStream = filePath.AsReadStream();

        return Deserialize(fluentStream, options, initial);
    }

    /// <summary>
    /// 将文件反序列化为指定类型的对象。
    /// </summary>
    /// <param name="filePath">给定的文件路径。</param>
    /// <param name="inputType">给定的输入类型。</param>
    /// <param name="initial">给定的初始对象（可选；默认使用 <see cref="Activator.CreateInstance(Type)"/> 新建）。</param>
    /// <param name="options">给定的 <see cref="BinarySerializerOptions"/>（可选；默认使用 <see cref="StaticDefaultInitializer{BinarySerializerOptions}.Default"/>）。</param>
    /// <returns>返回对象。</returns>
    public static object? DeserializeObject(FluentFilePath filePath, Type inputType,
        BinarySerializerOptions? options = null, object? initial = null)
    {
        options ??= BinarySerializerOptions.Default;

        // 从下层流还原对象时默认会自行释放此流，因此无需手动释放
        var fluentStream = filePath.AsReadStream();

        return DeserializeObject(fluentStream, inputType, options, initial);
    }


    /// <summary>
    /// 将指定实例序列化为文件。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    /// <param name="filePath">给定的文件路径。</param>
    /// <param name="instance">给定的 <typeparamref name="T"/>。</param>
    /// <param name="options">给定的 <see cref="BinarySerializerOptions"/>（可选；默认使用 <see cref="StaticDefaultInitializer{BinarySerializerOptions}.Default"/>）。</param>
    public static void Serialize<T>(FluentFilePath filePath, T instance, BinarySerializerOptions? options = null)
    {
        options ??= BinarySerializerOptions.Default;

        using var fluentStream = filePath.AsWriteStream();

        Serialize(fluentStream, instance, options);
    }

    /// <summary>
    /// 将指定对象序列化为文件。
    /// </summary>
    /// <param name="filePath">给定的文件路径。</param>
    /// <param name="obj">给定的对象。</param>
    /// <param name="inputType">给定的输入类型。</param>
    /// <param name="options">给定的 <see cref="BinarySerializerOptions"/>（可选；默认使用 <see cref="StaticDefaultInitializer{BinarySerializerOptions}.Default"/>）。</param>
    public static void SerializeObject(FluentFilePath filePath, object obj, Type inputType,
        BinarySerializerOptions? options = null)
    {
        options ??= BinarySerializerOptions.Default;

        using var fluentStream = filePath.AsWriteStream();

        SerializeObject(fluentStream, obj, inputType, options);
    }

    #endregion


    #region MemoryStream

    /// <summary>
    /// 将字节数组反序列化为指定类型的实例。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    /// <param name="bytes">给定的字节数组。</param>
    /// <param name="options">给定的 <see cref="BinarySerializerOptions"/>（可选；默认使用 <see cref="StaticDefaultInitializer{BinarySerializerOptions}.Default"/>）。</param>
    /// <param name="initial">给定的初始实例（可选；默认使用 <see cref="Activator.CreateInstance{T}()"/> 新建）。</param>
    /// <returns>返回 <typeparamref name="T"/>。</returns>
    public static T? Deserialize<T>(byte[] bytes, BinarySerializerOptions? options = null,
        T? initial = default)
    {
        options ??= BinarySerializerOptions.Default;

        var memoryStream = DependencyRegistration.CurrentContext.MemoryStreams.GetStream(bytes);

        // 从下层流还原对象时默认会自行释放此流，因此无需手动释放
        var fluentStream = new FluentStream(memoryStream, useBufferedStream: true);

        return Deserialize(fluentStream, options, initial);
    }

    /// <summary>
    /// 将字节数组反序列化为指定类型的对象。
    /// </summary>
    /// <param name="bytes">给定的字节数组。</param>
    /// <param name="inputType">给定的输入类型。</param>
    /// <param name="options">给定的 <see cref="BinarySerializerOptions"/>（可选；默认使用 <see cref="StaticDefaultInitializer{BinarySerializerOptions}.Default"/>）。</param>
    /// <param name="initial">给定的初始对象（可选；默认使用 <see cref="Activator.CreateInstance(Type)"/> 新建）。</param>
    /// <returns>返回对象。</returns>
    public static object? DeserializeObject(byte[] bytes, Type inputType,
        BinarySerializerOptions? options = null, object? initial = null)
    {
        options ??= BinarySerializerOptions.Default;

        var memoryStream = DependencyRegistration.CurrentContext.MemoryStreams.GetStream(bytes);

        // 从下层流还原对象时默认会自行释放此流，因此无需手动释放
        var fluentStream = new FluentStream(memoryStream, useBufferedStream: true);

        return DeserializeObject(fluentStream, inputType, options, initial);
    }


    /// <summary>
    /// 将指定类型的实例序列化为字节数组。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    /// <param name="instance">给定的类型实例。</param>
    /// <param name="options">给定的 <see cref="BinarySerializerOptions"/>（可选；默认使用 <see cref="StaticDefaultInitializer{BinarySerializerOptions}.Default"/>）。</param>
    /// <returns>返回字节数组。</returns>
    public static byte[] Serialize<T>(T instance, BinarySerializerOptions? options = null)
    {
        options ??= BinarySerializerOptions.Default;

        using var memoryStream = DependencyRegistration.CurrentContext.MemoryStreams.GetStream();

        using (var fluentStream = new FluentStream(memoryStream, useBufferedStream: true))
        {
            Serialize(fluentStream, instance, options);

            return memoryStream.GetReadOnlySequence().ToArray();
        }
    }

    /// <summary>
    /// 将指定类型的对象序列化为字节数组。
    /// </summary>
    /// <param name="obj">给定的对象。</param>
    /// <param name="inputType">给定的输入类型。</param>
    /// <param name="options">给定的 <see cref="BinarySerializerOptions"/>（可选；默认使用 <see cref="StaticDefaultInitializer{BinarySerializerOptions}.Default"/>）。</param>
    /// <returns>返回字节数组。</returns>
    public static byte[] SerializeObject(object obj, Type inputType, BinarySerializerOptions? options = null)
    {
        options ??= BinarySerializerOptions.Default;

        var memoryStream = DependencyRegistration.CurrentContext.MemoryStreams.GetStream();

        using (var fluentStream = new FluentStream(memoryStream, useBufferedStream: true))
        {
            SerializeObject(fluentStream, obj, inputType, options);

            return memoryStream.GetReadOnlySequence().ToArray();
        }
    }

    #endregion


    #region FluentStream

    /// <summary>
    /// 将二进制流反序列化为指定类型的实例核心。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    /// <param name="fluentStream">给定的 <see cref="FluentStream"/>。</param>
    /// <param name="options">给定的 <see cref="BinarySerializerOptions"/>。</param>
    /// <param name="initial">给定的初始实例（可选；默认使用 <see cref="Activator.CreateInstance{T}()"/> 新建）。</param>
    /// <returns>返回对象。</returns>
    public static T? Deserialize<T>(FluentStream fluentStream, BinarySerializerOptions options,
        T? initial = default)
    {
        if (options.Compressions.IsRefEnabled)
        {
            fluentStream.SwitchUnderlyWithNewMemory((fileStream, decompressedStream) =>
            {
                fileStream.Decompress(decompressedStream, options.Compressions);
            });
        }

        if (options.Algorithms.IsRefEnabled)
        {
            fluentStream.SwitchUnderlyWithNewMemory((fileOrDecompressedStream, decryptedStream) =>
            {
                fileOrDecompressedStream.ResetOriginalPositionIfNotBegin();

                fileOrDecompressedStream.FromPrivateRsa(decryptedStream);
            });
        }

        var result = fluentStream.FromUnderly(resultStream =>
        {
            resultStream.ResetOriginalPositionIfNotBegin();

            return DeserializeCore(resultStream, options, initial);
        });

        return result;
    }

    /// <summary>
    /// 将二进制流反序列化为指定类型的对象。
    /// </summary>
    /// <remarks>
    /// 说明：本方法支持解密、解压等增强功能（如果已启用对应选项的功能引用）。
    /// </remarks>
    /// <param name="fluentStream">给定的 <see cref="FluentStream"/>。</param>
    /// <param name="inputType">给定的输入类型。</param>
    /// <param name="options">给定的 <see cref="BinarySerializerOptions"/>。</param>
    /// <param name="initial">给定的初始对象（可选；默认使用 <see cref="Activator.CreateInstance(Type)"/> 新建）。</param>
    /// <returns>返回对象。</returns>
    public static object? DeserializeObject(FluentStream fluentStream, Type inputType,
        BinarySerializerOptions options, object? initial = null)
    {
        if (options.Compressions.IsRefEnabled)
        {
            fluentStream.SwitchUnderlyWithNewMemory((fileStream, decompressedStream) =>
            {
                fileStream.Decompress(decompressedStream, options.Compressions);
            });
        }

        if (options.Algorithms.IsRefEnabled)
        {
            fluentStream.SwitchUnderlyWithNewMemory((fileOrDecompressedStream, decryptedStream) =>
            {
                fileOrDecompressedStream.ResetOriginalPositionIfNotBegin();

                fileOrDecompressedStream.FromPrivateRsa(decryptedStream);
            });
        }

        var result = fluentStream.FromUnderly(resultStream =>
        {
            resultStream.ResetOriginalPositionIfNotBegin();

            return DeserializeObjectCore(resultStream, inputType, options, initial);
        });

        return result;
    }


    /// <summary>
    /// 将指定实例序列化为二进制流。
    /// </summary>
    /// <remarks>
    /// 说明：本方法支持加密、压缩等增强功能（如果已启用对应选项的功能引用）。
    /// </remarks>
    /// <typeparam name="T">指定的类型。</typeparam>
    /// <param name="fluentStream">给定的 <see cref="FluentStream"/>。</param>
    /// <param name="instance">给定的类型实例。</param>
    /// <param name="options">给定的 <see cref="BinarySerializerOptions"/>。</param>
    public static void Serialize<T>(FluentStream fluentStream, T instance, BinarySerializerOptions options)
    {
        var tempStream = fluentStream.CreateMemoryOrBufferedStream();

        SerializeCore(tempStream, instance, options);

        if (options.Algorithms.IsRefEnabled)
        {
            fluentStream.SwitchNewMemory(ref tempStream, options.Algorithms.DefaultStreamEncryptor);
        }

        if (options.Compressions.IsRefEnabled)
        {
            fluentStream.SwitchNewMemory(ref tempStream, (currentStream, compressedStream) =>
            {
                currentStream.Compress(compressedStream, options.Compressions);
            });
        }

        fluentStream.CopyFrom(ref tempStream, disposing: true);
    }

    /// <summary>
    /// 将指定对象序列化为二进制流。
    /// </summary>
    /// <remarks>
    /// 说明：本方法支持加密、压缩等增强功能（如果已启用对应选项的功能引用）。
    /// </remarks>
    /// <param name="fluentStream">给定的 <see cref="FluentStream"/>。</param>
    /// <param name="obj">给定的对象。</param>
    /// <param name="inputType">给定的输入类型。</param>
    /// <param name="options">给定的 <see cref="BinarySerializerOptions"/>。</param>
    public static void SerializeObject(FluentStream fluentStream, object obj, Type inputType,
        BinarySerializerOptions options)
    {
        var tempStream = fluentStream.CreateMemoryOrBufferedStream();

        SerializeObjectCore(tempStream, obj, inputType, options);

        if (options.Algorithms.IsRefEnabled)
        {
            fluentStream.SwitchNewMemory(ref tempStream, options.Algorithms.DefaultStreamEncryptor);
        }

        if (options.Compressions.IsRefEnabled)
        {
            fluentStream.SwitchNewMemory(ref tempStream, (currentStream, compressedStream) =>
            {
                currentStream.Compress(compressedStream, options.Compressions);
            });
        }

        fluentStream.CopyFrom(ref tempStream, disposing: true);
    }

    #endregion


    #region Stream

    /// <summary>
    /// 将二进制流反序列化为指定类型的实例。
    /// </summary>
    /// <remarks>
    /// 说明：核心方法只包含最基础的反序列化功能，不包含任何解密、解压等增强功能。
    /// </remarks>
    /// <typeparam name="T">指定的类型。</typeparam>
    /// <param name="stream">给定的 <see cref="Stream"/>。</param>
    /// <param name="options">给定的 <see cref="BinarySerializerOptions"/>。</param>
    /// <param name="initial">给定的初始实例（可选；默认使用 <see cref="Activator.CreateInstance{T}()"/> 新建）。</param>
    /// <returns>返回对象。</returns>
    public static T? DeserializeCore<T>(Stream stream, BinarySerializerOptions options, T? initial = default)
    {
        initial ??= Activator.CreateInstance<T>() ?? throw new ArgumentNullException(nameof(initial));

        var reader = new BinaryReader(stream, options.Encoding);

        var useVersion = reader.ReadVersion();

        var mappings = BinaryExpressionMapper<T>.GetMappings(options, useVersion);

        for (var i = 0; i < mappings.Count; i++)
        {
            mappings[i].Read(reader, initial);
        }

        return initial;
    }

    /// <summary>
    /// 将二进制流反序列化为指定类型的对象。
    /// </summary>
    /// <remarks>
    /// 说明：核心方法只包含最基础的反序列化功能，不包含任何解密、解压等增强功能。
    /// </remarks>
    /// <param name="stream">给定的 <see cref="Stream"/>。</param>
    /// <param name="inputType">给定的输入类型。</param>
    /// <param name="options">给定的 <see cref="BinarySerializerOptions"/>。</param>
    /// <param name="initial">给定的初始对象（可选；默认使用 <see cref="Activator.CreateInstance(Type)"/> 新建）。</param>
    /// <returns>返回对象。</returns>
    public static object? DeserializeObjectCore(Stream stream, Type inputType,
        BinarySerializerOptions options, object? initial = null)
    {
        initial ??= Activator.CreateInstance(inputType) ?? throw new ArgumentNullException(nameof(initial));

        var reader = new BinaryReader(stream, options.Encoding);

        var useVersion = reader.ReadVersion();

        var mappings = BinaryObjectMapper.GetMappings(inputType, options, useVersion);

        for (var i = 0; i < mappings.Count; i++)
        {
            mappings[i].ReadObject(reader, initial);
        }

        return initial;
    }


    /// <summary>
    /// 将指定实例序列化为二进制流。
    /// </summary>
    /// <remarks>
    /// 说明：核心方法只包含最基础的序列化功能，不包含任何加密、压缩等增强功能。
    /// </remarks>
    /// <typeparam name="T">指定的类型。</typeparam>
    /// <param name="stream">给定的 <see cref="Stream"/>。</param>
    /// <param name="instance">给定的类型实例。</param>
    /// <param name="options">给定的 <see cref="BinarySerializerOptions"/>。</param>
    public static void SerializeCore<T>(Stream stream, T instance, BinarySerializerOptions options)
    {
        var writer = new BinaryWriter(stream, options.Encoding);

        writer.WriteVersion(options);

        var mappings = BinaryExpressionMapper<T>.GetMappings(options, useVersion: null);

        for (var i = 0; i < mappings.Count; i++)
        {
            mappings[i].Write(writer, instance);
        }
    }

    /// <summary>
    /// 将指定对象序列化为二进制流。
    /// </summary>
    /// <remarks>
    /// 说明：核心方法只包含最基础的序列化功能，不包含任何加密、压缩等增强功能。
    /// </remarks>
    /// <param name="stream">给定的 <see cref="Stream"/>。</param>
    /// <param name="obj">给定的对象。</param>
    /// <param name="inputType">给定的输入类型。</param>
    /// <param name="options">给定的 <see cref="BinarySerializerOptions"/>。</param>
    public static void SerializeObjectCore(Stream stream, object obj, Type inputType,
        BinarySerializerOptions options)
    {
        var writer = new BinaryWriter(stream, options.Encoding);

        writer.WriteVersion(options);

        var mappings = BinaryObjectMapper.GetMappings(inputType, options, useVersion: null);

        for (var i = 0; i < mappings.Count; i++)
        {
            mappings[i].WriteObject(writer, obj);
        }
    }

    #endregion

}
