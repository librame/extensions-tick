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
/// 定义抽象实现泛型 <see cref="AbstractExtensionOptions"/>。
/// </summary>
/// <typeparam name="TOptions">指定的扩展信息类型。</typeparam>
public abstract class AbstractExtensionOptions<TOptions> : AbstractExtensionOptions
    where TOptions : IExtensionOptions
{
    /// <summary>
    /// 构造一个 <see cref="AbstractExtensionOptions{TOptions}"/>。
    /// </summary>
    /// <param name="parentOptions">给定的父级 <see cref="IExtensionOptions"/>（可空；为空则表示当前为父级扩展）。</param>
    /// <param name="directories">给定的 <see cref="DirectoryOptions"/>（可选；默认尝试从父级扩展选项中获取，如果从父级获取到的实例为空，则新建此实例）。</param>
    protected AbstractExtensionOptions(IExtensionOptions? parentOptions, DirectoryOptions? directories = null)
        : base(parentOptions, directories)
    {
        Notifier.PropertyChanging += AbstractExtensionOptions_PropertyChanging;
        Notifier.PropertyChanged += AbstractExtensionOptions_PropertyChanged;
    }


    private void AbstractExtensionOptions_PropertyChanging(object? sender, PropertyChangingEventArgs e)
    {
        PropertyChangingAction?.Invoke((TOptions)sender!, (NotifyPropertyChangingEventArgs)e);
    }

    private void AbstractExtensionOptions_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        PropertyChangedAction?.Invoke((TOptions)sender!, (NotifyPropertyChangedEventArgs)e);
    }


    /// <summary>
    /// 属性改变时动作。
    /// </summary>
    [JsonIgnore]
    public Action<TOptions, NotifyPropertyChangingEventArgs>? PropertyChangingAction { get; set; }

    /// <summary>
    /// 属性改变后动作。
    /// </summary>
    [JsonIgnore]
    public Action<TOptions, NotifyPropertyChangedEventArgs>? PropertyChangedAction { get; set; }
}
