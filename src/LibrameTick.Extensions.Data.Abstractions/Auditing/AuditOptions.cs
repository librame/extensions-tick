#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Data.Storing;
using Librame.Extensions.Infrastructure;

namespace Librame.Extensions.Data.Auditing;

/// <summary>
/// 定义实现 <see cref="IOptions"/> 的审计选项。
/// </summary>
public class AuditOptions : IOptions
{
    /// <summary>
    /// 启用审计（可自动生成每次变化实体的审计数据；默认启用）。
    /// </summary>
    public bool Enabling { get; set; } = true;

    /// <summary>
    /// 保存审计集合（可将自动生成的变化实体数据保存到选项设定的保存审计集合动作中；默认未启用）。
    /// </summary>
    public bool SaveAudits { get; set; } = false;


    /// <summary>
    /// 通知动作。
    /// </summary>
    [JsonIgnore]
    public Action<IEnumerable<Audit>>? NotificationAction { get; set; }
}
