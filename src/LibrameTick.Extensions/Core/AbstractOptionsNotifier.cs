#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core;

/// <summary>
/// 定义抽象实现 <see cref="IOptionsNotifier"/> 的选项通知器。
/// </summary>
public abstract class AbstractOptionsNotifier : IOptionsNotifier
{
    /// <summary>
    /// 构造一个独立属性通知器的 <see cref="AbstractOptionsNotifier"/>。
    /// </summary>
    /// <param name="sourceAliase">给定的源别名（独立属性通知器必须命名实例）。</param>
    protected AbstractOptionsNotifier(string sourceAliase)
    {
        Notifier = new PropertyNotifier(this, sourceAliase);
    }

    /// <summary>
    /// 构造一个 <see cref="AbstractOptionsNotifier"/>。
    /// </summary>
    /// <param name="parentNotifier">给定的父级 <see cref="IPropertyNotifier"/>。</param>
    /// <param name="sourceAliase">给定的源别名（如果此选项会在同一源中重复使用，则必须设定不同的源别名，否则会造成属性通知器的实例冲突）。</param>
    protected AbstractOptionsNotifier(IPropertyNotifier parentNotifier, string? sourceAliase)
    {
        Notifier = parentNotifier.WithSource(this, sourceAliase);
    }


    /// <summary>
    /// 属性通知器。
    /// </summary>
    [JsonIgnore]
    public IPropertyNotifier Notifier { get; protected set; }
}
