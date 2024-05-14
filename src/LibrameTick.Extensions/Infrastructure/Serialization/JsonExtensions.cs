#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Infrastructure.Serialization;

/// <summary>
/// 定义 JSON 静态扩展。
/// </summary>
public static class JsonExtensions
{
    /// <summary>
    /// 默认序列化器选项。
    /// </summary>
    public static readonly JsonSerializerOptions DefaultSerializerOptions = InitialSerializerOptions();

    private static JsonSerializerOptions InitialSerializerOptions()
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        options.Converters.Add(new JsonStringEncodingConverter());
        options.Converters.Add(new JsonStringEnumConverter());

        return options;
    }


    /// <summary>
    /// 将对象通过 JSON 序列化形式转换为指定类型实例。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    /// <param name="obj">给定的对象。</param>
    /// <param name="options">给定的 <see cref="JsonSerializerOptions"/>（可选；默认使用 <see cref="DefaultSerializerOptions"/>）。</param>
    /// <returns>返回 <typeparamref name="T"/>。</returns>
    public static T? AsByJson<T>(this object obj, JsonSerializerOptions? options = null)
    {
        options ??= DefaultSerializerOptions;

        var json = JsonSerializer.Serialize(obj, options);

        return JsonSerializer.Deserialize<T>(json, options);
    }


    /// <summary>
    /// 转为 JSON。
    /// </summary>
    /// <param name="obj">给定的对象。</param>
    /// <param name="options">给定的 <see cref="JsonSerializerOptions"/>（可选；默认使用 <see cref="DefaultSerializerOptions"/>）。</param>
    /// <returns>返回字符串。</returns>
    public static string AsJson(this object obj, JsonSerializerOptions? options = null)
        => JsonSerializer.Serialize(obj, options ?? DefaultSerializerOptions);

    /// <summary>
    /// 还原 JSON。
    /// </summary>
    /// <param name="json">给定的 JSON 字符串。</param>
    /// <param name="returnType">给定的返回类型。</param>
    /// <param name="options">给定的 <see cref="JsonSerializerOptions"/>（可选；默认使用 <see cref="DefaultSerializerOptions"/>）。</param>
    /// <returns>返回对象。</returns>
    public static object? FromJson(this string json, Type returnType, JsonSerializerOptions? options = null)
        => JsonSerializer.Deserialize(json, returnType, options ?? DefaultSerializerOptions);

    /// <summary>
    /// 还原 JSON。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    /// <param name="json">给定的 JSON 字符串。</param>
    /// <param name="options">给定的 <see cref="JsonSerializerOptions"/>（可选；默认使用 <see cref="DefaultSerializerOptions"/>）。</param>
    /// <returns>返回 <typeparamref name="T"/>。</returns>
    public static T? FromJson<T>(this string json, JsonSerializerOptions? options = null)
        => JsonSerializer.Deserialize<T>(json, options ?? DefaultSerializerOptions);


    //#region JSON File

    ///// <summary>
    ///// 反序列化 JSON 文件（支持枚举类型）。
    ///// </summary>
    ///// <typeparam name="T">指定的反序列化类型。</typeparam>
    ///// <param name="filePath">给定的文件路径。</param>
    ///// <param name="encoding">给定的字符编码（可选；默认为 <see cref="IDependencyContext.Encoding"/>）。</param>
    ///// <param name="options">给定的 <see cref="JsonSerializerOptions"/>（可选）。</param>
    ///// <returns>返回反序列化对象。</returns>
    //public static T? FromJsonFile<T>(this string filePath, Encoding? encoding = null,
    //    JsonSerializerOptions? options = null)
    //{
    //    var json = File.ReadAllText(filePath, encoding ?? Dependency.Encoding);
    //    return JsonSerializer.Deserialize<T>(json, options);
    //}

    ///// <summary>
    ///// 反序列化 JSON 文件（支持枚举类型）。
    ///// </summary>
    ///// <param name="filePath">给定的文件路径。</param>
    ///// <param name="returnType">给定的反序列化对象类型。</param>
    ///// <param name="encoding">给定的字符编码（可选；默认为 <see cref="IDependencyContext.Encoding"/>）。</param>
    ///// <param name="options">给定的 <see cref="JsonSerializerOptions"/>（可选）。</param>
    ///// <returns>返回反序列化对象。</returns>
    //public static object? FromJsonFile(this string filePath, Type returnType, Encoding? encoding = null,
    //    JsonSerializerOptions? options = null)
    //{
    //    var json = File.ReadAllText(filePath, encoding ?? Dependency.Encoding);
    //    return json.FromJson(returnType, options);
    //}


    ///// <summary>
    ///// 序列化 JSON 文件（支持枚举类型）。
    ///// </summary>
    ///// <param name="filePath">给定的文件路径。</param>
    ///// <param name="value">给定的对象值。</param>
    ///// <param name="encoding">给定的字符编码（可选；默认为 <see cref="IDependencyContext.Encoding"/>）。</param>
    ///// <param name="options">给定的 <see cref="JsonSerializerOptions"/>（可选；默认使用 <see cref="JsonExtensions.DefaultSerializerOptions"/>）。</param>
    ///// <param name="autoCreateDirectory">自动创建目录（可选；默认启用）。</param>
    ///// <returns>返回 JSON 字符串。</returns>
    //public static string AsJsonFile(this string filePath, object value, Encoding? encoding = null,
    //    JsonSerializerOptions? options = null, bool autoCreateDirectory = true)
    //{
    //    var json = value.AsJson(options);

    //    if (autoCreateDirectory)
    //    {
    //        var dir = Path.GetDirectoryName(filePath);
    //        dir!.CreateDirectory();
    //    }

    //    File.WriteAllText(filePath, json, encoding ?? Dependency.Encoding);
    //    return json;
    //}

    //#endregion

}
