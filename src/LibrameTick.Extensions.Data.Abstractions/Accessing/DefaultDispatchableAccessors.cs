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
/// 定义一个表示数据访问的默认可调度存取器集合。
/// </summary>
public class DefaultDispatchableAccessors : BaseDispatchableAccessors
{
    /// <summary>
    /// 构造一个 <see cref="DefaultDispatchableAccessors"/>。
    /// </summary>
    /// <param name="accessors">给定的 <see cref="IEnumerable{IAccessor}"/>。</param>
    /// <param name="factory">给定的 <see cref="IDispatcherFactory"/>。</param>
    public DefaultDispatchableAccessors(IEnumerable<IAccessor> accessors,
        IDispatcherFactory factory)
        : this(accessors, factory, DispatchingMode.Default)
    {
    }

    /// <summary>
    /// 构造一个 <see cref="DefaultDispatchableAccessors"/>。
    /// </summary>
    /// <param name="accessors">给定的 <see cref="IEnumerable{IAccessor}"/>。</param>
    /// <param name="factory">给定的 <see cref="IDispatcherFactory"/>。</param>
    /// <param name="mode">给定的 <see cref="DispatchingMode"/>。</param>
    protected DefaultDispatchableAccessors(IEnumerable<IAccessor> accessors,
        IDispatcherFactory factory, DispatchingMode mode)
        : base(factory.CreateBase(accessors, mode), mode)
    {
    }

}
