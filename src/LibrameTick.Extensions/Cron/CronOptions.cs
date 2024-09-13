#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Infrastructure;

namespace Librame.Extensions.Cron;

/// <summary>
/// 定义一个 CRON 选项。
/// </summary>
public sealed class CronOptions : IOptions
{
    /// <summary>
    /// 所有特殊整数标记（默认为 99）。
    /// </summary>
    public int AllSpec { get; set; } = 99;

    /// <summary>
    /// 无特殊整数标记（默认为 98）。
    /// </summary>
    public int NoSpec { get; set; } = 98;

    /// <summary>
    /// 表达式域分隔符（默认为英文空格）。
    /// </summary>
    public char DomainSeparator { get; set; } = ' ';

    /// <summary>
    /// 并集分隔符（默认为英文逗号）。
    /// </summary>
    public char UnionSeparator { get; set; } = ',';
}
