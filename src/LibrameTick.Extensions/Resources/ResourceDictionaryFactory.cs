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
/// 定义实现 <see cref="IResourceDictionaryFactory"/> 的泛型资源字典工厂。
/// </summary>
/// <typeparam name="TResourceBase">指定的资源基础类型。</typeparam>
public class ResourceDictionaryFactory<TResourceBase> : ResourceDictionaryFactory
{
    /// <summary>
    /// 构造一个 <see cref="ResourceDictionaryFactory{TResourceBase}"/>。
    /// </summary>
    /// <param name="resourceAssembly">给定的资源程序集（可选；默认使用与资源基础类型相同的程序集）。</param>
    public ResourceDictionaryFactory(Assembly? resourceAssembly = null)
        : base(typeof(TResourceBase), resourceAssembly)
    {
    }

}


/// <summary>
/// 定义实现 <see cref="IResourceDictionaryFactory"/> 的资源字典工厂。
/// </summary>
public class ResourceDictionaryFactory : IResourceDictionaryFactory
{
    private readonly ConcurrentDictionary<string, IResourceDictionary> _resources = new();

    private readonly Type _resourceBaseType;
    private readonly Assembly _resourceAssembly;


    /// <summary>
    /// 构造一个 <see cref="ResourceDictionaryFactory"/>。
    /// </summary>
    /// <param name="resourceBaseType">给定的资源基础类型。</param>
    /// <param name="resourceAssembly">给定的资源程序集（可选；默认使用与资源基础类型相同的程序集）。</param>
    public ResourceDictionaryFactory(Type resourceBaseType, Assembly? resourceAssembly = null)
    {
        _resourceBaseType = resourceBaseType;
        _resourceAssembly = resourceAssembly ?? resourceBaseType.Assembly;
    }


    /// <summary>
    /// 当前文化信息。
    /// </summary>
    public CultureInfo? CurrentCulture { get; private set; }


    /// <summary>
    /// 创建指定文化信息的资源字典。
    /// </summary>
    /// <param name="culture">给定的 <see cref="CultureInfo"/>（可选；默认使用 <see cref="CultureInfo.CurrentUICulture"/>）。</param>
    /// <returns>返回 <see cref="IResourceDictionary"/>。</returns>
    public IResourceDictionary Create(CultureInfo? culture = null)
    {
        if (culture is null)
            culture = CultureInfo.CurrentUICulture;

        CurrentCulture = culture;

        if (!_resources.TryGetValue(culture.Name, out var resource))
        {
            var resourceType = BuildResourceType(culture);
            resource = (IResourceDictionary)resourceType.NewByExpression();

            _resources[culture.Name] = resource;
        }

        return resource;
    }

    private Type BuildResourceType(CultureInfo culture)
    {
        // 类名不支持短横线
        var typeName = $"{_resourceBaseType.FullName}_{culture.Name.Replace('-', '_')}";

        var type = Type.GetType($"{typeName}, {_resourceAssembly}");
        if (type is null)
            throw new ArgumentException($"The specified resource type '{typeName}' could not be found in the assembly '{_resourceAssembly.GetName().Name}'.");

        return type;
    }

}
