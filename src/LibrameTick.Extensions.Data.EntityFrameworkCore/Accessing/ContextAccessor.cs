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
using Librame.Extensions.Data.ValueConversion;

namespace Librame.Extensions.Data.Accessing;

/// <summary>
/// 定义一个实现 <see cref="IAccessor"/> 的 <see cref="DbContext"/> 访问器。
/// </summary>
/// <typeparam name="TDbContext">指定的数据库上下文类型。</typeparam>
public class ContextAccessor<TDbContext> : AbstractContextAccessor
    where TDbContext : DbContext, IDbContext
{
    /// <summary>
    /// 构造一个 <see cref="ContextAccessor{TDbContext}"/>。
    /// </summary>
    /// <param name="context">给定的 <typeparamref name="TDbContext"/>。</param>
    /// <param name="options">给定的 <see cref="DbContextOptions{TDbContext}"/>。</param>
    /// <param name="dataOptionsMonitor">给定的 <see cref="IOptionsMonitor{DataExtensionOptions}"/>。</param>
    /// <param name="coreOptionsMonitor">给定的 <see cref="IOptionsMonitor{CoreExtensionOptions}"/>。</param>
    public ContextAccessor(TDbContext context, DbContextOptions<TDbContext> options,
        IOptionsMonitor<DataExtensionOptions> dataOptionsMonitor,
        IOptionsMonitor<CoreExtensionOptions> coreOptionsMonitor)
        : base(context, options, dataOptionsMonitor, coreOptionsMonitor)
    {
        CurrentContext = Interceptor.CreateInterface(context, this, InterceptContext);
    }


    /// <summary>
    /// 当前数据库上下文。
    /// </summary>
    public override IDbContext CurrentContext { get; protected set; }


    /// <summary>
    /// 模型创建后动作。
    /// </summary>
    public Action<IMutableEntityType, ContextAccessor<TDbContext>>? ModelCreatedAction { get; set; }


    private static void InterceptContext(IInterceptable interceptable, ContextAccessor<TDbContext> accessor)
    {
        var nameDescriptor = InterceptionDescriptor.InterceptMethod("OnModelCreating");
        nameDescriptor.ParameterTypes = new[] { typeof(ModelBuilder) };
        nameDescriptor.PostAction = (s, v) =>
        {
            if (!accessor.DataOptions.Access.AutomaticMapping)
                return;

            var modelBuilder = v.Parameters?[0]?.As<ModelBuilder>()!;

            // 默认尝试创建迁移程序集的模型
            if (!string.IsNullOrEmpty(accessor.RelationalExtension?.MigrationsAssembly))
                modelBuilder.CreateAssembliesModels(accessor.RelationalExtension.MigrationsAssembly);

            // 启用对实体加密属性功能的支持
            var converterFactory = accessor.OriginalContext.GetService<IEncryptionConverterFactory>();
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                entityType.UseEncryption(converterFactory, accessor);
                entityType.UseQueryFilters(accessor.DataOptions.QueryFilters, accessor);

                accessor.ModelCreatedAction?.Invoke(entityType, accessor);
            }
        };
    }

}
