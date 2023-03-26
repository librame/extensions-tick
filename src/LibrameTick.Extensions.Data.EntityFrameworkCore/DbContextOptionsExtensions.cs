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
/// <see cref="IDbContextOptions"/> 静态扩展。
/// </summary>
public static class DbContextOptionsExtensions
{

    /// <summary>
    /// 获取或默认上下文选项扩展。
    /// </summary>
    /// <typeparam name="TExtension">指定的上下文选项扩展类型。</typeparam>
    /// <param name="options">给定的 <see cref="IDbContextOptions"/>。</param>
    /// <param name="defaultExtension">给定的默认 <typeparamref name="TExtension"/>（可选；默认新建选项扩展实例）。</param>
    /// <returns>返回 <typeparamref name="TExtension"/>。</returns>
    public static TExtension GetOrDefault<TExtension>(this IDbContextOptions options,
        TExtension? defaultExtension = null)
        where TExtension : class, IDbContextOptionsExtension, new()
        => options?.FindExtension<TExtension>() ?? defaultExtension ?? new TExtension();


    /// <summary>
    /// 查找类似的上下文选项扩展集合。
    /// </summary>
    /// <typeparam name="TExtension">指定的上下文选项扩展类型。</typeparam>
    /// <param name="options">给定的 <see cref="IDbContextOptions"/>。</param>
    /// <returns>返回 <see cref="IEnumerable{TExtension}"/>。</returns>
    public static IEnumerable<TExtension> LikeExtensions<TExtension>(this IDbContextOptions options)
        where TExtension : class, IDbContextOptionsExtension
    {
        foreach (var extension in options.Extensions)
        {
            if (extension is TExtension exist)
            {
                yield return exist;
            }
        }
    }

}
