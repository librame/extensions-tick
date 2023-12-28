#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data;

/// <summary>
/// 定义一个模型创建器接口。
/// </summary>
public interface IModelCreator
{
    /// <summary>
    /// 预创建。
    /// </summary>
    /// <param name="dataContext">给定的 <see cref="DataContext"/>。</param>
    /// <param name="modelBuilder">给定的 <see cref="ModelBuilder"/>。</param>
    void PreCreating(DataContext dataContext, ModelBuilder modelBuilder);

    /// <summary>
    /// 后置创建。
    /// </summary>
    /// <param name="dataContext">给定的 <see cref="DataContext"/>。</param>
    /// <param name="modelBuilder">给定的 <see cref="ModelBuilder"/>。</param>
    void PostCreating(DataContext dataContext, ModelBuilder modelBuilder);

    /// <summary>
    /// 异常创建。
    /// </summary>
    /// <param name="dataContext">给定的 <see cref="DataContext"/>。</param>
    /// <param name="modelBuilder">给定的 <see cref="ModelBuilder"/>。</param>
    /// <param name="exception">给定的 <see cref="Exception"/>。</param>
    void ExceptionCreating(DataContext dataContext, ModelBuilder modelBuilder, Exception exception);
}
