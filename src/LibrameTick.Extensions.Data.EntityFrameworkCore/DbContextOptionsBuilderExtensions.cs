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
/// <see cref="DbContextOptionsBuilder"/> 静态扩展。
/// </summary>
public static class DbContextOptionsBuilderExtensions
{

    /// <summary>
    /// 添加或更新指定上下文选项扩展。
    /// </summary>
    /// <typeparam name="TExtension">指定的上下文选项扩展类型。</typeparam>
    /// <param name="builder">给定的 <see cref="DbContextOptionsBuilder"/>。</param>
    /// <param name="extension">给定的 <typeparamref name="TExtension"/>（可选；默认从构建器的选项集合中查找或创建此实例）。</param>
    /// <returns>返回 <typeparamref name="TExtension"/>。</returns>
    public static TExtension AddOrUpdateExtension<TExtension>(this DbContextOptionsBuilder builder,
        TExtension? extension = null)
        where TExtension : class, IDbContextOptionsExtension, new()
    {
        if (extension is null)
            extension = builder.Options.GetOrDefault<TExtension>();

        ((IDbContextOptionsBuilderInfrastructure)builder).AddOrUpdateExtension(extension);

        return extension;
    }

    /// <summary>
    /// 添加或更新指定上下文选项扩展。
    /// </summary>
    /// <typeparam name="TExtension">指定的上下文选项扩展类型。</typeparam>
    /// <param name="builder">给定的 <see cref="DbContextOptionsBuilder"/>。</param>
    /// <param name="configureFunc">给定的上下文选项扩展配置方法。</param>
    /// <returns>返回 <typeparamref name="TExtension"/>。</returns>
    public static TExtension AddOrUpdateExtension<TExtension>(this DbContextOptionsBuilder builder,
        Func<TExtension, TExtension> configureFunc)
        where TExtension : class, IDbContextOptionsExtension, new()
    {
        var extension = configureFunc(builder.Options.GetOrDefault<TExtension>());

        ((IDbContextOptionsBuilderInfrastructure)builder).AddOrUpdateExtension(extension);

        return extension;
    }

}
