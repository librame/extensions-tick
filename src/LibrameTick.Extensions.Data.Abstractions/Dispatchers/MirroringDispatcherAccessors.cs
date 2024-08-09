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

namespace Librame.Extensions.Dispatching;

/// <summary>
/// 定义一个表示数据访问的镜像调度器存取器集合，默认通过 <see cref="BaseDispatcher{IAccessor}"/> 与 <see cref="TransactionDispatcher{IAccessor}"/> 实现（支持针对读取异常切换与分布式写入事务遍历等功能）。
/// </summary>
public class MirroringDispatcherAccessors : BaseDispatcherAccessors
{
    /// <summary>
    /// 构造一个 <see cref="MirroringDispatcherAccessors"/>。
    /// </summary>
    /// <param name="accessors">给定的 <see cref="IEnumerable{IAccessor}"/>。</param>
    /// <param name="factory">给定的 <see cref="IDispatcherFactory"/>。</param>
    public MirroringDispatcherAccessors(IEnumerable<IAccessor> accessors, IDispatcherFactory factory)
        : this(accessors, factory, DispatchingMode.Mirroring)
    {
    }

    /// <summary>
    /// 构造一个 <see cref="MirroringDispatcherAccessors"/>。
    /// </summary>
    /// <param name="accessors">给定的 <see cref="IEnumerable{IAccessor}"/>。</param>
    /// <param name="factory">给定的 <see cref="IDispatcherFactory"/>。</param>
    /// <param name="mode">给定的 <see cref="DispatchingMode"/>。</param>
    protected MirroringDispatcherAccessors(IEnumerable<IAccessor> accessors,
        IDispatcherFactory factory, DispatchingMode mode)
        : base(factory.CreateBase(accessors, mode), factory.CreateTransaction(accessors, mode), mode)
    {
        // 镜像模式表示仅从单库读取、需从多库写入冗余数据
    }

}
