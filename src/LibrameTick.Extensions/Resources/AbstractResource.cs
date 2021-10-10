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
/// 定义抽象实现 <see cref="IResource"/> 的泛型资源。
/// </summary>
/// <typeparam name="TResource">指定的资源类型。</typeparam>
public abstract class AbstractResource<TResource> : AbstractResource
    where TResource : IResource
{
    /// <summary>
    /// 构造一个 <see cref="AbstractResource{TResource}"/>。
    /// </summary>
    protected AbstractResource()
        : base()
    {
    }

    /// <summary>
    /// 使用资源名称构造一个 <see cref="AbstractResource{TResource}"/>。
    /// </summary>
    /// <param name="resourceName">给定的资源名称。</param>
    protected AbstractResource(string resourceName)
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
/// 定义抽象实现 <see cref="IResource"/> 的资源。
/// </summary>
public abstract class AbstractResource : IResource
{
    private string? _resourceName;


    /// <summary>
    /// 构造一个 <see cref="AbstractResource"/>。
    /// </summary>
    protected AbstractResource()
    {
    }

    /// <summary>
    /// 使用资源名称构造一个 <see cref="AbstractResource"/>。
    /// </summary>
    /// <param name="resourceName">给定的资源名称。</param>
    protected AbstractResource(string resourceName)
    {
        _resourceName = resourceName;
    }


    /// <summary>
    /// 资源名称。
    /// </summary>
    public string ResourceName
    {
        get
        {
            if (string.IsNullOrEmpty(_resourceName))
                _resourceName = ResourceType.Name;

            return _resourceName;
        }
    }

    /// <summary>
    /// 资源类型。
    /// </summary>
    public virtual Type ResourceType
        => GetType();

}
