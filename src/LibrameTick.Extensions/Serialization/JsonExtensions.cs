#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Dependency;

namespace Librame.Extensions.Serialization;

/// <summary>
/// 定义 JSON 静态扩展。
/// </summary>
public static class JsonExtensions
{
    /// <summary>
    /// 当前 JSON 依赖。
    /// </summary>
    public static Lazy<IJsonDependency> JsonDependency
        => new(() => DependencyRegistration.InitializeDependency<Internal.JsonDependencyInitializer, IJsonDependency>());


    #region Serialize Json

    /// <summary>
    /// 将指定对象序列化为 JSON。
    /// </summary>
    /// <param name="obj">给定的对象。</param>
    /// <param name="options">给定的 <see cref="JsonSerializerOptions"/>（可选；默认使用 <see cref="JsonDependency"/> 选项）。</param>
    /// <returns>返回 JSON 字符串。</returns>
    public static string AsJson(this object obj, JsonSerializerOptions? options = null)
        => JsonSerializer.Serialize(obj, options ?? JsonDependency.Value.Options.Value);

    /// <summary>
    /// 从 JSON 反序列化为指定类型的对象。
    /// </summary>
    /// <param name="json">给定的 JSON 字符串。</param>
    /// <param name="returnType">给定的返回类型。</param>
    /// <param name="options">给定的 <see cref="JsonSerializerOptions"/>（可选；默认使用 <see cref="JsonDependency"/> 选项）。</param>
    /// <returns>返回对象。</returns>
    public static object? FromJson(this string json, Type returnType, JsonSerializerOptions? options = null)
        => JsonSerializer.Deserialize(json, returnType, options ?? JsonDependency.Value.Options.Value);

    /// <summary>
    /// 从 JSON 反序列化为指定类型的实例。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    /// <param name="json">给定的 JSON 字符串。</param>
    /// <param name="options">给定的 <see cref="JsonSerializerOptions"/>（可选；默认使用 <see cref="JsonDependency"/> 选项）。</param>
    /// <returns>返回 <typeparamref name="T"/>。</returns>
    public static T? FromJson<T>(this string json, JsonSerializerOptions? options = null)
        => JsonSerializer.Deserialize<T>(json, options ?? JsonDependency.Value.Options.Value);

    #endregion


    #region Serialize JsonFile

    /// <summary>
    /// 将指定对象序列化为 JSON 文件。
    /// </summary>
    /// <param name="obj">给定的对象。</param>
    /// <param name="filePath">给定的 JSON 文件路径。</param>
    /// <param name="encoding">给定的字符编码（可选）。</param>
    /// <param name="options">给定的 <see cref="JsonSerializerOptions"/>（可选；默认使用 <see cref="JsonDependency"/> 选项）。</param>
    /// <param name="jsonFormatter">给定的 JSON 格式化器（可选）。</param>
    /// <returns>返回 JSON 字符串。</returns>
    public static string AsJsonFile(this object obj, string filePath, Encoding? encoding = null,
        JsonSerializerOptions? options = null, Func<string, string>? jsonFormatter = null)
    {
        var json = obj.AsJson(options);

        if (jsonFormatter is not null)
        {
            json = jsonFormatter(json);
        }

        if (encoding is null)
        {
            File.WriteAllText(filePath, json);
        }
        else
        {
            File.WriteAllText(filePath, json, encoding);
        }

        return json;
    }

    /// <summary>
    /// 从 JSON 文件反序列化为指定类型的对象。
    /// </summary>
    /// <param name="filePath">给定的 JSON 文件路径。</param>
    /// <param name="returnType">给定的返回类型。</param>
    /// <param name="encoding">给定的字符编码（可选）。</param>
    /// <param name="options">给定的 <see cref="JsonSerializerOptions"/>（可选；默认使用 <see cref="JsonDependency"/> 选项）。</param>
    /// <param name="jsonFormatter">给定的 JSON 格式化器（可选）。</param>
    /// <returns>返回对象。</returns>
    public static object? FromJsonFile(this string filePath, Type returnType, Encoding? encoding = null,
        JsonSerializerOptions? options = null, Func<string, string>? jsonFormatter = null)
    {
        var json = string.Empty;

        if (encoding is null)
        {
            json = File.ReadAllText(filePath);
        }
        else
        {
            json = File.ReadAllText(filePath, encoding);
        }

        if (jsonFormatter is not null)
        {
            json = jsonFormatter(json);
        }

        return json.FromJson(returnType, options);
    }

    /// <summary>
    /// 从 JSON 文件反序列化为指定类型的对象。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    /// <param name="filePath">给定的 JSON 文件路径。</param>
    /// <param name="encoding">给定的字符编码（可选）。</param>
    /// <param name="options">给定的 <see cref="JsonSerializerOptions"/>（可选；默认使用 <see cref="JsonDependency"/> 选项）。</param>
    /// <param name="jsonFormatter">给定的 JSON 格式化器（可选）。</param>
    /// <returns>返回 <typeparamref name="T"/>。</returns>
    public static T? FromJsonFile<T>(this string filePath, Encoding? encoding = null,
        JsonSerializerOptions? options = null, Func<string, string>? jsonFormatter = null)
    {
        var json = string.Empty;

        if (encoding is null)
        {
            json = File.ReadAllText(filePath);
        }
        else
        {
            json = File.ReadAllText(filePath, encoding);
        }

        if (jsonFormatter is not null)
        {
            json = jsonFormatter(json);
        }

        return json.FromJson<T>(options);
    }

    #endregion

}
