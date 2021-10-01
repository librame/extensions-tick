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

namespace Librame.Extensions.Data.Specification;

/// <summary>
/// 定义写入访问器规约（默认按优先级进行升序排列）。
/// </summary>
public class WriteAccessorSpecification : BaseAccessorSpecification
{
    /// <summary>
    /// 构造一个默认 <see cref="WriteAccessorSpecification"/>。
    /// </summary>
    public WriteAccessorSpecification()
        : base()
    {
        SetAccess(AccessMode.Write | AccessMode.ReadWrite);
    }

    /// <summary>
    /// 使用指定的判断依据构造一个 <see cref="WriteAccessorSpecification"/>。
    /// </summary>
    /// <param name="criterion">给定的判断依据。</param>
    public WriteAccessorSpecification(Func<IAccessor, bool> criterion)
        : base(criterion)
    {
        SetAccess(AccessMode.Write | AccessMode.ReadWrite);
    }

}
