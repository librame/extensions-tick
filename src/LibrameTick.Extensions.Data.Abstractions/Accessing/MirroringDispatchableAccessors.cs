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
/// 定义一个表示数据访问的镜像可调度存取器集合，默认通过 <see cref="BaseDispatcher{IAccessor}"/> 与 <see cref="TransactionDispatcher{IAccessor}"/> 实现（支持针对读取异常切换与分布式写入事务遍历等功能）。
/// </summary>
public class MirroringDispatchableAccessors : BaseDispatchableAccessors
{
    /// <summary>
    /// 构造一个 <see cref="MirroringDispatchableAccessors"/>。
    /// </summary>
    /// <param name="accessors">给定的 <see cref="IEnumerable{IAccessor}"/>。</param>
    /// <param name="factory">给定的 <see cref="IDispatcherFactory"/>。</param>
    public MirroringDispatchableAccessors(IEnumerable<IAccessor> accessors, IDispatcherFactory factory)
        : this(accessors, factory, DispatchingMode.Mirroring)
    {
    }

    /// <summary>
    /// 构造一个 <see cref="MirroringDispatchableAccessors"/>。
    /// </summary>
    /// <param name="accessors">给定的 <see cref="IEnumerable{IAccessor}"/>。</param>
    /// <param name="factory">给定的 <see cref="IDispatcherFactory"/>。</param>
    /// <param name="mode">给定的 <see cref="DispatchingMode"/>。</param>
    protected MirroringDispatchableAccessors(IEnumerable<IAccessor> accessors,
        IDispatcherFactory factory, DispatchingMode mode)
        : base(factory.CreateBase(accessors, mode), factory.CreateTransaction(accessors, mode), mode)
    {
        // 镜像模式表示仅从单库读取、需从多库写入冗余数据
    }

}
