#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core;

/// <summary>
/// 定义一个可比较对象成员的键相等比较器。
/// </summary>
/// <typeparam name="T">指定的类型。</typeparam>
public static class KeyEqualityComparer<T>
{

    /// <summary>
    /// 通过指定键选择器创建相等比较器。
    /// </summary>
    /// <typeparam name="TKey">指定的键类型。</typeparam>
    /// <param name="keySelector">给定要比较的键选择器。</param>
    /// <returns>返回 <see cref="IEqualityComparer{T}"/>。</returns>
    public static IEqualityComparer<T> CreateBy<TKey>(Func<T, TKey> keySelector)
        => EqualityComparer<T>.Create((x, y) => x is not null && y is not null && keySelector(x)?.Equals(keySelector(y)) == true,
            obj => keySelector(obj)?.GetHashCode() ?? 0);

}
