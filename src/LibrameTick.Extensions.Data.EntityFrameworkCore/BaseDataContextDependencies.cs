#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Core;
using Librame.Extensions.Data.Sharding;
using Librame.Extensions.Data.ValueConversion;

namespace Librame.Extensions.Data;

/// <summary>
/// 定义一个基于 <see cref="BaseDataContext"/> 的 <see cref="IDataContextDependencies"/> 数据库上下文依赖集合实现。
/// </summary>
public sealed class BaseDataContextDependencies : IDataContextDependencies
{
    private readonly IInfrastructure<IServiceProvider> _scopeServices;
    private readonly IOptionsMonitor<CoreExtensionOptions> _coreExtOptions;
    private readonly IOptionsMonitor<DataExtensionOptions> _dataExtOptions;


    /// <summary>
    /// 构造一个 <see cref="BaseDataContextDependencies"/>。
    /// </summary>
    /// <param name="scopeServices">给定的 <see cref="IInfrastructure{IServiceProvider}"/>。</param>
    public BaseDataContextDependencies(IInfrastructure<IServiceProvider> scopeServices)
    {
        _scopeServices = scopeServices;
        _coreExtOptions = scopeServices.GetService<IOptionsMonitor<CoreExtensionOptions>>();
        _dataExtOptions = scopeServices.GetService<IOptionsMonitor<DataExtensionOptions>>();

        ShardingContext = scopeServices.GetService<IShardingContext>();
        EncryptionConverterFactory = scopeServices.GetService<IEncryptionConverterFactory>();
    }


    /// <summary>
    /// 分片上下文。
    /// </summary>
    public IShardingContext ShardingContext { get; init; }

    /// <summary>
    /// 加密转换器工厂。
    /// </summary>
    public IEncryptionConverterFactory EncryptionConverterFactory { get; init; }

    /// <summary>
    /// 数据扩展选项。
    /// </summary>
    public DataExtensionOptions DataExtOptions
        => _dataExtOptions.CurrentValue;

    /// <summary>
    /// 核心扩展选项。
    /// </summary>
    public CoreExtensionOptions CoreExtOptions
        => _coreExtOptions.CurrentValue;


    /// <summary>
    /// 获取指定服务实例。
    /// </summary>
    /// <typeparam name="TService">指定的服务类型。</typeparam>
    /// <returns>返回 <typeparamref name="TService"/>。</returns>
    public TService GetScopeService<TService>()
        where TService : class
        => _scopeServices.GetService<TService>();

}
