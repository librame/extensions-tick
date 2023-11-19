﻿#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data.Saving;

/// <summary>
/// 定义实现 <see cref="ISavingChangesContext"/> 的保存变化上下文。
/// </summary>
public sealed class SavingChangesContext : ISavingChangesContext
{

#pragma warning disable EF1001 // Internal EF Core API usage.

    /// <summary>
    /// 构造一个 <see cref="SavingChangesContext"/>。
    /// </summary>
    /// <param name="dataContext">给定的 <see cref="BaseDataContext"/>。</param>
    public SavingChangesContext(BaseDataContext dataContext)
    {
        var dbContextDependencies = dataContext.BaseDependencies.GetScopeService<IDbContextDependencies>();

        DataContext = dataContext;
        
        ChangesEntities = dbContextDependencies.StateManager
            .GetEntriesForState(added: true, modified: true, deleted: true)
            .Select(static s => new EntityEntry(s))
            .ToList();
    }


    /// <summary>
    /// 数据上下文。
    /// </summary>
    public BaseDataContext DataContext { get; init; }

    /// <summary>
    /// 变化的实体入口集合。
    /// </summary>
    public IReadOnlyList<EntityEntry> ChangesEntities { get; init; }

#pragma warning restore EF1001 // Internal EF Core API usage.


    /// <summary>
    /// 预处理保存变化。
    /// </summary>
    public void Preprocess()
    {
        // 按照添加的先后顺序执行
        foreach (var handler in DataContext.DataExtOptions.SavingChangesHandlers)
        {
            handler.PreHandling(this);
        }
    }

}
