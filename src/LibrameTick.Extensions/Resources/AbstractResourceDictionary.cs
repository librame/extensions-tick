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
/// 定义抽象实现 <see cref="IResourceDictionary"/> 的资源字典。
/// </summary>
/// <typeparam name="TResource">指定的资源类型。</typeparam>
public abstract class AbstractResourceDictionary<TResource> : AbstractResourceDictionary
{
    /// <summary>
    /// 使用资源名称构造一个 <see cref="AbstractResourceDictionary{TResource}"/>。
    /// </summary>
    /// <param name="resourceName">给定的资源名称。</param>
    protected AbstractResourceDictionary(string resourceName)
        : base(resourceName)
    {
    }


    /// <summary>
    /// 资源类型。
    /// </summary>
    public override Type ResourceType
        => typeof(TResource);

}


/// <summary>
/// 定义抽象实现 <see cref="IResourceDictionary"/> 的资源字典。
/// </summary>
public abstract class AbstractResourceDictionary : AbstractResource, IResourceDictionary
{
    /// <summary>
    /// 使用资源名称构造一个 <see cref="AbstractResourceDictionary"/>。
    /// </summary>
    /// <param name="resourceName">给定的资源名称。</param>
    protected AbstractResourceDictionary(string resourceName)
        : base(resourceName)
    {
        Pairs = new();
    }


    /// <summary>
    /// 键值对集合。
    /// </summary>
    protected Dictionary<string, object> Pairs { get; init; }

    /// <summary>
    /// 所有键集合。
    /// </summary>
    public IReadOnlyCollection<string> AllKeys
        => Pairs.Keys;

    /// <summary>
    /// 所有值集合。
    /// </summary>
    public IReadOnlyCollection<object> AllValues
        => Pairs.Values;


    /// <summary>
    /// 获取枚举器。
    /// </summary>
    /// <returns>返回 <see cref="IEnumerator{KeyValuePair}"/>。</returns>
    public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        => Pairs.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();


    /// <summary>
    /// 添加资源。
    /// </summary>
    /// <param name="key">给定的键。</param>
    /// <param name="value">给定的资源。</param>
    /// <returns>返回资源。</returns>
    public virtual object Add(string key, object value)
    {
        Pairs.Add(key, value);
        return value;
    }


    /// <summary>
    /// 获取资源。
    /// </summary>
    /// <param name="key">给定的键。</param>
    /// <returns>返回资源。</returns>
    public virtual object Get(string key)
        => Pairs[key];


    /// <summary>
    /// 尝试获取资源。
    /// </summary>
    /// <param name="key">给定的键。</param>
    /// <param name="value">输出资源。</param>
    /// <returns>返回是否获取的布尔值。</returns>
    public virtual bool TryGet(string key, [MaybeNullWhen(false)] out object value)
        => Pairs.TryGetValue(key, out value);


    /// <summary>
    /// 获取资源字符串。
    /// </summary>
    /// <param name="key">给定的键。</param>
    /// <returns>返回字符串。</returns>
    public virtual string GetString(string key)
        => (string)Get(key);

    /// <summary>
    /// 获取资源字符串。
    /// </summary>
    /// <param name="key">指定的键。</param>
    /// <param name="args">给定的格式化参数列表。</param>
    /// <returns>返回字符串。</returns>
    public virtual string GetString(string key, params object?[] args)
        => string.Format(GetString(key), args);

}
