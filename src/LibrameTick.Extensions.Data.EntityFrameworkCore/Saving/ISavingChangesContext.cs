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
/// 定义一个处理数据保存变化的上下文接口。
/// </summary>
public interface ISavingChangesContext
{
    /// <summary>
    /// 数据上下文。
    /// </summary>
    DataContext DataContext { get; }

    /// <summary>
    /// 数据扩展选项。
    /// </summary>
    DataExtensionOptions DataOptions { get; }

    /// <summary>
    /// 变化的实体入口集合。
    /// </summary>
    IReadOnlyList<EntityEntry> ChangesEntities { get; }


    /// <summary>
    /// 预处理保存变化。
    /// </summary>
    void Preprocess();
}
