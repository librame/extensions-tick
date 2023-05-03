﻿#region License

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
using Librame.Extensions.Data.Auditing;
using Librame.Extensions.Data.Sharding;
using Librame.Extensions.Data.Storing;
using Librame.Extensions.Device;
using Librame.Extensions.IdGenerators;

namespace Librame.Extensions.Data;

/// <summary>
/// 定义实现 <see cref="IExtensionOptions"/> 的数据扩展选项。
/// </summary>
public class DataExtensionOptions : AbstractExtensionOptions<DataExtensionOptions>
{
    private readonly Dictionary<TypeNamedKey, IObjectIdGenerator> _idGenerators = new();


    /// <summary>
    /// 访问选项。
    /// </summary>
    public AccessOptions Access { get; set; } = new();

    /// <summary>
    /// 审计选项。
    /// </summary>
    public AuditOptions Audit { get; set; } = new();

    /// <summary>
    /// 存储选项。
    /// </summary>
    public StoreOptions Store { get; set; } = new();

    /// <summary>
    /// 标识生成选项。
    /// </summary>
    public IdGenerationOptions IdGeneration { get; set; } = new();

    /// <summary>
    /// Mongo 标识选项。
    /// </summary>
    public MongoIdOptions Mongo { get; set; } = new();

    /// <summary>
    /// 雪花标识选项。
    /// </summary>
    public SnowflakeIdOptions Snowflake { get; set; } = new();

    /// <summary>
    /// 设备负载选项。
    /// </summary>
    public DeviceLoadOptions DeviceLoad { get; set; } = new();

    /// <summary>
    /// 设备负载每次实时计算（如果不启用，当首次等待计算后，下次先返回上次计算值，利用率将在后台计算后更新，以提升响应速度；默认不启用）。
    /// </summary>
    public bool DeviceLoadRealtimeForEverytime { get; set; }

    /// <summary>
    /// 创建设备负载主机集合键方法，用于缓存单次 <see cref="IDeviceLoader"/> 实例。
    /// </summary>
    [JsonIgnore]
    public Func<IEnumerable<string>, string> CreateDeviceLoadHostsKeyFunc { get; set; }
        = hosts => string.Join(',', hosts.Order());

    /// <summary>
    /// 创建设备负载器方法（默认使用 <see cref="AccessorDeviceLoader"/>）。
    /// </summary>
    [JsonIgnore]
    public Func<DataExtensionOptions, IEnumerable<string>, IDeviceLoader> CreateDeviceLoaderFunc { get; set; }
        = (options, hosts) => new AccessorDeviceLoader(options, hosts);


    /// <summary>
    /// 标识生成器字典集合（默认已集成 <see cref="string"/> “MongoDB”、<see cref="long"/> “雪花”、<see cref="Guid"/> “COMB for SQLServer/MySQL/Oracle” 等标识类型的生成器）。
    /// </summary>
    [JsonIgnore]
    public IReadOnlyDictionary<TypeNamedKey, IObjectIdGenerator> IdGenerators
        => _idGenerators;

    /// <summary>
    /// 分片策略列表集合（默认已集成 <see cref="CultureInfoShardingStrategy"/>、<see cref="DateTimeShardingStrategy"/>、<see cref="DateTimeOffsetShardingStrategy"/> 等分片策略）。
    /// </summary>
    [JsonIgnore]
    public List<IShardingStrategy> ShardingStrategies { get; init; } = new();

    /// <summary>
    /// 查询过滤器列表集合。
    /// </summary>
    [JsonIgnore]
    public List<IQueryFilter> QueryFilters { get; init; } = new();


    /// <summary>
    /// 添加实现 <see cref="IIdGenerator{TId}"/> 的标识生成器（推荐从 <see cref="AbstractIdGenerator{TId}"/> 派生）。
    /// </summary>
    /// <typeparam name="TId">指定的标识类型。</typeparam>
    /// <param name="idGenerator">给定的 <see cref="IIdGenerator{TId}"/>。</param>
    /// <param name="aliase">给定的别名（可选）。</param>
    public void AddIdGenerator<TId>(IIdGenerator<TId> idGenerator, string? aliase = null)
        where TId : IEquatable<TId>
        => _idGenerators.Add(new TypeNamedKey(idGenerator.GetType(), aliase), idGenerator);

    /// <summary>
    /// 添加实现 <see cref="IIdGenerator{TId}"/> 的标识生成器（推荐从 <see cref="AbstractIdGenerator{TId}"/> 派生）。
    /// </summary>
    /// <typeparam name="TId">指定的标识类型。</typeparam>
    /// <param name="idGenerator">给定的 <see cref="IIdGenerator{TId}"/>。</param>
    /// <param name="key">给定的 <see cref="TypeNamedKey"/>。</param>
    public void AddIdGenerator<TId>(IIdGenerator<TId> idGenerator, TypeNamedKey key)
        where TId : IEquatable<TId>
        => _idGenerators.Add(key, idGenerator);

}
