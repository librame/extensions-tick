#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions;

/// <summary>
/// 定义用于 JSON 的静态扩展。
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

        options.Converters.Add(new JsonConverters.JsonStringDateTimeConverter());
        options.Converters.Add(new JsonConverters.JsonStringDateTimeOffsetConverter());
        options.Converters.Add(new JsonConverters.JsonStringEncodingConverter());
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

}
