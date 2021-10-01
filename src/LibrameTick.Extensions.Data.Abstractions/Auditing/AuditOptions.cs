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
using Librame.Extensions.Data.Storing;

namespace Librame.Extensions.Data.Auditing;

/// <summary>
/// 定义实现 <see cref="IOptions"/> 的审计选项。
/// </summary>
public class AuditOptions : AbstractOptions
{
    /// <summary>
    /// 构造一个独立属性通知器的 <see cref="AuditOptions"/>（此构造函数适用于独立使用 <see cref="AuditOptions"/> 的情况）。
    /// </summary>
    /// <param name="sourceAliase">给定的源别名（独立属性通知器必须命名实例）。</param>
    public AuditOptions(string sourceAliase)
        : base(sourceAliase)
    {
    }

    /// <summary>
    /// 构造一个 <see cref="AuditOptions"/>。
    /// </summary>
    /// <param name="parentNotifier">给定的父级 <see cref="IPropertyNotifier"/>。</param>
    public AuditOptions(IPropertyNotifier parentNotifier)
        : base(parentNotifier, sourceAliase: null)
    {
    }


    /// <summary>
    /// 启用审计（默认启用）。
    /// </summary>
    public bool Enabling
    {
        get => Notifier.GetOrAdd(nameof(Enabling), true);
        set => Notifier.AddOrUpdate(nameof(Enabling), value);
    }

    /// <summary>
    /// 启用对实体添加状态的审计（默认启用）。
    /// </summary>
    public bool AddedState
    {
        get => Notifier.GetOrAdd(nameof(AddedState), true);
        set => Notifier.AddOrUpdate(nameof(AddedState), value);
    }

    /// <summary>
    /// 启用对实体修改状态的审计（默认启用）。
    /// </summary>
    public bool ModifiedState
    {
        get => Notifier.GetOrAdd(nameof(ModifiedState), true);
        set => Notifier.AddOrUpdate(nameof(ModifiedState), value);
    }

    /// <summary>
    /// 启用对实体删除状态的审计（默认启用）。
    /// </summary>
    public bool DeletedState
    {
        get => Notifier.GetOrAdd(nameof(DeletedState), true);
        set => Notifier.AddOrUpdate(nameof(DeletedState), value);
    }

    /// <summary>
    /// 启用对实体无变化状态的审计（默认不启用）。
    /// </summary>
    public bool UnchangedState
    {
        get => Notifier.GetOrAdd(nameof(UnchangedState), false);
        set => Notifier.AddOrUpdate(nameof(UnchangedState), value);
    }

    /// <summary>
    /// 保存审计集合（默认启用）。
    /// </summary>
    public bool SaveAudits
    {
        get => Notifier.GetOrAdd(nameof(SaveAudits), true);
        set => Notifier.AddOrUpdate(nameof(SaveAudits), value);
    }


    /// <summary>
    /// 通知动作。
    /// </summary>
    [JsonIgnore]
    public Action<IReadOnlyList<Audit>>? NotificationAction { get; set; }

}
