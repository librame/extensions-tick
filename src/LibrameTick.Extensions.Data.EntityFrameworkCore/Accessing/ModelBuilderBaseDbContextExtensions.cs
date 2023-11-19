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
/// 定义 <see cref="ModelBuilder"/> 与 <see cref="BaseDataContext"/> 的静态扩展。
/// </summary>
public static class ModelBuilderBaseDbContextExtensions
{

    /// <summary>
    /// 创建指定程序集模型集合的默认配置（支持过滤 <see cref="NotMappedAttribute"/> 定义的模型）。
    /// </summary>
    /// <param name="modelBuilder">给定的 <see cref="ModelBuilder"/>。</param>
    /// <param name="modelAssemblies">给定包含模型的程序集数组。</param>
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
            .Where(static p => !p.IsDynamic) // 动态程序集不支持导出类型集合
            .SelectMany(static s => s.ExportedTypes)
            .Where(p => p.IsConcreteType() && !p.IsNested && !p.IsDefined(notMappedType, inherit: false))
            .ToList();

        foreach (var modelType in modelTypes)
        {
            createMethod.MakeGenericMethod(modelType).Invoke(modelBuilder, new object[] { });
        }

        return modelBuilder;
    }


    /// <summary>
    /// 创建审计模型集合。
    /// </summary>
    /// <param name="modelBuilder">给定的 <see cref="ModelBuilder"/>。</param>
    /// <param name="dbContext">给定的 <see cref="BaseDataContext"/>。</param>
    /// <returns>返回 <see cref="ModelBuilder"/>。</returns>
    public static ModelBuilder CreateAuditingModels(this ModelBuilder modelBuilder,
        BaseDataContext dbContext)
    {
        var limitableMaxLength = dbContext.DataExtOptions.Store.LimitableMaxLengthOfProperty;
        var mapRelationship = dbContext.DataExtOptions.Store.MapRelationship;

        modelBuilder.Entity<Audit>(b =>
        {
            b.ToTableWithSharding(dbContext.BaseDependencies.ShardingContext);

            b.HasIndex(static i => new { i.TableName, i.EntityId }).HasDatabaseName();

            b.HasKey(static k => k.Id);

            b.Property(static p => p.Id).ValueGeneratedNever();

            if (limitableMaxLength > 0)
            {
                b.Property(static p => p.TableName).HasMaxLength(limitableMaxLength).IsRequired();
                b.Property(static p => p.EntityId).HasMaxLength(limitableMaxLength).IsRequired();
                b.Property(static p => p.StateName).HasMaxLength(limitableMaxLength);
                b.Property(static p => p.EntityTypeName).HasMaxLength(limitableMaxLength);
            }
        });

        modelBuilder.Entity<AuditProperty>(b =>
        {
            b.ToTableWithSharding(dbContext.BaseDependencies.ShardingContext);

            b.HasIndex(static i => new { i.AuditId, i.PropertyName }).HasDatabaseName();

            b.HasKey(static k => k.Id);

            b.Property(static x => x.Id).ValueGeneratedNever();

            if (limitableMaxLength > 0)
            {
                b.Property(static p => p.AuditId).HasMaxLength(limitableMaxLength).IsRequired();
                b.Property(static p => p.PropertyName).HasMaxLength(limitableMaxLength);
                b.Property(static p => p.PropertyTypeName).HasMaxLength(limitableMaxLength);
            }

            // MaxLength
            b.Property(static p => p.OldValue);
            b.Property(static p => p.NewValue);

            if (mapRelationship)
            {
                b.HasOne(static f => f.Audit).WithMany(static p => p.Properties).HasForeignKey(static fk => fk.AuditId);
            }
        });

        return modelBuilder;
    }

}
