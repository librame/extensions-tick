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
/// 定义实现 <see cref="IStringLocalizer"/> 的泛型资源字典字符串定位器。
/// </summary>
/// <typeparam name="TResource">指定的资源类型。</typeparam>
public class ResourceDictionaryStringLocalizer<TResource> : ResourceDictionaryStringLocalizer, IStringLocalizer<TResource>
    where TResource : IResource
{
    /// <summary>
    /// 构造一个 <see cref="ResourceDictionaryStringLocalizer{TResource}"/>。
    /// </summary>
    /// <param name="factory">给定的 <see cref="IResourceDictionaryFactory"/>。</param>
    public ResourceDictionaryStringLocalizer(IResourceDictionaryFactory factory)
        : base(factory)
    {
    }

}


/// <summary>
/// 定义实现 <see cref="IStringLocalizer"/> 的资源字典字符串定位器。
/// </summary>
public class ResourceDictionaryStringLocalizer : IStringLocalizer
{
    private readonly IResourceDictionary _factory;


    /// <summary>
    /// 构造一个 <see cref="ResourceDictionaryStringLocalizer"/>。
    /// </summary>
    /// <param name="factory">给定的 <see cref="IResourceDictionaryFactory"/>。</param>
    public ResourceDictionaryStringLocalizer(IResourceDictionaryFactory factory)
    {
        // 创建基于 CultureInfo.CurrentUICulture 的模块资源。
        _factory = factory.Create();
    }


    /// <summary>
    /// 获取具有给定名称的字符串资源。
    /// </summary>
    /// <param name="name">给定的资源名称。</param>
    /// <returns>返回 <see cref="LocalizedString"/>。</returns>
    public LocalizedString this[string name]
        => new LocalizedString(name, _factory.GetString(name, out var containsKey), !containsKey);

    /// <summary>
    /// 获取具有给定名称的字符串资源。
    /// </summary>
    /// <param name="name">给定的资源名称。</param>
    /// <param name="arguments">给定的格式化参数列表。</param>
    /// <returns>返回 <see cref="LocalizedString"/>。</returns>
    public LocalizedString this[string name, params object[] arguments]
        => new LocalizedString(name, _factory.GetString(name, out var containsKey, arguments), !containsKey);


    /// <summary>
    /// 获取所有字符串资源。
    /// </summary>
    /// <param name="includeParentCultures">是否包含父级文化信息。</param>
    /// <returns>返回 <see cref="IEnumerable{LocalizedString}"/>。</returns>
    public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        => _factory.Select(s => new LocalizedString(s.Key, s.Value.ToString() ?? string.Empty));

}
