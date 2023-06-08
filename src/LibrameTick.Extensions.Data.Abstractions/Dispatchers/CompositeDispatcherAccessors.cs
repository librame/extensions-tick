#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Dispatchers;

/// <summary>
/// 定义一个表示数据访问的复合调度器存取器集合。
/// </summary>
public class CompositeDispatcherAccessors : BaseDispatcherAccessors
{
    /// <summary>
    /// 构造一个 <see cref="CompositeDispatcherAccessors"/>。
    /// </summary>
    /// <param name="accessors">给定的 <see cref="IEnumerable{IAccessor}"/>。</param>
    /// <param name="factory">给定的 <see cref="IDispatcherFactory"/>。</param>
    public CompositeDispatcherAccessors(IEnumerable<IDispatcherAccessors> accessors, IDispatcherFactory factory)
        : this(accessors, factory, DispatchingMode.Compositing)
    {
    }

    /// <summary>
    /// 构造一个 <see cref="CompositeDispatcherAccessors"/>。
    /// </summary>
    /// <param name="dispatchables">给定的 <see cref="IEnumerable{IAccessor}"/>。</param>
    /// <param name="factory">给定的 <see cref="IDispatcherFactory"/>。</param>
    /// <param name="mode">给定的 <see cref="DispatchingMode"/>。</param>
    protected CompositeDispatcherAccessors(IEnumerable<IDispatcherAccessors> dispatchables,
        IDispatcherFactory factory, DispatchingMode mode)
        : base(factory.CreateCompositing(dispatchables.SelectMany(static s => s)), mode)
    {
        // 复合模式使用分割模式，表示需从多调度器读/写数据
    }

}
