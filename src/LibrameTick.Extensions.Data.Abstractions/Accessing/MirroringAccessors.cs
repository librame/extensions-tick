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
/// 定义一个表示数据访问集合的镜像存取器（支持针对读取异常切换与写入事务遍历等功能）。
/// </summary>
public class MirroringAccessors : RedundableAccessors
{
    /// <summary>
    /// 构造一个 <see cref="MirroringAccessors"/>。
    /// </summary>
    /// <param name="accessors">给定要复合的 <see cref="IEnumerable{IAccessor}"/>。</param>
    public MirroringAccessors(IEnumerable<IAccessor> accessors)
        : base(new ExceptionSwitchingDispatcher<IAccessor>(accessors),
            new TransactionTraversalDispatcher<IAccessor>(accessors))
    {
    }

}
