#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Text.Json.Serialization;

namespace Librame.Extensions.Core;

/// <summary>
/// 定义抽象实现 <see cref="IOptions"/>。
/// </summary>
public abstract class AbstractOptions : IOptions
{
    /// <summary>
    /// 构造一个独立属性通知器的 <see cref="AbstractOptions"/>。
    /// </summary>
    /// <param name="sourceAliase">给定的源别名（独立属性通知器必须命名实例）。</param>
    protected AbstractOptions(string sourceAliase)
    {
        Notifier = Instantiator.GetPropertyNotifier(this, sourceAliase);
    }

    /// <summary>
    /// 构造一个 <see cref="AbstractOptions"/>。
    /// </summary>
    /// <param name="parentNotifier">给定的父级 <see cref="IPropertyNotifier"/>。</param>
    /// <param name="sourceAliase">给定的源别名（如果此选项会在同一源中重复使用，则必须设定不同的源别名，否则会造成属性通知器的实例冲突）。</param>
    protected AbstractOptions(IPropertyNotifier parentNotifier, string? sourceAliase)
    {
        Notifier = parentNotifier.WithSource(this, sourceAliase);
    }


    /// <summary>
    /// 属性通知器。
    /// </summary>
    [JsonIgnore]
    public IPropertyNotifier Notifier { get; protected set; }
}
