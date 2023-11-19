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

namespace Librame.Extensions.Data;

/// <summary>
/// 定义一个实现 <see cref="DbContext"/> 与 <see cref="IDataContext"/> 的基础数据上下文。
/// </summary>
public class BaseDataContext : DbContext, IDataContext
{
    private DbContextOptions _options;
    private BaseDataContextDependencies? _dependencies;


    /// <summary>
    /// 构造一个 <see cref="BaseDataContext"/>。
    /// </summary>
    /// <param name="options">给定的 <see cref="DbContextOptions"/>。</param>
    public BaseDataContext(DbContextOptions options)
        : base(options)
    {
        _options = options;

        SavingChanges += BaseDataContext_SavingChanges;
    }

    private void BaseDataContext_SavingChanges(object? sender, SavingChangesEventArgs e)
    {
        var savingChangesContext = DataExtOptions.SavingChangesContextFactory(this);
        savingChangesContext.Preprocess();
    }


    /// <summary>
    /// 上下文类型。
    /// </summary>
    public virtual Type ContextType
        => GetType();

    /// <summary>
    /// 上下文依赖集合。
    /// </summary>
    IDataContextDependencies IDataContext.Dependencies
        => BaseDependencies;

    /// <summary>
    /// 基础上下文依赖集合。
    /// </summary>
    public BaseDataContextDependencies BaseDependencies
        => _dependencies ??= new BaseDataContextDependencies(this);

    /// <summary>
    /// 核心扩展选项。
    /// </summary>
    public CoreExtensionOptions CoreExtOptions
        => BaseDependencies.CoreExtOptions;

    /// <summary>
    /// 数据扩展选项。
    /// </summary>
    public DataExtensionOptions DataExtOptions
        => BaseDependencies.DataExtOptions;


    /// <summary>
    /// 存取器选项扩展。
    /// </summary>
    public AccessorDbContextOptionsExtension? AccessorExtension
        => _options.FindExtension<AccessorDbContextOptionsExtension>();

    /// <summary>
    /// 核心选项扩展。
    /// </summary>
    public CoreOptionsExtension? CoreExtension
        => _options.FindExtension<CoreOptionsExtension>();

    /// <summary>
    /// 关系型选项扩展。
    /// </summary>
    public RelationalOptionsExtension? RelationalExtension
        => _options.Extensions.OfType<RelationalOptionsExtension>().FirstOrDefault();


    /// <summary>
    /// 后置创建模型动作。
    /// </summary>
    public Action<BaseDataContext, ModelBuilder, IMutableEntityType>? PostModelCreatingAction { get; set; }

    /// <summary>
    /// 后置创建模型属性动作。
    /// </summary>
    public Action<BaseDataContext, ModelBuilder, IMutableEntityType, PropertyInfo>? PostModelCreatingPropertyAction { get; set; }


    /// <summary>
    /// 重写模型创建。
    /// </summary>
    /// <param name="modelBuilder">给定的 <see cref="ModelBuilder"/>。</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var modelCreator = CoreExtension?.ApplicationServiceProvider?.GetService<IModelCreator>();

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
