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
/// 定义一个处理数据保存变化的处理程序接口。
/// </summary>
public interface ISavingChangesHandler
{
    /// <summary>
    /// 已处理。
    /// </summary>
    bool IsHandled { get; }


    /// <summary>
    /// 预处理保存上下文。
    /// </summary>
    /// <param name="context">给定的 <see cref="ISavingChangesContext"/>。</param>
    void PreHandling(ISavingChangesContext context);
}
