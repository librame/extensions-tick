#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;

namespace Librame.Extensions.Data;

/// <summary>
/// 定义继承 <see cref="ModelSource"/> 的数据模型源。
/// </summary>
/// <param name="dependencies">给定的 <see cref="ModelSourceDependencies"/>。</param>
public sealed class DataModelSource(ModelSourceDependencies dependencies) : ModelSource(dependencies)
{

#pragma warning disable EF1001 // Internal EF Core API usage.

    /// <summary>
    /// 当前可变模型。
    /// </summary>
    internal Model? CurrentMutableModel { get; private set; }

    /// <summary>
    /// 当前规约调度器。
    /// </summary>
    internal ConventionDispatcher? CurrentConventionDispatcher { get; private set; }


    /// <summary>
    /// 创建模型。
    /// </summary>
    /// <param name="context">给定的 <see cref="DbContext"/>。</param>
    /// <param name="conventionSetBuilder">给定的 <see cref="IConventionSetBuilder"/>。</param>
    /// <param name="modelDependencies">给定的 <see cref="ModelDependencies"/>。</param>
    /// <returns>返回 <see cref="IModel"/>。</returns>
    protected override IModel CreateModel(DbContext context, IConventionSetBuilder conventionSetBuilder,
        ModelDependencies modelDependencies)
    {
        var model = base.CreateModel(context, conventionSetBuilder, modelDependencies);
        if (model is Model mutableModel)
        {
            CurrentMutableModel = mutableModel;
            CurrentConventionDispatcher = mutableModel.ConventionDispatcher;
        }

        return model;
    }

#pragma warning restore EF1001 // Internal EF Core API usage.

}
