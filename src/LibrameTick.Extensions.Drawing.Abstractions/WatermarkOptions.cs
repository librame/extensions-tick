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

namespace Librame.Extensions.Drawing;

/// <summary>
/// 定义实现 <see cref="IOptions"/> 的水印选项。
/// </summary>
public class WatermarkOptions : AbstractOptions
{
    /// <summary>
    /// 构造一个独立属性通知器的 <see cref="WatermarkOptions"/>（此构造函数适用于独立使用 <see cref="WatermarkOptions"/> 的情况）。
    /// </summary>
    /// <param name="sourceAliase">给定的源别名（独立属性通知器必须命名实例）。</param>
    public WatermarkOptions(string sourceAliase)
        : base(sourceAliase)
    {
        Font = new FontOptions(Notifier, nameof(Font)) { Size = 32 };
    }

    /// <summary>
    /// 构造一个 <see cref="WatermarkOptions"/>。
    /// </summary>
    /// <param name="parentNotifier">给定的父级 <see cref="IPropertyNotifier"/>。</param>
    /// <param name="sourceAliase">给定的源别名（可选）。</param>
    public WatermarkOptions(IPropertyNotifier parentNotifier, string? sourceAliase = null)
        : base(parentNotifier, sourceAliase)
    {
        Font = new FontOptions(Notifier, nameof(Font)) { Size = 32 };
    }


    /// <summary>
    /// 字体选项。
    /// </summary>
    public FontOptions Font { get; init; }


    /// <summary>
    /// 文件名称后缀。
    /// </summary>
    public string FileNameSuffix
    {
        get => Notifier.GetOrAdd(nameof(FileNameSuffix), "-wm");
        set => Notifier.AddOrUpdate(nameof(FileNameSuffix), value);
    }

    /// <summary>
    /// 水印模式。
    /// </summary>
    public WatermarkMode Mode
    {
        get => Notifier.GetOrAdd(nameof(Mode), WatermarkMode.Image);
        set => Notifier.AddOrUpdate(nameof(Mode), value);
    }

    /// <summary>
    /// 初始坐标（负值表示反向）。
    /// </summary>
    public Point InitialCoord
    {
        get => Notifier.GetOrAdd(nameof(InitialCoord), new Point(30, 50));
        set => Notifier.AddOrUpdate(nameof(InitialCoord), value);
    }

    /// <summary>
    /// 是否使用随机坐标（默认不随机）。
    /// </summary>
    public bool IsRandomCoord
    {
        get => Notifier.GetOrAdd(nameof(IsRandomCoord), false);
        set => Notifier.AddOrUpdate(nameof(IsRandomCoord), value);
    }

    /// <summary>
    /// 文本内容。
    /// </summary>
    public string Text
    {
        get => Notifier.GetOrAdd(nameof(Text), nameof(Librame));
        set => Notifier.AddOrUpdate(nameof(Text), value);
    }

    /// <summary>
    /// 图片文件。
    /// </summary>
    public string ImageFile
    {
        get => Notifier.GetOrAdd(nameof(ImageFile), "watermark.png");
        set => Notifier.AddOrUpdate(nameof(ImageFile), value);
    }

}
