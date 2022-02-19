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

namespace Librame.Extensions.Data.Specifications;

/// <summary>
/// 定义读取访问器规约（默认按优先级进行升序排列）。
/// </summary>
public class ReadAccessorSpecification : BaseAccessorSpecification
{
    /// <summary>
    /// 构造一个默认 <see cref="ReadAccessorSpecification"/>。
    /// </summary>
    public ReadAccessorSpecification()
        : base()
    {
        SetAccess(AccessMode.Read | AccessMode.ReadWrite);
    }

    /// <summary>
    /// 使用指定的判断依据构造一个 <see cref="ReadAccessorSpecification"/>。
    /// </summary>
    /// <param name="criterion">给定的判断依据。</param>
    public ReadAccessorSpecification(Func<IAccessor, bool> criterion)
        : base(criterion)
    {
        SetAccess(AccessMode.Read | AccessMode.ReadWrite);
    }

}
