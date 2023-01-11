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
/// <see cref="RedundableAccessors"/> 静态扩展。
/// </summary>
public static class RedundableAccessorsExtensions
{

    /// <summary>
    /// 获取可冗余的存取器集合。
    /// </summary>
    /// <param name="accessors">给定的 <see cref="IEnumerable{IAccessor}"/>。</param>
    /// <param name="mode">给定的 <see cref="RedundancyMode"/>。</param>
    /// <param name="dispatcherOptions">给定的 <see cref="DispatcherOptions"/>。</param>
    /// <returns>返回经过冗余处理后的存取器。</returns>
    public static IAccessor GetRedundableAccessors(this IEnumerable<IAccessor> accessors,
        RedundancyMode mode, DispatcherOptions dispatcherOptions)
    {
        if (mode == RedundancyMode.Mirroring)
            return new MirroringAccessors(accessors, dispatcherOptions);
        
        return new StripingAccessors(accessors, dispatcherOptions);
    }

}
