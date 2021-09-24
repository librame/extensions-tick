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
/// 定义 <see cref="IAccessor"/> 种子机。
/// </summary>
public interface IAccessorSeeder
{
    /// <summary>
    /// 播种。
    /// </summary>
    /// <typeparam name="TValue">指定的值类型。</typeparam>
    /// <param name="key">给定的键。</param>
    /// <param name="valueFactory">给定的值工厂方法。</param>
    /// <returns>返回 <typeparamref name="TValue"/>。</returns>
    TValue Seed<TValue>(string key, Func<string, TValue> valueFactory);

    /// <summary>
    /// 播种。
    /// </summary>
    /// <param name="key">给定的键。</param>
    /// <param name="valueFactory">给定的值工厂方法。</param>
    /// <returns>返回对象。</returns>
    object Seed(string key, Func<string, object> valueFactory);
}
