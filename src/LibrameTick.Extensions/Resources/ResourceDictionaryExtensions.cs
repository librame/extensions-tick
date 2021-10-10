#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Resources;

/// <summary>
/// <see cref="IResourceDictionary"/> 静态扩展。
/// </summary>
public static class ResourceDictionaryExtensions
{

    /// <summary>
    /// 获取资源字符串。
    /// </summary>
    /// <param name="resource">给定的 <see cref="IResourceDictionary"/>。</param>
    /// <param name="key">指定的键。</param>
    /// <param name="containsKey">输出包含此键的布尔值。</param>
    /// <returns>返回字符串值。</returns>
    public static string GetString(this IResourceDictionary resource, string key, out bool containsKey)
    {
        if (resource.TryGet(key, out var result))
        {
            containsKey = true;
            return result.ToString() ?? string.Empty;
        }
        else
        {
            containsKey = false;
            return string.Empty;
        }
    }

    /// <summary>
    /// 获取资源字符串。
    /// </summary>
    /// <param name="resource">给定的 <see cref="IResourceDictionary"/>。</param>
    /// <param name="key">指定的键。</param>
    /// <param name="containsKey">输出包含此键的布尔值。</param>
    /// <param name="args">给定的格式化参数列表。</param>
    /// <returns>返回字符串值。</returns>
    public static string GetString(this IResourceDictionary resource, string key, out bool containsKey, params object?[] args)
    {
        if (resource.TryGet(key, out var result))
        {
            containsKey = true;

            var format = result.ToString();
            if (string.IsNullOrEmpty(format))
                return string.Empty;

            return string.Format(format, args);
        }
        else
        {
            containsKey = false;
            return string.Empty;
        }
    }


    /// <summary>
    /// 尝试获取资源字符串。
    /// </summary>
    /// <param name="resource">给定的 <see cref="IResourceDictionary"/>。</param>
    /// <param name="key">指定的键。</param>
    /// <param name="value">输出字符串值。</param>
    /// <returns>返回布尔值。</returns>
    public static bool TryGetString(this IResourceDictionary resource, string key, out string value)
    {
        if (resource.TryGet(key, out var result))
        {
            value = result.ToString() ?? string.Empty;
            return true;
        }
        else
        {
            value = string.Empty;
            return false;
        }
    }

    /// <summary>
    /// 尝试获取资源字符串。
    /// </summary>
    /// <param name="resource">给定的 <see cref="IResourceDictionary"/>。</param>
    /// <param name="key">指定的键。</param>
    /// <param name="value">输出字符串值。</param>
    /// <param name="args">给定的格式化参数列表。</param>
    /// <returns>返回布尔值。</returns>
    public static bool TryGetString(this IResourceDictionary resource, string key, out string value, params object?[] args)
    {
        if (resource.TryGet(key, out var result))
        {
            var format = result.ToString();

            if (string.IsNullOrEmpty(format))
                value = string.Empty;
            else
                value = string.Format(format, args);

            return true;
        }
        else
        {
            value = string.Empty;
            return false;
        }
    }

}
