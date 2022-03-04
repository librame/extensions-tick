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
/// 定义实现 <see cref="IOptions"/> 的标识生成选项。
/// </summary>
public class IdGenerationOptions : IOptions
{
    /// <summary>
    /// 机器中心标识（默认 1）。
    /// </summary>
    public long DataCenterId { get; set; } = 1L;

    /// <summary>
    /// 机器标识（默认 1）。
    /// </summary>
    public long MachineId { get; set; } = 1L;

    /// <summary>
    /// 工作标识（默认 1）。
    /// </summary>
    public uint WorkId { get; set; } = 1;

}
