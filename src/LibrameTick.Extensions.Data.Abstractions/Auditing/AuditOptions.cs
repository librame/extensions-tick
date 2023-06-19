﻿#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Core;
using Librame.Extensions.Data.Storing;

namespace Librame.Extensions.Data.Auditing;

/// <summary>
/// 定义实现 <see cref="IOptions"/> 的审计选项。
/// </summary>
public class AuditOptions : IOptions
{
    /// <summary>
    /// 启用审计（默认启用）。
    /// </summary>
    public bool Enabling { get; set; } = true;

    /// <summary>
    /// 保存审计集合（默认启用）。
    /// </summary>
    public bool SaveAudits { get; set; } = true;


    /// <summary>
    /// 通知动作。
    /// </summary>
    [JsonIgnore]
    public Action<IEnumerable<Audit>>? NotificationAction { get; set; }
}
