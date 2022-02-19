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

namespace Librame.Extensions.Data;

/// <summary>
/// 定义 <see cref="CombIdentificationGenerator"/> 实例集合。
/// </summary>
public static class CombIdentificationGenerators
{

    /// <summary>
    /// 支持 MySQL 排序类型的 COMB 标识生成器（char(36)）。
    /// </summary>
    /// <param name="clock">给定的 <see cref="IClockBootstrap"/>（如使用本地时钟可参考 <see cref="Bootstrapper.GetClock()"/>）。</param>
    /// <param name="locker">给定的 <see cref="ILockerBootstrap"/>（如使用本地锁定器可参考 <see cref="Bootstrapper.GetLocker()"/>）。</param>
    /// <returns>返回 <see cref="CombIdentificationGenerator"/>。</returns>
    public static CombIdentificationGenerator ForMySql(IClockBootstrap clock, ILockerBootstrap locker)
        => new CombIdentificationGenerator(clock, locker, CombIdentificationGeneration.AsString);

    /// <summary>
    /// 支持 Oracle 排序类型的 COMB 标识生成器（raw(16)）。
    /// </summary>
    /// <param name="clock">给定的 <see cref="IClockBootstrap"/>（如使用本地时钟可参考 <see cref="Bootstrapper.GetClock()"/>）。</param>
    /// <param name="locker">给定的 <see cref="ILockerBootstrap"/>（如使用本地锁定器可参考 <see cref="Bootstrapper.GetLocker()"/>）。</param>
    /// <returns>返回 <see cref="CombIdentificationGenerator"/>。</returns>
    public static CombIdentificationGenerator ForOracle(IClockBootstrap clock, ILockerBootstrap locker)
        => new CombIdentificationGenerator(clock, locker, CombIdentificationGeneration.AsBinary);

    /// <summary>
    /// 支持 SQLite 排序类型的 COMB 标识生成器（text）。
    /// </summary>
    /// <param name="clock">给定的 <see cref="IClockBootstrap"/>（如使用本地时钟可参考 <see cref="Bootstrapper.GetClock()"/>）。</param>
    /// <param name="locker">给定的 <see cref="ILockerBootstrap"/>（如使用本地锁定器可参考 <see cref="Bootstrapper.GetLocker()"/>）。</param>
    /// <returns>返回 <see cref="CombIdentificationGenerator"/>。</returns>
    public static CombIdentificationGenerator ForSqlite(IClockBootstrap clock, ILockerBootstrap locker)
        => ForMySql(clock, locker); // 使用与 MySQL 数据库相同的排序方式

    /// <summary>
    /// 支持 SQL Server 排序类型的 COMB 标识生成器（uniqueidentifier）。
    /// </summary>
    /// <param name="clock">给定的 <see cref="IClockBootstrap"/>（如使用本地时钟可参考 <see cref="Bootstrapper.GetClock()"/>）。</param>
    /// <param name="locker">给定的 <see cref="ILockerBootstrap"/>（如使用本地锁定器可参考 <see cref="Bootstrapper.GetLocker()"/>）。</param>
    /// <returns>返回 <see cref="CombIdentificationGenerator"/>。</returns>
    public static CombIdentificationGenerator ForSqlServer(IClockBootstrap clock, ILockerBootstrap locker)
        => new CombIdentificationGenerator(clock, locker, CombIdentificationGeneration.AtEnd);

}
