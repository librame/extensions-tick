#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data.Accessing;

/// <summary>
/// <see cref="AccessorDescriptor"/> 静态扩展。
/// </summary>
public static class AccessorDescriptorExtensions
{

    /// <summary>
    /// 按指定的访问模式过滤访问器集合（支持访问模式的位与运算）。
    /// </summary>
    /// <param name="accessors">给定的 <see cref="IEnumerable{IAccessor}"/>。</param>
    /// <param name="access">给定的 <see cref="AccessMode"/>。</param>
    /// <returns>返回 <see cref="IEnumerable{IAccessor}"/>。</returns>
    public static IEnumerable<IAccessor> FilterAccessors(this IEnumerable<IAccessor> accessors, AccessMode access)
        => accessors.Where(p => (access & p.AccessorDescriptor?.Access) == p.AccessorDescriptor?.Access);

}
