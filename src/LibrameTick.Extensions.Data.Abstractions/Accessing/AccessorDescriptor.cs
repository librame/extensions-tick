#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Infrastructure;
using Librame.Extensions.Data.Sharding;
using Librame.Extensions.Infrastructure.Dispatching;

namespace Librame.Extensions.Data.Accessing;

/// <summary>
/// 定义存取器描述符。
/// </summary>
public class AccessorDescriptor : IEquatable<AccessorDescriptor>
{
    /// <summary>
    /// 构造一个 <see cref="AccessorDescriptor"/>。
    /// </summary>
    /// <param name="accessor">给定的存取器实例。</param>
    /// <param name="serviceType">给定的服务类型。</param>
    /// <param name="name">给定的名称。</param>
    /// <param name="group">给定的所属群组。</param>
    /// <param name="partition">给定的所属分区。</param>
    /// <param name="access">给定的访问模式。</param>
    /// <param name="dispatching">给定的调度模式。</param>
    /// <param name="priority">给定的优先级。</param>
    /// <param name="algorithm">给定的算法选项。</param>
    /// <param name="sharded">给定的分库特性。</param>
    /// <param name="shardingValues">给定的分片值集合。</param>
    /// <param name="loaderHost">给定的负载器主机。</param>
    public AccessorDescriptor(IAccessor accessor,
        Type serviceType,
        string name,
        int group,
        int partition,
        AccessMode access,
        DispatchingMode dispatching,
        //bool pooling,
        float priority,
        AlgorithmOptions algorithm,
        ShardingAttribute? sharded,
        List<IShardingValue>? shardingValues,
        string? loaderHost)
    {
        Accessor = accessor;
        ServiceType = serviceType;
        Name = name;
        Group = group;
        Partition = partition;
        Access = access;
        Dispatching = dispatching;
        //Pooling = pooling;
        Priority = priority;
        Algorithm = algorithm;
        Sharding = sharded;
        ShardingValues = shardingValues;
        LoaderHost = loaderHost;
    }


    /// <summary>
    /// 服务类型。
    /// </summary>
    public Type ServiceType { get; init; }

    /// <summary>
    /// 存取器实例。
    /// </summary>
    public IAccessor Accessor { get; init; }

    /// <summary>
    /// 名称。
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// 所属群组。
    /// </summary>
    public int Group { get; init; }

    /// <summary>
    /// 所属分区。
    /// </summary>
    public int Partition { get; init; }

    /// <summary>
    /// 访问模式。
    /// </summary>
    public AccessMode Access { get; init; }

    /// <summary>
    /// 调度模式。
    /// </summary>
    public DispatchingMode Dispatching { get; init; }

    ///// <summary>
    ///// 是否池化。
    ///// </summary>
    //public bool Pooling { get; init; }

    /// <summary>
    /// 优先级。
    /// </summary>
    public float Priority {  get; init; }

    /// <summary>
    /// 算法选项。
    /// </summary>
    public AlgorithmOptions Algorithm { get; init; }

    /// <summary>
    /// 分库特性。
    /// </summary>
    public ShardingAttribute? Sharding { get; init; }

    /// <summary>
    /// 分片值集合。
    /// </summary>
    public List<IShardingValue>? ShardingValues { get; init; }

    /// <summary>
    /// 负载器主机。
    /// </summary>
    public string? LoaderHost { get; init; }


    /// <summary>
    /// 比较存取器描述符的服务类型相等。
    /// </summary>
    /// <param name="other">给定的 <see cref="AccessorDescriptor"/>。</param>
    /// <returns>返回布尔值。</returns>
    public bool Equals(AccessorDescriptor? other)
        => ServiceType.Equals(other?.ServiceType);

    /// <summary>
    /// 获取哈希码。
    /// </summary>
    /// <returns>返回服务类型哈希码。</returns>
    public override int GetHashCode()
        => ServiceType.GetHashCode();

}
