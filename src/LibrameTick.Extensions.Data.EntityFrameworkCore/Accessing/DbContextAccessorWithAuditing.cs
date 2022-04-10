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
/// 定义继承 <see cref="DbContextAccessorWithAudit"/> 的数据库上下文存取器的泛型实现。
/// </summary>
/// <typeparam name="TAccessor">指定实现 <see cref="DbContextAccessorWithAudit"/> 的存取器类型。</typeparam>
public class DbContextAccessorWithAuditing<TAccessor> : DbContextAccessorWithAudit
    where TAccessor : DbContextAccessorWithAudit
{
    /// <summary>
    /// 使用指定的数据库上下文选项构造一个 <see cref="DbContextAccessorWithAuditing{TAccessor}"/> 实例。
    /// </summary>
    /// <remarks>
    /// 备注：如果需要注册多个 <see cref="DbContext"/> 扩展，参数必须使用泛型 <see cref="DbContextOptions{TAccessor}"/> 形式，
    /// 不能使用非泛型 <see cref="DbContextOptions"/> 形式，因为 <paramref name="options"/> 参数也会注册到容器中以供使用。
    /// </remarks>
    /// <param name="options">给定的 <see cref="DbContextOptions{TAccessor}"/>。</param>
    public DbContextAccessorWithAuditing(DbContextOptions<TAccessor> options)
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
/// 定义继承 <see cref="DbContextAccessor"/> 且集成审计功能的数据库上下文存取器。
/// </summary>
public class DbContextAccessorWithAudit : DbContextAccessor
{
    private IReadOnlyList<Audit>? _audits;


    /// <summary>
    /// 使用指定的数据库上下文选项构造一个 <see cref="DbContextAccessorWithAudit"/> 实例。
    /// </summary>
    /// <param name="options">给定的 <see cref="DbContextOptions"/>。</param>
    public DbContextAccessorWithAudit(DbContextOptions options)
        : base(options)
    {
        SavingChanges += DbContextAccessorWithAudit_SavingChanges;
        SavedChanges += DbContextAccessorWithAudit_SavedChanges;
    }


    /// <summary>
    /// 开始模型创建核心。
    /// </summary>
    /// <param name="modelBuilder">给定的 <see cref="ModelBuilder"/>。</param>
    protected override void OnModelCreatingCore(ModelBuilder modelBuilder)
    {
        // 手动映射审计模型
        modelBuilder.CreateAuditingModels(this);

        base.OnModelCreatingCore(modelBuilder);
    }


    private void DbContextAccessorWithAudit_SavedChanges(object? sender, SavedChangesEventArgs e)
    {
        var accessor = (sender as DbContextAccessorWithAudit)!;
        if (accessor._audits is not null)
            accessor.DataOptions.Audit.NotificationAction?.Invoke(accessor._audits);
    }

    private void DbContextAccessorWithAudit_SavingChanges(object? sender, SavingChangesEventArgs e)
    {
        var accessor = (sender as DbContextAccessorWithAudit)!;
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

}
