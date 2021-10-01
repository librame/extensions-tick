#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Data.Storing;

namespace Librame.Extensions.Data.Accessing;

/// <summary>
/// <see cref="ModelBuilder"/> 与 <see cref="AbstractDataAccessor"/> 静态扩展。
/// </summary>
public static class ModelBuilderDataAccessorExtensions
{

    /// <summary>
    /// 创建数据模型。
    /// </summary>
    /// <param name="modelBuilder">给定的 <see cref="ModelBuilder"/>。</param>
    /// <param name="dataAccessor">给定的 <see cref="IDataAccessor"/>。</param>
    /// <returns>返回 <see cref="ModelBuilder"/>。</returns>
    public static ModelBuilder CreateDataModel(this ModelBuilder modelBuilder, IDataAccessor dataAccessor)
    {
        var limitableMaxLength = dataAccessor.DataOptions.Store.LimitableMaxLengthOfProperty;
        var mapRelationship = dataAccessor.DataOptions.Store.MapRelationship;

        modelBuilder.Entity<Audit>(b =>
        {
            b.ToTableWithSharding(dataAccessor.ShardingManager);

            b.HasIndex(i => new { i.TableName, i.EntityId }).HasDatabaseName();

            b.HasKey(k => k.Id);

            b.Property(p => p.Id).ValueGeneratedNever();

            if (limitableMaxLength > 0)
            {
                b.Property(p => p.TableName).HasMaxLength(limitableMaxLength).IsRequired();
                b.Property(p => p.EntityId).HasMaxLength(limitableMaxLength).IsRequired();
                b.Property(p => p.StateName).HasMaxLength(limitableMaxLength);
                b.Property(p => p.EntityTypeName).HasMaxLength(limitableMaxLength);
            }
        });

        modelBuilder.Entity<AuditProperty>(b =>
        {
            b.ToTableWithSharding(dataAccessor.ShardingManager);

            b.HasIndex(i => new { i.AuditId, i.PropertyName }).HasDatabaseName();

            b.HasKey(k => k.Id);

            b.Property(x => x.Id).ValueGeneratedNever();

            if (limitableMaxLength > 0)
            {
                b.Property(p => p.AuditId).HasMaxLength(limitableMaxLength).IsRequired();
                b.Property(p => p.PropertyName).HasMaxLength(limitableMaxLength);
                b.Property(p => p.PropertyTypeName).HasMaxLength(limitableMaxLength);
            }

            // MaxLength
            b.Property(p => p.OldValue);
            b.Property(p => p.NewValue);

            if (mapRelationship)
            {
                b.HasOne(f => f.Audit).WithMany(p => p.Properties).HasForeignKey(fk => fk.AuditId);
            }
        });

        return modelBuilder;
    }

}
