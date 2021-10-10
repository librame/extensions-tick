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
/// 定义一个实现 <see cref="IResource"/> 的资源字典接口。
/// </summary>
public interface IResourceDictionary : IResource, IEnumerable<KeyValuePair<string, object>>
{
    /// <summary>
    /// 所有键集合。
    /// </summary>
    IReadOnlyCollection<string> AllKeys { get; }

    /// <summary>
    /// 所有值集合。
    /// </summary>
    IReadOnlyCollection<object> AllValues { get; }


    /// <summary>
    /// 添加资源。
    /// </summary>
    /// <param name="key">给定的键。</param>
    /// <param name="value">给定的资源。</param>
    /// <returns>返回资源。</returns>
    object Add(string key, object value);


    /// <summary>
    /// 获取资源。
    /// </summary>
    /// <param name="key">给定的键。</param>
    /// <returns>返回资源。</returns>
    object Get(string key);


    /// <summary>
    /// 尝试获取资源。
    /// </summary>
    /// <param name="key">给定的键。</param>
    /// <param name="value">输出资源。</param>
    /// <returns>返回是否获取的布尔值。</returns>
    bool TryGet(string key, [MaybeNullWhen(false)] out object value);


    /// <summary>
    /// 获取资源字符串。
    /// </summary>
    /// <param name="key">给定的键。</param>
    /// <returns>返回字符串。</returns>
    string GetString(string key);

    /// <summary>
    /// 获取资源字符串。
    /// </summary>
    /// <param name="key">指定的键。</param>
    /// <param name="args">给定的格式化参数列表。</param>
    /// <returns>返回字符串。</returns>
    string GetString(string key, params object?[] args);
}
