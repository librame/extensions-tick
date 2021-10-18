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
/// 定义抽象实现 <see cref="IExtensionOptions"/> 的泛型扩展选项。
/// </summary>
/// <typeparam name="TOptions">指定的扩展选项类型。</typeparam>
public abstract class AbstractExtensionOptions<TOptions> : AbstractExtensionOptions
    where TOptions : IExtensionOptions
{
    /// <summary>
    /// 使用 <see cref="Registration.GetRegisterableDirectories()"/> 构造一个 <see cref="AbstractExtensionOptions{TOptions}"/>。
    /// </summary>
    protected AbstractExtensionOptions()
        : base()
    {
    }

    /// <summary>
    /// 构造一个 <see cref="AbstractExtensionOptions{TOptions}"/>。
    /// </summary>
    /// <param name="directories">给定的 <see cref="IRegisterableDirectories"/>。</param>
    protected AbstractExtensionOptions(IRegisterableDirectories directories)
        : base(directories)
    {
    }


    /// <summary>
    /// 扩展类型。
    /// </summary>
    [JsonIgnore]
    public override Type ExtensionType
        => typeof(TOptions);
}


/// <summary>
/// 定义抽象实现 <see cref="IExtensionOptions"/>、<see cref="IExtensionInfo"/> 的扩展选项。
/// </summary>
public abstract class AbstractExtensionOptions : AbstractExtensionInfo, IExtensionOptions
{
    /// <summary>
    /// 使用 <see cref="Registration.GetRegisterableDirectories()"/> 构造一个 <see cref="AbstractExtensionOptions"/>。
    /// </summary>
    protected AbstractExtensionOptions()
        : this(Registration.GetRegisterableDirectories())
    {
    }

    /// <summary>
    /// 构造一个 <see cref="AbstractExtensionOptions"/>。
    /// </summary>
    /// <param name="directories">给定的 <see cref="IRegisterableDirectories"/>。</param>
    protected AbstractExtensionOptions(IRegisterableDirectories directories)
    {
        Directories = directories;

        Notifier.PropertyChanged += Notifier_PropertyChanged;
        Notifier.PropertyChanging += Notifier_PropertyChanging;
    }


    /// <summary>
    /// 目录集合。
    /// </summary>
    public IRegisterableDirectories Directories { get; init; }

    /// <summary>
    /// 属性通知器。
    /// </summary>
    [JsonIgnore]
    public IPropertyNotifier Notifier
        => new PropertyNotifier(this, ExtensionName);


    /// <summary>
    /// 属性改变后动作。
    /// </summary>
    [JsonIgnore]
    public Action<IExtensionOptions?, NotifyPropertyChangedEventArgs>? PropertyChangedAction { get; set; }

    /// <summary>
    /// 属性改变时动作。
    /// </summary>
    [JsonIgnore]
    public Action<IExtensionOptions?, NotifyPropertyChangingEventArgs>? PropertyChangingAction { get; set; }


    /// <summary>
    /// 属性改变后的事件方法。
    /// </summary>
    /// <param name="sender">给定的事件发起者。</param>
    /// <param name="e">给定的 <see cref="PropertyChangedEventArgs"/>。</param>
    protected virtual void Notifier_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        PropertyChangedAction?.Invoke((IExtensionOptions?)sender, (NotifyPropertyChangedEventArgs)e);
    }

    /// <summary>
    /// 属性改变时的事件方法。
    /// </summary>
    /// <param name="sender">给定的事件发起者。</param>
    /// <param name="e">给定的 <see cref="PropertyChangingEventArgs"/>。</param>
    protected virtual void Notifier_PropertyChanging(object? sender, PropertyChangingEventArgs e)
    {
        PropertyChangingAction?.Invoke((IExtensionOptions?)sender, (NotifyPropertyChangingEventArgs)e);
    }

}
