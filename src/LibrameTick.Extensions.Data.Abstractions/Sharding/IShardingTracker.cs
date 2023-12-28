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

///// <summary>
///// 定义一个分片对象跟踪器接口。
///// </summary>
//public interface IShardingTracker
//{
//    /// <summary>
//    /// 获取或新增指定存取器的分片描述符。
//    /// </summary>
//    /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
//    /// <param name="valueFactory">给定新增分片描述符的工厂方法。</param>
//    /// <returns>返回 <see cref="ShardingDescriptor"/>。</returns>
//    ShardingDescriptor GetOrAddDescriptor(IAccessor accessor,
//        Func<IAccessor, ShardingDescriptor> valueFactory);

//    /// <summary>
//    /// 获取或新增指定实体类型的分片描述符。
//    /// </summary>
//    /// <param name="entityType">给定的实体类型。</param>
//    /// <param name="valueFactory">给定新增分片描述符的工厂方法。</param>
//    /// <returns>返回 <see cref="ShardingDescriptor"/>。</returns>
//    ShardingDescriptor GetOrAddDescriptor(Type entityType,
//        Func<Type, ShardingDescriptor> valueFactory);


//    /// <summary>
//    /// 尝试获取实体分片属性。
//    /// </summary>
//    /// <param name="entityType">给定的实体类型。</param>
//    /// <param name="values">输出 <see cref="IReadOnlyCollection{IShardingValue}"/>。</param>
//    /// <returns>返回布尔值。</returns>
//    bool TryGetEntityValue(Type entityType, [MaybeNullWhen(false)] out IReadOnlyCollection<IShardingValue> values);

//    /// <summary>
//    /// 添加实体分片值。
//    /// </summary>
//    /// <param name="entityType">给定的实体类型。</param>
//    /// <param name="value">给定的 <see cref="IShardingValue"/>。</param>
//    /// <returns>返回 <see cref="IShardingTracker"/>。</returns>
//    IShardingTracker AddEntityValue(Type entityType, IShardingValue value);

//}
