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

namespace Librame.Extensions.Data.Accessing
{
    /// <summary>
    /// <see cref="IAccessor"/> 静态扩展。
    /// </summary>
    public static class AccessorExtensions
    {

        /// <summary>
        /// 对当前访问器进行分库。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
        /// <param name="shardingManager">给定的 <see cref="IShardingManager"/>。</param>
        /// <param name="descriptors">给定的 <see cref="IEnumerable{AccessorDescriptor}"/>。</param>
        /// <param name="basis">给定的分片依据。</param>
        /// <returns>返回 <see cref="IAccessor"/>。</returns>
        public static IAccessor ShardingDatabase(this IAccessor accessor, IShardingManager shardingManager,
            IEnumerable<AccessorDescriptor> descriptors, object? basis)
        {
            var descriptor = descriptors.FirstOrDefault(descr => descr.Accessor.AccessorId == accessor.AccessorId);
            if (descriptor?.ShardingNaming == null)
                return accessor;

            return accessor.ShardingDatabase(shardingManager, descriptor.ShardingNaming, basis);
        }

        /// <summary>
        /// 对当前访问器进行分库。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
        /// <param name="shardingManager">给定的 <see cref="IShardingManager"/>。</param>
        /// <param name="shardingNaming">给定的分库命名特性。</param>
        /// <param name="basis">给定的分片依据。</param>
        /// <returns>返回 <see cref="IAccessor"/>。</returns>
        public static IAccessor ShardingDatabase(this IAccessor accessor, IShardingManager shardingManager,
            ShardingNamingAttribute shardingNaming, object? basis)
        {
            if (string.IsNullOrEmpty(accessor.CurrentConnectionString))
                return accessor;

            var descriptor = shardingManager.GetNamingDescriptorFromConnectionString(accessor.CurrentConnectionString,
                shardingNaming, basis);

            var shardingName = descriptor.ToString();
            if (!shardingName.Equals(descriptor.BaseName))
            {
                var newConnectionString = accessor.CurrentConnectionString.Replace(descriptor.BaseName!, shardingName);
                accessor.ChangeConnection(newConnectionString);
            }

            return accessor;
        }

    }
}
