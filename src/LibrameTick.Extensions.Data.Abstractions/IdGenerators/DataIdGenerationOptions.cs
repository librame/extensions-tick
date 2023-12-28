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

namespace Librame.Extensions.IdGenerators;

/// <summary>
/// 定义实现 <see cref="IOptions"/> 的数据标识生成选项。
/// </summary>
public class DataIdGenerationOptions : IOptions
{
    private readonly Dictionary<TypeNamedKey, IObjectIdGenerator> _customIdGenerators = [];


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
    /// 自定义标识生成器集合（默认已集成 <see cref="string"/> “MongoDB”、<see cref="long"/> “雪花”、<see cref="Guid"/> “COMB for SQLServer/MySQL/Oracle” 等标识类型的生成器）。
    /// </summary>
    [JsonIgnore]
    public IReadOnlyDictionary<TypeNamedKey, IObjectIdGenerator> CustomIdGenerators
        => _customIdGenerators;


    /// <summary>
    /// 添加实现 <see cref="IIdGenerator{TId}"/> 的自定义标识生成器（推荐从 <see cref="AbstractIdGenerator{TId}"/> 派生）。
    /// </summary>
    /// <typeparam name="TId">指定的标识类型。</typeparam>
    /// <param name="idGenerator">给定的 <see cref="IIdGenerator{TId}"/>。</param>
    /// <param name="named">给定的命名（可选）。</param>
    public void AddCustomIdGenerator<TId>(IIdGenerator<TId> idGenerator, string? named = null)
        where TId : IEquatable<TId>
        => _customIdGenerators.Add(new TypeNamedKey(idGenerator.GetType(), named), idGenerator);

    /// <summary>
    /// 添加实现 <see cref="IIdGenerator{TId}"/> 的自定义标识生成器（推荐从 <see cref="AbstractIdGenerator{TId}"/> 派生）。
    /// </summary>
    /// <typeparam name="TId">指定的标识类型。</typeparam>
    /// <param name="idGenerator">给定的 <see cref="IIdGenerator{TId}"/>。</param>
    /// <param name="key">给定的 <see cref="TypeNamedKey"/>。</param>
    public void AddCustomIdGenerator<TId>(IIdGenerator<TId> idGenerator, TypeNamedKey key)
        where TId : IEquatable<TId>
        => _customIdGenerators.Add(key, idGenerator);

}
