#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Data.Accessing;

namespace Librame.Extensions.Dispatchers;

/// <summary>
/// 定义一个表示数据访问的默认调度器存取器集合。
/// </summary>
public class DefaultDispatcherAccessors : BaseDispatcherAccessors
{
    /// <summary>
    /// 构造一个 <see cref="DefaultDispatcherAccessors"/>。
    /// </summary>
    /// <param name="accessors">给定的 <see cref="IEnumerable{IAccessor}"/>。</param>
    /// <param name="factory">给定的 <see cref="IDispatcherFactory"/>。</param>
    public DefaultDispatcherAccessors(IEnumerable<IAccessor> accessors,
        IDispatcherFactory factory)
        : this(accessors, factory, DispatchingMode.Default)
    {
    }

    /// <summary>
    /// 构造一个 <see cref="DefaultDispatcherAccessors"/>。
    /// </summary>
    /// <param name="accessors">给定的 <see cref="IEnumerable{IAccessor}"/>。</param>
    /// <param name="factory">给定的 <see cref="IDispatcherFactory"/>。</param>
    /// <param name="mode">给定的 <see cref="DispatchingMode"/>。</param>
    protected DefaultDispatcherAccessors(IEnumerable<IAccessor> accessors,
        IDispatcherFactory factory, DispatchingMode mode)
        : base(factory.CreateBase(accessors, mode), mode)
    {
    }

}
