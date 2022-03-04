#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Data.Auditing;
using Librame.Extensions.Data.Storing;

namespace Librame.Extensions.Data.Accessing;

/// <summary>
/// 定义抽象继承 <see cref="AbstractDbContextAccessorWithAudit"/> 含审计功能的数据库上下文存取器的泛型实现。
/// </summary>
/// <typeparam name="TAccessor">指定实现 <see cref="AbstractDbContextAccessorWithAudit"/> 的存取器类型。</typeparam>
public abstract class AbstractDbContextAccessorWithAudit<TAccessor> : AbstractDbContextAccessorWithAudit
    where TAccessor : AbstractDbContextAccessorWithAudit
{
    /// <summary>
    /// 使用指定的数据库上下文选项构造一个 <see cref="AbstractDbContextAccessorWithAudit{TAccessor}"/> 实例。
    /// </summary>
    /// <remarks>
    /// 备注：如果需要注册多个 <see cref="DbContext"/> 扩展，参数必须使用泛型 <see cref="DbContextOptions{TAccessor}"/> 形式，
    /// 不能使用非泛型 <see cref="DbContextOptions"/> 形式，因为 <paramref name="options"/> 参数也会注册到容器中以供使用。
    /// </remarks>
    /// <param name="options">给定的 <see cref="DbContextOptions{TAccessor}"/>。</param>
    protected AbstractDbContextAccessorWithAudit(DbContextOptions<TAccessor> options)
        : base(options)
    {
    }


    /// <summary>
    /// 存取器类型。
    /// </summary>
    public override Type AccessorType
        => typeof(TAccessor);
}


/// <summary>
/// 定义抽象继承 <see cref="AbstractDbContextAccessor"/> 含审计功能的数据库上下文存取器。
/// </summary>
public abstract class AbstractDbContextAccessorWithAudit : AbstractDbContextAccessor
{
    private IReadOnlyList<Audit>? _audits;


#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。

    /// <summary>
    /// 使用指定的数据库上下文选项构造一个 <see cref="AbstractDbContextAccessorWithAudit"/> 实例。
    /// </summary>
    /// <param name="options">给定的 <see cref="DbContextOptions"/>。</param>
    protected AbstractDbContextAccessorWithAudit(DbContextOptions options)
        : base(options)
    {
        SavingChanges += AbstractDataAccessor_SavingChanges;
        SavedChanges += AbstractDataAccessor_SavedChanges;
    }

#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。


    private void AbstractDataAccessor_SavedChanges(object? sender, SavedChangesEventArgs e)
    {
        var accessor = (sender as AbstractDbContextAccessorWithAudit)!;
        if (accessor._audits is not null)
            accessor.DataOptions.Audit.NotificationAction?.Invoke(accessor._audits);
    }

    private void AbstractDataAccessor_SavingChanges(object? sender, SavingChangesEventArgs e)
    {
        var accessor = (sender as AbstractDbContextAccessorWithAudit)!;
        var auditOptions = accessor.DataOptions.Audit;

        if (!auditOptions.Enabling)
            return;

        var auditingManager = accessor.GetService<IAuditingManager>();

#pragma warning disable EF1001 // Internal EF Core API usage.

        var entityEntries = ((IDbContextDependencies)accessor).StateManager
            .GetEntriesForState(auditOptions.AddedState, auditOptions.ModifiedState,
                auditOptions.DeletedState, auditOptions.UnchangedState);

        accessor._audits = auditingManager.GetAudits(entityEntries.Select(s => new EntityEntry(s)));

#pragma warning restore EF1001 // Internal EF Core API usage.

        // 保存审计数据
        if (accessor._audits.Count > 0 && auditOptions.SaveAudits)
            accessor.Set<Audit>().AddRange(accessor._audits);
    }


    ///// <summary>
    ///// 审计数据集。
    ///// </summary>
    //public DbSet<Audit> Audits { get; set; }

    ///// <summary>
    ///// 审计属性数据集。
    ///// </summary>
    //public DbSet<AuditProperty> AuditProperties { get; set; }


    ///// <summary>
    ///// 开始模型创建。
    ///// </summary>
    ///// <param name="modelBuilder">给定的 <see cref="ModelBuilder"/>。</param>
    //protected override void OnModelCreating(ModelBuilder modelBuilder)
    //{
    //    OnDataModelCreating(modelBuilder);

    //    // 启用对实体加密属性功能的支持
    //    var converterFactory = this.GetService<IEncryptionConverterFactory>();
    //    foreach (var entityType in modelBuilder.Model.GetEntityTypes())
    //    {
    //        entityType.UseEncryption(converterFactory, this);
    //        entityType.UseQueryFilters(DataOptions.QueryFilters, this);

    //        ModelCreatingPostAction?.Invoke(entityType);
    //    }
    //}

    ///// <summary>
    ///// 开始数据模型创建。
    ///// </summary>
    ///// <param name="modelBuilder">给定的 <see cref="ModelBuilder"/>。</param>
    //protected virtual void OnDataModelCreating(ModelBuilder modelBuilder)
    //    => modelBuilder.CreateDataModel(this);

}
