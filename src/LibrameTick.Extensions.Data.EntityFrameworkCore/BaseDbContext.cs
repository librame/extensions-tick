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
using Librame.Extensions.Data.Accessing;
using Librame.Extensions.Data.Saving;
using Librame.Extensions.Data.Sharding;
using Librame.Extensions.Data.ValueConversion;

namespace Librame.Extensions.Data;

/// <summary>
/// 定义一个实现 <see cref="DbContext"/> 与 <see cref="IDbContext"/> 的基础数据库上下文。
/// </summary>
public class BaseDbContext : DbContext, IDbContext
{
    private readonly IOptionsMonitor<DataExtensionOptions> _dataOptions;
    private readonly IOptionsMonitor<CoreExtensionOptions> _coreOptions;
    private readonly DbContextOptions _options;
    private readonly ISaveChangesEventHandler _saveChangesEventHandler;


    /// <summary>
    /// 构造一个 <see cref="BaseDbContext"/>。
    /// </summary>
    /// <param name="shardingContext">给定的 <see cref="IShardingContext"/>。</param>
    /// <param name="encryptionConverterFactory">给定的 <see cref="IEncryptionConverterFactory"/>。</param>
    /// <param name="dataOptions">给定的 <see cref="IOptionsMonitor{DataExtensionOptions}"/>。</param>
    /// <param name="coreOptions">给定的 <see cref="IOptionsMonitor{CoreExtensionOptions}"/>。</param>
    /// <param name="options">给定的 <see cref="DbContextOptions"/>。</param>
    public BaseDbContext(IShardingContext shardingContext,
        IEncryptionConverterFactory encryptionConverterFactory,
        IOptionsMonitor<DataExtensionOptions> dataOptions,
        IOptionsMonitor<CoreExtensionOptions> coreOptions,
        DbContextOptions options)
        : base(options)
    {
        _dataOptions = dataOptions;
        _coreOptions = coreOptions;
        _options = options;
        _saveChangesEventHandler = dataOptions.CurrentValue.SaveChangesEventHandler(this);

        EncryptionConverterFactory = encryptionConverterFactory;
        ShardingContext = shardingContext;
        
        SavingChanges += BaseDbContext_SavingChanges;
        SavedChanges += BaseDbContext_SavedChanges;
        SaveChangesFailed += BaseDbContext_SaveChangesFailed;
    }


    private void BaseDbContext_SavingChanges(object? sender, SavingChangesEventArgs e)
        => _saveChangesEventHandler.SavingChanges(this, e.AcceptAllChangesOnSuccess);

    private void BaseDbContext_SavedChanges(object? sender, SavedChangesEventArgs e)
        => _saveChangesEventHandler.SavedChanges(this, e.AcceptAllChangesOnSuccess, e.EntitiesSavedCount);

    private void BaseDbContext_SaveChangesFailed(object? sender, SaveChangesFailedEventArgs e)
        => _saveChangesEventHandler.SaveChangesFailed(this, e.AcceptAllChangesOnSuccess, e.Exception);


    /// <summary>
    /// 数据扩展选项。
    /// </summary>
    public DataExtensionOptions DataOptions
        => _dataOptions.CurrentValue;

    /// <summary>
    /// 核心扩展选项。
    /// </summary>
    public CoreExtensionOptions CoreOptions
        => _coreOptions.CurrentValue;

    /// <summary>
    /// 加密转换器工厂。
    /// </summary>
    public IEncryptionConverterFactory EncryptionConverterFactory { get; init; }

    /// <summary>
    /// 存取器选项扩展。
    /// </summary>
    public AccessorDbContextOptionsExtension? AccessorExtension
        => _options.FindExtension<AccessorDbContextOptionsExtension>();

    /// <summary>
    /// 关系型选项扩展。
    /// </summary>
    public RelationalOptionsExtension? RelationalExtension
        => _options.Extensions.OfType<RelationalOptionsExtension>().FirstOrDefault();


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
    public virtual Type ContextType
        => GetType();

    /// <summary>
    /// 分片上下文。
    /// </summary>
    public IShardingContext ShardingContext { get; init; }


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
        PreOnModelCreating(modelBuilder);

        OnModelCreatingCore(modelBuilder);

        PostOnModelCreating(modelBuilder);
    }

    /// <summary>
    /// 预模型创建。
    /// </summary>
    /// <param name="modelBuilder">给定的 <see cref="ModelBuilder"/>。</param>
    protected virtual void PreOnModelCreating(ModelBuilder modelBuilder)
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
    protected virtual void OnModelCreatingCore(ModelBuilder modelBuilder)
    {
    }

    /// <summary>
    /// 后置模型配置。
    /// </summary>
    /// <param name="modelBuilder">给定的 <see cref="ModelBuilder"/>。</param>
    protected virtual void PostOnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.ClrType.GetProperties())
            {
                // 启用对实体加密属性的支持
                if (DataOptions.Access.AutoEncryption && property.IsEncrypted())
                    entityType.UseEncryption(property, this);

                // 启用对强类型属性的支持
                if (DataOptions.Access.AutoStronglyTyped && property.IsStronglyTypedIdentifier(out var valueType))
                    entityType.UseStronglyTypedIdentifier(property, valueType!);

                // 支持将 GUID 类型统一处理为 Chars 类型（跨数据库支持 GUID）
                if (DataOptions.Access.AutoGuidToChars && property.IsGuidOrNullable(out var isNullable))
                    entityType.UseGuidToChars(property, modelBuilder, isNullable);

                // 支持数据版本标识以支持并发功能
                if (DataOptions.Access.AutoRowVersion && entityType.IsRowVersionType())
                    entityType.UseRowVersion(property, modelBuilder);

                PostModelCreatedPropertyAction?.Invoke(property, modelBuilder, this);
            }

            // 全局查询过滤器
            entityType.UseQueryFilters(DataOptions.QueryFilters, this);

            PostModelCreatedAction?.Invoke(entityType, modelBuilder, this);
        }
    }

}
