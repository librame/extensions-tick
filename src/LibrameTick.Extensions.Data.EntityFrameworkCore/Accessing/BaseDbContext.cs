#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Core;
using Librame.Extensions.Data.Auditing;
using Librame.Extensions.Data.Sharding;
using Librame.Extensions.Data.Storing;
using Librame.Extensions.Data.ValueConversion;
using Microsoft.EntityFrameworkCore;

namespace Librame.Extensions.Data.Accessing;

/// <summary>
/// 定义一个实现 <see cref="DbContext"/> 与 <see cref="IDbContext"/> 的基础数据库上下文。
/// </summary>
public class BaseDbContext : DbContext, IDbContext
{
    private IReadOnlyList<Audit>? _savingAudits;

    private readonly IOptionsMonitor<DataExtensionOptions> _dataOptionsMonitor;
    private readonly IOptionsMonitor<CoreExtensionOptions> _coreOptionsMonitor;
    private readonly DbContextOptions _dbContextOptions;


    /// <summary>
    /// 构造一个 <see cref="BaseDbContext"/>。
    /// </summary>
    /// <param name="encryptionConverterFactory">给定的 <see cref="IEncryptionConverterFactory"/>。</param>
    /// <param name="shardingManager">给定的 <see cref="IShardingManager"/>。</param>
    /// <param name="dataOptionsMonitor">给定的 <see cref="IOptionsMonitor{DataExtensionOptions}"/>。</param>
    /// <param name="coreOptionsMonitor">给定的 <see cref="IOptionsMonitor{CoreExtensionOptions}"/>。</param>
    /// <param name="options">给定的 <see cref="DbContextOptions"/>。</param>
    public BaseDbContext(IEncryptionConverterFactory encryptionConverterFactory,
        IShardingManager shardingManager,
        IOptionsMonitor<DataExtensionOptions> dataOptionsMonitor,
        IOptionsMonitor<CoreExtensionOptions> coreOptionsMonitor,
        DbContextOptions options)
        : base(options)
    {
        _dataOptionsMonitor = dataOptionsMonitor;
        _coreOptionsMonitor = coreOptionsMonitor;
        _dbContextOptions = options;
        EncryptionConverterFactory = encryptionConverterFactory;
        ShardingManager = shardingManager;

        if (DataOptions.Audit.Enabling)
        {
            SavingChanges += BaseDbContext_SavingChanges;
            SavedChanges += BaseDbContext_SavedChanges;
        }
    }


    /// <summary>
    /// 数据扩展选项。
    /// </summary>
    public DataExtensionOptions DataOptions
        => _dataOptionsMonitor.CurrentValue;

    /// <summary>
    /// 核心扩展选项。
    /// </summary>
    public CoreExtensionOptions CoreOptions
        => _coreOptionsMonitor.CurrentValue;

    /// <summary>
    /// 加密转换器工厂。
    /// </summary>
    public IEncryptionConverterFactory EncryptionConverterFactory { get; init; }

    /// <summary>
    /// 分片管理器工厂。
    /// </summary>
    public IShardingManager ShardingManager { get; init; }

    /// <summary>
    /// 存取器选项扩展。
    /// </summary>
    public AccessorDbContextOptionsExtension? AccessorExtension
        => _dbContextOptions.FindExtension<AccessorDbContextOptionsExtension>();

    /// <summary>
    /// 关系型选项扩展。
    /// </summary>
    public RelationalOptionsExtension? RelationalExtension
        => _dbContextOptions.Extensions.OfType<RelationalOptionsExtension>().FirstOrDefault();


    /// <summary>
    /// 后置已创建模型属性动作。
    /// </summary>
    public Action<PropertyInfo, ModelBuilder, BaseDbContext>? PostModelCreatedPropertyAction { get; set; }

    /// <summary>
    /// 后置已创建模型动作。
    /// </summary>
    public Action<IMutableEntityType, ModelBuilder, BaseDbContext>? PostModelCreatedAction { get; set; }


