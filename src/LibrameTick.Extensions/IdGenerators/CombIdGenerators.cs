#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Bootstraps;

namespace Librame.Extensions.IdGenerators;

/// <summary>
/// 定义 <see cref="CombIdGenerator"/> 实例集合。
/// </summary>
public static class CombIdGenerators
{

    /// <summary>
    /// 支持 MySQL 排序类型的 COMB 标识生成器（char(36)）。当使用 Guid.ToString() 方法进行格式化时连续。
    /// </summary>
    /// <param name="options">给定的 <see cref="IdGenerationOptions"/>。</param>
    /// <param name="clock">给定的 <see cref="IClockBootstrap"/>（可选；默认使用 <see cref="Bootstrapper.GetClock()"/>）。</param>
    /// <returns>返回 <see cref="CombIdGenerator"/>。</returns>
    public static CombIdGenerator ForMySql(IdGenerationOptions options, IClockBootstrap? clock = null)
        => new CombIdGenerator(CombIdGeneration.AsString, options, clock ?? Bootstrapper.GetClock());

    /// <summary>
    /// 支持 Oracle 排序类型的 COMB 标识生成器（raw(16)）。当使用 Guid.ToByteArray() 方法进行格式化时连续。
    /// </summary>
    /// <param name="options">给定的 <see cref="IdGenerationOptions"/>。</param>
    /// <param name="clock">给定的 <see cref="IClockBootstrap"/>（可选；默认使用 <see cref="Bootstrapper.GetClock()"/>）。</param>
    /// <returns>返回 <see cref="CombIdGenerator"/>。</returns>
    public static CombIdGenerator ForOracle(IdGenerationOptions options, IClockBootstrap? clock = null)
        => new CombIdGenerator(CombIdGeneration.AsBinary, options, clock ?? Bootstrapper.GetClock());

    /// <summary>
    /// 支持 SQLite 排序类型的 COMB 标识生成器（text）。当使用 Guid.ToString() 方法进行格式化时连续。
    /// </summary>
    /// <param name="options">给定的 <see cref="IdGenerationOptions"/>。</param>
    /// <param name="clock">给定的 <see cref="IClockBootstrap"/>（可选；默认使用 <see cref="Bootstrapper.GetClock()"/>）。</param>
    /// <returns>返回 <see cref="CombIdGenerator"/>。</returns>
    public static CombIdGenerator ForSqlite(IdGenerationOptions options, IClockBootstrap? clock = null)
        => ForMySql(options, clock); // 使用与 MySQL 数据库相同的排序方式

    /// <summary>
    /// 支持 SQL Server 排序类型的 COMB 标识生成器（uniqueidentifier）。连续性体现于 GUID 的第4块（Data4）。
    /// </summary>
    /// <param name="options">给定的 <see cref="IdGenerationOptions"/>。</param>
    /// <param name="clock">给定的 <see cref="IClockBootstrap"/>（可选；默认使用 <see cref="Bootstrapper.GetClock()"/>）。</param>
    /// <returns>返回 <see cref="CombIdGenerator"/>。</returns>
    public static CombIdGenerator ForSqlServer(IdGenerationOptions options, IClockBootstrap? clock = null)
        => new CombIdGenerator(CombIdGeneration.AtEnd, options, clock ?? Bootstrapper.GetClock());

}
