#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Compression;

/// <summary>
/// 定义 <see cref="ICompressionAdapter"/> 解析器接口。
/// </summary>
public interface ICompressionAdapterResolver
{
    /// <summary>
    /// 获取压缩选项。
    /// </summary>
    /// <value>
    /// 返回 <see cref="CompressionOptions"/>。
    /// </value>
    CompressionOptions Options { get; }


    /// <summary>
    /// 解析指定类型的压缩适配器。
    /// </summary>
    /// <typeparam name="TAdapted">指定的被适配类型。</typeparam>
    /// <typeparam name="TCompressed">指定的被压缩类型。</typeparam>
    /// <param name="attribute">给定的 <see cref="CompressionAdapterAttribute"/>。</param>
    /// <returns>返回 <see cref="CompressionAdapter{TAdapted, TCompressed}"/>。</returns>
    CompressionAdapter<TAdapted, TCompressed>? Resolve<TAdapted, TCompressed>(
        CompressionAdapterAttribute? attribute);

    /// <summary>
    /// 解析指定类型的压缩适配器。
    /// </summary>
    /// <typeparam name="TAdapted">指定的被适配类型。</typeparam>
    /// <typeparam name="TCompressed">指定的被压缩类型。</typeparam>
    /// <param name="name">给定的适配器名称（可选）。</param>
    /// <returns>返回 <see cref="CompressionAdapter{TAdapted, TCompressed}"/>。</returns>
    CompressionAdapter<TAdapted, TCompressed>? Resolve<TAdapted, TCompressed>(string? name = null);

    /// <summary>
    /// 解析指定类型的压缩适配器。
    /// </summary>
    /// <param name="typeToAdapt">给定的适配类型。</param>
    /// <param name="typeToCompress">给定的压缩类型。</param>
    /// <param name="attribute">给定的 <see cref="CompressionAdapterAttribute"/>。</param>
    /// <returns>返回 <see cref="ICompressionAdapter"/>。</returns>
    ICompressionAdapter? Resolve(Type typeToAdapt, Type typeToCompress, CompressionAdapterAttribute? attribute);

    /// <summary>
    /// 解析指定类型的压缩适配器。
    /// </summary>
    /// <param name="typeToAdapt">给定的适配类型。</param>
    /// <param name="typeToCompress">给定的压缩类型。</param>
    /// <param name="name">给定的适配器名称（可选）。</param>
    /// <returns>返回 <see cref="ICompressionAdapter"/>。</returns>
    ICompressionAdapter? Resolve(Type typeToAdapt, Type typeToCompress, string? name = null);
}