    /// <summary>
    /// 上下文类型。
    /// </summary>
    public Type ContextType
        => GetType();


    ///// <summary>
    ///// 配置上下文。
    ///// </summary>
    ///// <param name="optionsBuilder">给定的 <see cref="DbContextOptionsBuilder"/>。</param>
    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //}


    ///// <summary>
    ///// 配置自定义规范。
    ///// </summary>
    ///// <param name="configurationBuilder">给定的 <see cref="ModelConfigurationBuilder"/>。</param>
    //protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    //{
    //}


    /// <summary>
    /// 重写模型创建。
    /// </summary>
    /// <param name="modelBuilder">给定的 <see cref="ModelBuilder"/>。</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        PreModelCreating(modelBuilder);

        ModelCreatingCore(modelBuilder);

        PostModelCreating(modelBuilder);
    }

    /// <summary>
    /// 预模型创建。
    /// </summary>
    /// <param name="modelBuilder">给定的 <see cref="ModelBuilder"/>。</param>
    protected virtual void PreModelCreating(ModelBuilder modelBuilder)
    {
        // 支持迁移程序集模型
        if (DataOptions.Access.AutoMapping && !string.IsNullOrEmpty(RelationalExtension?.MigrationsAssembly))
            modelBuilder.CreateAssembliesModels(RelationalExtension.MigrationsAssembly);

        // 支持数据审计
        if (DataOptions.Audit.Enabling)
            modelBuilder.CreateAuditingModels(this);
    }

    /// <summary>
    /// 模型创建核心。
    /// </summary>
    /// <param name="modelBuilder">给定的 <see cref="ModelBuilder"/>。</param>
    protected virtual void ModelCreatingCore(ModelBuilder modelBuilder)
    {
    }

    /// <summary>
    /// 后置模型配置。
    /// </summary>
    /// <param name="modelBuilder">给定的 <see cref="ModelBuilder"/>。</param>
    protected virtual void PostModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.ClrType.GetProperties())
            {
                // 启用对实体加密属性的支持
                if (property.IsEncrypted())
                    entityType.UseEncryption(property, this);

                // 启用对强类型属性的支持
                if (property.IsStronglyTypedIdentifier(out var valueType))
                    entityType.UseStronglyTypedIdentifier(property, valueType!);

                // 支持将 GUID 类型统一处理为 Chars 类型（跨数据库支持 GUID）
                if (DataOptions.Access.GuidToChars)
                    entityType.UseGuidToChars(property, modelBuilder);

                PostModelCreatedPropertyAction?.Invoke(property, modelBuilder, this);
            }

            // 全局查询过滤器
            entityType.UseQueryFilters(DataOptions.QueryFilters, this);

            PostModelCreatedAction?.Invoke(entityType, modelBuilder, this);
        }
    }


    private void BaseDbContext_SavedChanges(object? sender, SavedChangesEventArgs e)
    {
        var dbContext = (sender as BaseDbContext)!;
        if (dbContext._savingAudits is not null)
            dbContext.DataOptions.Audit.NotificationAction?.Invoke(dbContext._savingAudits);
    }

    private void BaseDbContext_SavingChanges(object? sender, SavingChangesEventArgs e)
    {
        var dbContext = (sender as BaseDbContext)!;
        var auditOptions = dbContext.DataOptions.Audit;

        var auditingManager = dbContext.GetService<IAuditingManager>();

#pragma warning disable EF1001 // Internal EF Core API usage.

        var entityEntries = ((IDbContextDependencies)dbContext).StateManager
            .GetEntriesForState(auditOptions.AddedState, auditOptions.ModifiedState,
                auditOptions.DeletedState, auditOptions.UnchangedState);

        dbContext._savingAudits = auditingManager.GetAudits(entityEntries.Select(s => new EntityEntry(s)));

#pragma warning restore EF1001 // Internal EF Core API usage.

        // 保存审计数据
        if (dbContext._savingAudits.Count > 0 && auditOptions.SaveAudits)
            dbContext.Set<Audit>().AddRange(dbContext._savingAudits);
    }

}
