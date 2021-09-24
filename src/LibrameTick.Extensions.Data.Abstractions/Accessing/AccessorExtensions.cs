#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Data.Sharding;

namespace Librame.Extensions.Data.Accessing;

/// <summary>
/// <see cref="IAccessor"/> 静态扩展。
/// </summary>
public static class AccessorExtensions
{

    /// <summary>
    /// 对访问器进行分库。
    /// </summary>
    /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
    /// <param name="shardingManager">给定的 <see cref="IShardingManager"/>。</param>
    /// <param name="basis">给定的分片依据。</param>
    /// <returns>返回 <see cref="IAccessor"/>。</returns>
    public static IAccessor ShardingDatabase(this IAccessor accessor, IShardingManager shardingManager, object? basis)
    {
        if (accessor.AccessorDescriptor?.Sharded is null || string.IsNullOrEmpty(accessor.CurrentConnectionString))
            return accessor;

        var shardDescriptor = shardingManager.GetDescriptorFromConnectionString(accessor.CurrentConnectionString,
            accessor.AccessorDescriptor.Sharded, basis);

        var shardName = shardDescriptor.ToString();
        if (!shardName.Equals(shardDescriptor.BaseName))
        {
            var newConnectionString = accessor.CurrentConnectionString.Replace(shardDescriptor.BaseName!, shardName);
            accessor.ChangeConnection(newConnectionString);
        }

        return accessor;
    }

}
