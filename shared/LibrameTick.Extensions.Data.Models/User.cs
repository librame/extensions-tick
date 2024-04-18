#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Dependencies;
using Librame.Extensions.Data.Sharding;
using System;
using System.Globalization;

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 用户模型。
    /// </summary>
    [ShardingTable("%ci:n3lwl_%dto:d6", typeof(CultureInfoShardingStrategy), typeof(DateTimeOffsetShardingStrategy))]
    public class User : AbstractCreationIdentifier<string, string>, IPartitioning<int>
        , IShardingValue<CultureInfo>, IShardingValue<DateTimeOffset>
    {
        /// <summary>
        /// 名称。
        /// </summary>
        public virtual string? Name { get; set; }

        /// <summary>
        /// 密码。
        /// </summary>
        [Encrypted]
        public virtual string? Passwd { get; set; }

        /// <summary>
        /// 分区。
        /// </summary>
        public virtual int Partition { get; set; }


        /// <summary>
        /// 获取分片值。
        /// </summary>
        /// <returns>返回 <see cref="CultureInfo"/>。</returns>
        public virtual CultureInfo GetShardedValue(CultureInfo? defaultValue)
            => CultureInfo.CurrentCulture;

        /// <summary>
        /// 获取分片值。
        /// </summary>
        /// <returns>返回 <see cref="DateTimeOffset"/>。</returns>
        public virtual DateTimeOffset GetShardedValue(DateTimeOffset defaultValue)
            => CreatedTime; // 默认使用创建时间分片

    }
}
