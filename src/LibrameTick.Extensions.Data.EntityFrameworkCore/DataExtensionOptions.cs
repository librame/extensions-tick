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
using Librame.Extensions.Data.Auditing;
using Librame.Extensions.Data.Sharding;
using Librame.Extensions.Data.Storing;
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
    /// 雪花标识参数。
    /// </summary>
    public SnowflakeIdOptions SnowflakeParameters { get; set; } = new();


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
