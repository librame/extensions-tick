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
using Librame.Extensions.Data.Storing;
using Microsoft.EntityFrameworkCore;

namespace Librame.Extensions.Data.Accessing
{
    /// <summary>
    /// <see cref="ModelBuilder"/> 与 <see cref="AbstractDataAccessor"/> 静态扩展。
    /// </summary>
    public static class ModelBuilderDataAccessorExtensions
    {

        /// <summary>
        /// 创建数据模型。
        /// </summary>
        /// <param name="modelBuilder">给定的 <see cref="ModelBuilder"/>。</param>
        /// <param name="shardingManager">给定的 <see cref="IShardingManager"/>。</param>
        /// <returns>返回 <see cref="ModelBuilder"/>。</returns>
        public static ModelBuilder CreateDataModel(this ModelBuilder modelBuilder,
            IShardingManager shardingManager)
        {
            modelBuilder.Entity<Audit>(b =>
            {
                b.ToTableByPluralize(shardingManager);

                b.HasKey(k => k.Id);

                b.HasIndex(i => i.Name);

                b.Property(p => p.Id).ValueGeneratedNever();

                b.Property(p => p.Name).HasMaxLength(50);
                b.Property(p => p.Passwd).HasMaxLength(50);
                b.Property(p => p.CreatedTime).HasMaxLength(50);
            });

            return modelBuilder;
        }

    }
}
