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
using Librame.Extensions.IdGenerators;

namespace Librame.Extensions.Data;

/// <summary>
/// <see cref="IIdGeneratorFactory"/> 静态扩展。
/// </summary>
public static class IdGeneratorFactoryExtensions
{

    /// <summary>
    /// 获取支持 MySQL 排序类型的 COMB 标识生成器（char(36)）。
    /// </summary>
    /// <param name="factory">给定的 <see cref="IIdGeneratorFactory"/>。</param>
    /// <returns>返回 <see cref="IIdGenerator{Guid}"/>。</returns>
    public static IIdGenerator<Guid> GetCombIdGeneratorForMySql(this IIdGeneratorFactory factory)
        => factory.GetIdGenerator<Guid>(typeof(CombIdGenerator), nameof(CombIdGenerators.ForMySql));

    /// <summary>
    /// 获取支持 Oracle 排序类型的 COMB 标识生成器（raw(16)）。
    /// </summary>
    /// <param name="factory">给定的 <see cref="IIdGeneratorFactory"/>。</param>
    /// <returns>返回 <see cref="IIdGenerator{Guid}"/>。</returns>
    public static IIdGenerator<Guid> GetCombIdGeneratorForOracle(this IIdGeneratorFactory factory)
        => factory.GetIdGenerator<Guid>(typeof(CombIdGenerator), nameof(CombIdGenerators.ForOracle));

    /// <summary>
    /// 获取支持 SQLite 排序类型的 COMB 标识生成器（text）。
    /// </summary>
    /// <param name="factory">给定的 <see cref="IIdGeneratorFactory"/>。</param>
    /// <returns>返回 <see cref="IIdGenerator{Guid}"/>。</returns>
    public static IIdGenerator<Guid> GetCombIdGeneratorForSqlite(this IIdGeneratorFactory factory)
        => factory.GetCombIdGeneratorForMySql(); // 使用与 MySQL 数据库相同的排序方式

    /// <summary>
    /// 获取支持 SQL Server 排序类型的 COMB 标识生成器（uniqueidentifier）。
    /// </summary>
    /// <param name="factory">给定的 <see cref="IIdGeneratorFactory"/>。</param>
    /// <returns>返回 <see cref="IIdGenerator{Guid}"/>。</returns>
    public static IIdGenerator<Guid> GetCombIdGeneratorForSqlServer(this IIdGeneratorFactory factory)
        => factory.GetIdGenerator<Guid>(typeof(CombIdGenerator), nameof(CombIdGenerators.ForSqlServer));

    /// <summary>
    /// 获取 COMB <see cref="Guid"/> 格式的雪花标识生成器。
    /// </summary>
    /// <param name="factory">给定的 <see cref="IIdGeneratorFactory"/>。</param>
    /// <returns>返回 <see cref="IIdGenerator{Guid}"/>。</returns>
    public static IIdGenerator<Guid> GetCombSnowflakeIdGenerator(this IIdGeneratorFactory factory)
        => factory.GetIdGenerator<Guid>(typeof(CombSnowflakeIdGenerator));

    /// <summary>
    /// 获取 MongoDB 字符串型标识生成器（可生成长度 24 位且包含数字、字母的字符串标识）。
    /// </summary>
    /// <param name="factory">给定的 <see cref="IIdGeneratorFactory"/>。</param>
    /// <returns>返回 <see cref="IIdGenerator{String}"/>。</returns>
    public static IIdGenerator<string> GetMongoIdGenerator(this IIdGeneratorFactory factory)
        => factory.GetIdGenerator<string>(typeof(MongoIdGenerator));

    /// <summary>
    /// 获取雪花 64 位整型标识生成器（可生成长度 18 位的长整数标识）。
    /// </summary>
    /// <param name="factory">给定的 <see cref="IIdGeneratorFactory"/>。</param>
    /// <returns>返回 <see cref="IIdGenerator{Int64}"/>。</returns>
    public static IIdGenerator<long> GetSnowflakeIdGenerator(this IIdGeneratorFactory factory)
        => factory.GetIdGenerator<long>(typeof(SnowflakeIdGenerator));

    /// <summary>
    /// 获取标识生成器。
    /// </summary>
    /// <typeparam name="TId">指定的标识类型。</typeparam>
    /// <param name="factory">给定的 <see cref="IIdGeneratorFactory"/>。</param>
    /// <param name="generatorType">给定的标识生成器类型。</param>
    /// <param name="named">给定的命名（可选）。</param>
    /// <returns>返回 <see cref="IIdGenerator{TId}"/>。</returns>
    public static IIdGenerator<TId> GetIdGenerator<TId>(this IIdGeneratorFactory factory,
        Type generatorType, string? named = null)
        where TId : IEquatable<TId>
        => factory.GetIdGenerator<TId>(new TypeNamedKey(generatorType, named));

}
