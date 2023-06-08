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
using Librame.Extensions.Dispatchers;

namespace Librame.Extensions.Data.Storing;

/// <summary>
/// 定义实现 <see cref="IStorePartitioner{T}"/> 的基础存储分区器。
/// </summary>
/// <typeparam name="T">指定的类型。</typeparam>
public class BaseStorePartitioner<T> : IStorePartitioner<T>
    where T : class
{
    private readonly IDispatcher<IAccessor> _accessors;
    private readonly IEnumerable<int> _paratitions;


    /// <summary>
    /// 构造一个 <see cref="BaseStorePartitioner{T}"/>。
    /// </summary>
    /// <param name="accessors">给定的 <see cref="IDispatcher{IAccessor}"/>。</param>
    public BaseStorePartitioner(IDispatcher<IAccessor> accessors)
    {
        _accessors = accessors;
        _paratitions = Enumerable.Range(1, accessors.Count);

        var accessorParatitions = accessors.Select(static s => s.AccessorDescriptor!.Partition).Order();
        if (!_paratitions.SequenceEqual(accessorParatitions))
            throw new InvalidDataException($"Accessor partitions must be numbered sequentially, starting with 1.");
    }


    /// <summary>
    /// 计算分区集合。
    /// </summary>
    /// <param name="values">给定的 <see cref="IEnumerable{T}"/>。</param>
    /// <returns>返回 <see cref="Dictionary{IAccessor, List}"/>。</returns>
    public virtual Dictionary<IAccessor, List<T>> CalcPartitions(IEnumerable<T> values)
    {
        var dict = new Dictionary<IAccessor, List<T>>();

        foreach (var pair in ToPartitions(values))
        {
            var accessor = _accessors.First(p => p.AccessorDescriptor?.Partition == pair.Key);
            dict.Add(accessor, pair.Value);
        }

        return dict;
    }


    /// <summary>
    /// 获取或计算分区。
    /// </summary>
    /// <param name="value">给定的 <typeparamref name="T"/>。</param>
    /// <returns>返回分区。</returns>
    public virtual int GetOrCalcPartition(T value)
    {
        if (value is IPartitioning<int> partitioning && partitioning.Partition > 0)
            return partitioning.Partition;

        return CalcPartition(value);
    }


    private Dictionary<int, List<T>> ToPartitions(IEnumerable<T> values)
    {
        var dict = new Dictionary<int, List<T>>();

        foreach (var value in values)
        {
            var partition = GetOrCalcPartition(value);

            if (value is IPartitioning<int> partitioning && partitioning.Partition < 1)
                partitioning.Partition = partition;

            if (dict.TryGetValue(partition, out List<T>? result))
                result.Add(value);
            else
                dict[partition] = new List<T>() { value };
        }

        return dict;
    }

    private int CalcPartition(T value)
    {
        if (value is IIdentifier<long> longId)
            return CalcPartition(longId.Id);

        return CalcPartition(value.GetHashCode());
    }

    private int CalcPartition(long code)
        => (int)(code % _accessors.Count + 1); // 对存取器数量取模计算实体所属分区

}
