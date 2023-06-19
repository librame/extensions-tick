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
/// 定义一个用于 <see cref="IDbContext"/> 的保存变化事件处理程序接口。
/// </summary>
public interface ISaveChangesEventHandler
{
    /// <summary>
    /// 保存变化。
    /// </summary>
    /// <param name="context">给定的 <see cref="IDbContext"/>。</param>
    /// <param name="acceptAllChangesOnSuccess">指示在将更改成功发送到数据库后是否调用 ChangeTracker.AcceptAllChanges()。</param>
    void SavingChanges(IDbContext context, bool acceptAllChangesOnSuccess);

    /// <summary>
    /// 保存成功。
    /// </summary>
    /// <param name="context">给定的 <see cref="IDbContext"/>。</param>
    /// <param name="acceptAllChangesOnSuccess">指示在将更改成功发送到数据库后是否调用 ChangeTracker.AcceptAllChanges()。</param>
    /// <param name="entitiesSavedCount">给定的实体保存数。</param>
    void SavedChanges(IDbContext context, bool acceptAllChangesOnSuccess, int entitiesSavedCount);

    /// <summary>
    /// 保存失败。
    /// </summary>
    /// <param name="context">给定的 <see cref="IDbContext"/>。</param>
    /// <param name="acceptAllChangesOnSuccess">指示在将更改成功发送到数据库后是否调用 ChangeTracker.AcceptAllChanges()。</param>
    /// <param name="exception">给定的 <see cref="Exception"/>。</param>
    void SaveChangesFailed(IDbContext context, bool acceptAllChangesOnSuccess, Exception exception);
}
