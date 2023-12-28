#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Data.Accessing;

namespace Librame.Extensions.Data;

/// <summary>
/// 定义一个实现 <see cref="DbContext"/> 与 <see cref="IDataContext"/> 数据上下文。
/// </summary>
public class DataContext : DbContext, IDataContext
{
    private readonly DbContextOptions _options;
    private DataContextServices? _contextServices;


    /// <summary>
    /// 构造一个 <see cref="DataContext"/>。
    /// </summary>
    /// <param name="options">给定的 <see cref="DbContextOptions"/>。</param>
    public DataContext(DbContextOptions options)
        : base(options)
    {
        _options = options;
        
        SavingChanges += BaseDataContext_SavingChanges;
    }

    private void BaseDataContext_SavingChanges(object? sender, SavingChangesEventArgs e)
    {
        var newConnectionString = CurrentServices.ShardingContext.ShardingDatabase(this);
        if (newConnectionString is not null)
        {
            PostAccessor?.ChangeConnection(newConnectionString);
        }

        var savingChangesContext = CurrentServices.DataOptions.SavingChangesContextFactory(this);
        savingChangesContext.Preprocess();
    }


    /// <summary>
    /// 数据上下文类型。
    /// </summary>
    public virtual Type ContextType
        => GetType();

    /// <summary>
    /// 数据上下文服务集合。
    /// </summary>
    IDataContextServices IDataContext.Services
        => CurrentServices;

    /// <summary>
    /// 当前数据上下文服务集合。
    /// </summary>
    public DataContextServices CurrentServices
        => _contextServices ??= new DataContextServices(this, _options);

    /// <summary>
    /// 后置存取器。
    /// </summary>
    public IAccessor? PostAccessor { get; set; }


    /// <summary>
    /// 重写模型创建。
    /// </summary>
    /// <param name="modelBuilder">给定的 <see cref="ModelBuilder"/>。</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var modelCreator = CurrentServices.ContextCoreOptions?.
            ApplicationServiceProvider?.GetService<IModelCreator>();

        try
        {
            modelCreator?.PreCreating(this, modelBuilder);

            OnModelCreatingCore(modelBuilder);

            modelCreator?.PostCreating(this, modelBuilder);
        }
        catch (Exception ex)
        {
            modelCreator?.ExceptionCreating(this, modelBuilder, ex);
        }
    }

    /// <summary>
    /// 模型创建核心。
    /// </summary>
    /// <param name="modelBuilder">给定的 <see cref="ModelBuilder"/>。</param>
    protected virtual void OnModelCreatingCore(ModelBuilder modelBuilder)
    {
    }


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

}
