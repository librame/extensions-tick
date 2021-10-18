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

namespace Librame.Extensions.Data;

/// <summary>
/// <see cref="IIdentificationGeneratorFactory"/> 静态扩展。
/// </summary>
public static class IdentificationGeneratorFactoryExtensions
{

    /// <summary>
    /// 获取支持 MySQL 排序类型的 COMB 标识生成器（char(36)）。
    /// </summary>
    /// <param name="factory">给定的 <see cref="IIdentificationGeneratorFactory"/>。</param>
    /// <returns>返回 <see cref="IIdentificationGenerator{Guid}"/>。</returns>
    public static IIdentificationGenerator<Guid> GetCombIdGeneratorForMySql(this IIdentificationGeneratorFactory factory)
        => factory.GetIdGenerator<Guid>(typeof(CombIdentificationGenerator), nameof(CombIdentificationGenerators.ForMySql));

    /// <summary>
    /// 获取支持 Oracle 排序类型的 COMB 标识生成器（raw(16)）。
    /// </summary>
    /// <param name="factory">给定的 <see cref="IIdentificationGeneratorFactory"/>。</param>
    /// <returns>返回 <see cref="IIdentificationGenerator{Guid}"/>。</returns>
    public static IIdentificationGenerator<Guid> GetCombIdGeneratorForOracle(this IIdentificationGeneratorFactory factory)
        => factory.GetIdGenerator<Guid>(typeof(CombIdentificationGenerator), nameof(CombIdentificationGenerators.ForOracle));

    /// <summary>
    /// 获取支持 SQLite 排序类型的 COMB 标识生成器（text）。
    /// </summary>
    /// <param name="factory">给定的 <see cref="IIdentificationGeneratorFactory"/>。</param>
    /// <returns>返回 <see cref="IIdentificationGenerator{Guid}"/>。</returns>
    public static IIdentificationGenerator<Guid> GetCombIdGeneratorForSqlite(this IIdentificationGeneratorFactory factory)
        => factory.GetCombIdGeneratorForMySql(); // 使用与 MySQL 数据库相同的排序方式

    /// <summary>
    /// 获取支持 SQL Server 排序类型的 COMB 标识生成器（uniqueidentifier）。
    /// </summary>
    /// <param name="factory">给定的 <see cref="IIdentificationGeneratorFactory"/>。</param>
    /// <returns>返回 <see cref="IIdentificationGenerator{Guid}"/>。</returns>
    public static IIdentificationGenerator<Guid> GetCombIdGeneratorForSqlServer(this IIdentificationGeneratorFactory factory)
        => factory.GetIdGenerator<Guid>(typeof(CombIdentificationGenerator), nameof(CombIdentificationGenerators.ForSqlServer));

    /// <summary>
    /// 获取 COMB <see cref="Guid"/> 格式的雪花标识生成器。
    /// </summary>
    /// <param name="factory">给定的 <see cref="IIdentificationGeneratorFactory"/>。</param>
    /// <returns>返回 <see cref="IIdentificationGenerator{Guid}"/>。</returns>
    public static IIdentificationGenerator<Guid> GetCombSnowflakeIdGenerator(this IIdentificationGeneratorFactory factory)
        => factory.GetIdGenerator<Guid>(typeof(CombSnowflakeIdentificationGenerator));

    /// <summary>
    /// 获取 MongoDB 字符串型标识生成器（可生成长度 24 位且包含数字、字母的字符串标识）。
    /// </summary>
    /// <param name="factory">给定的 <see cref="IIdentificationGeneratorFactory"/>。</param>
    /// <returns>返回 <see cref="IIdentificationGenerator{String}"/>。</returns>
    public static IIdentificationGenerator<string> GetMongoIdGenerator(this IIdentificationGeneratorFactory factory)
        => factory.GetIdGenerator<string>(typeof(MongoIdentificationGenerator));

    /// <summary>
    /// 获取雪花 64 位整型标识生成器（可生成长度 18 位的长整数标识）。
    /// </summary>
    /// <param name="factory">给定的 <see cref="IIdentificationGeneratorFactory"/>。</param>
    /// <returns>返回 <see cref="IIdentificationGenerator{Int64}"/>。</returns>
    public static IIdentificationGenerator<long> GetSnowflakeIdGenerator(this IIdentificationGeneratorFactory factory)
        => factory.GetIdGenerator<long>(typeof(SnowflakeIdentificationGenerator));

    /// <summary>
    /// 获取标识生成器。
    /// </summary>
    /// <typeparam name="TId">指定的标识类型。</typeparam>
    /// <param name="factory">给定的 <see cref="IIdentificationGeneratorFactory"/>。</param>
    /// <param name="generatorType">给定的标识生成器类型。</param>
    /// <param name="aliase">给定的别名（可选）。</param>
    /// <returns>返回 <see cref="IIdentificationGenerator{TId}"/>。</returns>
    public static IIdentificationGenerator<TId> GetIdGenerator<TId>(this IIdentificationGeneratorFactory factory,
        Type generatorType, string? aliase = null)
        where TId : IEquatable<TId>
        => factory.GetIdGenerator<TId>(new TypeNamedKey(generatorType, aliase));

}
