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

namespace Librame.Extensions.Drawing.Processing;

/// <summary>
/// 定义实现 <see cref="IOptions"/> 的缩放选项。
/// </summary>
public class ScaleOptions : AbstractOptions
{
    /// <summary>
    /// 构造一个独立属性通知器的 <see cref="ScaleOptions"/>（此构造函数适用于独立使用 <see cref="ScaleOptions"/> 的情况）。
    /// </summary>
    /// <param name="sourceAliase">给定的源别名（独立属性通知器必须命名实例）。</param>
    public ScaleOptions(string sourceAliase)
        : base(sourceAliase)
    {
    }

    /// <summary>
    /// 构造一个 <see cref="ScaleOptions"/>。
    /// </summary>
    /// <param name="parentNotifier">给定的父级 <see cref="IPropertyNotifier"/>。</param>
    /// <param name="sourceAliase">给定的源别名（可选）。</param>
    public ScaleOptions(IPropertyNotifier parentNotifier, string? sourceAliase = null)
        : base(parentNotifier, sourceAliase)
    {
    }


    /// <summary>
    /// 添加缩放尺寸后缀。
    /// </summary>
    public bool AddScaleSizeSuffix
    {
        get => Notifier.GetOrAdd(nameof(AddScaleSizeSuffix), true);
        set => Notifier.AddOrUpdate(nameof(AddScaleSizeSuffix), value);
    }

    /// <summary>
    /// 缩放描述符列表。
    /// </summary>
    public List<ScaleDescriptor> Descriptors { get; init; }
        = new List<ScaleDescriptor>
        {
            new("-small", new Size(100, 60), AddWatermark: false),
            new("-medium", new Size(1000, 600), AddWatermark: true),
            new("-large", new Size(2000, 1200), AddWatermark: false)
        };

}
