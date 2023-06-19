﻿#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Data;
using Librame.Extensions.Data.Accessing;

namespace Microsoft.EntityFrameworkCore;

/// <summary>
/// <see cref="DbContextOptionsBuilder"/> 存取器静态扩展。
/// </summary>
public static class AccessorDbContextOptionsBuilderExtensions
{

    /// <summary>
    /// 使用存取器扩展（默认尝试从 <see cref="DbContextOptionsBuilder{TContext}"/> 参数中提取存取器服务类型）。
    /// </summary>
    /// <param name="builder">给定的 <see cref="DbContextOptionsBuilder"/>。</param>
    /// <param name="optionsAction">给定的存取器选项构建器配置动作（可选）。</param>
    /// <returns>返回 <see cref="DbContextOptionsBuilder"/>。</returns>
    public static DbContextOptionsBuilder UseAccessor(this DbContextOptionsBuilder builder,
        Action<AccessorDbContextOptionsBuilder>? optionsAction = null)
    {
        var builderType = builder.GetType();
        if (!builderType.IsGenericType || builderType.GenericTypeArguments.Length != 1)
            throw new NotSupportedException($"This method only supports the generic ${nameof(DbContextOptionsBuilder)}<TContext> parameter.");
        
        return builder.UseAccessor(builderType.GenericTypeArguments[0], optionsAction);
    }

    /// <summary>
    /// 使用存取器扩展。
    /// </summary>
    /// <param name="builder">给定的 <see cref="DbContextOptionsBuilder"/>。</param>
    /// <param name="accessorType">给定的存取器类型。</param>
    /// <param name="optionsAction">给定的存取器选项构建器配置动作（可选）。</param>
    /// <returns>返回 <see cref="DbContextOptionsBuilder"/>。</returns>
    public static DbContextOptionsBuilder UseAccessor(this DbContextOptionsBuilder builder,
        Type accessorType, Action<AccessorDbContextOptionsBuilder>? optionsAction = null)
    {
        builder.AddOrUpdateExtension<AccessorDbContextOptionsExtension>(c => c.WithAccessorType(accessorType));

        optionsAction?.Invoke(new AccessorDbContextOptionsBuilder(builder));

        return builder;
    }

}
