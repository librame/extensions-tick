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
using Librame.Extensions.Data.Accessing;
using Librame.Extensions.Data.Sharding;
using Librame.Extensions.Data.ValueConversion;

namespace Librame.Extensions.Data;

/// <summary>
/// 定义一个实现 <see cref="IDataContextServices"/> 的数据上下文服务集合。
/// </summary>
/// <remarks>
/// 构造一个 <see cref="DataContextServices"/>。
/// </remarks>
/// <param name="context">给定的 <see cref="DataContext"/>。</param>
/// <param name="options">给定的 <see cref="DbContextOptions"/>。</param>
public sealed class DataContextServices(DataContext context, DbContextOptions options) : IDataContextServices
{
    private readonly IShardingContext _shardingContext = context.GetService<IShardingContext>();
    private readonly IEncryptionConverterFactory _encryptionConverterFactory = context.GetService<IEncryptionConverterFactory>();
    private readonly IRelationalConnection _relationalConnection = context.GetService<IRelationalConnection>();
    private readonly IOptionsMonitor<CoreExtensionOptions> _coreOptions = context.GetService<IOptionsMonitor<CoreExtensionOptions>>();
    private readonly IOptionsMonitor<DataExtensionOptions> _dataOptions = context.GetService<IOptionsMonitor<DataExtensionOptions>>();
    private readonly DataContext _context = context;
    private readonly DbContextOptions _options = options;

    private ShardingDescriptor? _initialShardingDescriptor;


    /// <summary>
    /// 分片上下文。
    /// </summary>
    public IShardingContext ShardingContext
        => _shardingContext;

    /// <summary>
    /// 加密转换器工厂。
    /// </summary>
    public IEncryptionConverterFactory EncryptionConverterFactory
        => _encryptionConverterFactory;

    /// <summary>
    /// 关系型数据库连接。
    /// </summary>
    public IRelationalConnection RelationalConnection
        => _relationalConnection;

    /// <summary>
    /// 数据扩展选项。
    /// </summary>
    public DataExtensionOptions DataOptions
        => _dataOptions.CurrentValue;

    /// <summary>
    /// 核心扩展选项。
    /// </summary>
    public CoreExtensionOptions CoreOptions
        => _coreOptions.CurrentValue;


    /// <summary>
    /// 核心选项扩展。
    /// </summary>
    public CoreOptionsExtension? ContextCoreOptions
        => _options.FindExtension<CoreOptionsExtension>();

    /// <summary>
    /// 存取器选项扩展。
    /// </summary>
    public AccessorDbContextOptionsExtension? ContextAccessorOptions
        => _options.FindExtension<AccessorDbContextOptionsExtension>();

    /// <summary>
    /// 关系型选项扩展。
    /// </summary>
    public RelationalOptionsExtension? ContextRelationalOptions
        => RelationalOptionsExtension.Extract(_options);


    /// <summary>
    /// 初始连接字符串。
    /// </summary>
    public string? InitialConnectionString
        => _relationalConnection.ConnectionString;

    /// <summary>
    /// 初始数据库名称。
    /// </summary>
    public string? InitialDatabaseName
        => _dataOptions.CurrentValue.Access.ParseDatabaseName(InitialConnectionString);

    /// <summary>
    /// 根据初始连接字符串创建的分片描述符。
    /// </summary>
    public ShardingDescriptor? InitialShardingDescriptor
    {
        get
        {
            if (_initialShardingDescriptor is null)
            {
                // 默认优先使用配置分片规则，其次使用分片标注特性
                var attribute = ContextAccessorOptions?.Sharding ??
                    ShardingDatabaseAttribute.GetDatabase(_context.ContextType, InitialDatabaseName);

                if (attribute is null) return null;

                var shardingValues = ContextAccessorOptions?.ShardingValues
                    ?? _context.ContextType.GetImplementedShardingValues().ToList();

                _initialShardingDescriptor = new ShardingDescriptor(ShardingContext.StrategyProvider, attribute, shardingValues);
            }
            
            return _initialShardingDescriptor;
        }
    }


    /// <summary>
    /// 获取指定数据上下文服务实例。
    /// </summary>
    /// <typeparam name="TService">指定的服务类型。</typeparam>
    /// <returns>返回 <typeparamref name="TService"/>。</returns>
    public TService GetContextService<TService>()
        where TService : class
        => _context.GetService<TService>();

}
