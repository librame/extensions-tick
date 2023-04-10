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
/// 定义一个表示数据访问的复合可调度存取器集合，默认通过 <see cref="TransactionDispatcher{IAccessor}"/> 实现（支持针对分布式写入事务遍历等功能）。
/// </summary>
public class CompositingDispatchableAccessors : BaseDispatchableAccessors
{
    /// <summary>
    /// 构造一个 <see cref="CompositingDispatchableAccessors"/>。
    /// </summary>
    /// <param name="accessors">给定的 <see cref="IEnumerable{IAccessor}"/>。</param>
    /// <param name="factory">给定的 <see cref="IDispatcherFactory"/>。</param>
    public CompositingDispatchableAccessors(IEnumerable<IAccessor> accessors, IDispatcherFactory factory)
        : base(factory.CreateTransaction(accessors))
    {
    }

}
