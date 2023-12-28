#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Data.Sharding;

namespace Librame.Extensions.Data.Accessing;

/// <summary>
/// 定义 <see cref="IAccessor"/> 静态扩展。
/// </summary>
public static class AccessorExtensions
{

    #region Sharding

    /// <summary>
    /// 获取当前数据库上下文配置的分表描述符集合。
    /// </summary>
    /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
    /// <returns>返回 <see cref="IReadOnlyList{ShardingDescr}"/>。</returns>
    public static IReadOnlyList<ShardingDescriptor>? GetShardingTableDescriptors(this IAccessor accessor)
        => accessor.ShardingContext.Finder.FindTables(accessor.CurrentContext);

    #endregion

}
