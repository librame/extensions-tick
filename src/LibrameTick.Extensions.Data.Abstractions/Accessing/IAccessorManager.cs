#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data.Accessing;

/// <summary>
/// <see cref="IAccessor"/> 管理器。
/// </summary>
public interface IAccessorManager
{
    /// <summary>
    /// 已注册的访问器列表。
    /// </summary>
    IReadOnlyList<IAccessor> Accessors { get; }


    /// <summary>
    /// 获取读取访问器。
    /// </summary>
    /// <param name="group">给定的所属群组（可选；默认返回初始访问器）。</param>
    /// <param name="basis">给定的分片依据（可选）。</param>
    /// <returns>返回 <see cref="IAccessor"/>。</returns>
    IAccessor GetReadAccessor(int? group = null, object? basis = null);

    /// <summary>
    /// 获取写入访问器。
    /// </summary>
    /// <param name="group">给定的所属群组（可选；默认返回初始访问器）。</param>
    /// <param name="basis">给定的分片依据（可选）。</param>
    /// <returns>返回 <see cref="IAccessor"/>。</returns>
    IAccessor GetWriteAccessor(int? group = null, object? basis = null);
}
