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
    /// 模型实体创建后动作。
    /// </summary>
    public Action<IMutableEntityType, BaseDbContext>? ModelEntityCreatedAction { get; set; }


    /// <summary>
    /// 上下文类型。
    /// </summary>
    public Type ContextType
        => GetType();


    /// <summary>
    /// 重写模型创建。
    /// </summary>
    /// <param name="modelBuilder">给定的 <see cref="ModelBuilder"/>。</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // 默认尝试自动映射迁移程序集的模型
        if (DataOptions.Access.AutoMapping && !string.IsNullOrEmpty(RelationalExtension?.MigrationsAssembly))
            modelBuilder.CreateAssembliesModels(RelationalExtension.MigrationsAssembly);

        if (DataOptions.Audit.Enabling)
            modelBuilder.CreateAuditingModels(this);

        CustomModelCreating(modelBuilder);

        GeneralModelConfiguring(modelBuilder);
    }

    /// <summary>
    /// 自定义模型创建。
    /// </summary>
    /// <param name="modelBuilder">给定的 <see cref="ModelBuilder"/>。</param>
    protected virtual void CustomModelCreating(ModelBuilder modelBuilder)
    {
    }

    /// <summary>
    /// 通用模型配置。
    /// </summary>
    /// <param name="modelBuilder">给定的 <see cref="ModelBuilder"/>。</param>
    protected virtual void GeneralModelConfiguring(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.ClrType.GetProperties())
            {
                // 启用对实体加密属性功能的支持
                entityType.UseEncryption(property, this);
                
                // 支持将 GUID 类型统一处理为 Chars 类型（跨数据库支持 GUID）
                entityType.UseGuidToChars(property, modelBuilder, this);
            }

            // 全局查询过滤器
            entityType.UseQueryFilters(DataOptions.QueryFilters, this);

            ModelEntityCreatedAction?.Invoke(entityType, this);
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
