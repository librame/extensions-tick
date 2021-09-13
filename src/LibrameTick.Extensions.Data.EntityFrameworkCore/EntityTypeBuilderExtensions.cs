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
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Librame.Extensions.Data
{
    /// <summary>
    /// <see cref="EntityTypeBuilder"/> 静态扩展。
    /// </summary>
    public static class EntityTypeBuilderExtensions
    {
        /// <summary>
        /// 通过复数化实体类型名称映射表名（此方法仅支持关系型数据库）。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <param name="builder">给定的 <see cref="EntityTypeBuilder{TEntity}"/>。</param>
        /// <param name="shardingManager">给定的 <see cref="IShardingManager"/>。</param>
        /// <param name="basis">给定的分片依据（可选）。</param>
        /// <returns>返回 <see cref="EntityTypeBuilder{TEntity}"/>。</returns>
        public static EntityTypeBuilder<TEntity> ToTableByPluralize<TEntity>(this EntityTypeBuilder<TEntity> builder,
            IShardingManager shardingManager, object? basis = null)
            where TEntity : class
            => builder.ToTableByPluralize(shardingManager, schema: null, basis);

        /// <summary>
        /// 通过复数化实体类型名称映射表名（此方法仅支持关系型数据库）。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <param name="builder">给定的 <see cref="EntityTypeBuilder{TEntity}"/>。</param>
        /// <param name="shardingManager">给定的 <see cref="IShardingManager"/>。</param>
        /// <param name="schema">给定的架构。</param>
        /// <param name="basis">给定的分片依据（可选）。</param>
        /// <returns>返回 <see cref="EntityTypeBuilder{TEntity}"/>。</returns>
        public static EntityTypeBuilder<TEntity> ToTableByPluralize<TEntity>(this EntityTypeBuilder<TEntity> builder,
            IShardingManager shardingManager, string? schema, object? basis = null)
            where TEntity : class
        {
            var shardingNaming = shardingManager.GetNamingDescriptorFromEntity(typeof(TEntity), basis);

            builder.Metadata.SetTableName(shardingNaming);
            builder.Metadata.SetSchema(schema);

            return builder;
        }

    }
}
