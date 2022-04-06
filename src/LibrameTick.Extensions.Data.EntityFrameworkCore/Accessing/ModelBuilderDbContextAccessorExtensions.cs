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
/// 定义 <see cref="ModelBuilder"/> 与 <see cref="AbstractDbContextAccessor"/> 的静态扩展。
/// </summary>
public static class ModelBuilderDbContextAccessorExtensions
{

    /// <summary>
    /// 创建指定程序集模型集合的默认配置（支持过滤 <see cref="NotMappedAttribute"/> 定义的模型）。
    /// </summary>
    /// <param name="modelBuilder">给定的 <see cref="ModelBuilder"/>。</param>
    /// <param name="modelAssemblies">给定包含模型的 <see cref="IEnumerable{String}"/> 程序集集合。</param>
    /// <returns>返回 <see cref="ModelBuilder"/>。</returns>
    public static ModelBuilder CreateAssembliesModels(this ModelBuilder modelBuilder,
        params string[] modelAssemblies)
        => modelBuilder.CreateAssembliesModels(modelAssemblies.AsEnumerable());

    /// <summary>
    /// 创建指定程序集模型集合的默认配置（支持过滤 <see cref="NotMappedAttribute"/> 定义的模型）。
    /// </summary>
    /// <param name="modelBuilder">给定的 <see cref="ModelBuilder"/>。</param>
    /// <param name="modelAssemblies">给定包含模型的 <see cref="IEnumerable{String}"/> 程序集集合。</param>
    /// <returns>返回 <see cref="ModelBuilder"/>。</returns>
    public static ModelBuilder CreateAssembliesModels(this ModelBuilder modelBuilder,
        IEnumerable<string> modelAssemblies)
    {
        var notMappedType = typeof(NotMappedAttribute);

        var createMethod = typeof(ModelBuilder).GetType()
            .GetMethod(nameof(modelBuilder.Entity), Type.EmptyTypes);
        if (createMethod is null)
            return modelBuilder;

        var modelTypes = modelAssemblies
            .Select(Assembly.Load)
            .Where(p => !p.IsDynamic) // 动态程序集不支持导出类型集合
            .SelectMany(s => s.ExportedTypes)
            .Where(p => p.IsConcreteType() && !p.IsNested && !p.IsDefined(notMappedType, inherit: false))
            .ToList();

        foreach (var modelType in modelTypes)
        {
            createMethod.MakeGenericMethod(modelType)
                .Invoke(modelBuilder, new object[] { });
        }

        return modelBuilder;
    }


    /// <summary>
    /// 创建审计模型集合。
    /// </summary>
    /// <param name="modelBuilder">给定的 <see cref="ModelBuilder"/>。</param>
    /// <param name="accessor">给定的 <see cref="AbstractDbContextAccessor"/>。</param>
    /// <returns>返回 <see cref="ModelBuilder"/>。</returns>
    public static ModelBuilder CreateAuditingModels(this ModelBuilder modelBuilder,
        AbstractDbContextAccessor accessor)
    {
        var limitableMaxLength = accessor.DataOptions.Store.LimitableMaxLengthOfProperty;
        var mapRelationship = accessor.DataOptions.Store.MapRelationship;

        modelBuilder.Entity<Audit>(b =>
        {
            b.ToTableWithSharding(accessor.ShardingManager);

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
            b.ToTableWithSharding(accessor.ShardingManager);

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
