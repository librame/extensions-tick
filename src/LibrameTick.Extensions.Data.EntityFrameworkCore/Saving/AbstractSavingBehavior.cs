#region License

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
/// 定义实现 <see cref="ISavingContext{BaseDbContext, EntityEntry}"/> 的抽象保存行为。
/// </summary>
public abstract class AbstractSavingBehavior : ISavingBehavior<BaseDbContext, EntityEntry>
{
    /// <summary>
    /// 处理保存上下文。
    /// </summary>
    /// <param name="context">给定的 <see cref="ISavingContext{BaseDbContext, EntityEntry}"/>。</param>
    public virtual void Handle(ISavingContext<BaseDbContext, EntityEntry> context)
    {
        HandleCore(context);

        context.AddOrUpdateBehavior(GetType(), this);
    }

    /// <summary>
    /// 处理保存上下文核心。
    /// </summary>
    /// <param name="context">给定的 <see cref="ISavingContext{BaseDbContext, EntityEntry}"/>。</param>
    protected abstract void HandleCore(ISavingContext<BaseDbContext, EntityEntry> context);

}
