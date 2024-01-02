#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core;

/// <summary>
/// 定义抽象继承 <see cref="AbstractExtensionBuilder"/> 并自动注册当前扩展构建器实例。
/// </summary>
/// <typeparam name="TBuilder">指定的扩展构建器类型。</typeparam>
public abstract class AbstractExtensionBuilder<TBuilder> : AbstractExtensionBuilder
    where TBuilder : IExtensionBuilder
{
    /// <summary>
    /// 构造一个父级 <see cref="AbstractExtensionBuilder{TBuilder}"/>。
    /// </summary>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="services"/> 为空。
    /// </exception>
    /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
    protected AbstractExtensionBuilder(IServiceCollection services)
        : base(services)
    {
        Services.AddSingleton(typeof(TBuilder), this);
    }

    /// <summary>
    /// 构造一个子级 <see cref="AbstractExtensionBuilder{TBuilder}"/>。
    /// </summary>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="parentBuilder"/> 为空。
    /// </exception>
    /// <param name="parentBuilder">给定的父级 <see cref="IExtensionBuilder"/>。</param>
    protected AbstractExtensionBuilder(IExtensionBuilder parentBuilder)
        : base(parentBuilder)
    {
        Services.AddSingleton(typeof(TBuilder), this);
    }

}


/// <summary>
/// 定义抽象实现 <see cref="IExtensionBuilder"/>。
/// </summary>
public abstract class AbstractExtensionBuilder : AbstractExtensionInfo, IExtensionBuilder
{
    /// <summary>
    /// 构造一个父级 <see cref="AbstractExtensionBuilder"/>。
    /// </summary>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="services"/> 为空。
    /// </exception>
    /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
    protected AbstractExtensionBuilder(IServiceCollection services)
    {
        Services = services;
        ServiceCharacteristics = [];
        ReplacedServices = new Dictionary<Type, Type>();
    }

    /// <summary>
    /// 构造一个子级 <see cref="AbstractExtensionBuilder"/>。
    /// </summary>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="parentBuilder"/> 为空。
    /// </exception>
    /// <param name="parentBuilder">给定的父级 <see cref="IExtensionBuilder"/>。</param>
    protected AbstractExtensionBuilder(IExtensionBuilder parentBuilder)
        : this(parentBuilder.Services)
    {
        ParentBuilder = parentBuilder;
    }


    /// <summary>
    /// 父级构建器。
    /// </summary>
    public IExtensionBuilder? ParentBuilder { get; init; }

    /// <summary>
    /// 替换服务字典集合。
    /// </summary>
    public IDictionary<Type, Type> ReplacedServices { get; init; }

    /// <summary>
    /// 服务集合。
    /// </summary>
    public IServiceCollection Services { get; init; }

    /// <summary>
    /// 服务特征集合。
    /// </summary>
    public ServiceCharacteristicCollection ServiceCharacteristics { get; init; }
}
