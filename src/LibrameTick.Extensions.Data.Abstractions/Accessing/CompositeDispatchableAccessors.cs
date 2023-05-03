#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Dispatchers;

namespace Librame.Extensions.Data.Accessing;

/// <summary>
/// 定义一个表示数据访问的复合可调度存取器集合。
/// </summary>
public class CompositeDispatchableAccessors : BaseDispatchableAccessors
{
    /// <summary>
    /// 构造一个 <see cref="CompositeDispatchableAccessors"/>。
    /// </summary>
    /// <param name="accessors">给定的 <see cref="IEnumerable{IAccessor}"/>。</param>
    /// <param name="factory">给定的 <see cref="IDispatcherFactory"/>。</param>
    public CompositeDispatchableAccessors(IEnumerable<IDispatchableAccessors> accessors, IDispatcherFactory factory)
        : this(accessors, factory, DispatchingMode.Compositing)
    {
    }

    /// <summary>
    /// 构造一个 <see cref="CompositeDispatchableAccessors"/>。
    /// </summary>
    /// <param name="dispatchables">给定的 <see cref="IEnumerable{IAccessor}"/>。</param>
    /// <param name="factory">给定的 <see cref="IDispatcherFactory"/>。</param>
    /// <param name="mode">给定的 <see cref="DispatchingMode"/>。</param>
    protected CompositeDispatchableAccessors(IEnumerable<IDispatchableAccessors> dispatchables,
        IDispatcherFactory factory, DispatchingMode mode)
        : base(factory.CreateCompositing(dispatchables.SelectMany(s => s)), mode)
    {
        // 复合模式使用分割模式，表示需从多调度器读/写数据
    }

}
