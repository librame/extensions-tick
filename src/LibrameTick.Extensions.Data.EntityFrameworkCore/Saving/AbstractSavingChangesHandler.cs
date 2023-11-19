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
/// 定义实现 <see cref="ISavingChangesHandler"/> 的抽象保存变化处理程序。
/// </summary>
public abstract class AbstractSavingChangesHandler : ISavingChangesHandler
{
    /// <summary>
    /// 已处理。
    /// </summary>
    public bool IsHandled { get; set; }


    /// <summary>
    /// 预处理保存上下文。
    /// </summary>
    /// <param name="context">给定的 <see cref="ISavingChangesContext"/>。</param>
    public void PreHandling(ISavingChangesContext context)
    {
        if (IsHandled) return;

        PreHandlingCore(context);

        IsHandled = true;
    }

    /// <summary>
    /// 预处理保存上下文核心。
    /// </summary>
    /// <param name="context">给定的 <see cref="ISavingChangesContext"/>。</param>
    protected abstract void PreHandlingCore(ISavingChangesContext context);

}
