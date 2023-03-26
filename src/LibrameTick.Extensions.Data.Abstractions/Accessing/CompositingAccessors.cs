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
/// 定义一个表示数据访问集合的复合存取器，默认通过 <see cref="TransactionTraversalDispatcher{IAccessor}"/> 实现（支持针对分布式写入事务遍历等功能）。
/// </summary>
public class CompositingAccessors : RedundableAccessors
{
    /// <summary>
    /// 构造一个 <see cref="CompositingAccessors"/>。
    /// </summary>
    /// <param name="accessors">给定的 <see cref="IEnumerable{IAccessor}"/>。</param>
    /// <param name="dispatcherOptions">给定的 <see cref="DispatcherOptions"/>。</param>
    public CompositingAccessors(IEnumerable<IAccessor> accessors, DispatcherOptions dispatcherOptions)
        : base(new TransactionTraversalDispatcher<IAccessor>(accessors, dispatcherOptions))
    {
    }

}
