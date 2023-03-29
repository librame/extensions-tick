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
/// <see cref="IShardingManager"/> 静态扩展。
/// </summary>
public static class ShardingManagerExtensions
{

    /// <summary>
    /// 创建分片描述符。
    /// </summary>
    /// <param name="manager">给定的 <see cref="IShardingManager"/>。</param>
    /// <param name="attribute">给定的 <see cref="ShardedAttribute"/>。</param>
    /// <returns>返回 <see cref="ShardedDescriptor"/>。</returns>
    /// <exception cref="ArgumentException">
    /// <see cref="ShardedAttribute.BaseName"/> is null or empty.
    /// </exception>
    public static ShardedDescriptor CreateDescriptor(this IShardingManager manager, ShardedAttribute attribute)
    {
        if (string.IsNullOrEmpty(attribute.BaseName))
            throw new ArgumentException($"The {nameof(attribute)}.{nameof(attribute.BaseName)} is null or empty.");

        var descriptor = new ShardedDescriptor(attribute.BaseName, attribute.Suffix);

        if (attribute.DefaultStrategyType is not null)
            descriptor.DefaultStrategy = manager.GetStrategy(attribute.DefaultStrategyType);

        return descriptor;
    }


    /// <summary>
    /// 对存取器的数据库分片。
    /// </summary>
    /// <param name="manager">给定的 <see cref="IShardingManager"/>。</param>
    /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
    /// <returns>返回是否分片的布尔值。</returns>
    public static bool ShardDatabase(this IShardingManager manager, IAccessor accessor)
        => manager.ShardDatabase(accessor, out _);

    /// <summary>
    /// 对存取器的数据库分片。
    /// </summary>
    /// <param name="manager">给定的 <see cref="IShardingManager"/>。</param>
    /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
    /// <param name="descriptor">输出 <see cref="ShardedDescriptor"/>。</param>
    /// <returns>返回是否分片的布尔值。</returns>
    public static bool ShardDatabase(this IShardingManager manager, IAccessor accessor,
        [MaybeNullWhen(false)] out ShardedDescriptor descriptor)
    {
        var attribute = accessor.AccessorDescriptor?.Sharded;
        if (attribute is null)
        {
            descriptor = null;
            return false;
        }

        descriptor = manager.CreateDescriptor(attribute);

        // 使用分片策略格式化分片名称
        descriptor.DefaultStrategy?.FormatSuffix(descriptor);

        if (descriptor.IsNeedShardingForConnectionString(accessor.CurrentConnectionString!,
            out var newConnectionString))
        {
            // 切换为分片数据连接
            accessor.ChangeConnection(newConnectionString!);

            return true;
        }

        return false;
    }


    /// <summary>
    /// 通过实体类型特性获取分片信息。
    /// </summary>
    /// <param name="manager">给定的 <see cref="IShardingManager"/>。</param>
    /// <param name="entityType">给定的实体类型。</param>
    /// <param name="entity">给定的实体对象。</param>
    /// <param name="tableName">给定的表名。</param>
    /// <returns>返回 <see cref="ShardedDescriptor"/>。</returns>
    public static ShardedDescriptor GetShardedByEntity(this IShardingManager manager,
        Type entityType, object? entity, string? tableName)
    {
        var attribute = ShardedAttribute.ParseFromEntity(entityType, tableName);
        var descriptor = manager.CreateDescriptor(attribute);

        descriptor.ReferenceType = entityType;
        descriptor.ReferenceValue = entity;
        descriptor.ReferenceName = tableName;

        // 使用分片策略格式化分片名称
        descriptor.DefaultStrategy?.FormatSuffix(descriptor);
        descriptor.Entity?.Properties.ForEach(p => p.Strategy.FormatSuffix(descriptor));

        return descriptor;
    }

}
