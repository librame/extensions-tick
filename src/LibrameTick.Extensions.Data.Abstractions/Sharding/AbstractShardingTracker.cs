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

namespace Librame.Extensions.Data.Sharding;

/// <summary>
/// 定义抽象实现 <see cref="IShardingTracker"/> 的分片对象跟踪器。
/// </summary>
public abstract class AbstractShardingTracker : IShardingTracker
{
    private readonly ConcurrentDictionary<IAccessor, ShardingDescriptor> _accessorDescriptors;
    private readonly ConcurrentDictionary<Type, ShardingDescriptor> _entityDescriptors;
    private readonly ConcurrentDictionary<Type, List<IShardingValue>> _entityValues;


    /// <summary>
    /// 构造一个 <see cref="AbstractShardingTracker"/>。
    /// </summary>
    protected AbstractShardingTracker()
    {
        _accessorDescriptors = new();
        _entityDescriptors = new();
        _entityValues = new();
    }


    /// <summary>
    /// 获取或新增指定存取器的分片描述符。
    /// </summary>
    /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
    /// <param name="valueFactory">给定新增分片描述符的工厂方法。</param>
    /// <returns>返回 <see cref="ShardingDescriptor"/>。</returns>
    public virtual ShardingDescriptor GetOrAddDescriptor(IAccessor accessor,
        Func<IAccessor, ShardingDescriptor> valueFactory)
        => _accessorDescriptors.GetOrAdd(accessor, valueFactory);

    /// <summary>
    /// 获取或新增指定实体类型的分片描述符。
    /// </summary>
    /// <param name="entityType">给定的实体类型。</param>
    /// <param name="valueFactory">给定新增分片描述符的工厂方法。</param>
    /// <returns>返回 <see cref="ShardingDescriptor"/>。</returns>
    public virtual ShardingDescriptor GetOrAddDescriptor(Type entityType,
        Func<Type, ShardingDescriptor> valueFactory)
        => _entityDescriptors.GetOrAdd(entityType, valueFactory);


    /// <summary>
    /// 尝试获取实体分片属性。
    /// </summary>
    /// <param name="entityType">给定的实体类型。</param>
    /// <param name="values">输出 <see cref="IReadOnlyCollection{IShardingValue}"/>。</param>
    /// <returns>返回布尔值。</returns>
    public virtual bool TryGetEntityValue(Type entityType, [MaybeNullWhen(false)] out IReadOnlyCollection<IShardingValue> values)
    {
        if (_entityValues.TryGetValue(entityType, out var result))
        {
            values = result.AsReadOnlyCollection();
            return true;
        }

        values = null;
        return false;
    }

    /// <summary>
    /// 添加实体分片值。
    /// </summary>
    /// <param name="entityType">给定的实体类型。</param>
    /// <param name="value">给定的 <see cref="IShardingValue"/>。</param>
    /// <returns>返回 <see cref="IShardingTracker"/>。</returns>
    public virtual IShardingTracker AddEntityValue(Type entityType, IShardingValue value)
    {
        if (_entityValues.TryGetValue(entityType, out var result))
        {
            result.Add(value);
        }
        else
        {
            _entityValues.TryAdd(entityType, new List<IShardingValue> { value });
        }

        return this;
    }

}
