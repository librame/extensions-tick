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

namespace Librame.Extensions.Data.Storing;

/// <summary>
/// 定义实现 <see cref="IOptionsNotifier"/> 的存储选项。
/// </summary>
public class StoreOptions : AbstractOptionsNotifier
{
    /// <summary>
    /// 构造一个独立属性通知器的 <see cref="StoreOptions"/>（此构造函数适用于独立使用 <see cref="StoreOptions"/> 的情况）。
    /// </summary>
    /// <param name="sourceAliase">给定的源别名（独立属性通知器必须命名实例）。</param>
    public StoreOptions(string sourceAliase)
        : base(sourceAliase)
    {
    }

    /// <summary>
    /// 构造一个 <see cref="StoreOptions"/>。
    /// </summary>
    /// <param name="parentNotifier">给定的父级 <see cref="IPropertyNotifier"/>。</param>
    public StoreOptions(IPropertyNotifier parentNotifier)
        : base(parentNotifier, sourceAliase: null)
    {
    }


    /// <summary>
    /// 映射关系（默认启用）。
    /// </summary>
    public bool MapRelationship
    {
        get => Notifier.GetOrAdd(nameof(MapRelationship), true);
        set => Notifier.AddOrUpdate(nameof(MapRelationship), value);
    }

    /// <summary>
    /// 可限制的最大属性长度（默认为 250）。
    /// </summary>
    /// <remarks>
    /// 在 MySQL 中如果长度超出 255 会被转换为不能作为主键或唯一性约束 的 BLOB/TEXT 类型。
    /// </remarks>
    public int LimitableMaxLengthOfProperty
    {
        get => Notifier.GetOrAdd(nameof(LimitableMaxLengthOfProperty), 250);
        set => Notifier.AddOrUpdate(nameof(LimitableMaxLengthOfProperty), value);
    }

}
