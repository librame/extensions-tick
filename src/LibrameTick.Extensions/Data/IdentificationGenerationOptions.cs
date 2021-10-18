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
/// 定义实现 <see cref="IOptionsNotifier"/> 的标识生成选项。
/// </summary>
public class IdentificationGenerationOptions : AbstractOptionsNotifier
{
    /// <summary>
    /// 构造一个默认 <see cref="IdentificationGenerationOptions"/>。
    /// </summary>
    /// <param name="sourceAliase">给定的源别名。</param>
    public IdentificationGenerationOptions(string sourceAliase)
        : base(sourceAliase)
    {
    }

    /// <summary>
    /// 构造一个 <see cref="IdentificationGenerationOptions"/>。
    /// </summary>
    /// <param name="parentNotifier">给定的父级 <see cref="IPropertyNotifier"/>。</param>
    /// <param name="sourceAliase">给定的源别名（可选）。</param>
    public IdentificationGenerationOptions(IPropertyNotifier parentNotifier, string? sourceAliase = null)
        : base(parentNotifier, sourceAliase)
    {
    }


    /// <summary>
    /// 机器中心标识（默认 1）。
    /// </summary>
    public long DataCenterId
    {
        get => Notifier.GetOrAdd(nameof(DataCenterId), 1L);
        set => Notifier.AddOrUpdate(nameof(DataCenterId), value);
    }

    /// <summary>
    /// 机器标识（默认 1）。
    /// </summary>
    public long MachineId
    {
        get => Notifier.GetOrAdd(nameof(MachineId), 1L);
        set => Notifier.AddOrUpdate(nameof(MachineId), value);
    }

    /// <summary>
    /// 工作标识（默认 1）。
    /// </summary>
    public uint WorkId
    {
        get => Notifier.GetOrAdd(nameof(WorkId), 1u);
        set => Notifier.AddOrUpdate(nameof(WorkId), value);
    }

}
