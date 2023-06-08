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

namespace Librame.Extensions.Data.Storing;

/// <summary>
/// 定义一个存储分区器接口。
/// </summary>
/// <typeparam name="T">指定的类型。</typeparam>
public interface IStorePartitioner<T>
    where T : class
{
    /// <summary>
    /// 计算分区集合。
    /// </summary>
    /// <param name="values">给定的 <see cref="IEnumerable{T}"/>。</param>
    /// <returns>返回 <see cref="Dictionary{IAccessor, List}"/>。</returns>
    Dictionary<IAccessor, List<T>> CalcPartitions(IEnumerable<T> values);

    /// <summary>
    /// 获取或计算分区。
    /// </summary>
    /// <param name="value">给定的 <typeparamref name="T"/>。</param>
    /// <returns>返回分区。</returns>
    int GetOrCalcPartition(T value);
}
