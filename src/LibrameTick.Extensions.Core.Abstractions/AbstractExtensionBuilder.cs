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
/// 定义抽象实现 <see cref="IExtensionBuilder"/> 并自动注册当前扩展构建器实例。
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
        ServiceCharacteristics = new ServiceCharacteristicCollection();
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
    {
        ParentBuilder = parentBuilder;
        Services = parentBuilder.Services;
        ServiceCharacteristics = new ServiceCharacteristicCollection();
        ReplacedServices = new Dictionary<Type, Type>();
    }


    /// <summary>
    /// 父级构建器。
    /// </summary>
    public IExtensionBuilder? ParentBuilder { get; init; }

    /// <summary>
    /// 扩展选项类型。
    /// </summary>
    public abstract Type ExtensionOptionsType { get; }

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


    /// <summary>
    /// 将扩展选项保存为 JSON 文件。
    /// </summary>
    /// <param name="services">给定的 <see cref="IServiceProvider"/>。</param>
    /// <returns>返回保存路径。</returns>
    public abstract string SaveOptionsAsJson(IServiceProvider services);

    /// <summary>
    /// 将扩展选项保存为 JSON 文件。
    /// </summary>
    /// <param name="services">给定的 <see cref="IServiceProvider"/>。</param>
    /// <param name="options">输出 <see cref="IExtensionOptions"/>。</param>
    /// <returns>返回保存路径。</returns>
    public abstract string SaveOptionsAsJson(IServiceProvider services, out IExtensionOptions options);
}
